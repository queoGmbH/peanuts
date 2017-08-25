using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Bill {
    /// <summary>
    /// ViewModel für das Formular, zum Ablehnen einer Rechnung.
    /// </summary>
    public class BillRefuseViewModel {
        public Core.Domain.Accounting.Bill Bill { get; private set; }

        public User User { get; private set; }

        public BillRefuseViewModel(Core.Domain.Accounting.Bill bill, User user) : this(bill, user, new BillRefuseCommand()) {
            
        }

        public BillRefuseViewModel(Core.Domain.Accounting.Bill bill, User user, BillRefuseCommand billRefuseCommand) {
            Bill = bill;
            User = user;
            Require.NotNull(bill, "bill");
            Require.NotNull(user, "user");
            Require.NotNull(billRefuseCommand, "billRefuseCommand");
        }

        /// <summary>
        /// Ruft das Command zum Ablehnen einer Rechnung ab.
        /// </summary>
        public BillRefuseCommand BillRefuseCommand { get; private set; }
    }
}