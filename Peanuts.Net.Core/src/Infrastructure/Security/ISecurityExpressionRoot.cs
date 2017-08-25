namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security {
    public interface ISecurityExpressionRoot {
        /// <summary>
        ///     Überprüft, ob dem Nutzer mindestens eine der Rollen zugewiesen ist.
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        bool HasAnyRole(params string[] roles);

        /// <summary>
        ///     Überprüft, ob der Nutzer das entsprechende Recht besitzt.
        /// </summary>
        /// <param name="authority"></param>
        /// <returns></returns>
        bool HasAuthority(IGrantedAuthority authority);

        /// <summary>
        ///     Überprüft, ob einem Nutzer die Rolle zugewiesen ist.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        bool HasRole(string role);

        /// <summary>
        ///     Ruft ab, ob der aktuelle Nutzer authentifiziert ist.
        /// </summary>
        /// <returns></returns>
        bool IsAuthenticated();
    }
}