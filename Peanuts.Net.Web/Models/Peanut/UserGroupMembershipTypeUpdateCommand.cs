using System.ComponentModel.DataAnnotations;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut {
    /// <summary>
    /// Command zum Ändern der Mitgliedsart in einer Gruppe.
    /// </summary>
    public class UserGroupMembershipTypeUpdateCommand {
        public UserGroupMembershipTypeUpdateCommand() {
        }

        public UserGroupMembershipTypeUpdateCommand(UserGroupMembershipType userGroupMembershipType) {

            UserGroupMembershipType = userGroupMembershipType;
        }

        /// <summary>
        /// Ruft ab oder legt fest, welche Mitgliedschaftsart das Mitglied erhalten soll.
        /// </summary>
        [Required]
        public UserGroupMembershipType UserGroupMembershipType {
            get; set;
        }
    }
}