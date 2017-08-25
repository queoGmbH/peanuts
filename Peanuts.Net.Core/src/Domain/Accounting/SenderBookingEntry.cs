using Com.QueoFlow.Peanuts.Net.Core.Domain.Transactions;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting {
    /// <summary>
    ///     Beschreibt einen abgehende Buchung.
    /// </summary>
    public class SenderBookingEntry : BookingEntry {
        public SenderBookingEntry() {
        }

        public SenderBookingEntry(Booking booking, Account account) : base(booking, account) {
        }

        public override double Amount {
            get {
                /*Die Summe wird abgezogen und ist damit negativ*/
                return -Booking.Amount;
            }
        }

        public override BookingEntryType BookingEntryType {
            get { return BookingEntryType.Out; }
        }
    }
}