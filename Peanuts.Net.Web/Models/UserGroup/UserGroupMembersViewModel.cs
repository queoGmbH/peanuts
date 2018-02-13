using System.Collections.Generic;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    /// <summary>
    /// ViewModel für die Anzeige der Mitglieder einer Gruppe.
    /// </summary>
    public class UserGroupMembersViewModel {
        public UserGroupMembersViewModel(IList<UserGroupMembership> userGroupMembers, UserGroupMembershipOptions userGroupMembershipOptions) {
            Require.NotNull(userGroupMembers, "userGroupMembers");
            Require.NotNull(userGroupMembershipOptions, "userGroupMembershipOptions");

            UserGroupMembers = userGroupMembers;
            UserGroupMembershipOptions = userGroupMembershipOptions;
            
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
        
    }
}