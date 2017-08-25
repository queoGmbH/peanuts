using System.Collections.Generic;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Users {
    /// <summary>
    ///     Rollen die einem Nutzer gegeben werden können.
    /// </summary>
    public static class Roles {
        public static IList<string> AllRoles {
            get {
                return new List<string>() {
                    Member,
                    Administrator
                };
            }
        }

        // ReSharper disable InconsistentNaming

        /// <summary>
        ///     Rolle für einen registrierten, aber noch nicht freigegebenen Nutzer.
        /// </summary>
        public const string Member = "Member";

        /// <summary>
        ///     Administrator der Anwendung
        /// </summary>
        public const string Administrator = "Administrator";

        // ReSharper restore InconsistentNaming
    }
}