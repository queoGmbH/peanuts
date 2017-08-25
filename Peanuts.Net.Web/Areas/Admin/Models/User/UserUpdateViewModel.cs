using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.User {
    /// <summary>
    ///     Das ViewModel für die Update View
    /// </summary>
    public class UserUpdateViewModel {
        /// <summary>
        ///     Ctor.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <param name="userGroups"></param>
        /// <param name="userUpdateCommand"></param>
        public UserUpdateViewModel(Core.Domain.Users.User user, IList<string> roles, IList<Core.Domain.Users.UserGroup> userGroups,
            UserUpdateCommand userUpdateCommand) {
            Require.NotNull(user, "user");

            UserToUpdate = user;
            Roles = roles;
            UserGroups = userGroups;
            UserUpdateCommand = userUpdateCommand;
        }

        /// <summary>
        ///     Ctor.
        /// </summary>
        public UserUpdateViewModel(Core.Domain.Users.User user)
            : this(user, new List<string>(), new List<Core.Domain.Users.UserGroup>(), new UserUpdateCommand(user)) {
        }

        /// <summary>
        ///     Gibt und setzt die verfügbaren Gruppen
        /// </summary>
        public IList<Core.Domain.Users.UserGroup> UserGroups { get; set; }

        /// <summary>
        ///     Gibt oder setzt die Roles
        /// </summary>
        public IList<string> Roles { get; set; }

        /// <summary>
        ///     Ruft den zu bearbeitenden Nutzer ab.
        /// </summary>
        public Core.Domain.Users.User UserToUpdate { get; private set; }

        /// <summary>
        ///     Das Command für das Bearbeiten eines Users
        /// </summary>
        public UserUpdateCommand UserUpdateCommand { get; set; }
    }
}