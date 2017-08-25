using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    /// <summary>
    ///     Schnittstelle für Persistierung für <see cref="UserGroup" />.
    /// </summary>
    public interface IUserGroupDao : IGenericDao<UserGroup, int> {
        /// <summary>
        ///     Liefert <code>true</code>, wenn der Gruppe noch Nutzer zugeordnet sind, ansonsten <code>false</code>.
        /// </summary>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        bool AreUsersAssigned(UserGroup userGroup);

        IPage<UserGroupMembership> FindMembershipsByGroups(IPageable pageRequest, IList<UserGroup> userGroups, IList<UserGroupMembershipType> membershipTypes = null);

        IPage<UserGroupMembership> FindMembershipsByUser(IPageable pageRequest, User user, IList<UserGroupMembershipType> membershipTypes = null);

        UserGroupMembership Save(UserGroupMembership membership);

        /// <summary>
        /// Ruft die Mitgliedschaften anderer Nutzer in den Gruppen ab, in denen ich Mitglied bin.
        /// </summary>
        /// <param name="pageRequest">Seiteninformationen</param>
        /// <param name="user">Der Nutzer, von dessen Gruppen die Mitgliedschaften abgerufen werden sollen.</param>
        /// <param name="membershipTypes">Die zu berücksichtigenden Arten von Mitgliedschaften oder NULL, wenn das egal ist.</param>
        /// <returns></returns>
        IPage<UserGroupMembership> FindOtherMembershipsInUsersGroups(IPageable pageRequest, User user, IList<UserGroupMembershipType> membershipTypes = null);

        /// <summary>
        /// Sucht nach der Mitgliedschaft eines Nutzers in einer Gruppe.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        UserGroupMembership FindMembershipsByUserAndGroup(User user, UserGroup userGroup);

        /// <summary>
        /// Ruft die Gruppen ab, in denen der Nutzer kein Mitglied ist.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        IPage<UserGroup> FindUserGroupsWhereUserIsNoMember(IPageable pageRequest, User user);
    }
}