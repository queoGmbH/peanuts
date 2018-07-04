using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    /// <summary>
    ///     Beschreibt einen Service der Methoden zur Verfügung stellt, um Rechnungen zu verwalten.
    /// </summary>
    public interface IBillService {
        /// <summary>
        ///     Setzt den Status des Schuldners einer Rechnung auf <see cref="BillAcceptState.Accepted">Akzeptiert</see>.
        /// </summary>
        /// <param name="bill">Die zu bestätigende Rechnung.</param>
        /// <param name="debitor">Der Schuldner</param>
        /// <param name="user">Wer führt die Ablehnung durch.</param>
        /// <param name="billUrl">Die Url zum Anzeigen der Rechnung.</param>
        /// <param name="settleBillUrl">Die Url zum Formular auf dem eine Rechnung manuell abgerechnet werden kann.</param>
        void AcceptBill(Bill bill, User debitor, User user, string billUrl, string settleBillUrl);

        /// <summary>
        ///     Erstellt eine Rechnung.
        /// </summary>
        /// <param name="userGroup">Die Gruppe in welcher die Rechnung erstellt wird.</param>
        /// <param name="billDto">Informationen zur Rechnung</param>
        /// <param name="creditor">Wer ist der Kreditor/Gläubiger/Begünstigte der Rechnung.</param>
        /// <param name="debitors">Wer sind die Debitoren/Schuldner in der Gruppe, in welcher die Rechnung erstellt wird?</param>
        /// <param name="guestDebitors">Wer sind die Schuldner/Debitoren außerhalb der Gruppe, in der die Rechnung erstellt wird?</param>
        /// <param name="createdFromPeanut">
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
        Bill Create(UserGroup userGroup, BillDto billDto, UserGroupMembership creditor, IList<BillUserGroupDebitorDto> debitors,
            IList<BillGuestDebitorDto> guestDebitors, Peanut createdFromPeanut, User creator);

        /// <summary>
        ///     Setzt den Status des Schuldners einer Rechnung auf <see cref="BillAcceptState.Refused">Abgelehnt</see>.
        /// </summary>
        /// <param name="bill">Die Rechnung</param>
        /// <param name="debitor">Der Schuldner</param>
        /// <param name="refuseComment">Warum wird die Rechnung abgelehnt</param>
        /// <param name="user">Wer führt die Ablehnung durch.</param>
        /// <param name="billUrl">Die Url, zum Anzeigen der Rechnung.</param>
        void DeclineBill(Bill bill, User debitor, string refuseComment, User user, string billUrl);

        /// <summary>
        ///     Löscht eine Rechnung.
        /// </summary>
        /// <param name="bill"></param>
        void Delete(Bill bill);

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
        IPage<Bill> FindCreditorBillsForUser(IPageable pageRequest, User user, bool? isSettled);

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
        IPage<Bill> FindBillsWhereUserIsDebitor(IPageable pageRequest, User user, bool? isSettled);

        /// <summary>
        ///     Sucht nach nicht abgerechneten Rechnungen, bei denen der Nutzer der Kreditor ist (der Gläubiger) und bei denen
        ///     mindestens 1 Nutzer die Zahlung der Rechnung abgelehnt hat.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        IPage<Bill> FindRefusedBillsWhereUserIsCreditor(PageRequest pageRequest, User user);

        /// <summary>
        ///     Ruft alle offenen Rechnungen für einen Nutzer ab, egal ob Eingans- oder Ausgangsrechnungen.
        /// </summary>
        /// <param name="pageRequest">Seiteninformationen</param>
        /// <param name="user">Die Rechnungen welches Nutzers sollen abgerufen werden?</param>
        /// <returns></returns>
        IPage<Bill> FindPendingBillsByUser(IPageable pageRequest, User user);

        /// <summary>
        ///     Rechnet eine Rechnung ab.
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="user">Wer ändert die Rechnung.</param>
        void Settle(Bill bill, User user);

        /// <summary>
        /// Sucht nach allen offenen Rechnungen die durch alle Nutzer akzeptiert wurden.
        /// </summary>
        /// <returns></returns>
        IList<Bill> FindSettleableBills();

        /// <summary>
        /// Rechnet alle abrechenbaren Rechnungen ab.
        /// Eine Rechnung kann abgerechnet werden, wenn sie bisher nicht abgerechnet wurde und alle Schuldner zugesagt haben.
        /// </summary>
        /// <returns></returns>
        IList<Bill> SettleAllSettleableBills();
    }
}