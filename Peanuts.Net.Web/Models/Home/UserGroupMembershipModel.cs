using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Home {
    public class UserGroupMembershipModel {

        public UserGroupMembershipModel(UserGroupMembership userGroupMembership, int karma) {
            Require.NotNull(userGroupMembership, "userGroupMembership");

            UserGroupMembership = userGroupMembership;
            Karma = karma;
        }

        public UserGroupMembership UserGroupMembership { get; private set; }

        public int Karma { get; private set; }

    }
}