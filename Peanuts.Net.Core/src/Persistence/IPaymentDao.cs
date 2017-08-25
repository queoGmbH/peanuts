using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    public interface IPaymentDao : IGenericDao<Payment, int> {
        /// <summary>
        ///     Ruft alle bestätigten Zahlungen ab, bei denen der Nutzer entweder der ist, der die Zahlung
        ///     <see cref="Payment.RequestSender">erfasst</see> oder <see cref="Payment.RequestRecipient">akzeptiert</see> hat.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        IPage<Payment> FindAcceptedPaymentsByUser(IPageable pageRequest, User user);

        /// <summary>
        ///     Ruft alle abgelehnten Zahlungen für einen Nutzer ab.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        IPage<Payment> FindDeclinedPaymentsByUser(IPageable pageRequest, User user);

        /// <summary>
        ///     Ruft alle offenen Zahlungen für einen Nutzer ab.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        IPage<Payment> FindPendingPaymentsByUser(IPageable pageRequest, User currentUser);
    }
}