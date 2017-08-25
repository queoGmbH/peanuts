using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    public class UserGroupMembershipDao : GenericDao<UserGroupMembership, int>, IUserGroupMembershipDao {
    }
}