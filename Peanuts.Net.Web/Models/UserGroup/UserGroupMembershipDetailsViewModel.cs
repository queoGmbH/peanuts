using System.Collections.Generic;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    /// <summary>
    ///     ViewModel für die Anzeige einer Gruppen-Mitgliedschaft.
    /// </summary>
    public class UserGroupMembershipDetailsViewModel {
        public UserGroupMembershipDetailsViewModel(UserGroupMembership userGroupMembership, UserGroupMembership currentUsersMembershipInGroup, UserGroupMembershipOptions userGroupMembershipOptions) {
            Require.NotNull(userGroupMembership, "userGroupMembership");
            Require.NotNull(userGroupMembershipOptions, "userGroupMembershipOptions");

            UserGroupMembership = userGroupMembership;
            CurrentUsersMembershipInGroup = currentUsersMembershipInGroup;
            UserGroupMembershipOptions = userGroupMembershipOptions;

            if (currentUsersMembershipInGroup != null) {
                UserGroupMembershipUpdateCommand = new UserGroupMembershipUpdateCommand(currentUsersMembershipInGroup);
            }
        }

        /// <summary>
        ///     Ruft die anzuzeigende Mitgliedschaft ab.
        /// </summary>
        public UserGroupMembership UserGroupMembership { get; }

        public UserGroupMembership CurrentUsersMembershipInGroup { get; }

        /// <summary>
        ///     Ruft die Optionen der Seite ab.
        /// </summary>
        public UserGroupMembershipOptions UserGroupMembershipOptions { get; }

        /// <summary>
        /// Ruft das Command zum Ändern der Einstellungen für die Mitgliedschaft in der Gruppe ab.
        /// </summary>
        public UserGroupMembershipUpdateCommand UserGroupMembershipUpdateCommand { get; }
    }
}