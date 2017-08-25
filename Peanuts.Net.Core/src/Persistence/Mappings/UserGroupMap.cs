using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    public class UserGroupMap : EntityMap<UserGroup> {
        protected UserGroupMap() {
            Map(financialBrokerPool => financialBrokerPool.AdditionalInformations).Nullable().Length(4000);
            Map(financialBrokerPool => financialBrokerPool.ChangedAt).Nullable();
            References(financialBrokerPool => financialBrokerPool.ChangedBy).Nullable().ForeignKey("FK_USER_GROUP_CHANGED_BY_USER");

            Map(financialBrokerPool => financialBrokerPool.CreatedAt).Not.Nullable();
            References(financialBrokerPool => financialBrokerPool.CreatedBy).Not.Nullable().ForeignKey("FK_USER_GROUP_CREATED_BY_USER");

            Map(financialBrokerPool => financialBrokerPool.Name).Not.Nullable().Length(255);
            Map(financialBrokerPool => financialBrokerPool.BalanceOverdraftLimit).Nullable();
        }
    }
}