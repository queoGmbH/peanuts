using System.ComponentModel.DataAnnotations;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Payment {
    [DtoFor(typeof(Core.Domain.Accounting.Payment))]
    public class DeclinePaymentCommand {

        /// <summary>
        /// Ruft den Grund für das Ablehnen ab oder legt diesen fest.
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string DeclineReason { get; set; }

    }
}