using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security;

using Common.Logging;

using Microsoft.AspNet.Identity;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security {
    /// <summary>
    ///     Der SecurityContext für die Anwendung bzw. einen Request.
    ///     Der Context ist immer im Bereich eines <see cref="CurrentClaimsIdentity">Nutzers</see>.
    /// </summary>
    public class SecurityContextOld : ISecurityContextOld {
        private readonly Guid _contextId = Guid.NewGuid();

        /// <summary>
        ///     Identity, welche alle Rollen des angemeldeten Nutzers enthält.
        ///     Die Identity enthält NICHT die Rechte.
        /// </summary>
        private readonly ClaimsIdentity _currentClaimsIdentity;

        /// <summary>
        ///     Rollen und Rechte des angemeldeten Nutzers.
        /// </summary>
        private readonly IList<IGrantedAuthority> _grantedAuthorities;

        private readonly List<string> _roles;

        private readonly AuthorizationManager _authorizationManager = new AuthorizationManager();

        /*Der original SecurityContext, von welchem aus die Impersonifizierung gestartet wurde.*/
        private SecurityContextOld _impersonationSecurityContext;

        public SecurityContextOld(ClaimsIdentity currentClaimsIdentity, List<string> roles) {
            Require.NotNull(currentClaimsIdentity, "currentClaimsIdentity");
            Require.NotNull(roles, "roles");

            _grantedAuthorities = _authorizationManager.GetAuthorities(roles);
            _currentClaimsIdentity = currentClaimsIdentity;
            _roles = roles;
        }

        public SecurityContextOld(ClaimsIdentity impersonatedClaimsIdentity, List<string> roles, SecurityContextOld impersonationContext)
            : this(impersonatedClaimsIdentity, roles) {
            Require.NotNull(impersonationContext, "impersonationContext");

            _impersonationSecurityContext = impersonationContext;
        }

        /// <summary>
        ///     Ruft den aktuellen SecurityContext ab.
        /// </summary>
        public static ISecurityContextOld Current {
            get {
                if (!IsAttachedToSession()) {
                    throw new SecurityException("Es existiert kein SecurityContext");
                }
                return GetFromSession();
            }
        }

        /// <summary>
        ///     Ruft die Id des Kontextes ab.
        /// </summary>
        public Guid ContextId {
            get { return _contextId; }
        }

        /// <summary>
        ///     Ruft den aktuellen Nutzer ab.
        ///     Ist kein Nutzer authentifiziert oder wird kein zugehöriger Nutzer gefunden dann null.
        ///     Im Falle von Impersonifizierung, wird hier der simulierte Nutzer geliefert.
        /// </summary>
        public ClaimsIdentity CurrentClaimsIdentity {
            get { return _currentClaimsIdentity; }
        }

        public IIdentity CurrentIdentity {
            get { return CurrentClaimsIdentity; }
        }

        /// <summary>
        ///     Ruft ab, ob aktuell ein Nutzer simuliert wird.
        /// </summary>
        public bool IsImpersonation {
            get { return _impersonationSecurityContext != null; }
        }

        /// <summary>
        ///     Hinterlegt den SessionContext an der aktuellen Session.
        /// </summary>
        /// <param name="securityContext"></param>
        public static void AttachToSession(SecurityContextOld securityContext) {
            HttpContext.Current.Session["SecurityContext"] = securityContext;
        }

        /// <summary>
        ///     Entfernt den an der Session hinterlegten SecurityContext.
        /// </summary>
        public static void DetachFromSession() {
            HttpContext.Current.Session["SecurityContext"] = null;
        }

        /// <summary>
        ///     Ruft den SecurityContext von der Session ab.
        ///     Ist kein SecurityContext an der Session hinterlegt, wird null geliefert.
        /// </summary>
        /// <returns></returns>
        public static SecurityContextOld GetFromSession() {
            return HttpContext.Current.Session["SecurityContext"] as SecurityContextOld;
        }

        /// <summary>
        ///     Ruft ab, ob der SecurityContext an der Session hinterlegt ist.
        /// </summary>
        /// <returns></returns>
        public static bool IsAttachedToSession() {
            return HttpContext.Current.Session["SecurityContext"] is SecurityContextOld;
        }

        /// <summary>
        ///     Ruft ab, ob eine Impersonifizierung eines Nutzers möglich ist.
        /// </summary>
        public bool CanImpersonateUser(User user) {
            if (IsImpersonation) {
                /*Wenn bereits eine Impersonifizierung aktiv ist, kann keine andere gemacht werden.*/
                return false;
            }

            if (user.HasRole(Roles.Administrator)) {
                return false;
            }

            if (IsCurrentUser(user)) {
                return false;
            }

            /*Nur Administratoren dürfen impersonifizieren.*/
            return HasRole(Roles.Administrator);
        }

        public bool HasAnyRole(string[] roles) {
            return false;
        }

        bool ISecurityContextOld.HasRole(string role) {
            return HasRole(role);
        }

        private bool HasRole(string administrator) {
            return false;
        }

        /// <summary>
        ///     Deaktiviert die Simulation der Nutzung der Anwendung im Kontext eines anderen Nutzers und führt die Anwendung
        ///     wieder im Kontext des eigentlich angemeldeten Nutzers aus.
        /// </summary>
        public void DisableImpersonation() {
            LogManager.GetLogger<SecurityContextOld>().InfoFormat("Impersonifizierung wird deaktiviert: Nutzer {0} hat die Simulation beendet", _currentClaimsIdentity.Name);

            AttachToSession(_impersonationSecurityContext);

            LogManager.GetLogger<SecurityContextOld>().InfoFormat("Impersonifizierung wurde deaktiviert");
        }

        /// <summary>
        ///     Aktiviert die Simulation der Nutzung der Anwendung im Kontext eines anderen Nutzers.
        /// </summary>
        /// <param name="user"></param>
        public void EnableImpersonation(User user) {
            Require.NotNull(user, "user");

            if (!CanImpersonateUser(user)) {
                LogManager.GetLogger<SecurityContextOld>().ErrorFormat("Unerlaubter Impersonifizierungs-Versuch: Nutzer {0} wollte {1} simulieren", _currentClaimsIdentity.Name, user);
                throw new SecurityException("Der Nutzer darf nicht impersonifiziert werden.");
            }

            LogManager.GetLogger<SecurityContextOld>().InfoFormat("Impersonifizierung aktiviert: Nutzer {0} simuliert {1}", _currentClaimsIdentity.Name, user);

            
            /*TODO: Das gibt es an anderer Stelle schon mal. Sollte nur einmal existieren.*/
            SecurityUser impersonatedSecurityUser = new SecurityUser(user.BusinessId.ToString(), user.UserName, user.Email, user.PasswordHash, user.IsEnabled, user.Roles.ToList(), user.FirstName, user.LastName);
            /*zu simulierende ClaimsIdentity erstellen*/
            ClaimsIdentity impersonatedClaimsIdentity = new ClaimsIdentity();
            /*Rollen als Claims hinzufügen*/
            foreach (string role in _roles) {
                impersonatedClaimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            impersonatedClaimsIdentity.AddClaim(new Claim(ClaimTypes.Name, impersonatedSecurityUser.UserName));
            impersonatedClaimsIdentity.AddClaim(new Claim(ClaimTypes.Email, impersonatedSecurityUser.Email));
            impersonatedClaimsIdentity.AddClaim(new Claim(ClaimTypes.PrimarySid, impersonatedSecurityUser.Id));
            impersonatedClaimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, impersonatedSecurityUser.Id));

            /* Impersonifizierten Context mit Backup auf diesen Context erstellen*/
            SecurityContextOld impersonationContext = new SecurityContextOld(impersonatedClaimsIdentity, impersonatedSecurityUser.Roles.ToList(), this);

            /* Impersonifizierten Context aktivieren */
            AttachToSession(impersonationContext);
        }
        
        /// <summary>
        ///     Liefert einen Wert, der angibt, ob der Nutzer eine bestimmte Rolle (ein bestimmtes Recht) besitzt.
        /// </summary>
        /// <param name="expectedAuthority"></param>
        /// <returns></returns>
        public bool HasAuthority(IGrantedAuthority expectedAuthority) {
            return _grantedAuthorities.Contains(expectedAuthority);
        }

        /// <summary>
        ///     Überprüft, ob der übergebene Domain-Nutzer der aktuell am System angemeldete Nutzer ist.
        ///     TODO: Was wenn Impersonifizierung aktiv ist?
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsCurrentUser(User user) {
            if (user == null) {
                return false;
            }

            if (CurrentIdentity == null) {
                return false;
            }

            return user.BusinessId.ToString() == CurrentIdentity.GetUserId<string>();
        }
    }
}