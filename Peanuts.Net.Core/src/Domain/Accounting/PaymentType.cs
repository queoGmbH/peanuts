namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {
    /// <summary>
    ///     Definiert Arten, wie eine Zahlung erfolgen kann.
    /// </summary>
    public enum PaymentType {
        /// <summary>
        ///     Die Zahlung erfolgt(e) in bar.
        /// </summary>
        Cash,

        /// <summary>
        ///     Die Zahlung wird/wurde per PayPal durchgeführt.
        /// </summary>
        PayPal
    }
}