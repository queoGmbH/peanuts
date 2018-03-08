using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    public class UserGroupMembershipUpdateCommand {
        /// <inheritdoc />
        public UserGroupMembershipUpdateCommand() {
        }

        /// <inheritdoc />
        public UserGroupMembershipUpdateCommand(UserGroupMembership userGroupMembership) {
            Require.NotNull(userGroupMembership, "userGroupMembership");

            UserGroupMembershipDto = userGroupMembership.GetDto();
        }

        public UserGroupMembershipDto UserGroupMembershipDto { get; set; }
    }
}