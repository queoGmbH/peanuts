using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

using FluentNHibernate.Mapping;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    /// <summary>
    ///     Bildet das Mapping für einen <see cref="UserGroupMembership" /> ab.
    /// </summary>
    public class UserGroupMembershipMap : EntityMap<UserGroupMembership> {
        public UserGroupMembershipMap() {
            References(membership => membership.User).ForeignKey("FK_USER_IN_GROUP").UniqueKey("UIDX_USER_PER_GROUP");
            References(membership => membership.UserGroup).ForeignKey("FK_GROUP_WITH_USER").UniqueKey("UIDX_USER_PER_GROUP");

            References(user => user.CreatedBy).Nullable().NotFound.Ignore();
            Map(user => user.CreatedAt).Not.Nullable();

            References(user => user.ChangedBy).Nullable().NotFound.Ignore();
            Map(user => user.ChangedAt).Nullable();

            Map(user => user.MembershipType).Not.Nullable();
            References(user => user.Account).ForeignKey("FK_MEMBERSHIP_ACCOUNT").Cascade.All().LazyLoad(Laziness.False);
        }
    }
    
}