namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    public class UserGroupUpdateViewModel {
        public UserGroupUpdateViewModel(Core.Domain.Users.UserGroup userGroup)
            : this(userGroup, new UserGroupUpdateCommand(userGroup)) {
        }

        public UserGroupUpdateViewModel(Core.Domain.Users.UserGroup userGroup, UserGroupUpdateCommand userGroupUpdateCommand) {
            UserGroup = userGroup;
            UserGroupUpdateCommand = userGroupUpdateCommand;
        }

        public Core.Domain.Users.UserGroup UserGroup { get; set; }

        public UserGroupUpdateCommand UserGroupUpdateCommand { get; set; }
    }
}