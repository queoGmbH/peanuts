using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

using Spring.Transaction.Interceptor;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    /// <summary>
    ///     Diese Klasse beschreibt einen Service für Buchungsaufgaben
    /// </summary>
    public class BookingService : IBookingService {
        /// <summary>
        ///     Liefert oder setzt den DAO für Buchungen
        /// </summary>
        public IBookingDao BookingDao { get; set; }

        /// <summary>
        ///     Führt eine Buchung durch, in dem auf dem Empfänger-Konto der Betrag gutgeschrieben wird und auf dem Absende-Konto
        ///     der Betrag abgebucht wird.
        /// </summary>
        /// <param name="sender">Das Sender-Konto.</param>
        /// <param name="recipient">Das Empfänger-Konto.</param>
        /// <param name="amount">Der zu buchende Betrag.</param>
        /// <param name="bookingText">Der Buchungstext.</param>
        /// <returns>Die Buchungsnummer</returns>
        [Transaction]
        public Booking Book(Account sender, Account recipient, double amount, string bookingText) {
            Require.Gt(amount, 0, "amount");
            Require.NotNull(recipient, "recipient");
            Require.NotNull(sender, "sender");
            Require.NotNullOrWhiteSpace(bookingText, "bookingText");

            /*Buchung erstellen.*/
            Booking booking = new Booking(sender, recipient, amount, DateTime.Now, bookingText);

            /*Dem Nutzer der das Geld gesendet hat, wird es seinem Konto gutgeschrieben*/
            sender.Book(-amount);

            /*Dem Nutzer der das Geld erhalten hat, wird es von seinem Konto abgezogen*/
            recipient.Book(amount);

            BookingDao.Save(booking);

            return booking;
        }

        /// <summary>
        ///     Ruft chronologisch alle Buchungen eines Kontos ab.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public IPage<BookingEntry> FindByAccount(IPageable pageRequest, Account account) {
            Require.NotNull(account, "account");
            return BookingDao.FindByAccount(pageRequest, account);
        }
    }
}