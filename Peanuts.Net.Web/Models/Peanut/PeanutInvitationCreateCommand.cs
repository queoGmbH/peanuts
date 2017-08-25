using System.ComponentModel.DataAnnotations;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut {
    /// <summary>
    /// Command zum Einladen eines Nutzer zur Teilnahme an einem Peanut.
    /// </summary>
    public class PeanutInvitationCreateCommand {

        /// <summary>
        /// Ruft den Nutzer ab, der zum Peanut eingeladen werden soll oder legt diesen fest.
        /// Wenn dies nicht gesetzt ist werden alle verbleibenden Nutzer der Gruppe eingeladen.
        /// </summary>
        public UserGroupMembership UserGroupMembership { get; set; }

        /// <summary>
        /// Ruft die Art und Weise ab, wie der Teilnehmer am Peanut teilnehmen soll oder legt diese fest.
        /// </summary>
        [Required]
        public PeanutParticipationType PeanutParticipationType { get; set; }

    }
}