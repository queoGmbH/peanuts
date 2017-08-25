using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Transactions;

using FluentNHibernate.Mapping;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    public class SenderBookingEntryMap : SubclassMap<SenderBookingEntry> {
        public SenderBookingEntryMap() {
            DiscriminatorValue(BookingEntryType.Out);
        }
    }
}