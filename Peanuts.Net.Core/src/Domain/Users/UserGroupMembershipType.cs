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