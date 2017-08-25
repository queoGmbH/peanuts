using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    public class AccountMap : EntityMap<Account> {
        public AccountMap() {
            Map(account => account.Balance).Not.Nullable();
            HasOne(account => account.Membership)
                    .ForeignKey("FK_MEMBERSHIP_ACCOUNT")
                    .Cascade.All()
                    .PropertyRef(Objects.GetPropertyName<UserGroupMembership>(mem => mem.Account));
        }
    }
}