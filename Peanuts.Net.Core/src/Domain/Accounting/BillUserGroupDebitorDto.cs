using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {
    [DtoFor(typeof(BillUserGroupDebitor))]
    public class BillUserGroupDebitorDto {
        public BillUserGroupDebitorDto() {
            Portion = 1;
        }

        public BillUserGroupDebitorDto(UserGroupMembership userGroupMembership, double portion) {
            Require.NotNull(userGroupMembership, "userGroupMembership");

            UserGroupMembership = userGroupMembership;
            Portion = portion;
        }

        /// <summary>
        ///     Ruft den Anteil am Gesamtbetrag der Rechnung ab, den dieser Debitor hat oder legt diesen fest.
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
        public double Portion { get; set; }

        /// <summary>
        ///     Ruft das Gruppenkonto des Debitors ab.
        /// </summary>
        public UserGroupMembership UserGroupMembership { get; set; }

    }
}