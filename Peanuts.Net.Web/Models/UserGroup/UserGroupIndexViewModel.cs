using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    public class UserGroupIndexViewModel {
        public UserGroupIndexViewModel(IList<UserGroupMembership> currentMemberships, IList<UserGroupMembership> myRequestedMemberships, IList<UserGroupMembership> invitations, IList<UserGroupMembership> requestedMembershipsInMyGroups) {
            Require.NotNull(currentMemberships, "currentMemberships");
            Require.NotNull(invitations, "invitations");
            Require.NotNull(myRequestedMemberships, "myRequestedMemberships");
            Require.NotNull(requestedMembershipsInMyGroups, "requestedMembershipsInMyGroups");

            CurrentMemberships = currentMemberships;
            MyRequestedMemberships = myRequestedMemberships;
            Invitations = invitations;
            RequestedMembershipsInMyGroups = requestedMembershipsInMyGroups;
        }

        /// <summary>
        /// Ruft meine aktuellen Mitgliedschaften ab.
        /// </summary>
        public IList<UserGroupMembership> CurrentMemberships { get; set; }

        /// <summary>
        /// Ruft von mir angefragte Mitgliedschaften ab.
        /// </summary>
        public IList<UserGroupMembership> MyRequestedMemberships { get; set; }

        /// <summary>
        /// Ruft Anfragen für Mitgliedschaften in Gruppen ab, in denen ich Administrator bin.
        /// </summary>
        public IList<UserGroupMembership> RequestedMembershipsInMyGroups {
            get; set;
        }

        /// <summary>
        /// Ruft an mich gesendete Einladungen ab, Mitglied in einer Gruppe zu werden.
        /// </summary>
        public IList<UserGroupMembership> Invitations { get; set; }
    }
}