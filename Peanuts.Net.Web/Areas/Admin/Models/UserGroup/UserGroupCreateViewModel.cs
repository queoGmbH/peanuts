using System.Collections.Generic;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.UserGroup {
    public class UserGroupCreateViewModel {
        public UserGroupCreateViewModel(UserGroupCreateCommand userGroupCreateCommand, IList<Core.Domain.Users.User> users) {
            UserGroupCreateCommand = userGroupCreateCommand;
            Users = users;
        }

        public UserGroupCreateCommand UserGroupCreateCommand { get; set; }

        public IList<Core.Domain.Users.User> Users { get; set; }
    }
}