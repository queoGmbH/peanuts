using System.Collections.Generic;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut {

    /// <summary>
    /// ViewModel für das Formular zur Erstellung eines Peanuts.
    /// </summary>
    public class PeanutCreateViewModel {

        public PeanutCreateViewModel(IList<Core.Domain.Users.UserGroup> userGroups, IDictionary<Core.Domain.Users.UserGroup, PeanutParticipationType[]> participationTypesByUserGroup) : this(new PeanutCreateCommand(), userGroups, participationTypesByUserGroup) {
        }

        public PeanutCreateViewModel(PeanutCreateCommand peanutCreateCommand, IList<Core.Domain.Users.UserGroup> userGroups, IDictionary<Core.Domain.Users.UserGroup, PeanutParticipationType[]> participationTypesByUserGroup) {
            Require.NotNull(peanutCreateCommand, "peanutCreateCommand");
            Require.NotNull(userGroups, "userGroups");
            Require.NotNull(participationTypesByUserGroup, "participationTypesByUserGroup");

            PeanutCreateCommand = peanutCreateCommand;
            UserGroups = userGroups;
            ParticipationTypesByUserGroup = participationTypesByUserGroup;
        }

        public IDictionary<Core.Domain.Users.UserGroup, PeanutParticipationType[]> ParticipationTypesByUserGroup { get; set; }

        /// <summary>
        /// Ruft eine Liste der Gruppen ab, in welchen der Nutzer ein Peanut erstellen kann.
        /// </summary>
        public IList<Core.Domain.Users.UserGroup> UserGroups { get; private set; }

        /// <summary>
        /// Ruft das Command zur Erstellung des Peanuts ab.
        /// </summary>
        public PeanutCreateCommand PeanutCreateCommand { get; private set; }
    }
}