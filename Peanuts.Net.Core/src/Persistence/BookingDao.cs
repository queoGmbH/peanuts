using System;
using System.Linq.Expressions;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

using NHibernate;

using Spring.Data.NHibernate.Generic;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    public class BookingDao : GenericDao<Booking, int>, IBookingDao {
        public IPage<BookingEntry> FindByAccount(IPageable pageRequest, Account account) {
            HibernateDelegate<IPage<BookingEntry>> finder = delegate(ISession session) {
                IQueryOver<BookingEntry, BookingEntry> queryOverBookingEntries = session.QueryOver<BookingEntry>();
                IQueryOver<BookingEntry, Booking> joinBooking = queryOverBookingEntries.JoinQueryOver(entry => entry.Booking).OrderBy(booking => booking.BookingDate).Desc;
                queryOverBookingEntries = queryOverBookingEntries.Where(entry => entry.Account == account);

                return FindPage(queryOverBookingEntries, pageRequest);
            };

            return HibernateTemplate.Execute(finder);
        }
    }
}