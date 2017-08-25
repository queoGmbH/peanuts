using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {
    /// <summary>
    ///     Bildet einen Debitor einer Rechnung ab, der in der Gruppe, in welcher die Rechnung gestellt wurde Mitglied ist.
    /// </summary>
    public class BillUserGroupDebitor : Entity, IBillCreditor {
        private BillAcceptState _billAcceptState;
        private readonly double _portion;
        private readonly UserGroupMembership _userGroupMembership;
        private string _refuseComment;

        public BillUserGroupDebitor() {
        }

        public BillUserGroupDebitor(UserGroupMembership userGroupMembership, double portion,
            BillAcceptState billAcceptState = BillAcceptState.Pending) {
            Require.NotNull(userGroupMembership, "userGroupMembership");
            Require.Gt(portion, 0, "portion");

            _userGroupMembership = userGroupMembership;
            _portion = portion;
            _billAcceptState = billAcceptState;
        }

        /// <summary>
        ///     Ruft ab, ob der Debitor die Zahlung des Rechnungsbetrages (bzw. seines Anteils am Rechnungsbetrag) akzeptiert oder
        ///     abgelehnt hat.
        /// </summary>
        public virtual BillAcceptState BillAcceptState {
            get { return _billAcceptState; }
        }

        public virtual string DisplayName {
            get { return _userGroupMembership.User.DisplayName; }
        }

        /// <summary>
        ///     Ruft den Anteil am Gesamtbetrag der Rechnung ab, den dieser Debitor hat.
        /// </summary>
        /// <remarks>
        ///     Es handelt sich hier nicht um den tatsächlichen Betrag, den der Debitor schuldet, sondern nur um seinen Anteil am
        ///     Gesamtbetrag der Rechnung.
        ///     Der Betrag ein einzelner Debitor(Schuldner) zu zahlen hat, ergibt sich aus dem Anteil den der Debitor an der Summe
        ///     aller Anteile aller Debitoren hat.
        ///     Bsp.:
        ///     Rechnungsbetrag: 20, 00 €
        ///     Debitor 1 => Anteil: 1 => 4,00€ (1/5)
        ///     Debitor 2 => Anteil: 2 => 8,00€ (2/5)
        ///     Debitor 3 => Anteil: 2 => 8,00€ (2/5)
        ///     ===============================
        ///     Summe aller Anteile: 5
        /// </remarks>
        public virtual double Portion {
            get { return _portion; }
        }

        /// <summary>
        /// Ruft die Begründung der Ablehnung ab.
        /// </summary>
        public virtual string RefuseComment {
            get { return _refuseComment; }
        }

        /// <summary>
        ///     Ruft das Gruppenkonto des Debitors ab.
        /// </summary>
        public virtual UserGroupMembership UserGroupMembership {
            get { return _userGroupMembership; }
        }

        /// <summary>
        /// Markiert die Rechnung als vom Schuldner akzeptiert.
        /// </summary>
        public virtual void Accept() {
            _billAcceptState = BillAcceptState.Accepted;
            _refuseComment = null;
        }

        /// <summary>
        /// Markiert die Rechnung als vom Schuldner abgelehnt.
        /// </summary>
        /// <param name="refuseComment"></param>
        public virtual void Refuse(string refuseComment) {
            _billAcceptState = BillAcceptState.Refused;
            _refuseComment = refuseComment;
        }
    }
}