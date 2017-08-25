using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut {
    public class PeanutParticipationUpdateViewModel {

        public PeanutParticipationUpdateViewModel(PeanutParticipation peanutParticipation, IList<PeanutParticipationType> participationTypes, PeanutParticipationUpdateCommand peanutParticipationUpdateCommand) {
            PeanutParticipation = peanutParticipation;
            ParticipationTypes = participationTypes;
            PeanutParticipationUpdateCommand = peanutParticipationUpdateCommand;
        }

        public PeanutParticipation PeanutParticipation { get; private set; }

        public IList<PeanutParticipationType> ParticipationTypes { get; private set; }

        /// <summary>
        /// Ruft das Command zum Ändern des Status ab.
        /// </summary>
        public PeanutParticipationUpdateCommand PeanutParticipationUpdateCommand { get; private set; }

    }
}