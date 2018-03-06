using System;
using System.Collections.Generic;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

using Common.Logging;

using Spring.Transaction.Interceptor;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    /// <summary>
    ///     Service der Methoden bereitstellt um Rechnungen zu verwalten.
    /// </summary>
    public class BillService : IBillService {
        /// <summary>
        ///     Liefert oder setzt den BillDao
        /// </summary>
        public IBillDao BillDao { get; set; }

        /// <summary>
        ///     Liefert oder setzt den BookingService
        /// </summary>
        public IBookingService BookingService { get; set; }

        /// <summary>
        ///     Liefert oder setzt den NotificationService
        /// </summary>
        public INotificationService NotificationService { get; set; }

        /// <summary>
        ///     Liefert oder setzt den PeanutService
        /// </summary>
        public IPeanutService PeanutService { get; set; }

        /// <summary>
        ///     Setzt den Status des Schuldners einer Rechnung auf <see cref="BillAcceptState.Accepted">Akzeptiert</see>.
        /// </summary>
        /// <param name="bill">Die zu bestätigende Rechnung.</param>
        /// <param name="debitor">Der Schuldner</param>
        /// <param name="user">Wer führt die Ablehnung durch.</param>
        /// <param name="billUrl">Die Url zum Anzeigen der Rechnung.</param>
        /// <param name="settleBillUrl">Die Url zum Formular auf dem eine Rechnung manuell abgerechnet werden kann.</param>
        [Transaction]
        public void AcceptBill(Bill bill, User debitor, User user, string billUrl, string settleBillUrl) {
            Require.NotNull(bill, "bill");
            Require.NotNull(debitor, "debitor");
            if (!bill.IsSettled) {
                BillAcceptState? billAcceptState = bill.GetDebitorState(debitor);
                if (billAcceptState != null && billAcceptState != BillAcceptState.Accepted) {
                    bill.AcceptByDebitor(debitor, new EntityChangedDto(user, DateTime.Now));

                    if (bill.HasEveryoneAccepted) {
                        NotificationService.SendBillAcceptedNotification(bill, billUrl, settleBillUrl);
                    }
                }
            } else {
                throw new InvalidOperationException("Die Rechnung kann nicht akzeptiert werden da diese bereits abgerechnet wurde.");
            }
        }

        /// <summary>
        ///     Erstellt eine Rechnung.
        /// </summary>
        /// <param name="userGroup">Die Gruppe in welcher die Rechnung erstellt wird.</param>
        /// <param name="billDto">Informationen zur Rechnung</param>
        /// <param name="creditor">Wer ist der Kreditor/Gläubiger/Begünstigte der Rechnung.</param>
        /// <param name="debitors">Wer sind die Debitoren/Schuldner in der Gruppe, in welcher die Rechnung erstellt wird?</param>
        /// <param name="guestDebitors">Wer sind die Schuldner/Debitoren außerhalb der Gruppe, in der die Rechnung erstellt wird?</param>
        /// <param name="peanut">
        ///     Ruft einen optional zu verlinkenden Peanut ab, aus bzw. für welchen die Rechnung
        ///     erstellt wird.
        /// </param>
        /// <param name="creator">Wer erstellt die Rechnung?</param>
        /// <returns></returns>
        /// <remarks>
        ///     Der Anteil eines Schuldners/Debitors am Gesamtrechnungsbetrag, ergibt sich aus seinem
        ///     <see cref="BillUserGroupDebitor.Portion">Anteil</see> an der Summe aller Anteile.
        ///     Ist ein Schuldner mehrfach in der Liste der Debitoren, werden seine Einträge zusammengefasst, indem die Anteile
        ///     summiert werden.
        /// </remarks>
        [Transaction]
        public Bill Create(UserGroup userGroup, BillDto billDto, UserGroupMembership creditor, IList<BillUserGroupDebitorDto> debitors,
            IList<BillGuestDebitorDto> guestDebitors, Peanut peanut, User creator) {
            Require.NotNull(creator, "creator");
            Require.NotNull(userGroup, "userGroup");
            Require.NotNull(billDto, "billDto");
            Require.NotNull(creditor, "creditor");
            Require.NotNull(debitors, "debitors");
            Require.NotNull(guestDebitors, "guestDebitors");

            IList<BillUserGroupDebitor> debitorEntities = GetDebitors(debitors, creator);
            IList<BillGuestDebitor> guestDebitorEntities = GetGuestDebitors(guestDebitors);
            Bill bill = new Bill(userGroup, billDto, creditor, debitorEntities, guestDebitorEntities, new EntityCreatedDto(creator, DateTime.Now));

            if (peanut != null) {
                PeanutService.ClearPeanut(peanut, bill);
            }
            return BillDao.Save(bill);
        }

        /// <summary>
        ///     Setzt den Status des Schuldners einer Rechnung auf <see cref="BillAcceptState.Refused">Abgelehnt</see>.
        /// </summary>
        /// <param name="bill">Die Rechnung</param>
        /// <param name="debitor">Der Schuldner</param>
        /// <param name="refuseComment">Warum wird die Rechnung abgelehnt</param>
        /// <param name="user">Wer führt die Ablehnung durch.</param>
        /// <param name="billUrl">Die Url, zum Anzeigen der Rechnung.</param>
        [Transaction]
        public void DeclineBill(Bill bill, User debitor, string refuseComment, User user, string billUrl) {
            Require.NotNull(bill, "bill");
            Require.NotNull(debitor, "debitor");
            Require.NotNullOrWhiteSpace(refuseComment, "refuseComment");
            Require.NotNullOrWhiteSpace(billUrl, "billUrl");

            BillAcceptState? billAcceptState = bill.GetDebitorState(debitor);
            if (billAcceptState != null && billAcceptState != BillAcceptState.Refused) {
                BillUserGroupDebitor rejectingDebitor = bill.RefuseByDebitor(debitor, refuseComment, new EntityChangedDto(user, DateTime.Now));
                if (rejectingDebitor != null) {
                    NotificationService.SendBillDeclinedNotification(bill, rejectingDebitor, billUrl);
                }
            }
        }

        /// <summary>
        ///     Löscht eine Rechnung.
        /// </summary>
        /// <param name="bill"></param>
        [Transaction]
        public void Delete(Bill bill) {
            Require.NotNull(bill, "bill");

            if (bill.IsSettled) {
                throw new InvalidOperationException("Eine abgerechnete Rechnung darf nicht gelöscht werden!");
            }
            Peanut billForPeanut = PeanutService.FindFromBill(bill);
            if (billForPeanut != null) {
                billForPeanut.RemoveAssociatedBill(bill);
            }
            BillDao.Delete(bill);
        }

        /// <summary>
        ///     Sucht nach allen offenen Rechnungen die durch alle Nutzer akzeptiert wurden.
        /// </summary>
        /// <returns></returns>
        public IList<Bill> FindSettleableBills() {
            IList<Bill> unsettledBills = BillDao.FindUnsettledBills();
            return unsettledBills.Where(bill => bill.HasEveryoneAccepted).ToList();
        }

        /// <summary>
        ///     Ruft die Ausgangsrechnungen eines Nutzers ab.
        ///     Also die, bei denen der Nutzer Geld erhält.
        /// </summary>
        /// <param name="pageRequest">Seiteninformationen für die Abfrage</param>
        /// <param name="user">Für welchen Nutzer sollen die Rechnungen abgerufen werden.</param>
        /// <param name="isSettled">
        ///     Die Rechnungen welchen Status sollen abgerufen werden?
        ///     true => Es werden nur <see cref="Bill.IsSettled">abgerechnete Rechnungen</see> abgerufen.
        ///     false => Es werden nur <see cref="Bill.IsSettled">NICHT abgerechnete Rechnungen</see> abgerufen.
        ///     null => Ob die Rechnung <see cref="Bill.IsSettled">abgerechnet ist</see> spielt keine Rolle.
        /// </param>
        /// <returns></returns>
        public IPage<Bill> FindCreditorBillsForUser(IPageable pageRequest, User user, bool? isSettled) {
            return BillDao.FindCreditorBillsForUser(pageRequest, user, isSettled);
        }

        /// <summary>
        ///     Ruft die Eingangsrechnungen eines Nutzers ab.
        ///     Also die, bei denen der Nutzer Geld schuldet.
        /// </summary>
        /// <param name="pageRequest">Seiteninformationen für die Abfrage</param>
        /// <param name="user">Für welchen Nutzer sollen die Rechnungen abgerufen werden.</param>
        /// <param name="isSettled">
        ///     Die Rechnungen welchen Status sollen abgerufen werden?
        ///     true => Es werden nur <see cref="Bill.IsSettled">abgerechnete Rechnungen</see> abgerufen.
        ///     false => Es werden nur <see cref="Bill.IsSettled">NICHT abgerechnete Rechnungen</see> abgerufen.
        ///     null => Ob die Rechnung <see cref="Bill.IsSettled">abgerechnet ist</see> spielt keine Rolle.
        /// </param>
        /// <returns></returns>
        public IPage<Bill> FindBillsWhereUserIsDebitor(IPageable pageRequest, User user, bool? isSettled) {
            return BillDao.FindBillsWhereUserIsDebitor(pageRequest, user, isSettled);
        }

        public IPage<Bill> FindRefusedBillsWhereUserIsCreditor(PageRequest pageRequest, User user) {
            return BillDao.FindRefusedBillsWhereUserIsCreditor(pageRequest, user);
        }

        /// <summary>
        ///     Ruft alle offenen Rechnungen für einen Nutzer ab, egal ob Eingans- oder Ausgangsrechnungen.
        /// </summary>
        /// <param name="pageRequest">Seiteninformationen</param>
        /// <param name="user">Die Rechnungen welches Nutzers sollen abgerufen werden?</param>
        /// <returns></returns>
        public IPage<Bill> FindPendingBillsByUser(IPageable pageRequest, User user) {
            return BillDao.FindPendingBillsByUser(pageRequest, user);
        }

        /// <summary>
        ///     Rechnet eine Rechnung ab.
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="user">Wer ändert die Rechnung.</param>
        [Transaction]
        public void Settle(Bill bill, User user) {
            Require.NotNull(bill, "bill");
            Require.NotNull(user, "user");

            if (bill.IsSettled) {
                return;
            }

            foreach (BillUserGroupDebitor billUserGroupDebitor in bill.UserGroupDebitors) {
                if (!billUserGroupDebitor.UserGroupMembership.Equals(bill.Creditor)) {
                    BookingService.Book(billUserGroupDebitor.UserGroupMembership.Account,
                        bill.Creditor.Account,
                        bill.GetPartialAmountByPortion(billUserGroupDebitor.Portion),
                        string.Format("{0}: {1}", bill.Id, bill.Subject));
                }
            }

            bill.Settle(new EntityChangedDto(user, DateTime.Now));
            LogManager.GetLogger<BillService>().Info($"Die Rechnung mit der Nummer {bill.Id} und dem Betreff {bill.Subject} wurde abgerechnet.");
        }

        private static IList<BillUserGroupDebitor> GetDebitors(IList<BillUserGroupDebitorDto> debitors, User creator) {
            Dictionary<UserGroupMembership, List<BillUserGroupDebitorDto>> debitorsByMembership = debitors.GroupBy(dto => dto.UserGroupMembership).ToDictionary(g => g.Key, g => g.ToList());

            IList<BillUserGroupDebitor> debitorEntities = new List<BillUserGroupDebitor>();
            foreach (UserGroupMembership userGroupMembership in debitorsByMembership.Keys) {
                BillAcceptState billAcceptState;
                if (userGroupMembership.User.Equals(creator)) {
                    /*Wenn ich selber die Rechnung erstelle, kann ich die auch gleich akzeptieren*/
                    billAcceptState = BillAcceptState.Accepted;
                } else if (userGroupMembership.AutoAcceptBills) {
                    /*Wenn das Mitglied das automatische akzeptieren der Rechnung aktiviert hat, gleich akzeptieren*/
                    billAcceptState = BillAcceptState.Accepted;
                } else {
                    /*Die anderen müssen die Rechnung explizit akzeptieren.*/
                    billAcceptState = BillAcceptState.Pending;
                }
                BillUserGroupDebitor billUserGroupDebitor = new BillUserGroupDebitor(userGroupMembership, debitorsByMembership[userGroupMembership].Sum(dto => dto.Portion), billAcceptState);
                debitorEntities.Add(billUserGroupDebitor);
            }
            return debitorEntities;
        }

        private static IList<BillGuestDebitor> GetGuestDebitors(IList<BillGuestDebitorDto> guestDebitors) {
            return guestDebitors.Select(deb => new BillGuestDebitor(deb)).ToList();
        }

        [Transaction]
        public IList<Bill> SettleAllSettleableBills() {
            IList<Bill> settleableBills = FindSettleableBills();
            IList<Bill> settledBills = new List<Bill>();
            foreach (Bill settleableBill in settleableBills) {
                Settle(settleableBill, settleableBill.Creditor.User);
                settledBills.Add(settleableBill);
            }

            return settledBills;

        }
    }
}