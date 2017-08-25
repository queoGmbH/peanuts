using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Transactions;

using FluentNHibernate.Mapping;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    public class RecipientBookingEntryMap : SubclassMap<RecipientBookingEntry> {
        public RecipientBookingEntryMap() {
            DiscriminatorValue(BookingEntryType.In);
        }
    }
}