using System.ComponentModel.DataAnnotations;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    /// <summary>
    ///     Command mit den Informationen zu einem erhaltenen Betrag.
    /// </summary>
    public class GotMoneyCommand {

        /// <summary>
        /// Ruft Informationen über die Zahlung ab oder legt diese fest.
        /// </summary>
        public PaymentDto PaymentDto {
            get; set;
        }

        [Required]
        public UserGroupMembership Sender {
            get; set;
        }
        
    }
}