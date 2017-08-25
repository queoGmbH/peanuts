namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {
    /// <summary>
    ///     Definiert möglich Status einer Zahlung.
    /// </summary>
    public enum PaymentStatus {
        /// <summary>
        ///     Die Zahlung ist bisher nicht bestätigt.
        /// </summary>
        Pending,

        /// <summary>
        ///     Die Zahlung wurde bestätigt.
        /// </summary>
        Accecpted,

        /// <summary>
        ///     Die Zahlung wurde abgelehnt.
        /// </summary>
        Declined
    }
}