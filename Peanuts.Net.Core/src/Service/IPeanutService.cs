using System;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    /// <summary>
    ///     Schnittstelle die einen Service zur Verwaltung von Peanuts beschreibt.
    /// </summary>
    public interface IPeanutService {
        /// <summary>
        ///     Fügt einem Peanut neue Teilnehmer hinzu oder ändert die bestehende Teilnahme.
        /// </summary>
        /// <param name="peanut">Der Peanut, dem neue Teilnahmen hinzugefügt werden sollen.</param>
        /// <param name="participations">Die hinzuzufügenden Teilnahmen</param>
        /// <param name="user">Der Nutzer, der die Teilnehmer hinzufügt.</param>
        /// <returns>Liste mit allen Teilnahmen des Peanuts</returns>
        IList<PeanutParticipation> AddOrUpdateParticipations(Peanut peanut, IDictionary<UserGroupMembership, PeanutParticipationDto> participations,
            User user);

        /// <summary>
        ///     Rechnet den Peanut ab und speichert die Rechnung an diesem
        /// </summary>
        /// <param name="peanut"></param>
        /// <param name="bill">Die Rechnung zum Peanut</param>
        /// <returns></returns>
        Peanut ClearPeanut(Peanut peanut, Bill bill);

        /// <summary>
        ///     Erstellt einen neuen Peanut.
        /// </summary>
        /// <param name="userGroup">Für welche Gruppe soll der Peanut erstellt werden.</param>
        /// <param name="peanutDto">Informationen über den Peanut</param>
        /// <param name="initialParticipators">Initiale Teilnehmer des Peanuts.</param>
        /// <param name="requirements">Ruft die Voraussetzungen zur Durchführung des Peanuts ab.</param>
        /// <param name="user">Wer erstellt den Peanut.</param>
        /// <returns></returns>
        Peanut Create(UserGroup userGroup, PeanutDto peanutDto, IList<RequirementDto> requirements,
            IDictionary<UserGroupMembership, PeanutParticipationDto> initialParticipators, User user);

        /// <summary>
        ///     Sucht nach Peanuts, an denen der Nutzer teilnehmen kann, da das Peanut in seiner Gruppe erstellt wurde und er
        ///     bisher nicht als Teilnehmer eingetragen ist bzw. seine Teilnahme nicht bestätigt hat.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        IPage<Peanut> FindAttendablePeanutsForUser(IPageable pageRequest, User user, DateTime from, DateTime to);

        /// <summary>
        ///     Sucht nach dem Peanut, aus dem die Rechnung erstellt wurde.
        ///     Wurde die Rechnung unabhängig von einem Peanut erstellt, wird null geliefert.
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        Peanut FindFromBill(Bill bill);

        /// <summary>
        ///     Ruft alle Teilnahmen eines Nutzers an Peanuts in einem bestimmten Zeitraum seitenweise ab.
        ///     Die Sortierung erfolgt nach Datum des Peanuts.
        /// </summary>
        /// <param name="pageRequest">Seiteninformationen</param>
        /// <param name="forUser">Die Teilnahmen welches Nutzers sollen abgerufen werden?</param>
        /// <param name="from">Frühester Termin ab dem Peanuts gefunden werden</param>
        /// <param name="to">Spätester Termin bis zu dem Peanuts gefunden werden</param>
        /// <param name="participationStates">Die <see cref="PeanutParticipation.ParticipationState">Status</see> der zu berücksichtigenden Teilnahmen oder null, wenn der Status egal ist.</param>
        /// <returns></returns>
        IPage<PeanutParticipation> FindParticipationsOfUser(IPageable pageRequest, User forUser, DateTime from, DateTime to, IList<PeanutParticipationState> participationStates = null);

        /// <summary>
        /// Aktualisiert den aktuellen Status des Peanuts.
        /// </summary>
        /// <param name="peanut"></param>
        /// <param name="peanutState"></param>
        /// <param name="notificationOptions"></param>
        /// <param name="user"></param>
        void UpdateState(Peanut peanut, PeanutState peanutState, PeanutUpdateNotificationOptions notificationOptions, User user);

        /// <summary>
        ///     Entfernt Teilnahmen an einem Peanut.
        /// </summary>
        /// <param name="peanut"></param>
        /// <param name="participators"></param>
        /// <param name="user"></param>
        void RemoveParticipations(Peanut peanut, IList<UserGroupMembership> participators, User user);

        /// <summary>
        ///     Aktualisiert einen Peanut
        /// </summary>
        /// <param name="peanut">Der zu ändernde Peanut</param>
        /// <param name="peanutDto">Die neuen Infos zum Peanut</param>
        /// <param name="requirements">Die Anforderungsliste für den Peanut</param>
        /// <param name="updateComment">Änderungskommentar</param>
        /// <param name="notificationOptions">Optionen für die Benachrichtigung der Teilnehmer des Peanuts.</param>
        /// <param name="user">Welcher Nutzer führt die Änderungen durch?</param>
        void Update(Peanut peanut, PeanutDto peanutDto, IList<RequirementDto> requirements, string updateComment,
            PeanutUpdateNotificationOptions notificationOptions, User user);

        /// <summary>
        /// Hinterlässt einen Kommentar am Peanut.
        /// </summary>
        /// <param name="peanut"></param>
        /// <param name="updateComment"></param>
        /// <param name="notificationOptions"></param>
        /// <param name="user"></param>
        void AddComment(Peanut peanut, string updateComment, PeanutUpdateNotificationOptions notificationOptions, User user);

        /// <summary>
        /// Lädt einen Nutzer zur Teilnahme an einem Peanut ein.
        /// </summary>
        /// <param name="peanut">Der Peanut, zu welchem der Nutzer eingeladen wird.</param>
        /// <param name="userGroupMembership">Der Nutzer der eingeladen wird bzw. seine Mitgliedschaft in der Gruppe</param>
        /// <param name="peanutParticipationDto">Teilnahme-Optionen</param>
        /// <param name="peanutInvitationNotificationOptions">Benachrichtigungsoptionen</param>
        /// <param name="user">Der einladende Nutzer.</param>
        void InviteUser(Peanut peanut, UserGroupMembership userGroupMembership, PeanutParticipationDto peanutParticipationDto, PeanutInvitationNotificationOptions peanutInvitationNotificationOptions, User user);

        /// <summary>
        ///     Lädt alle Mitglieder der Gruppe zum Peanut ein
        /// </summary>
        /// <param name="peanut"></param>
        /// <param name="userGroup"></param>
        /// <param name="invitationNotificationOptions"></param>
        /// <param name="peanutInvitationNotificationOptions"></param>
        /// <param name="user"></param>
        void InviteAllGroupMembers(Peanut peanut, UserGroup userGroup, PeanutParticipationType invitationNotificationOptions, PeanutInvitationNotificationOptions peanutInvitationNotificationOptions, User user);


        /// <summary>
        /// Sucht seitenweise nach abgerechneten Peanuts in einem bestimmten Zeitraum.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="userGroup">Die Gruppe, in welcher gesucht werden soll</param>
        /// <param name="from">Der früheste Zeitpunkt für einen zu berücksichtigenden Peanut oder null, wenn ab dem ersten Peanut gesucht werden soll.</param>
        /// <param name="to">Der späteste Zeitpunkt für einen zu berücksichtigenden Peanut oder null, wenn bis zum letzten Peanut gesucht werden soll.</param>
        /// <returns></returns>
        IPage<Peanut> FindBilledPeanutsInGroup(IPageable pageRequest, UserGroup userGroup, DateTime? from = null, DateTime? to = null);

        /// <summary>
        /// Ruft die Statistiken der Peanuts für ein Mitglied einer Gruppe ab.
        /// 
        /// Wenn bisher keine Peanuts in der Gruppe existieren, wird null geliefert.
        /// </summary>
        /// <param name="userGroupMembership"></param>
        /// <returns></returns>
        PeanutsUserGroupMembershipStatistics GetPeanutsUserGroupMembershipStatistics(UserGroupMembership userGroupMembership);


        /// <summary>
        /// Ruft die Karmas der aktiven Gruppenmitglieder ab.
        /// </summary>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        IDictionary<UserGroupMembership, int> GetUserGroupMembersKarma(UserGroup userGroup);

        int GetUserGroupMemberKarma(UserGroupMembership userGroupMembership);

        /// <summary>
        /// Liefert alle Peanuts der Gruppe
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        IPage<Peanut> FindAllPeanutsInGroup(IPageable pageRequest, UserGroup userGroup);
    }
}