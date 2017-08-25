using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    /// <summary>
    ///     Command mit den Informationen zu einem zu zahlenden Betrag.
    /// </summary>
    public class PayMoneyCommand : IValidatableObject {
        public PaymentDto PaymentDto { get; set; }

        /// <summary>
        ///     Ruft den Empfänger des Betrags ab oder legt diesen fest.
        /// </summary>
        [Required]
        public UserGroupMembership Recipient { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            IList<ValidationResult> validationResult = new List<ValidationResult>();

            /*Die Zahlung per PayPal geht nur, wenn der andere Nutzer einen PayPal-Namen eingetragen hat.*/
            if (PaymentDto.PaymentType == PaymentType.PayPal && string.IsNullOrWhiteSpace(Recipient.User.PayPalBusinessName)) {
                validationResult.Add(new ValidationResult("Die Zahlung per PayPal ist an den ausgewählten Nutzer nicht möglich!"));
            }

            return validationResult;
        }
    }
}