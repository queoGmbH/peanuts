using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {
    [DtoFor(typeof(Bill))]
    public class BillDto : IValidatableObject{
        public BillDto() {
        }

        public BillDto(string subject, double amount) {
            Amount = amount;
            Subject = subject;
        }

        /// <summary>
        ///     Ruft den Rechnungsbetrag ab oder legt diesen fest.
        /// </summary>
        public virtual double Amount { get; set; }

        /// <summary>
        ///     Ruft den Betreff der Rechnung ab oder legt diesen fest.
        /// </summary>
        [Required]
        public virtual string Subject { get; set; }

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            IList<ValidationResult> errors = new List<ValidationResult>();

            if (Amount <= 0.01) {
                errors.Add(new ValidationResult("Es muss ein positiver Rechnungsbetrag angegeben werden"));
            }

            return errors;
        }
    }
}