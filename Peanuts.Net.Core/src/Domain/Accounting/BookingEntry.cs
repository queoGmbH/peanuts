using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Transactions;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {
    /// <summary>
    ///     Stellt eine konkrete Buchung auf einem Konto dar.
    /// </summary>
    public abstract class BookingEntry : Entity {
        private readonly Account _account;
        private readonly Booking _booking;

        public BookingEntry() {
        }

        protected BookingEntry(Booking booking, Account account) {
            _booking = booking;
            _account = account;
        }

        /// <summary>
        ///     Ruft das Konto ab, auf dem die Buchung durchgeführt wird.
        /// </summary>
        public virtual Account Account {
            get { return _account; }
        }

        /// <summary>
        /// Ruft das entgegengesetzte Konto ab, das im Gegenzug be- oder entlastet wurde.
        /// </summary>
        public virtual Account OpponentAccount {
            get {
                if (BookingEntryType == BookingEntryType.In) {
                    return Booking.Sender;
                } else {
                    return Booking.Recipient;
                }
            }
        }

        /// <summary>
        ///     Ruft die Betrag ab, um die es bei der Buchung geht.
        /// </summary>
        public abstract double Amount { get; }

        public virtual Booking Booking {
            get { return _booking; }
        }

        /// <summary>
        ///     Ruft ab, ob es sich bei der Buchung um eine Eingangs- oder Ausgangsbuchung handelt.
        /// </summary>
        public abstract BookingEntryType BookingEntryType { get; }

        /// <summary>
        ///     Ruft die eindeutige Id der Buchung ab, zu der dieser Eintrag gemacht wurde.
        ///     Es gibt jeweils 2 Buchungseinträge mit dieser Id.
        /// </summary>
        public virtual long BookingId {
            get { return _booking.Id; }
        }

        /// <summary>
        ///     Ruft das Datum der Buchung ab.
        /// </summary>
        public virtual DateTime EntryDate {
            get { return _booking.BookingDate; }
        }

        /// <summary>
        ///     Ruft einen Buchungstext ab.
        /// </summary>
        public virtual string EntryText {
            get { return _booking.BookingText; }
        }
    }
}