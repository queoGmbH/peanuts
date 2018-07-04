using System.Collections.Generic;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    /// <summary>
    /// ViewModel für die Anzeige einer Gruppen-Mitgliedschaft.
    /// </summary>
    public class UserGroupAdministrationViewModel {
        public UserGroupAdministrationViewModel(Core.Domain.Users.UserGroup userGroup, UserGroupMembership currentUsersMembershipInGroup, IList<UserGroupMembership> userGroupMembers, UserGroupMembershipOptions userGroupMembershipOptions) {
            Require.NotNull(userGroup, "userGroup");
            Require.NotNull(userGroupMembers, "userGroupMembers");
            Require.NotNull(userGroupMembershipOptions, "userGroupMembershipOptions");

            UserGroup = userGroup;
            CurrentUsersMembershipInGroup = currentUsersMembershipInGroup;
            UserGroupMembers = userGroupMembers;
            UserGroupMembershipOptions = userGroupMembershipOptions;
            
        }

        /// <summary>
        /// Ruft die Gruppe ab, die administriert werden soll.
        /// </summary>
        public Core.Domain.Users.UserGroup UserGroup {
            get; private set;
        }

        public UserGroupMembership CurrentUsersMembershipInGroup { get; }

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