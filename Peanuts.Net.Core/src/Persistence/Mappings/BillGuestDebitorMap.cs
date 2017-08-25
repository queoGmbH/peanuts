using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    public class BillGuestDebitorMap : EntityMap<BillGuestDebitor> {
        public BillGuestDebitorMap() {
            Map(debitor => debitor.Name).Not.Nullable().Length(255);
            Map(debitor => debitor.Email).Not.Nullable().Length(255);
            Map(debitor => debitor.Portion).Not.Nullable();
        }
    }
}