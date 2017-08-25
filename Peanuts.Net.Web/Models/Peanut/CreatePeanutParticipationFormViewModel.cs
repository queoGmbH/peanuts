using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut {

    /// <summary>
    /// ViewModel für das Formular zum Erstellen einer Teilnahme an einem Peanut.
    /// </summary>
    public class PeanutParticipationCreateFormViewModel {

        public PeanutParticipationCreateFormViewModel(Core.Domain.Peanuts.Peanut peanut, IList<PeanutParticipationType> peanutParticipationTypes, PeanutParticipationCreateCommand peanutParticipationCreateCommand) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(peanutParticipationTypes, "peanutParticipationTypes");
            Require.NotNull(peanutParticipationCreateCommand, "peanutParticipationCreateCommand");

            Peanut = peanut;
            PeanutParticipationTypes = peanutParticipationTypes;
            PeanutParticipationCreateCommand = peanutParticipationCreateCommand;
        }

        public PeanutParticipationCreateFormViewModel(Core.Domain.Peanuts.Peanut peanut, IList<PeanutParticipationType> peanutParticipationTypes) : this(peanut, peanutParticipationTypes, new PeanutParticipationCreateCommand()) {
            
        }

        /// <summary>
        /// Ruft den Peanut ab, für den die Teilnahme erstellt werden soll.
        /// </summary>
        public Core.Domain.Peanuts.Peanut Peanut { get; private set; }

        /// <summary>
        /// Ruft die möglichen Arten ab, wie an dem Peanut teilgenommen werden kann.
        /// </summary>
        public IList<PeanutParticipationType> PeanutParticipationTypes { get; private set; }

        /// <summary>
        /// Ruft das Command zum Erstellen der Teilnahme ab.
        /// </summary>
        public PeanutParticipationCreateCommand PeanutParticipationCreateCommand { get; private set; }
    }
}