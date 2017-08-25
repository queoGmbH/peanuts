using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security {
    /// <summary>
    /// Konkrete Implementierung für eine <see cref="ISecurityExpressionRoot"/> im Web-Context.
    /// </summary>
    /// <remarks>
    /// Der Einfachheit halber wird direkt versucht auf den Principal zuzugreifen und die Identity zu bestimmen.
    /// Auch der SecurityContext wird direkt erzeugt. Das könnte man auch noch delegieren.
    /// </remarks>
    public class WebSecurityExpressionRootFactory:ISecurityExpressionRootFactory {
        
        public ISecurityExpressionRoot CreateSecurityExpressionRoot() {
            SecurityContext securityContext = GetSecurityContext();
            SecurityExpressionRoot securityExpressionRoot = new SecurityExpressionRoot(securityContext);
            return securityExpressionRoot;
        }

        /// <summary>
        /// Der Teil könnte eigentlich in den SecurityContextHolder ausgelagert werden.
        /// </summary>
        /// <returns></returns>
        private static SecurityContext GetSecurityContext() {
            // Das holen vom Principal oder der ClaimsIdentity kann in einen entsprechenden Holder ausgelagert werden.
            IPrincipal currentPrincipal = Thread.CurrentPrincipal;
            ClaimsIdentity identity = currentPrincipal.Identity as ClaimsIdentity;
            if (identity == null) {
                // Erst einmal eine Exception. Besser wäre ein EmptySecurityContext (ohne Berechtigungen!).
                throw new InvalidOperationException("Kann keine Identity bestimmen.");
            }
            SecurityContext securityContext = new SecurityContext(identity);
            return securityContext;
        }
    }
}