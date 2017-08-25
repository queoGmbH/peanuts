using System.Collections.Generic;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

using NHibernate;
using NHibernate.Criterion;

using Spring.Data.NHibernate.Generic;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    /// <summary>
    ///     Konkrete Implementierung für die Persistierung von <see cref="UserGroup" />.
    /// </summary>
    public class UserGroupDao : GenericDao<UserGroup, int>, IUserGroupDao {
        /// <summary>
        ///     Liefert <code>true</code>, wenn der Gruppe Nutzer (egal in welchem Verhältnis) zugeordnet sind, ansonsten
        ///     <code>false</code>.
        /// </summary>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        public bool AreUsersAssigned(UserGroup userGroup) {
            Require.NotNull(userGroup, nameof(userGroup));

            HibernateDelegate<bool> findAssignedUserCount = delegate(ISession session) {
                return session.QueryOver<UserGroupMembership>().Where(membership => membership.UserGroup == userGroup).RowCountInt64() > 0;
            };
            return HibernateTemplate.Execute(findAssignedUserCount);
        }

        public IPage<UserGroupMembership> FindMembershipsByGroups(IPageable pageRequest, IList<UserGroup> userGroups,
            IList<UserGroupMembershipType> membershipTypes = null) {
            Require.NotNull(pageRequest, "pageRequest");
            Require.NotNull(userGroups, "userGroup");

            HibernateDelegate<IPage<UserGroupMembership>> finder = delegate(ISession session) {
                IQueryOver<UserGroupMembership, UserGroupMembership> queryOver = session.QueryOver<UserGroupMembership>();
                queryOver.WhereRestrictionOn(membership => membership.UserGroup).IsIn(userGroups.ToList());

                if (membershipTypes != null && membershipTypes.Any()) {
                    queryOver.WhereRestrictionOn(memberShip => memberShip.MembershipType).IsIn(membershipTypes.ToArray());
                }

                return FindPage(queryOver, pageRequest);
            };
            return HibernateTemplate.Execute(finder);
        }

        public IPage<UserGroupMembership> FindMembershipsByUser(IPageable pageRequest, User user,
            IList<UserGroupMembershipType> membershipTypes = null) {
            Require.NotNull(pageRequest, "pageRequest");
            Require.NotNull(user, "user");

            HibernateDelegate<IPage<UserGroupMembership>> finder = delegate(ISession session) {
                IQueryOver<UserGroupMembership, UserGroupMembership> queryOver = session.QueryOver<UserGroupMembership>();
                queryOver.Where(membership => membership.User == user);

                if (membershipTypes != null && membershipTypes.Any()) {
                    queryOver.WhereRestrictionOn(memberShip => memberShip.MembershipType).IsIn(membershipTypes.ToArray());
                }

                return FindPage(queryOver, pageRequest);
            };
            return HibernateTemplate.Execute(finder);
        }

        public UserGroupMembership Save(UserGroupMembership membership) {
            Require.NotNull(membership, "membership");

            HibernateTemplate.SaveOrUpdate(membership);

            return membership;
        }

        public IPage<UserGroupMembership> FindOtherMembershipsInUsersGroups(IPageable pageRequest, User user, IList<UserGroupMembershipType> membershipTypes = null) {
            Require.NotNull(pageRequest, "pageRequest");
            Require.NotNull(user, "user");

            HibernateDelegate<IPage<UserGroupMembership>> finder = delegate (ISession session) {
                
                IQueryOver<UserGroupMembership, UserGroupMembership> queryOver = session.QueryOver<UserGroupMembership>();
                /*Die Mitgliedschaften des Nutzers nicht!*/
                queryOver.Where(membership => membership.User != user);

                /*Nur die der Gruppen des Nutzers*/
                var userGroupQueryOver = QueryOver.Of<UserGroupMembership>().Where(mem => mem.User == user).And(mem => mem.MembershipType == UserGroupMembershipType.Administrator || mem.MembershipType == UserGroupMembershipType.Member).Select(mem => mem.UserGroup.Id);
                queryOver.WithSubquery.WhereProperty(membership => membership.UserGroup).In(userGroupQueryOver);

                if (membershipTypes != null && membershipTypes.Any()) {
                    /*Einschränkung der Mitgliedschaften*/
                    queryOver.WhereRestrictionOn(memberShip => memberShip.MembershipType).IsIn(membershipTypes.ToArray());
                }

                return FindPage(queryOver, pageRequest);
            };
            return HibernateTemplate.Execute(finder);
        }

        public UserGroupMembership FindMembershipsByUserAndGroup(User user, UserGroup userGroup) {
            Require.NotNull(userGroup, "userGroup");
            Require.NotNull(user, "user");

            HibernateDelegate<UserGroupMembership> finder = delegate (ISession session) {

                IQueryOver<UserGroupMembership, UserGroupMembership> queryOver = session.QueryOver<UserGroupMembership>();
                queryOver
                    /*Mitgliedschaft des Nutzers*/
                    .Where(membership => membership.User == user)
                    /*In der Gruppe*/
                    .And(membership => membership.UserGroup == userGroup);

                return queryOver.SingleOrDefault();
            };
            return HibernateTemplate.Execute(finder);
        }

        public IPage<UserGroup> FindUserGroupsWhereUserIsNoMember(IPageable pageRequest, User user) {
                Require.NotNull(pageRequest, "pageRequest");
                Require.NotNull(user, "user");

                HibernateDelegate<IPage<UserGroup>> finder = delegate (ISession session) {
                    var subQuery = QueryOver.Of<UserGroupMembership>().Where(membership => membership.User == user).Select(mem => mem.UserGroup.Id);
                    IQueryOver<UserGroup, UserGroup> queryOver = session.QueryOver<UserGroup>();
                    queryOver.WithSubquery.WhereProperty(g => g.Id).NotIn(subQuery);
                    
                    return FindPage(queryOver, pageRequest);
                };
                return HibernateTemplate.Execute(finder);
            }
    }
}