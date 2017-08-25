using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Payment {
    public class PendingPaymentsViewModel {
        public PendingPaymentsViewModel(User user, IList<Core.Domain.Accounting.Payment> pendingPaymentsAsSender,
            IList<Core.Domain.Accounting.Payment> pendingPaymentsAsRecipient, IPage<Core.Domain.Accounting.Payment> declinedPayments,
            IPage<Core.Domain.Accounting.Payment> acceptedPayments) {
            Require.NotNull(pendingPaymentsAsSender, "pendingPaymentsAsSender");
            Require.NotNull(pendingPaymentsAsRecipient, "pendingPaymentsAsRecipient");
            Require.NotNull(acceptedPayments, "acceptedPayments");
            Require.NotNull(declinedPayments, "declinedPayments");
            Require.NotNull(user, "user");

            User = user;
            PendingPaymentsAsSender = pendingPaymentsAsSender;
            PendingPaymentsAsRecipient = pendingPaymentsAsRecipient;
            DeclinedPayments = declinedPayments;
            AcceptedPayments = acceptedPayments;
        }

        public IPage<Core.Domain.Accounting.Payment> AcceptedPayments { get; private set; }

        public IPage<Core.Domain.Accounting.Payment> DeclinedPayments { get; private set; }

        public IList<Core.Domain.Accounting.Payment> PendingPaymentsAsRecipient { get; private set; }

        public IList<Core.Domain.Accounting.Payment> PendingPaymentsAsSender { get; private set; }

        public User User { get; private set; }
    }

}