using System.Collections.Generic;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut {
    /// <summary>
    /// Command zum Einladen mehrere Nutzer zur Teilnahme an einem Peanut.
    /// </summary>
    public class PeanutInvitationsCreateCommand {

        /// <summary>
        /// Ruft die Liste mit einzuladenden Nutzern ab oder legt diese fest.
        /// </summary>
        public IDictionary<string, PeanutInvitationCreateCommand> Invitations { get; set; }

    }
}