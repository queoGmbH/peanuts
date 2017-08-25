namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts {
    /// <summary>
    /// Auflistung möglicher Status, die eine Teilnahme eines Gruppenmitglieds an einem Peanut haben kann.
    /// </summary>
    public enum PeanutParticipationState {

        /// <summary>
        /// Die Teilnahme ist akzeptiert bzw. bestätigt.
        /// </summary>
        /// <remarks>
        /// Ein Nutzer muss seine Teilnahme unter Umständen bestätigen.
        /// Zum Beispiel wenn es eine gravierende Änderung am Peanut gab.
        /// </remarks>
        Confirmed,
        
        /// <summary>
        /// Der Nutzer hat die Teilnahme angefragt.
        /// </summary>
        Requested,

        /// <summary>
        /// Die Teilnahme wurde abgelehnt.
        /// </summary>
        Refused,

        /// <summary>
        /// Der Nutzer hat seine Teilnahme noch nicht akzeptiert.
        /// Das kann u.a. sein, wenn der Nutzer eingeladen wurde und erst noch zusagen muss
        /// oder wenn es gravierende Änderungen am Peanut gab die auch erst bestätigt werden müssen.
        /// </summary>
        Pending,
    }
}