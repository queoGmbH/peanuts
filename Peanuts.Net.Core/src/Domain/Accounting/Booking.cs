using System;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {
    /// <summary>
    ///     Diese Klasse beschreibt eine Buchung eines Betrags von einem Sender zu einem Empfänger
    /// </summary>
    public class Booking : Entity {
        private readonly double _amount;
        private readonly DateTime _bookingDate;
        private readonly string _bookingText;

        private readonly RecipientBookingEntry _recipientAccountEntry;
        private readonly SenderBookingEntry _senderAccountEntry;

        /// <summary>
        ///     Standardkonstruktor
        /// </summary>
        public Booking() {
        }

        /// <summary>
        ///     Erzeugt eine neue Instanz des <see cref="Booking" />
        /// </summary>
        /// <param name="sender">Der Sender</param>
        /// <param name="recipient">Der Empfänger</param>
        /// <param name="amount">Der Betrag</param>
        /// <param name="bookingDate">Das Buchungsdatum</param>
        /// <param name="bookingText">Der Buchungstext</param>
        public Booking(Account sender, Account recipient, double amount, DateTime bookingDate, string bookingText) {
            Require.NotNull(sender, "sender");
            Require.NotNull(recipient, "recipient");
            Require.Gt(amount, 0, "amount");

            _bookingDate = bookingDate;
            _bookingText = bookingText;
            _amount = amount;

            _recipientAccountEntry = new RecipientBookingEntry(this, recipient);
            _senderAccountEntry = new SenderBookingEntry(this, sender);
        }

        /// <summary>
        ///     Liefert den Betrag welcher verbucht wurde
        /// </summary>
        public virtual double Amount {
            get { return _amount; }
        }

        /// <summary>
        ///     Liefert das Datum der Buchung
        /// </summary>
        public virtual DateTime BookingDate {
            get { return _bookingDate; }
        }

        /// <summary>
        ///     Liefert den Buchungstext
        /// </summary>
        public virtual string BookingText {
            get { return _bookingText; }
        }

        /// <summary>
        ///     Liefert den Empfänger der Buchung
        /// </summary>
        public virtual Account Recipient {
            get { return _recipientAccountEntry.Account; }
        }

        /// <summary>
        ///     Liefert den Buchungseintrag der beim Empfänger hinterlegt wurde
        /// </summary>
        public virtual RecipientBookingEntry RecipientAccountEntry {
            get { return _recipientAccountEntry; }
        }

        /// <summary>
        ///     Liefert den Sender der Buchung
        /// </summary>
        public virtual Account Sender {
            get { return _senderAccountEntry.Account; }
        }

        /// <summary>
        ///     Liefert den Buchungseintrag der beim Sender hinterlegt wurde
        /// </summary>
        public virtual SenderBookingEntry SenderAccountEntry {
            get { return _senderAccountEntry; }
        }
    }
}