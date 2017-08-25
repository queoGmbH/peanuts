using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

using NHibernate;

using Spring.Data.NHibernate.Generic;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    public class PaymentDao : GenericDao<Payment, int>, IPaymentDao {
        /// <summary>
        ///     Ruft alle bestätigten Zahlungen ab, bei denen der Nutzer entweder der ist, der die Zahlung
        ///     <see cref="Payment.RequestSender">erfasst</see> oder <see cref="Payment.RequestRecipient">akzeptiert</see> hat.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public IPage<Payment> FindAcceptedPaymentsByUser(IPageable pageRequest, User user) {
            HibernateDelegate<IPage<Payment>> finder = delegate(ISession session) {
                IQueryOver<Payment, Payment> queryOver = session.QueryOver<Payment>();
                queryOver.Where(payment => payment.RequestRecipient == user || payment.RequestSender == user)
                        .And(payment => payment.PaymentStatus == PaymentStatus.Accecpted);
                return FindPage(queryOver, pageRequest);
            };

            return HibernateTemplate.Execute(finder);
        }

        /// <summary>
        ///     Ruft alle abgelehnten Zahlungen für einen Nutzer ab.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public IPage<Payment> FindDeclinedPaymentsByUser(IPageable pageRequest, User user) {
            HibernateDelegate<IPage<Payment>> finder = delegate(ISession session) {
                IQueryOver<Payment, Payment> queryOver = session.QueryOver<Payment>();
                queryOver.Where(payment => payment.RequestSender == user).And(payment => payment.PaymentStatus == PaymentStatus.Declined);
                return FindPage(queryOver, pageRequest);
            };

            return HibernateTemplate.Execute(finder);
        }

        /// <summary>
        ///     Ruft alle offenen Zahlungen für einen Nutzer ab.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public IPage<Payment> FindPendingPaymentsByUser(IPageable pageRequest, User user) {
            HibernateDelegate<IPage<Payment>> finder = delegate(ISession session) {
                IQueryOver<Payment, Payment> queryOver = session.QueryOver<Payment>();
                queryOver.Where(payment => payment.RequestRecipient == user || payment.RequestSender == user)
                        .And(payment => payment.PaymentStatus == PaymentStatus.Pending);
                return FindPage(queryOver, pageRequest);
            };

            return HibernateTemplate.Execute(finder);
        }
    }
}