using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    public class BillUserGroupDebitorMap : EntityMap<BillUserGroupDebitor> {
        public BillUserGroupDebitorMap() {
            References(bill => bill.UserGroupMembership).Not.Nullable().ForeignKey("FK_USERGROUPMEMBER_WITH_BILLS");

            Map(debitor => debitor.BillAcceptState).Not.Nullable();
            Map(debitor => debitor.Portion).Not.Nullable();

            Map(debitor => debitor.RefuseComment).Length(1000);
        }
    }
}