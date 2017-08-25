using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut {
    /// <summary>
    ///     ViewModel für die Detailseite eines Peanuts.
    /// </summary>
    public class PeanutShowViewModel {
        public PeanutShowViewModel(Core.Domain.Peanuts.Peanut peanut, PeanutParticipation usersPeanutParticipation,
            IList<UserGroupMembership> invitableUsers, IList<PeanutParticipationType> participationTypes, PeanutEditOptions peanutEditOptions) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(peanutEditOptions, "peanutEditOptions");
            Require.NotNull(participationTypes, "participationTypes");
            Require.NotNull(invitableUsers, "invitableUsers");

            Peanut = peanut;
            UsersPeanutParticipation = usersPeanutParticipation;
            PeanutEditOptions = peanutEditOptions;
            InviteableUsers = invitableUsers;
            ParticipationTypes = participationTypes;
        }

        /// <summary>
        /// Ruft die Nutzer ab, die zum Peanut eingeladen werden können, da sie bisher nicht Teilnehmen oder abgesagt haben.
        /// </summary>
        public IList<UserGroupMembership> InviteableUsers { get; private set; }

        /// <summary>
        /// Ruft die Arten ab, wie ein Nutzer an einem peanut Teilnehmen kann.
        /// </summary>
        public IList<PeanutParticipationType> ParticipationTypes { get; private set; }

        /// <summary>
        ///     Ruft den angezeigten Peanut ab.
        /// </summary>
        public Core.Domain.Peanuts.Peanut Peanut { get; private set; }

        public PeanutCommentCreateCommand PeanutCommentCreateCommand { get; private set; }

        public PeanutEditOptions PeanutEditOptions { get; private set; }

        public PeanutInvitationCreateCommand PeanutInvitationCreateCommand { get; set; }

        public PeanutParticipation UsersPeanutParticipation { get; private set; }

        public PeanutParticipationUpdateCommand PeanutParticipationUpdateCommand { get; private set; }
    }
}