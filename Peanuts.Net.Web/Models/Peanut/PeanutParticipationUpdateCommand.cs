using System.ComponentModel.DataAnnotations;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut {
    /// <summary>
    /// Command zum Ändern einer Teilnahme an einem Peanut.
    /// </summary>
    public class PeanutParticipationUpdateCommand {
        public PeanutParticipationUpdateCommand() {
        }

        public PeanutParticipationUpdateCommand(PeanutParticipation participation) {
            Require.NotNull(participation, "participation");

            PeanutParticipationType = participation.ParticipationType;
        }

        /// <summary>
        /// Ruft ab oder legt fest, wie an dem Peanut teilgenommen werden soll.
        /// </summary>
        [Required]
        public PeanutParticipationType PeanutParticipationType {
            get; set;
        }
    }
}