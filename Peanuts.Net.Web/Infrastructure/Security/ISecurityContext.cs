using System.Security.Principal;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security {
    /// <summary>
    ///     Die Schnittstelle für den SecurityContext welcher Methoden zum Prüfen der Berechtigung bereitstellt.
    /// </summary>
    public interface ISecurityContextOld {

        /// <summary>
        ///     Ruft ab, ob aktuell ein Nutzer simuliert wird.
        /// </summary>
        bool IsImpersonation { get; }

        /// <summary>
        /// Ruft ab, ob eine Impersonifizierung des Nutzers möglich ist.
        /// </summary>
        bool CanImpersonateUser(User user);


        IIdentity CurrentIdentity { get; }

        /// <summary>
        ///     Deaktiviert die Simulation der Nutzung der Anwendung im Kontext eines anderen Nutzers und führt die Anwendung
        ///     wieder im Kontext des angemeldeten Nutzers aus.
        /// </summary>
        void DisableImpersonation();

        /// <summary>
        ///     Aktiviert die Simulation der Nutzung der Anwendung im Kontext eines anderen Nutzers.
        /// </summary>
        /// <param name="user"></param>
        void EnableImpersonation(User user);

        /// <summary>
        ///     Überprüft, ob dem Nutzer mindestens eine der Rollen zugewiesen ist.
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        bool HasAnyRole(string[] roles);

        /// <summary>
        ///     Überprüft, ob einem Nutzer die Rolle zugewiesen ist.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        bool HasRole(string role);

        /// <summary>
        /// Überprüft, ob der übergebene Domain-Nutzer der aktuell am System angemeldete Nutzer ist.
        /// 
        /// TODO: Was wenn Impersonifizierung aktiv ist?
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool IsCurrentUser(User user);
    }
}