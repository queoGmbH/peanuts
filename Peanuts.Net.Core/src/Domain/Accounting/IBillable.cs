using System;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {
    /// <summary>
    ///     Beschreibt etwas, das abrechenbar ist.
    /// </summary>
    public interface IBillable {
        /// <summary>
        ///     Ruft den Betrag ab, der verrechnet wird.
        /// </summary>
        double Amount { get; }

        /// <summary>
        ///     Ruft das Abrechnungsdatum ab.
        /// </summary>
        DateTime BillingDate { get; }

        /// <summary>
        ///     Ruft den Empfänger der Zahlung (Kreditor) ab.
        /// </summary>
        Account Recipient { get; }

        /// <summary>
        ///     Ruft den Absender der Zahlung (Debitor) ab.
        /// </summary>
        Account Sender { get; }
    }

}