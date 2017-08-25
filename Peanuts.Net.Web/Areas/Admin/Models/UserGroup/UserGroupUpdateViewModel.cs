using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.UserGroup {
    /// <summary>
    /// ViewModel zur Aktualisierung von <see cref="UserGroup"/>
    /// </summary>
    public class UserGroupUpdateViewModel {
        public UserGroupUpdateViewModel(UserGroupUpdateCommand userGroupUpdateCommand, IList<Core.Domain.Users.User> users) {
            UserGroupUpdateCommand = userGroupUpdateCommand;
            Users = users;
        }

        public UserGroupUpdateCommand UserGroupUpdateCommand { get; set; }

        public IList<Core.Domain.Users.User> Users { get; set; }
    }
}