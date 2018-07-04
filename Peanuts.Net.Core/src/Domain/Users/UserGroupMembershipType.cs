namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Users {
    public enum UserGroupMembershipType {
        /// <summary>
        ///     Der Nutzer ist Admin der Gruppe.
        /// </summary>
        Administrator,

        /// <summary>
        ///     Der Nutzer ist normaler Nutzer der Gruppe.
        /// </summary>
        Member,

        /// <summary>
        /// Das Mitglied der Gruppe ist inaktiv. Das bedeutet, dass er zwar noch in der Gruppe ist, aber nicht aktiver Teilnehmer an den Aktivitäten der Gruppe.
        /// </summary>
        Inactive,

        /// <summary>
        /// Das Mitglied ist Gast in der Gruppe. 
        /// Es ist also nicht wirklich aktiv, kann aber trotzdem an den Aktivitäten der Gruppe teilnehmen.
        /// </summary>
        Guest,

        /// <summary>
        ///     Der Nutzer hat die Gruppe verlassen.
        /// </summary>
        Quit,

        /// <summary>
        ///     Der Nutzer hat die Mitgliedschaft beantragt.
        /// </summary>
        Request,

        /// <summary>
        ///     Der Nutzer wurde eingeladen in der Gruppe Mitglied zu werden.
        /// </summary>
        Invited
    }
}