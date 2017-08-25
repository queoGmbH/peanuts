using Com.QueoFlow.Peanuts.Net.Core.Domain.Transactions;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {
    /// <summary>
    ///     Beschreibt eine eingehende Buchung.
    /// </summary>
    public class RecipientBookingEntry : BookingEntry {
        public RecipientBookingEntry() {
        }

        public RecipientBookingEntry(Booking booking, Account account) : base(booking, account) {
        }

        public override double Amount {
            get {
                /*Der Betrag wird gutgeschrieben und ist damit positiv.*/
                return Booking.Amount;
            }
        }

        public override BookingEntryType BookingEntryType {
            get { return BookingEntryType.In; }
        }
    }
}