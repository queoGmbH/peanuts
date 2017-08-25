using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security {
    /// <summary>
    ///     Der SecurityContext.
    /// </summary>
    public class SecurityContext : ISecurityContext {
        private readonly ClaimsIdentity _claimsIdentity;
        private readonly string _displayName;
        private readonly string _email;
        private readonly string _username;

        /// <summary>
        ///     Erzeugt einen neuen SecurityContext anhand des Claims.
        /// </summary>
        public SecurityContext(ClaimsIdentity claimsIdentity) {
            _claimsIdentity = claimsIdentity;
            _username = GetUsername(claimsIdentity);
            _email = GetEmail(claimsIdentity);
            _displayName = GetDisplayName(claimsIdentity);
        }

        /// <summary>
        ///     Liefert die ClaimsIdentity
        /// </summary>
        public ClaimsIdentity ClaimsIdentity {
            get { return _claimsIdentity; }
        }

        /// <summary>
        ///     Liefert den Anzeigenamen.
        /// </summary>
        public string DisplayName {
            get { return _displayName; }
        }

        /// <summary>
        ///     Liefert die E-Mail Adresse.
        /// </summary>
        public string Email {
            get { return _email; }
        }

        /// <summary>
        ///     Liefert den Nutzernamen.
        /// </summary>
        public string Username {
            get { return _username; }
        }

        /// <summary>
        ///     Liefert alle Authorities die dem Nutzer zugeordnet sind.
        /// </summary>
        /// <returns></returns>
        public IList<IGrantedAuthority> GetAuthorities() {
            IEnumerable<Claim> claims = _claimsIdentity.FindAll(ClaimTypes.Role);
            List<IGrantedAuthority> simpleGrantedAuthorities =
                    claims.Select(claim => (IGrantedAuthority)new SimpleGrantedAuthority(claim.Value)).ToList();
            return simpleGrantedAuthorities;
        }

        private static string GetDisplayName(ClaimsIdentity claimsIdentity) {
            Claim displayNameClaim = claimsIdentity.FindFirst("DisplayName");
            string displayName;
            if (displayNameClaim == null) {
                displayName = "";
            } else {
                displayName = displayNameClaim.Value;
            }
            return displayName;
        }

        private static string GetEmail(ClaimsIdentity claimsIdentity) {
            Claim emailClaim = claimsIdentity.FindFirst(ClaimTypes.Email);
            if (emailClaim != null) {
                return emailClaim.Value;
            } else {
                return string.Empty;
            }
        }

        private static string GetUsername(ClaimsIdentity claimsIdentity) {
            Claim usernameClaim = claimsIdentity.FindFirst(ClaimTypes.Name);
            if (usernameClaim != null) {
                return usernameClaim.Value;
            } else {
                return "Unbekannt";
            }
        }
    }
}