using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    public class PayMoneyViewModel {
        public PayMoneyViewModel(IList<UserGroupMembership> groupMembers, PayMoneyCommand payMoneyCommand) {
            Require.NotNull(groupMembers, "groupMembers");
            Require.NotNull(payMoneyCommand, "payMoneyCommand");

            GroupMembers = groupMembers;
            PayMoneyCommand = payMoneyCommand;
        }

        public PayMoneyViewModel(IList<UserGroupMembership> groupMembers) : this(groupMembers, new PayMoneyCommand()) {
            
        }


        /// <summary>
        /// Ruft die Liste der Gruppen-Mitglieder ab, an die bezahlt werden kann.
        /// </summary>
        public IList<UserGroupMembership> GroupMembers { get; private set; }

        /// <summary>
        /// Ruft das Command mit den Eingaben des Nutzers für die Zahlung ab.
        /// </summary>
        public PayMoneyCommand PayMoneyCommand { get; private set; }

    }
}