namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.UserGroup {

    /// <summary>
    /// ViewModel für das Löschen eines <see cref="UserGroup"/>s.
    /// </summary>
    public class UserGroupDeleteViewModel {
        public UserGroupDeleteViewModel(Core.Domain.Users.UserGroup userGroup) {
            UserGroup = userGroup;
        }

        public Core.Domain.Users.UserGroup UserGroup { get; set; }
    }
}