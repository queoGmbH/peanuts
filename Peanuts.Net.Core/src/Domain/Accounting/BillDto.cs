using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {
    [DtoFor(typeof(Bill))]
    public class BillDto {
        public BillDto() {
        }

        public BillDto(string subject, double amount) {
            Amount = amount;
            Subject = subject;
        }

        /// <summary>
        ///     Ruft den Rechnungsbetrag ab oder legt diesen fest.
        /// </summary>
        public virtual double Amount { get; set; }

        /// <summary>
        ///     Ruft den Betreff der Rechnung ab oder legt diesen fest.
        /// </summary>
        public virtual string Subject { get; set; }
    }
}