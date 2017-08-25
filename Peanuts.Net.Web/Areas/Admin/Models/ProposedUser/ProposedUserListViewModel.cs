using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.ProposedUser {
    public class ProposedUserListViewModel {
        public ProposedUserListViewModel(IPage<Core.Domain.ProposedUsers.ProposedUser> users) {
            Users = users;
        }

        public IPage<Core.Domain.ProposedUsers.ProposedUser> Users { get; set; }
    }
}