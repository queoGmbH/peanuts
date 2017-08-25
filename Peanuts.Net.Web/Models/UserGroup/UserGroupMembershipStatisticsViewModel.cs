using System.Collections.Generic;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    /// <summary>
    /// ViewModel für die Anzeige einer Gruppen-Mitgliedschaft.
    /// </summary>
    public class UserGroupMembershipStatisticsViewModel {
        public UserGroupMembershipStatisticsViewModel(UserGroupMembership userGroupMembership, IList<UserGroupMembership> userGroupMembers, UserGroupMembershipOptions userGroupMembershipOptions, PeanutsUserGroupMembershipStatistics statistics, IDictionary<UserGroupMembership, int> karmas) {
            Require.NotNull(userGroupMembership, "userGroupMembership");
            Require.NotNull(userGroupMembers, "userGroupMembers");
            Require.NotNull(userGroupMembershipOptions, "userGroupMembershipOptions");
            Require.NotNull(karmas, "karmas");

            UserGroupMembership = userGroupMembership;
            UserGroupMembers = userGroupMembers;
            UserGroupMembershipOptions = userGroupMembershipOptions;
            Statistics = statistics;

            Karmas = karmas;
        }

        /// <summary>
        /// Ruft die anzuzeigende Mitgliedschaft ab.
        /// </summary>
        public UserGroupMembership UserGroupMembership {
            get; private set;
        }

        /// <summary>
        /// Ruft die Mitgliedschaften der Gruppe auf.
        /// </summary>
        public IList<UserGroupMembership> UserGroupMembers {
            get; private set;
        }

        /// <summary>
        /// Ruft die Optionen der Seite ab.
        /// </summary>
        public UserGroupMembershipOptions UserGroupMembershipOptions {
            get; private set;
        }

        public PeanutsUserGroupMembershipStatistics Statistics { get; private set; }

        public IDictionary<UserGroupMembership, int> Karmas { get; private set; }
        
    }
}