using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.ProposedUser {
    public class ProposedUserDeleteViewModel {
        public ProposedUserDeleteViewModel(Core.Domain.ProposedUsers.ProposedUser userToDelete) {
            Require.NotNull(userToDelete, "userToDelete");

            UserToDelete = userToDelete;
        }

        public Core.Domain.ProposedUsers.ProposedUser UserToDelete { get; private set; }
    }
}