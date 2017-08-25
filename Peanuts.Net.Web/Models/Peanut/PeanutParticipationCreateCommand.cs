using System.ComponentModel.DataAnnotations;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut {
    /// <summary>
    /// Command zum Erstellen einer Teilnahme an einem Peanut.
    /// </summary>
    public class PeanutParticipationCreateCommand {

        /// <summary>
        /// Ruft ab oder legt fest, wie an dem Peanut teilgenommen werden soll.
        /// </summary>
        [Required]
        public PeanutParticipationType PeanutParticipationType { get; set; }

    }
}