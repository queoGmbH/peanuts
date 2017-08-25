using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;

using FluentNHibernate.Mapping;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    public class BookingEntryMap : EntityMap<BookingEntry> {
        protected BookingEntryMap() {
            References(entry => entry.Booking).ForeignKey("FK_BOOKINGENTRY_BOOKING").Not.Nullable().UniqueKey("UIDX_BOOKING_PER_TYPE");
            References(entry => entry.Account).ForeignKey("FK_ACCOUNT_WITH_BOOKING_ENTRIES").Not.Nullable().Access.CamelCaseField(Prefix.Underscore);

            DiscriminateSubClassesOnColumn("BookingEntryType").SqlType("nvarchar(100)").UniqueKey("UIDX_BOOKING_PER_TYPE");
        }
    }
}