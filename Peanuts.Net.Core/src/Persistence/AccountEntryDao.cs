using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    public class AccountingEntryDao : GenericDao<BookingEntry, int>, IAccountingEntryDao {
        public IPage<BookingEntry> FindByAccount(IPageable pageRequest, Account account) {
            throw new NotImplementedException();
        }
    }

    public interface IAccountingEntryDao : IGenericDao<BookingEntry, int> {
        /// <summary>
        ///     Ruft chronologisch alle Buchungen eines Kontos ab.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        IPage<BookingEntry> FindByAccount(IPageable pageRequest, Account account);
    }
}