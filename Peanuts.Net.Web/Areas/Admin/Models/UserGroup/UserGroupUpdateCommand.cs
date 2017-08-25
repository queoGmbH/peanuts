using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.UserGroup {
    /// <summary>
    ///     Command zur Aktualisierung vom <see cref="UserGroup" />
    /// </summary>
    [DtoFor(typeof(Core.Domain.Users.UserGroup))]
    public class UserGroupUpdateCommand {
        public UserGroupUpdateCommand() {
        }

        public UserGroupUpdateCommand(UserGroupDto userGroupDto) {
            UserGroupDto = userGroupDto;
        }

        public bool CanUserChangePassword { get; set; }

        public UserGroupDto UserGroupDto { get; set; }
    }
}