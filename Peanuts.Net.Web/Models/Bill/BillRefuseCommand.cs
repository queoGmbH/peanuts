using System.ComponentModel.DataAnnotations;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Bill {

    /// <summary>
    /// Command zum Ablehnen einer Rechnung, wobei der Nutzer eine Begründung eintragen muss, warum er die Rechnung ablehnt.
    /// </summary>
    public class BillRefuseCommand {

        [Required]
        [StringLength(1000)]
        public string RefuseComment { get; set; }

    }
}