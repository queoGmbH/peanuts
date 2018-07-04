using System.Collections.Generic;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    /// <summary>
    ///     ViewModel für die Anzeige einer Gruppen-Mitgliedschaft.
    /// </summary>
    public class UserGroupMembershipStatisticsViewModel {
        public UserGroupMembershipStatisticsViewModel(
            Core.Domain.Users.UserGroup userGroup, UserGroupMembership currentUsersMembershipInGroup, IList<UserGroupMembership> userGroupMembers,
            UserGroupMembershipOptions userGroupMembershipOptions, PeanutsUserGroupMembershipStatistics statistics,
            IDictionary<UserGroupMembership, int> karmas) {
            Require.NotNull(userGroup, "userGroup");
            Require.NotNull(userGroupMembers, "userGroupMembers");
            Require.NotNull(userGroupMembershipOptions, "userGroupMembershipOptions");
            Require.NotNull(karmas, "karmas");

            UserGroup = userGroup;
            CurrentUsersMembershipInGroup = currentUsersMembershipInGroup;
            UserGroupMembershipOptions = userGroupMembershipOptions;
            Statistics = statistics;
            Karmas = karmas;
        }

        public UserGroupMembership CurrentUsersMembershipInGroup { get; }

        public IDictionary<UserGroupMembership, int> Karmas { get; }

        public PeanutsUserGroupMembershipStatistics Statistics { get; }

        public Core.Domain.Users.UserGroup UserGroup { get; }

        /// <summary>
        ///     Ruft die Optionen der Seite ab.
        /// </summary>
        public UserGroupMembershipOptions UserGroupMembershipOptions { get; }
    }
}