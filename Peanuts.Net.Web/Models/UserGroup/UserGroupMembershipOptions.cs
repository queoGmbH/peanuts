using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    /// <summary>
    /// Sammelt Optionen, die auf der Detailseite einer Mitgliedschaft zur Verfügung stehen und ob ein Nutzer darauf Zugriff hat.
    /// </summary>
    public class UserGroupMembershipOptions {
        public UserGroupMembershipOptions(UserGroupMembership userGroupMembership, UserGroupMembership currentUsersMembership) {
            if (currentUsersMembership==null || !userGroupMembership.UserGroup.Equals(currentUsersMembership.UserGroup)) {
                return;
            }

            CanQuitMembership = userGroupMembership.IsActiveMembership && currentUsersMembership.User.Equals(userGroupMembership.User);

            CanAcceptInvitation = userGroupMembership.MembershipType == UserGroupMembershipType.Invited && currentUsersMembership.User.Equals(userGroupMembership.User);
            CanRefuseInvitation = userGroupMembership.MembershipType == UserGroupMembershipType.Invited && currentUsersMembership.User.Equals(userGroupMembership.User);

            CanAcceptRequest = userGroupMembership.MembershipType == UserGroupMembershipType.Request && currentUsersMembership.MembershipType == UserGroupMembershipType.Administrator;
            CanRefuseRequest = userGroupMembership.MembershipType == UserGroupMembershipType.Request && currentUsersMembership.MembershipType == UserGroupMembershipType.Administrator;

            CanViewUserGroupMembers = userGroupMembership.IsActiveMembership;
        }

        /// <summary>
        /// Ruft ab, ob die Mitgliedschaft in der Gruppe beendet werden kann.
        /// </summary>
        public bool CanQuitMembership { get; private set; }

        /// <summary>
        /// Ruft ab, ob eine Einladung in die Gruppe akzeptiert werden kann.
        /// </summary>
        public bool CanAcceptInvitation {
            get; private set;
        }

        /// <summary>
        /// Ruft ab, ob eine Einladung in die Gruppe abgelehnt werden kann.
        /// </summary>
        public bool CanRefuseInvitation {
            get; private set;
        }

        /// <summary>
        /// Ruft ab, ob eine Anfrage zur Mitgliedschaft in die Gruppe akzeptiert werden kann.
        /// </summary>
        public bool CanAcceptRequest {
            get; private set;
        }

        /// <summary>
        /// Ruft ab, ob eine Anfrage zur Mitgliedschaft in die Gruppe abgelehnt werden kann.
        /// </summary>
        public bool CanRefuseRequest {
            get; private set;
        }

        /// <summary>
        /// Ruft ab, ob die Liste der Mitglieder in der Gruppe angezeigt werden kann.
        /// </summary>
        public bool CanViewUserGroupMembers {
            get; private set;
        }
    }
}