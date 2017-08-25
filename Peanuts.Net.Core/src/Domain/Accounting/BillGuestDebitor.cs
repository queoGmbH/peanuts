using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {
    /// <summary>
    ///     Bildet einen Debitor einer Rechnung ab, der nicht in der Gruppe enthalten ist (Gast).
    /// </summary>
    public class BillGuestDebitor : Entity, IBillCreditor {
        private readonly string _email;
        private readonly string _name;
        private readonly double _portion;

        public BillGuestDebitor() {
        }

        public BillGuestDebitor(string name, string email, double portion) {
            Require.NotNullOrWhiteSpace(name, "name");
            Require.NotNullOrWhiteSpace(email, "email");
            Require.Gt(portion, 0, "portion");

            _email = email;
            _name = name;
            _portion = portion;
        }

        public BillGuestDebitor(BillGuestDebitorDto billGuestDebitorDto) {
            Require.NotNull(billGuestDebitorDto, "billGuestDebitorDto");

            _email = billGuestDebitorDto.Email;
            _name = billGuestDebitorDto.Name;
            _portion = billGuestDebitorDto.Portion;
        }
        
        public virtual string DisplayName {
            get { return string.Format("{0} ({1})", _name, _email); }
        }

        /// <summary>
        ///     Ruft die E-Mail des Gastes/Debitors ab.
        /// </summary>
        public virtual string Email {
            get { return _email; }
        }

        /// <summary>
        ///     Ruft den Namen des Gastes/Debitors ab.
        /// </summary>
        public virtual string Name {
            get { return _name; }
        }

        /// <summary>
        ///     Ruft den Anteil am Gesamtbetrag der Rechnung ab, den dieser Kreditor hat.
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
    }
}