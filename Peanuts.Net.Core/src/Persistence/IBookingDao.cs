using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    public interface IBookingDao : IGenericDao<Booking, int> {
        /// <summary>
        ///     Ruft chronologisch alle Buchungen eines Kontos ab.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        IPage<BookingEntry> FindByAccount(IPageable pageRequest, Account account);
    }
}