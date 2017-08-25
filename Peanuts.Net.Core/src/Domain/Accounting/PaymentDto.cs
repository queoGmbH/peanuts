using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {

    /// <summary>
    /// Diese Klasse stellt ein Dto für eine Zahlung dar.
    /// </summary>
    [DtoFor(typeof(Payment))]
    public class PaymentDto {

        /// <summary>
        /// Der Betrag
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Die Art der Zahlung
        /// </summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// Der Buchungstext der Zahlung
        /// </summary>
        public string Text { get; set; }
    }
}