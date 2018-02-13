using System;
using System.Collections.Generic;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

using Spring.Transaction.Interceptor;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    /// <summary>
    ///     Konkrete Implementierung für <see cref="IUserGroupService" />.
    /// </summary>
    public class UserGroupService : IUserGroupService {
        public UserGroupService() {
        }

        public UserGroupService(IUserGroupDao userGroupDao) {
            UserGroupDao = userGroupDao;
        }

        /// <summary>
        ///     Liefert oder setzt den NotificationService
        /// </summary>
        public INotificationService NotificationService { get; set; }

        public IUserGroupDao UserGroupDao { get; set; }

        /// <summary>
        ///     Fügt neue Mitglieder zu einer Gruppe hinzu.
        /// </summary>
        /// <param name="userGroup">Die Gruppe der die Nutzer hinzugefügt werden sollen.</param>
        /// <param name="usersToAdd">Die Nutzer die der Gruppe hinzugefügt werden sollen.</param>
        /// <param name="createdBy">Wer fügt das Mitglied hinzu.</param>
        /// <returns></returns>
        [Transaction]
        public IList<UserGroupMembership> AddMembers(UserGroup userGroup, IDictionary<User, UserGroupMembershipType> usersToAdd, User createdBy) {
            Require.NotNull(userGroup, "userGroup");
            Require.NotNull(usersToAdd, "usersToAdd");
            Require.NotNull(createdBy, "createdBy");

            List<UserGroupMembership> memberships = new List<UserGroupMembership>();

            IPage<UserGroupMembership> currentMemberships = UserGroupDao.FindMembershipsByGroups(PageRequest.All, new List<UserGroup> { userGroup });
            Dictionary<User, UserGroupMembership> currentMembershipsByUser = currentMemberships.GroupBy(member => member.User)
                    .ToDictionary(g => g.Key, g => g.Single());
            foreach (KeyValuePair<User, UserGroupMembershipType> userToAdd in usersToAdd) {
                if (currentMembershipsByUser.ContainsKey(userToAdd.Key)) {
                    /*Nutzer ist oder war bereits Mitglied der Gruppe*/
                    UserGroupMembership existingMembership = currentMembershipsByUser[userToAdd.Key];
                    if (existingMembership.MembershipType != userToAdd.Value) {
                        existingMembership.Update(userToAdd.Value, new UserGroupMembershipDto(), new EntityChangedDto(createdBy, DateTime.Now));
                    }
                    memberships.Add(existingMembership);
                } else {
                    /*Neue Mitgliedschaft*/
                    UserGroupMembership newMembership = new UserGroupMembership(userGroup,
                        userToAdd.Key,
                        userToAdd.Value,
                        new EntityCreatedDto(createdBy, DateTime.Now));
                    UserGroupDao.Save(newMembership);
                    memberships.Add(newMembership);
                }
            }

            return memberships;
        }

        /// <summary>
        ///     Liefert <code>true</code>, wenn der Maklergrupper Nutzer zugeordnet sind, ansonsten <code>false</code>.
        /// </summary>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        public bool AreUsersAssigned(UserGroup userGroup) {
            Require.NotNull(userGroup, nameof(userGroup));
            return UserGroupDao.AreUsersAssigned(userGroup);
        }

        /// <summary>
        ///     Liefert <code>true</code>, wenn der Nutzer an einem Peanut der Gruppe teilnehmen darf, ansonsten <code>false</code>
        /// </summary>
        /// <param name="userGroupMembership"></param>
        /// <returns></returns>
        public bool IsUserSolvent(UserGroupMembership userGroupMembership) {
            Require.NotNull(userGroupMembership, nameof(userGroupMembership));
            if (userGroupMembership.UserGroup.BalanceOverdraftLimit.HasValue) {
                return userGroupMembership.Account.Balance >= userGroupMembership.UserGroup.BalanceOverdraftLimit;
            }
            return true;
        }

        /// <summary>
        ///     Erzeugt einen neuen Gruppen.
        /// </summary>
        /// <param name="userGroupDto">Die allgemeinen Daten für den Gruppen</param>
        /// <param name="createdBy">Der Nutzer der den Gruppen erstellt</param>
        /// <returns></returns>
        [Transaction]
        public UserGroup Create(UserGroupDto userGroupDto, IDictionary<User, UserGroupMembershipType> initialUsers, User createdBy) {
            Require.NotNull(userGroupDto, nameof(userGroupDto));
            Require.NotNull(createdBy, nameof(createdBy));
            Require.NotNull(initialUsers, "initialUsers");

            DateTime createdAt = DateTime.Now;
            EntityCreatedDto entityCreatedDto = new EntityCreatedDto(createdBy, createdAt);
            UserGroup userGroup = new UserGroup(userGroupDto, entityCreatedDto);
            userGroup = UserGroupDao.Save(userGroup);

            foreach (KeyValuePair<User, UserGroupMembershipType> userGroupMembership in initialUsers) {
                UserGroupDao.Save(new UserGroupMembership(userGroup,
                    userGroupMembership.Key,
                    userGroupMembership.Value,
                    new EntityCreatedDto(createdBy, DateTime.Now)));
            }

            return userGroup;
        }

        /// <summary>
        ///     Löscht den Gruppen.
        /// </summary>
        /// <param name="userGroup"></param>
        [Transaction]
        public void Delete(UserGroup userGroup) {
            Require.NotNull(userGroup, nameof(userGroup));
            if (UserGroupDao.AreUsersAssigned(userGroup)) {
                throw new InvalidOperationException("Die Nutzergruppe kann nicht gelöscht werden, da noch Nutzer zugeordnet sind.");
            }
            UserGroupDao.Delete(userGroup);
        }

        public IPage<UserGroupMembership> FindMembershipsByGroups(IPageable pageRequest, IList<UserGroup> userGroups,
            IList<UserGroupMembershipType> membershipTypes = null) {
            return UserGroupDao.FindMembershipsByGroups(pageRequest, userGroups, membershipTypes);
        }

        public IPage<UserGroupMembership> FindMembershipsByUser(IPageable pageRequest, User user,
            IList<UserGroupMembershipType> membershipTypes = null) {
            return UserGroupDao.FindMembershipsByUser(pageRequest, user, membershipTypes);
        }

        /// <summary>
        ///     Sucht nach der Mitgliedschaft eines Nutzers in einer Gruppe.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        public UserGroupMembership FindMembershipByUserAndGroup(User user, UserGroup userGroup) {
            return UserGroupDao.FindMembershipsByUserAndGroup(user, userGroup);
        }

        /// <summary>
        ///     Ruft die Mitgliedschaften anderer Nutzer in den Gruppen ab, in denen ich Mitglied bin.
        /// </summary>
        /// <param name="pageRequest">Seiteninformationen</param>
        /// <param name="user">Der Nutzer, von dessen Gruppen die Mitgliedschaften abgerufen werden sollen.</param>
        /// <param name="membershipTypes">Die zu berücksichtigenden Arten von Mitgliedschaften oder NULL, wenn das egal ist.</param>
        /// <returns></returns>
        public IPage<UserGroupMembership> FindOtherMembershipsInUsersGroups(IPageable pageRequest, User user,
            IList<UserGroupMembershipType> membershipTypes = null) {
            return UserGroupDao.FindOtherMembershipsInUsersGroups(pageRequest, user, membershipTypes);
        }

        /// <summary>
        ///     Ruft die Gruppen ab, in denen der Nutzer kein Mitglied ist.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public IPage<UserGroup> FindUserGroupsWhereUserIsNoMember(IPageable pageRequest, User user) {
            return UserGroupDao.FindUserGroupsWhereUserIsNoMember(pageRequest, user);
        }

        /// <summary>
        ///     Liefert eine Page von Gruppen entsprechend dem <see cref="IPageable" />
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <returns></returns>
        public IPage<UserGroup> GetAll(IPageable pageRequest) {
            IPage<UserGroup> userGroups = UserGroupDao.GetAll(pageRequest);
            return userGroups;
        }

        /// <summary>
        ///     Liefert alle Gruppen
        /// </summary>
        /// <returns></returns>
        public IList<UserGroup> GetAll() {
            IList<UserGroup> userGroups = UserGroupDao.GetAll();
            return userGroups;
        }

        /// <summary>
        ///     Ruft die Anzahl der im System erfassten Gruppen ab.
        /// </summary>
        /// <returns></returns>
        public long GetCount() {
            return UserGroupDao.GetCount();
        }

        /// <summary>
        ///     Lädt den Nutzer zu einer Mitgliedschaft in einer Gruppe ein.
        /// </summary>
        /// <param name="userGroup"></param>
        /// <param name="user"></param>
        /// <param name="currentUser"></param>
        /// <param name="allMembershipsUrl"></param>
        [Transaction]
        public void Invite(UserGroup userGroup, User user, User currentUser, string allMembershipsUrl) {
            AddMembers(userGroup, new Dictionary<User, UserGroupMembershipType> { { user, UserGroupMembershipType.Invited } }, currentUser);
            NotificationService.SendUserGroupInvitationNotification(user, allMembershipsUrl);
        }

        [Transaction]
        public void QuitOrRemoveMemberships(IList<UserGroupMembership> membershipsToQuitOrRemove, User changedBy) {
            Require.NotNull(membershipsToQuitOrRemove, "membershipsToQuitOrRemove");
            Require.NotNull(changedBy, "changedBy");

            foreach (UserGroupMembership userGroupMembership in membershipsToQuitOrRemove) {
                userGroupMembership.Update(UserGroupMembershipType.Quit, new EntityChangedDto(changedBy, DateTime.Now));
            }
        }

        /// <summary>
        ///     Fordert eine Mitgliedschaft für eine Gruppe an und versendet eine Benachrichtigung an alle Admins dieser Gruppe
        /// </summary>
        /// <param name="userGroup"></param>
        /// <param name="currentUser"></param>
        /// <param name="editUrl"></param>
        [Transaction]
        public void RequestMembership(UserGroup userGroup, User currentUser, string editUrl) {
            AddMembers(userGroup,
                new Dictionary<User, UserGroupMembershipType> { { currentUser, UserGroupMembershipType.Request } },
                currentUser);
            IPage<UserGroupMembership> memberships = FindMembershipsByGroups(PageRequest.All,
                new List<UserGroup>() { userGroup },
                new List<UserGroupMembershipType>() { UserGroupMembershipType.Administrator });
            foreach (UserGroupMembership userGroupMembership in memberships) {
                NotificationService.SendRequestMembershipNotification(userGroupMembership.User, editUrl);
            }
        }

        /// <summary>
        ///     Aktualisiert einen Gruppen.
        /// </summary>
        /// <param name="userGroup">Zu aktualisierender Gruppen.</param>
        /// <param name="userGroupDto">Die "neuen" Daten.</param>
        /// <param name="changedBy">Nutzer der die Änderungen durchführt.</param>
        /// <returns></returns>
        [Transaction]
        public UserGroup Update(UserGroup userGroup, UserGroupDto userGroupDto, User changedBy) {
            Require.NotNull(userGroup, nameof(userGroup));
            Require.NotNull(userGroupDto, nameof(userGroupDto));
            Require.NotNull(changedBy, nameof(changedBy));

            if (userGroupDto.Equals(userGroup.GetDto())) {
                return userGroup;
            }

            EntityChangedDto entityChangedDto = new EntityChangedDto(changedBy, DateTime.Now);
            userGroup.Update(userGroupDto, entityChangedDto);
            return userGroup;
        }

        [Transaction]
        public void UpdateMembershipTypes(IDictionary<UserGroupMembership, UserGroupMembershipType> memberships, User changedBy) {
            Require.NotNull(memberships, "memberships");
            Require.NotNull(changedBy, "changedBy");

            foreach (UserGroupMembership userGroupMembership in memberships.Keys) {
                UserGroupMembershipType newMembershipType = memberships[userGroupMembership];
                if (userGroupMembership.MembershipType != newMembershipType) {
                    userGroupMembership.Update(newMembershipType, new EntityChangedDto(changedBy, DateTime.Now));
                }
            }
        }
    }
}