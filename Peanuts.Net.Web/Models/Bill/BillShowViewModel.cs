using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Bill {
    public class BillShowViewModel {
        public BillShowViewModel(Core.Domain.Accounting.Bill bill, BillOptions billOptions, Core.Domain.Peanuts.Peanut createdFromPeanut) {
            Require.NotNull(bill, "bill");
            Require.NotNull(billOptions, "billOptions");

            Bill = bill;
            BillOptions = billOptions;
            CreatedFromPeanut = createdFromPeanut;
        }

        /// <summary>
        /// Ruft die anzuzeigende Rechnung ab.
        /// </summary>
        public Core.Domain.Accounting.Bill Bill { get; private set; }

        /// <summary>
        /// Ruft den Peanut ab, aus dem die Rechnung erstellt wurde oder null, wenn die Rechnung unabhängig von einem Peanut erstellt wurde.
        /// </summary>
        public Core.Domain.Peanuts.Peanut CreatedFromPeanut { get; private set; }

        /// <summary>
        /// Ruft die Optionen ab, die der Nutzer der sich die Rechnung anzeigen lässt, für diese hat.
        /// </summary>
        public BillOptions BillOptions { get; private set; }
    }
}