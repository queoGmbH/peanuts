using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.ProposedUser {
    public class ProposedUserCreateViewModel {
        public ProposedUserCreateViewModel() {
            ProposedUserCreateCommand = new ProposedUserCreateCommand();
        }

        public ProposedUserCreateViewModel(ProposedUserCreateCommand proposedUserCreateCommand,
            IList<Core.Domain.Users.UserGroup> financialBrokerPools) {
            ProposedUserCreateCommand = proposedUserCreateCommand;
            FinancialBrokerPools = financialBrokerPools;
        }

        public IList<Core.Domain.Users.UserGroup> FinancialBrokerPools { get; set; }

        public ProposedUserCreateCommand ProposedUserCreateCommand { get; set; }
    }
}