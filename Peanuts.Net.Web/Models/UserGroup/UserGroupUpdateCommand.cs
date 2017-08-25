using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    /// <summary>
    /// Command zum Ändern einer UserGroup.
    /// </summary>
    [DtoFor(typeof(Core.Domain.Users.UserGroup))]
    public class UserGroupUpdateCommand {
        
        
        /// <summary>
        /// Erzeugt eine neue Instanz von <see cref="UserGroupUpdateCommand"/>.
        /// </summary>
        /// <param name="userGroup"></param>
        public UserGroupUpdateCommand(Core.Domain.Users.UserGroup userGroup) {
            Require.NotNull(userGroup, "userGroup");

            UserGroupDto = userGroup.GetDto();
        }

        /// <summary>
        /// Erzeugt eine neue Instanz von <see cref="UserGroupUpdateCommand"/>.
        /// </summary>
        public UserGroupUpdateCommand() {
            UserGroupDto = new UserGroupDto();
        }

        /// <summary>
        /// Liefert oder setzt den <see cref="UserGroupDto"/>.
        /// </summary>
        public UserGroupDto UserGroupDto {
            get; set;
        }

    }
}