using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

using Spring.Transaction.Interceptor;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    public class PaymentService : IPaymentService {
        public IBookingService BookingService { get; set; }

        public IPaymentDao PaymentDao { get; set; }

        /// <summary>
        ///     Akzeptiert eine Zahlung und führt die Buchungen auf den Konten durch.
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="user"></param>
        [Transaction]
        public void AcceptPayment(Payment payment, User user) {
            Require.NotNull(payment, "payment");
            Require.NotNull(user, "user");

            if (payment.PaymentStatus == PaymentStatus.Accecpted) {
                throw new InvalidOperationException("Die Zahlung wurde bereits akzeptiert und gebucht.");
            }

            payment.Accept(new EntityChangedDto(user, DateTime.Now));
            /*Der Nutzer der das tatsächliche Geld empfangen hat, ist der dem das Geld vom Konto abgezogen wird.*/
            Account bookingRecipient = payment.Recipient;
            /*Der Nutzer der das tatsächliche Geld abgegeben hat, ist der dem das Geld auf dem Konto gutgeschrieben wird.*/
            Account bookingSender = payment.Sender;
            BookingService.Book(bookingRecipient, bookingSender, payment.Amount, payment.Text);
        }

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
        [Transaction]
        public Payment Create(PaymentDto paymentDto, Account recipient, Account sender, User requestRecipient, User requestSender, User creator, string paymentUrl) {
            Payment payment = new Payment(paymentDto, recipient, sender, requestRecipient, requestSender, new EntityCreatedDto(creator, DateTime.Now));
            PaymentDao.Save(payment);
            NotificationService.SendPaymentReceivedNotification(payment,paymentUrl);
            return payment;
        }

        /// <summary>
        ///    Liefert oder setzt den NotificationService
        /// </summary>
        public INotificationService NotificationService { get; set; }

        /// <summary>
        ///     Lehnt eine Zahlung ab.
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="reason"></param>
        /// <param name="user"></param>
        [Transaction]
        public void DeclinePayment(Payment payment, string reason, User user) {
            Require.NotNull(payment, "payment");
            Require.NotNull(user, "user");

            if (payment.PaymentStatus == PaymentStatus.Accecpted) {
                throw new InvalidOperationException("Die Zahlung wurde bereits akzeptiert und gebucht.");
            }

            payment.Decline(reason, new EntityChangedDto(user, DateTime.Now));
        }

        /// <summary>
        ///     Löscht eine Zahlung.
        /// </summary>
        /// <remarks>
        ///     Das Löschen einer Zahlung ist nur möglich, wenn die Zahlung nicht akzeptiert und dadurch abgerechnet ist.
        /// </remarks>
        /// <param name="payment"></param>
        [Transaction]
        public void Delete(Payment payment) {
            Require.NotNull(payment, "payment");
            if (payment.PaymentStatus == PaymentStatus.Accecpted) {
                throw new InvalidOperationException("Die Zahlung wurde bereits akzeptiert und gebucht.");
            }
            PaymentDao.Delete(payment);
        }

        /// <summary>
        ///     Ruft alle bestätigten Zahlungen ab, bei denen der Nutzer entweder der ist, der die Zahlung <see cref="Payment.RequestSender">erfasst</see> oder <see cref="Payment.RequestRecipient">akzeptiert</see> hat.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public IPage<Payment> FindAcceptedPaymentsByUser(IPageable pageRequest, User user) {
            return PaymentDao.FindAcceptedPaymentsByUser(pageRequest, user);
        }

        /// <summary>
        ///     Ruft alle abgelehnten Zahlungen für einen Nutzer ab.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public IPage<Payment> FindDeclinedPaymentsByUser(IPageable pageRequest, User user) {
            return PaymentDao.FindDeclinedPaymentsByUser(pageRequest, user);
        }

        /// <summary>
        ///     Ruft alle offenen Zahlungen für einen Nutzer ab.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public IPage<Payment> FindPendingPaymentsByUser(IPageable pageRequest, User user) {
            return PaymentDao.FindPendingPaymentsByUser(pageRequest, user);
        }
    }
}