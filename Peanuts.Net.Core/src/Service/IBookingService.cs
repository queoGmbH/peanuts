using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    public interface IBookingService {
        /// <summary>
        ///     Führt eine Buchung durch, in dem auf dem Empfänger-Konto der Betrag gutgeschrieben wird und auf dem Absende-Konto
        ///     der Betrag abgebucht wird.
        /// </summary>
        /// <param name="sender">Das Sender-Konto.</param>
        /// <param name="recipient">Das Empfänger-Konto.</param>
        /// <param name="amount">Der zu buchende Betrag.</param>
        /// <param name="bookingText">Der Buchungstext.</param>
        /// <returns>Die Buchungsnummer</returns>
        Booking Book(Account sender, Account recipient, double amount, string bookingText);

        /// <summary>
        ///     Ruft chronologisch alle Buchungen eines Kontos ab.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        IPage<BookingEntry> FindByAccount(IPageable pageRequest, Account account);
    }
}