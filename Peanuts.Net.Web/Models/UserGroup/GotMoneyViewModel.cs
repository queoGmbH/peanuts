using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    public class GotMoneyViewModel {
        public GotMoneyViewModel(IList<UserGroupMembership> groupMembers, GotMoneyCommand gotMoneyCommand) {
            Require.NotNull(groupMembers, "groupMembers");
            Require.NotNull(gotMoneyCommand, "gotMoneyCommand");

            GroupMembers = groupMembers;
            GotMoneyCommand = gotMoneyCommand;
        }

        public GotMoneyViewModel(IList<UserGroupMembership> groupMembers) : this(groupMembers, new GotMoneyCommand()) {

        }

        /// <summary>
        /// Ruft die Liste der Gruppen-Mitglieder ab, an die bezahlt werden kann.
        /// </summary>
        public IList<UserGroupMembership> GroupMembers {
            get; private set;
        }

        /// <summary>
        /// Ruft das Command mit den Eingaben des Nutzers für die Zahlung ab.
        /// </summary>
        public GotMoneyCommand GotMoneyCommand {
            get; private set;
        }

    }
}