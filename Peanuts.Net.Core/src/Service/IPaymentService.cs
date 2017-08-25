using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    /// <summary>
    ///     Schnittstelle, die einen Service beschreibt, der Methoden anbietet, um Zahlungen zu verwalten.
    /// </summary>
    public interface IPaymentService {
        /// <summary>
        ///     Akzeptiert eine Zahlung und führt die Buchungen durch.
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="user"></param>
        void AcceptPayment(Payment payment, User user);

        /// <summary>
        ///     Erstellt eine Zahlung. Die Zahlung muss vom Nutzer der die Zahlung erhalten hat, erst bestätigt werden.
        /// </summary>
        /// <param name="paymentDto"></param>
        /// <param name="recipient"></param>
        /// <param name="sender"></param>
        /// <param name="requestRecipient">Der Nutzer, der die Zahlung bestätigen muss.</param>
        /// <param name="requestSender">Der Nutzer, der auf die Bestätigung der Zahlung wartet.</param>
        /// <param name="creator"></param>
        /// <param name="paymentUrl"></param>
        /// <returns></returns>
        Payment Create(PaymentDto paymentDto, Account recipient, Account sender, User requestRecipient, User requestSender, User creator, string paymentUrl);

        /// <summary>
        ///     Lehnt eine Zahlung ab.
        /// </summary>
        /// <param name="payment">Die Zahlung, die abgelehnt werden soll.</param>
        /// <param name="reason">Der Grund für die Ablehnung.</param>
        /// <param name="user">Der Nutzer der die Zahlung ablehnt.</param>
        void DeclinePayment(Payment payment, string reason, User user);

        /// <summary>
        ///     Löscht eine Zahlung.
        /// </summary>
        /// <remarks>
        ///     Das Löschen einer Zahlung ist nur möglich, wenn die Zahlung nicht akzeptiert und dadurch abgerechnet ist.
        /// </remarks>
        /// <param name="payment"></param>
        void Delete(Payment payment);

        /// <summary>
        ///     Ruft alle bestätigten Zahlungen ab, bei denen der Nutzer entweder der ist, der die Zahlung <see cref="Payment.RequestSender">erfasst</see> oder <see cref="Payment.RequestRecipient">akzeptiert</see> hat.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        IPage<Payment> FindAcceptedPaymentsByUser(IPageable pageRequest, User user);

        /// <summary>
        ///     Ruft alle von einem Nutzer erfassten Zahlungen ab, bei denen der andere Nutzer widersprochen hat, die Zahlung erhalten oder getätigt zu haben.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        IPage<Payment> FindDeclinedPaymentsByUser(IPageable pageRequest, User user);

        /// <summary>
        ///     Ruft alle offenen Zahlungen für einen Nutzer ab.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        IPage<Payment> FindPendingPaymentsByUser(IPageable pageRequest, User user);
    }
}