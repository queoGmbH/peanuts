using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;

using FluentNHibernate.Mapping;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    public class BillMap : EntityMap<Bill> {
        public BillMap() {
            References(bill => bill.UserGroup).Not.Nullable().ForeignKey("FK_USERGROUP_WITH_BILLS");
            Map(bill => bill.Amount).Not.Nullable();
            Map(bill => bill.Subject).Not.Nullable().Length(254);
            References(bill => bill.Creditor).Nullable().ForeignKey("FK_CREDITOR_OF_BILL");

            HasMany(bill => bill.UserGroupDebitors)
                    .Cascade.AllDeleteOrphan()
                    .Access.CamelCaseField(Prefix.Underscore)
                    .Table("tblBillUserGroupDebitor")
                    .ForeignKeyConstraintName("FK_BILL_WITH_GROUP_CREDITORS");

            HasMany(bill => bill.GuestDebitors)
                    .Cascade.AllDeleteOrphan()
                    .Access.CamelCaseField(Prefix.Underscore)
                    .Table("tblBillGestDebitor")
                    .ForeignKeyConstraintName("FK_BILL_WITH_GUEST_CREDITORS");

            References(bill => bill.CreatedBy).Not.Nullable().ForeignKey("FK_CREATOR_OF_BILL");
            Map(bill => bill.CreatedAt).Not.Nullable();
            References(bill => bill.ChangedBy).Nullable().NotFound.Ignore();
            Map(bill => bill.ChangedAt).Nullable();

            Map(bill => bill.IsSettled).Not.Nullable();
            Map(bill => bill.SettlementDate).Nullable();
        }
    }
}