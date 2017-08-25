using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Payment {
    public class DeclinePaymentViewModel {
        public DeclinePaymentViewModel(Core.Domain.Accounting.Payment payment, DeclinePaymentCommand declinePaymentCommand) {
            Require.NotNull(payment, "payment");
            Require.NotNull(declinePaymentCommand, "declinePaymentCommand");

            Payment = payment;
            DeclinePaymentCommand = declinePaymentCommand;
        }

        public DeclinePaymentViewModel(Core.Domain.Accounting.Payment payment) : this(payment, new DeclinePaymentCommand()) {
            
        }

        public Core.Domain.Accounting.Payment Payment { get; private set; }

        public DeclinePaymentCommand DeclinePaymentCommand { get; private set; }
    }
}