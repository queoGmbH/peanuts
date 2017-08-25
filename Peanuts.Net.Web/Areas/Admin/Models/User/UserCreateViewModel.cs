using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.User {
    /// <summary>
    ///     Das ViewModel für die Erstellung eines Users
    /// </summary>
    public class UserCreateViewModel {
        /// <summary>
        ///     Ctor.
        /// </summary>
        public UserCreateViewModel() {
            UserCreateCommand = new UserCreateCommand();
        }

        /// <summary>
        ///     Ctor.
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="userCreateCommand"></param>
        /// <param name="userGroups"></param>
        /// <param name="proposedUser"></param>
        public UserCreateViewModel(IList<string> roles, UserCreateCommand userCreateCommand,IList<Core.Domain.Users.UserGroup> userGroups,
            Core.Domain.ProposedUsers.ProposedUser proposedUser = null) {
            Roles = roles;
            UserCreateCommand = userCreateCommand;
            ProposedUser = proposedUser;
            UserGroups = userGroups;
        }

        /// <summary>
        ///     Liefert die möglichen Rollen
        /// </summary>
        public IList<string> Roles { get; set; }

        ///<summary>
        ///     Liefert die möglichen MaklerPools
        /// </summary>
        public IList<Core.Domain.Users.UserGroup> UserGroups { get; set; }

        /// <summary>
        ///     Das Command für das Erstellen eines Users
        /// </summary>
        public UserCreateCommand UserCreateCommand { get; set; }

        /// <summary>
        ///     Liefert den übernommenen beantragten Nutzer
        /// </summary>
        public Core.Domain.ProposedUsers.ProposedUser ProposedUser { get; set; }
    }
}