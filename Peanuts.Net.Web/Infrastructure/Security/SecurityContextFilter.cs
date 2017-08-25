using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc.Filters;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security {
    /// <summary>
    ///     Filter, der dazu dient den SecurityContext zu erzeugen und am HttpContext zu hinterlegen.
    ///     Dieser Filter muss als erster in der Pipeline aufgerufen werden, weil sonst der SecurityContext noch nicht erzeugt
    ///     ist.
    /// </summary>
    public class SecurityContextFilter : IAuthenticationFilter {
        /// <summary>Authenticates the request.</summary>
        /// <param name="filterContext">The context to use for authentication.</param>
        public void OnAuthentication(AuthenticationContext filterContext) {
            if (!filterContext.Principal.Identity.IsAuthenticated) {
                /*Kein angemeldeter Nutzer*/

                /*SecurityContext aus der Session entfernen.*/
                SecurityContextOld.DetachFromSession();
                /*SecurityContext aus dem HttpContext entfernen.*/
                filterContext.HttpContext.Items["SecurityContext"] = null;
                return;
            }

            /* Zunächst prüfen, ob der SecurityContext bereits hinterlegt ist.
                         * dann muss der SecurityContext nicht erneut erzeugt werden.
                        */
            if (SecurityContextOld.IsAttachedToSession()) {
                return;
            }

            AuthorizationManager authorizationManager = new AuthorizationManager();
            //IUserService userService = ContextRegistry.GetContext().GetObject<IUserService>();
            //User user = userService.GetByBusinessId(Guid.Parse(filterContext.Principal.Identity.GetUserId()));

            ClaimsIdentity currentClaimsIdentity = filterContext.Principal.Identity as ClaimsIdentity;
            if (currentClaimsIdentity == null || currentClaimsIdentity.IsAuthenticated == false) {
                /*Kein angemeldeter Nutzer*/

                /*SecurityContext aus der Session entfernen.*/
                SecurityContextOld.DetachFromSession();
                /*SecurityContext aus dem HttpContext entfernen.*/
                filterContext.HttpContext.Items["SecurityContext"] = null;

                return;
            }

            List<Claim> claimRoles = currentClaimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            List<string> roles = claimRoles.Select(claimsRole => claimsRole.Value.ToString()).ToList();

            /*Security Context erzeugen*/
            SecurityContextOld securityContext = new SecurityContextOld(currentClaimsIdentity, roles);

            /*SecurityContext an der Session hinterlegen.*/
            SecurityContextOld.AttachToSession(securityContext);
            /*Zusätzlich den SecurityContext am HttpContext hinterlegen.*/
            filterContext.HttpContext.Items["SecurityContext"] = securityContext;
        }

        /// <summary>Adds an authentication challenge to the current <see cref="T:System.Web.Mvc.ActionResult" />.</summary>
        /// <param name="filterContext">The context to use for the authentication challenge.</param>
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext) {
        }
    }
}