using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {

    /// <summary>
    /// ViewModel für die Anzeige einer Gruppen-Mitgliedschaft.
    /// </summary>
    public class UserGroupMembershipDetailsViewModel {
        public UserGroupMembershipDetailsViewModel(UserGroupMembership userGroupMembership, IList<UserGroupMembership> userGroupMembers, UserGroupMembershipOptions userGroupMembershipOptions, IPage<Core.Domain.Peanuts.Peanut> peanuts) {
            Require.NotNull(userGroupMembership, "userGroupMembership");
            Require.NotNull(userGroupMembers, "userGroupMembers");
            Require.NotNull(userGroupMembershipOptions, "userGroupMembershipOptions");

            UserGroupMembership = userGroupMembership;
            UserGroupMembers = userGroupMembers;
            UserGroupMembershipOptions = userGroupMembershipOptions;
            Peanuts = peanuts;
        }

        /// <summary>
        /// Ruft die anzuzeigende Mitgliedschaft ab.
        /// </summary>
        public UserGroupMembership UserGroupMembership { get; private set; }

        /// <summary>
        /// Ruft die Mitgliedschaften der Gruppe auf.
        /// </summary>
        public IList<UserGroupMembership> UserGroupMembers { get; private set; }

        /// <summary>
        /// Ruft die Optionen der Seite ab.
        /// </summary>
        public UserGroupMembershipOptions UserGroupMembershipOptions { get; private set; }

        /// <summary>
        /// Liefert die Liste der Peanuts
        /// </summary>
        public IPage<Core.Domain.Peanuts.Peanut> Peanuts { get; set; }
    }
}