using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {
    /// <summary>
    ///     Bildet eine Rechnung ab.
    /// </summary>
    /// <remarks>
    ///     Eine Rechnung wird von einem Kreditor an einen Debitor gestellt.
    ///     Wenn alle die Rechnung akzeptiert haben, wird die Rechnung gebucht, und die Beträge auf die Konten der jeweiligen
    ///     Nutzer gebucht.
    /// </remarks>
    public class Bill : Entity {
        private readonly UserGroupMembership _creditor;
        private readonly IList<BillUserGroupDebitor> _userGroupDebitors = new List<BillUserGroupDebitor>();
        private readonly IList<BillGuestDebitor> _guestDebitors = new List<BillGuestDebitor>();

        private readonly UserGroup _userGroup;
        private double _amount;

        private DateTime? _changedAt;
        private User _changedBy;
        private readonly DateTime _createdAt;
        private readonly User _createdBy;

        private DateTime? _settlementDate;
        private bool _isSettled;
        private string _subject;

        public Bill() {
        }

        public Bill(UserGroup userGroup, BillDto billDto, UserGroupMembership creditor, IList<BillUserGroupDebitor> debitors, IList<BillGuestDebitor> guestDebitors, EntityCreatedDto createdDto) {
            Require.NotNull(createdDto, "createdDto");
            Require.NotNull(userGroup, "userGroup");
            Require.NotNull(billDto, "billDto");
            Require.NotNull(creditor, "creditor");
            Require.NotNull(debitors, "debitors");
            Require.NotNull(guestDebitors, "guestDebitors");

            _userGroup = userGroup;
            _createdBy = createdDto.CreatedBy;
            _createdAt = createdDto.CreatedAt;
            _creditor = creditor;

            Update(billDto);

            _userGroupDebitors = debitors;
            _guestDebitors = guestDebitors;
        }

        private void Update(BillDto billDto) {
            Require.NotNull(billDto, "billDto");

            _subject = billDto.Subject;
            _amount = billDto.Amount;
        }

        /// <summary>
        ///     Ruft den Rechnungsbetrag ab.
        /// </summary>
        public virtual double Amount {
            get { return _amount; }
        }

        /// <summary>
        ///     Ruft ab, wann die Rechnung zuletzt geändert wurde.
        /// </summary>
        public virtual DateTime? ChangedAt {
            get { return _changedAt; }
        }

        /// <summary>
        /// Ruft den Gläubiger der Rechnung ab.
        /// </summary>
        public virtual UserGroupMembership Creditor {
            get { return _creditor; }
        }

        /// <summary>
        /// Ruft die Gesamtzahl der Schuldner ab.
        /// </summary>
        public virtual int DebitorCount {
            get {
                return _userGroupDebitors.Count + _guestDebitors.Count;
            }
        }

        /// <summary>
        /// Ruft ab, ob die Rechnung gebucht/abgerechnet wurde.
        /// Nachdem die Rechnung abgerechnet wurde, kann sie nicht mehr geändert werden.
        /// </summary>
        public virtual bool IsSettled {
            get { return _isSettled; }
        }

        /// <summary>
        ///     Ruft ab, wann die Rechnung geändert wurde.
        /// </summary>
        public virtual User ChangedBy {
            get { return _changedBy; }
        }

        /// <summary>
        ///     Ruft ab, wann die Rechnung erstellt wurde.
        /// </summary>
        public virtual DateTime CreatedAt {
            get { return _createdAt; }
        }

        /// <summary>
        ///     Ruft ab, wer die Rechnung erstellt hat.
        /// </summary>
        public virtual User CreatedBy {
            get { return _createdBy; }
        }

        /// <summary>
        ///     Ruft die Liste der Gruppenmitglieder ab, an welche die Rechnung gestellt wird.
        /// </summary>
        public virtual IList<BillUserGroupDebitor> UserGroupDebitors {
            get { return new ReadOnlyCollection<BillUserGroupDebitor>(_userGroupDebitors); }
        }

        /// <summary>
        ///     Ruft die Liste der Gäste ab, an welche die Rechnung gestellt wird.
        /// </summary>
        public virtual IList<BillGuestDebitor> GuestDebitors {
            get { return new ReadOnlyCollection<BillGuestDebitor>(_guestDebitors); }
        }

        /// <summary>
        ///     Ruft das Datum ab, an dem die Rechnung gebucht wurde.
        /// </summary>
        /// <remarks>
        ///     Wird eine Rechnung gebucht, dann werden die Konten der Debitoren belastet und der Betrag dem Kreditor-Konto
        ///     gutgeschrieben.
        /// </remarks>
        public virtual DateTime? SettlementDate {
            get { return _settlementDate; }
        }

        /// <summary>
        ///     Ruft den Betreff der Rechnung ab.
        /// </summary>
        public virtual string Subject {
            get { return _subject; }
        }

        /// <summary>
        ///     Ruft die Gruppe ab, für welche die Rechnung gestellt wurde.
        /// </summary>
        public virtual UserGroup UserGroup {
            get { return _userGroup; }
        }

        public virtual string DisplayName {
            get {
                return string.Format("{0}: {1:C}", _subject, _amount);
            }
        }

        /// <summary>
        /// Ruft ab, ob mindestens einer der Debitoren die Rechnung abgelehnt hat.
        /// </summary>
        public virtual bool HasAnyoneRefused {
            get { return _userGroupDebitors.Any(deb => deb.BillAcceptState == BillAcceptState.Refused); }
        }

        /// <summary>
        /// Ruft ab, ob alle Debitoren die Rechnung bestätigt haben.
        /// </summary>
        public virtual bool HasEveryoneAccepted {
            get {
                return _userGroupDebitors.All(deb => deb.BillAcceptState == BillAcceptState.Accepted);
            }
        }

        /// <summary>
        /// Aktualisiert die Daten der Rechnung.
        /// </summary>
        /// <param name="billDto"></param>
        /// <param name="entityChanged"></param>
        public virtual void Update(BillDto billDto, EntityChangedDto entityChanged) {

            Update(billDto);
            Update(entityChanged);
        }

        private void Update(EntityChangedDto entityChanged) {
            Require.NotNull(entityChanged, "entityChanged");

            _changedBy = entityChanged.ChangedBy;
            _changedAt = entityChanged.ChangedAt;
        }

        /// <summary>
        /// Liefert den ungerundeten Anteil am Gesamtbetrag der Rechnung.
        /// </summary>
        /// <param name="portion"></param>
        /// <returns></returns>
        public virtual double GetPartialAmountByPortion(double portion) {

            double portionSum = SumAllPortions();
            double portionsPortion = portion / portionSum;

            return _amount * portionsPortion;
        }

        private double SumAllPortions() {
            return _userGroupDebitors.Sum(deb => deb.Portion) + _guestDebitors.Sum(deb => deb.Portion);
        }

        /// <summary>
        /// Ruft den Antwortstatus eines Schuldners ab.
        /// Ist der Nutzer kein Schuldner, wird null geliefert.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual BillAcceptState? GetDebitorState(User user) {
            var debitor = _userGroupDebitors.SingleOrDefault(debMem => debMem.UserGroupMembership.User.Equals(user));
            if (debitor == null) {
                return null;
            } else {
                return debitor.BillAcceptState;
            }


        }

        /// <summary>
        /// Akzeptiert die Zahlung der Rechnung durch einen Debitor.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="entityChangedDto"></param>
        public virtual void AcceptByDebitor(User user, EntityChangedDto entityChangedDto) {
            Require.NotNull(user, "user");
            Require.NotNull(entityChangedDto, "entityChangedDto");

            BillUserGroupDebitor debitor = _userGroupDebitors.SingleOrDefault(debMem => debMem.UserGroupMembership.User.Equals(user));
            if (debitor != null) {
                debitor.Accept();
                Update(entityChangedDto);
            }
        }

        /// <summary>
        /// Lehnt die Zahlung der Rechnung durch einen Debitor mit Begründung ab.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="refuseComment"></param>
        /// <param name="entityChangedDto"></param>
        public virtual BillUserGroupDebitor RefuseByDebitor(User user, string refuseComment, EntityChangedDto entityChangedDto) {
            Require.NotNull(user, "user");
            Require.NotNull(entityChangedDto, "entityChangedDto");


            BillUserGroupDebitor debitor = _userGroupDebitors.SingleOrDefault(debMem => debMem.UserGroupMembership.User.Equals(user));
            if (debitor != null) {
                debitor.Refuse(refuseComment);
                Update(entityChangedDto);
            }

            return debitor;
        }

        /// <summary>
        /// Markiert eine Rechnung als abgerechnet.
        /// </summary>
        /// <param name="entityChangedDto"></param>
        public virtual void Settle(EntityChangedDto entityChangedDto) {
            Require.NotNull(entityChangedDto, "entityChangedDto");

            _isSettled = true;
            _settlementDate = entityChangedDto.ChangedAt;

            Update(entityChangedDto);
        }
    }
}