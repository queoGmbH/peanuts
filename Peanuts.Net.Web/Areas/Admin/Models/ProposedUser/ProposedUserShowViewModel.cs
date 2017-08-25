namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.ProposedUser {
    public class ProposedUserShowViewModel {
        public ProposedUserShowViewModel() {
            User = new Core.Domain.ProposedUsers.ProposedUser();
        }

        public ProposedUserShowViewModel(Core.Domain.ProposedUsers.ProposedUser user) {
            User = user;
        }

        public Core.Domain.ProposedUsers.ProposedUser User { get; set; }
    }
}