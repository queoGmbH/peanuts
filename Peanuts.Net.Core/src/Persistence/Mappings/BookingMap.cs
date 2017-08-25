using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    public class BookingMap : EntityMap<Booking> {
        public BookingMap() {
            HasOne(booking => booking.RecipientAccountEntry).ForeignKey("FK_BOOKING_RECIPIENT_ENTRY").Cascade.All().PropertyRef(Objects.GetPropertyName<BookingEntry>(e => e.Booking));
            HasOne(booking => booking.SenderAccountEntry).ForeignKey("FK_BOOKING_SENDER_ENTRY").Cascade.All().PropertyRef(Objects.GetPropertyName<BookingEntry>(e => e.Booking));

            Map(booking => booking.Amount).Not.Nullable();
            Map(booking => booking.BookingDate).Not.Nullable();
            Map(booking => booking.BookingText).Length(1000);
        }
    }
}