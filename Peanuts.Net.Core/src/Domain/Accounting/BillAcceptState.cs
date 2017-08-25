namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {
    /// <summary>
    /// Listet mögliche Status für die Akzeptanz einer Rechnung auf.
    /// </summary>
    public enum BillAcceptState {

        /// <summary>
        /// Es gab bisher weder eine Zustimmung noch eine Ablehnung.
        /// </summary>
        Pending,

        /// <summary>
        /// Die Zahlung der Rechnung wurde akzeptiert.
        /// </summary>
        Accepted,

        /// <summary>
        /// Die Zahlung der Rechnung wurde abgelehnt.
        /// </summary>
        Refused

    }
}