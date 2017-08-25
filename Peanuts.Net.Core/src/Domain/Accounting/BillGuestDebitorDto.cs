using System.ComponentModel.DataAnnotations;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {
    [DtoFor(typeof(BillGuestDebitor))]
    public class BillGuestDebitorDto {
        public BillGuestDebitorDto() {
            Portion = 1;
        }

        public BillGuestDebitorDto(string name, string email, double portion) {
            Name = name;
            Email = email;
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
        [Required]
        public double Portion {
            get; set;
        }

        /// <summary>
        ///     Ruft die E-Mail des Gastes/Debitors ab oder legt diesen fest.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        ///     Ruft den Namen des Gastes/Debitors ab oder legt diesen fest.
        /// </summary>
        [Required]
        public string Name {
            get; set;
        }

    }
}