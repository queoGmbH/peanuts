using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    public class UserGroupMembershipInvitationViewModel {
        public UserGroupMembershipInvitationViewModel(Core.Domain.Users.UserGroup userGroup, IList<User> invitableUsers) {
            Require.NotNull(invitableUsers, "invitableUsers");
            Require.NotNull(userGroup, "userGroup");

            UserGroup = userGroup;
            InvitableUsers = invitableUsers;
        }

        /// <summary>
        ///     Ruft die Liste der Nutzer ab, die eingeladen werden können.
        /// </summary>
        public IList<User> InvitableUsers { get; private set; }

        /// <summary>
        ///     Ruft die Gruppe ab, in die ein Nutzer eingeladen werden soll.
        /// </summary>
        public Core.Domain.Users.UserGroup UserGroup { get; private set; }

        public User User { get; set; }
    }
}