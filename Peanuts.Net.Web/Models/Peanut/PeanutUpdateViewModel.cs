using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut {
    /// <summary>
    /// ViewModel für das Formular zur Erstellung eines Peanuts.
    /// </summary>
    public class PeanutUpdateViewModel {

        public PeanutUpdateViewModel(Core.Domain.Peanuts.Peanut peanut, IList<PeanutParticipationType> participationTypes) : this(peanut, new PeanutUpdateCommand(peanut), participationTypes) {
        }

        public PeanutUpdateViewModel(Core.Domain.Peanuts.Peanut peanut, PeanutUpdateCommand peanutUpdateCommand, IList<PeanutParticipationType> participationTypes) {
            Require.NotNull(peanutUpdateCommand, "peanutCreateCommand");
            Require.NotNull(participationTypes, "participationTypes");
            Require.NotNull(peanut, "peanut");

            Peanut = peanut;
            PeanutUpdateCommand = peanutUpdateCommand;
            ParticipationTypes = participationTypes;

        }
        
        /// <summary>
        /// Ruft das Command zum Ändern des Peanuts ab.
        /// </summary>
        public PeanutUpdateCommand PeanutUpdateCommand {
            get; private set;
        }

        /// <summary>
        /// Ruft die verfügbaren Teilnahmearten ab.
        /// </summary>
        public IList<PeanutParticipationType> ParticipationTypes {
            get; private set;
        }

        public Core.Domain.Peanuts.Peanut Peanut { get; private set; }
    }
}