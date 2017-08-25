using System.Collections.Generic;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    public class UserGroupCreateViewModel {
        public UserGroupCreateViewModel(UserGroupCreateCommand userGroupCreateCommand) {
            UserGroupCreateCommand = userGroupCreateCommand;
        }

        public UserGroupCreateCommand UserGroupCreateCommand { get; set; }
        
    }
}