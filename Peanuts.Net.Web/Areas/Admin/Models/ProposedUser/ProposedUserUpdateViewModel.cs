using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.ProposedUser {
    /// <summary>
    ///     ViewModel für die Anzeige der Aktualisierung von
    ///     <see cref="ProposedUser" />
    /// </summary>
    public class ProposedUserUpdateViewModel {
        public ProposedUserUpdateViewModel(Core.Domain.ProposedUsers.ProposedUser user,
            IList<Core.Domain.Users.UserGroup> financialBrokerPools, ProposedUserUpdateCommand proposedUserUpdateCommand) {
            Require.NotNull(financialBrokerPools, nameof(financialBrokerPools));
            Require.NotNull(user, nameof(user));
            Require.NotNull(proposedUserUpdateCommand, nameof(proposedUserUpdateCommand));

            FinancialBrokerPools = financialBrokerPools;
            UserToUpdate = user;
            ProposedUserUpdateCommand = proposedUserUpdateCommand;
        }

        public ProposedUserUpdateViewModel(Core.Domain.ProposedUsers.ProposedUser user)
            : this(user, new List<Core.Domain.Users.UserGroup>(), new ProposedUserUpdateCommand()) {
        }

        public IList<Core.Domain.Users.UserGroup> FinancialBrokerPools { set; get; }

        public ProposedUserUpdateCommand ProposedUserUpdateCommand { get; set; }

        public Core.Domain.ProposedUsers.ProposedUser UserToUpdate { get; private set; }
    }
}