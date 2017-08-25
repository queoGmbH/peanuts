namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Transactions {
    public enum BookingEntryType {
        /// <summary>
        ///     Haben => Das Geld wird gutgeschrieben
        /// </summary>
        In,

        /// <summary>
        ///     Soll => Das Geld wird abgezogen
        /// </summary>
        Out
    }
}