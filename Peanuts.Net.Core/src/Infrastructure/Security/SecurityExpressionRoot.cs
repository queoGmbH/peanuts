namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security {
    /// <summary>
    ///     Basisimplmentierung für die Auswertung von Berechtigungen.
    /// </summary>
    public class SecurityExpressionRoot : ISecurityExpressionRoot {
        private readonly SecurityContext _securityContext;

        public SecurityExpressionRoot(SecurityContext securityContext) {
            _securityContext = securityContext;
        }

        /// <summary>
        ///     Überprüft, ob dem Nutzer mindestens eine der Rollen zugewiesen ist.
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public bool HasAnyRole(string[] roles) {
            foreach (string role in roles) {
                if (HasRole(role)) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Überprüft, ob der Nutzer das entsprechende Recht besitzt.
        /// </summary>
        /// <param name="authority"></param>
        /// <returns></returns>
        public bool HasAuthority(IGrantedAuthority authority) {
            return _securityContext.GetAuthorities().Contains(authority);
        }

        /// <summary>
        ///     Liefert einen Wert, der angibt, ob der Nutzer eine bestimmte Rolle (ein bestimmtes Recht) besitzt.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool HasRole(string role) {
            SimpleGrantedAuthority authority = new SimpleGrantedAuthority(role);

            return HasAuthority(authority);
        }

        /// <summary>
        ///     Ruft ab, ob der aktuelle Nutzer authentifiziert ist.
        /// </summary>
        /// <returns></returns>
        public bool IsAuthenticated() {
            return _securityContext.ClaimsIdentity != null && _securityContext.ClaimsIdentity.IsAuthenticated;
        }
    }
}