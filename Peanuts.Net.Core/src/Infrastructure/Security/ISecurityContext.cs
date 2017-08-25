using System.Collections.Generic;
using System.Security.Claims;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security {
    /// <summary>
    ///     Schnittstelle für einen Security-Kontext.
    /// </summary>
    public interface ISecurityContext {
        /// <summary>
        ///     Liefert die ClaimsIdentity
        /// </summary>
        ClaimsIdentity ClaimsIdentity { get; }

        /// <summary>
        ///     Liefert den Anzeigenamen.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        ///     Liefert die Email-Adresse
        /// </summary>
        string Email { get; }

        /// <summary>
        ///     Liefert den Nutzernamen
        /// </summary>
        string Username { get; }

        /// <summary>
        ///     Liefert alle Authorities die dem Nutzer zugeordnet sind.
        /// </summary>
        /// <returns></returns>
        IList<IGrantedAuthority> GetAuthorities();
    }
}