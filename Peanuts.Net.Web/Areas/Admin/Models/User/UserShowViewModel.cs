namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.User {
    /// <summary>
    ///     Das Viewmodel für die UserShow
    /// </summary>
    public class UserShowViewModel {
        public UserShowViewModel() {
            User = new Core.Domain.Users.User();
        }

        public UserShowViewModel(Core.Domain.Users.User user) {
            User = user;
        }

        public Core.Domain.Users.User User { get; set; }
    }
}