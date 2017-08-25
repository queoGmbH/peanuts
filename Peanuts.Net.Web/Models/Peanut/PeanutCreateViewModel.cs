using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut {

    /// <summary>
    /// ViewModel für das Formular zur Erstellung eines Peanuts.
    /// </summary>
    public class PeanutCreateViewModel {

        public PeanutCreateViewModel(IList<Core.Domain.Users.UserGroup> userGroups, IList<PeanutParticipationType> participationTypes) : this(new PeanutCreateCommand(), userGroups, participationTypes) {
        }

        public PeanutCreateViewModel(PeanutCreateCommand peanutCreateCommand, IList<Core.Domain.Users.UserGroup> userGroups, IList<PeanutParticipationType> participationTypes) {
            Require.NotNull(peanutCreateCommand, "peanutCreateCommand");
            Require.NotNull(userGroups, "userGroups");
            Require.NotNull(participationTypes, "participationTypes");

            PeanutCreateCommand = peanutCreateCommand;
            UserGroups = userGroups;
            ParticipationTypes = participationTypes;
        }

        /// <summary>
        /// Ruft eine Liste der Gruppen ab, in welchen der Nutzer ein Peanut erstellen kann.
        /// </summary>
        public IList<Core.Domain.Users.UserGroup> UserGroups { get; private set; }

        /// <summary>
        /// Ruft das Command zur Erstellung des Peanuts ab.
        /// </summary>
        public PeanutCreateCommand PeanutCreateCommand { get; private set; }

        /// <summary>
        /// Ruft die verfügbaren Teilnahmearten ab.
        /// </summary>
        public IList<PeanutParticipationType> ParticipationTypes { get; private set; }


    }
}