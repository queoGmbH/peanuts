using System;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    public interface IUserGroupService {
        /// <summary>
        ///     Fügt neue Mitglieder zu einer Gruppe hinzu.
        /// </summary>
        /// <param name="userGroup">Die Gruppe der die Nutzer hinzugefügt werden sollen.</param>
        /// <param name="usersToAdd">Die Nutzer die der Gruppe hinzugefügt werden sollen.</param>
        /// <param name="createdBy">Wer fügt das Mitglied hinzu.</param>
        /// <returns></returns>
        IList<UserGroupMembership> AddMembers(UserGroup userGroup, IDictionary<User, UserGroupMembershipType> usersToAdd, User createdBy);

        /// <summary>
        ///     Liefert <code>true</code>, wenn der Gruppe Nutzer zugeordnet sind, ansonsten <code>false</code>.
        /// </summary>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        bool AreUsersAssigned(UserGroup userGroup);

        /// <summary>
        ///     Liefert <code>true</code>, wenn der Nutzer an einem Peanut der Gruppe teilnehmen darf, ansonsten <code>false</code>
        /// </summary>
        /// <param name="userGroupMembership"></param>
        /// <returns></returns>
        bool IsUserSolvent(UserGroupMembership userGroupMembership);

        /// <summary>
        ///     Erzeugt eine neue Gruppe.
        /// </summary>
        /// <param name="userGroupDto">Die allgemeinen Daten für die Gruppe</param>
        /// <param name="createdBy">Der Nutzer der die Gruppe erstellt</param>
        /// <returns></returns>
        UserGroup Create(UserGroupDto userGroupDto, IDictionary<User, UserGroupMembershipType> initialUsers, User createdBy);

        /// <summary>
        ///     Löscht die Gruppe.
        /// </summary>
        /// <param name="userGroup"></param>
        void Delete(UserGroup userGroup);

        /// <summary>
        ///     Sucht nach Mitgliedschaften von Nutzern in einer Gruppe.
        /// </summary>
        /// <param name="pageRequest">Seiteninformationen</param>
        /// <param name="userGroups">Die Gruppen deren Mitgliedschaften abgerufen werden sollen.</param>
        /// <param name="membershipTypes">Die zu berücksichtigenden Arten von Mitgliedschaften oder NULL, wenn das egal ist.</param>
        /// <returns></returns>
        IPage<UserGroupMembership> FindMembershipsByGroups(IPageable pageRequest, IList<UserGroup> userGroups, IList<UserGroupMembershipType> membershipTypes = null);

        /// <summary>
        /// Ruft die Mitgliedschaften anderer Nutzer in den Gruppen ab, in denen ich Mitglied bin.
        /// </summary>
        /// <param name="pageRequest">Seiteninformationen</param>
        /// <param name="user">Der Nutzer, von dessen Gruppen die Mitgliedschaften abgerufen werden sollen.</param>
        /// <param name="membershipTypes">Die zu berücksichtigenden Arten von Mitgliedschaften oder NULL, wenn das egal ist.</param>
        /// <returns></returns>
        IPage<UserGroupMembership> FindOtherMembershipsInUsersGroups(IPageable pageRequest, User user, IList<UserGroupMembershipType> membershipTypes = null);

        /// <summary>
        ///     Ruft die Mitgliedschaften eines Nutzers ab.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        IPage<UserGroupMembership> FindMembershipsByUser(IPageable pageRequest, User user, IList<UserGroupMembershipType> membershipTypes = null);

        /// <summary>
        ///     Liefert eine Page von Gruppen entsprechend dem <see cref="IPageable" />
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <returns></returns>
        IPage<UserGroup> GetAll(IPageable pageRequest);

        /// <summary>
        ///     Liefert alle Gruppen
        /// </summary>
        /// <returns></returns>
        IList<UserGroup> GetAll();

        /// <summary>
        ///     Ruft die Anzahl der im System erfassten Gruppen ab.
        /// </summary>
        /// <returns></returns>
        long GetCount();

        /// <summary>
        ///     Löst eine Mitgliedschaft eines Nutzers in einer Gruppe auf, in dem die Mitgliedschaft den Status
        ///     <see cref="UserGroupMembershipType.Quit" /> erhält oder komplett entfernt wird.
        /// </summary>
        /// <param name="membershipsToQuitOrRemove"></param>
        /// <param name="changedBy">Von wem wird die Änderung durchgeführt.</param>
        void QuitOrRemoveMemberships(IList<UserGroupMembership> membershipsToQuitOrRemove, User changedBy);

        /// <summary>
        ///     Aktualisiert eine Gruppe.
        /// </summary>
        /// <param name="userGroup">Zu aktualisierende Gruppe.</param>
        /// <param name="userGroupDto">Die "neuen" Daten.</param>
        /// <param name="changedBy">Nutzer der die Änderungen durchführt.</param>
        /// <returns></returns>
        UserGroup Update(UserGroup userGroup, UserGroupDto userGroupDto, User changedBy);

        /// <summary>
        ///     Ändert die Art einer Liste von Mitgliedschaften.
        /// </summary>
        /// <param name="membershipsToUpdate"></param>
        /// <param name="changedBy"></param>
        void UpdateMembershipTypes(IDictionary<UserGroupMembership, UserGroupMembershipType> membershipsToUpdate, User changedBy);

        /// <summary>
        /// Sucht nach der Mitgliedschaft eines Nutzers in einer Gruppe.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        UserGroupMembership FindMembershipByUserAndGroup(User user, UserGroup userGroup);

        /// <summary>
        /// Ruft die Gruppen ab, in denen der Nutzer kein Mitglied ist.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        IPage<UserGroup> FindUserGroupsWhereUserIsNoMember(IPageable pageRequest, User user);

        /// <summary>
        /// Fordert eine Mitgliedschaft für eine Gruppe an und versendet eine Benachrichtigung an alle Admins dieser Gruppe
        /// </summary>
        /// <param name="userGroup"></param>
        /// <param name="currentUser"></param>
        /// <param name="editUrl"></param>
        void RequestMembership(UserGroup userGroup, User currentUser, string editUrl);

        /// <summary>
        /// Lädt den Nutzer zu einer Mitgliedschaft in einer Gruppe ein.
        /// </summary>
        /// <param name="userGroup"></param>
        /// <param name="user"></param>
        /// <param name="currentUser"></param>
        /// <param name="allMembershipsUrl"></param>
        void Invite(UserGroup userGroup, User user, User currentUser, string allMembershipsUrl);
    }
}