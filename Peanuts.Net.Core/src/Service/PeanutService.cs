using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;
using Spring.Transaction.Interceptor;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    /// <summary>
    ///     Service der Methoden zur Verwaltung von Peanuts bereitstellt.
    /// </summary>
    public class PeanutService : IPeanutService {
        public INotificationService NotificationService { private get; set; }

        public IPeanutDao PeanutDao { private get; set; }

        /// <summary>
        ///     Liefert oder setzt den PeanutParticipationTypeService
        /// </summary>
        public IPeanutParticipationTypeService PeanutParticipationTypeService { get; set; }

        /// <summary>
        ///     Liefert oder setzt den UserGroupService
        /// </summary>
        public IUserGroupService UserGroupService { get; set; }

        [Transaction]
        public void AddComment(Peanut peanut, string updateComment, PeanutUpdateNotificationOptions notificationOptions, User user) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(user, "user");
            Require.NotNull(notificationOptions, "notificationOptions");
            Require.NotNullOrWhiteSpace(updateComment, "updateComment");

            if (!string.IsNullOrWhiteSpace(updateComment)) {
                /*Wenn es einen Änderungskommentar gibt, dann diesen hinzufügen.*/
                peanut.AddComment(updateComment, new EntityCreatedDto(user, DateTime.Now));
            }
            if (notificationOptions.SendNotification) {
                NotificationService.SendPeanutCommentNotification(peanut, updateComment, notificationOptions, user);
            }
        }

        [Transaction]
        public IList<PeanutParticipation> AddOrUpdateParticipations(
            Peanut peanut,
            IDictionary<UserGroupMembership, PeanutParticipationDto> participations, User user) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(participations, "participations");
            Require.NotNull(user, "user");

            if (peanut.IsFixed) {
                throw new InvalidOperationException("Einem fixierten Peanut können keine weiteren Teilnehmer hinzugefügt werden.");
            }

            AssertMaximumParticipationsAreNotViolated(peanut, participations);

            foreach (UserGroupMembership membership in participations.Keys) {
                PeanutParticipation existingParticipation =
                    peanut.Participations.SingleOrDefault(part => part.UserGroupMembership.Equals(membership));
                if (existingParticipation != null) {
                    /*Der Nutzer nimmt bereits teil. In diesem Falle wird die Art der Teilnahme angepasst und der Status auf Confirmed gesetzt.*/
                    if (!existingParticipation.GetDto().Equals(participations[membership])) {
                        existingParticipation.Update(participations[membership], new EntityChangedDto(user, DateTime.Now));
                    }
                } else {
                    /*Es handelt sich um einen neuen Teilnehmer*/
                    PeanutParticipation newParticipation = new PeanutParticipation(
                        peanut,
                        membership,
                        participations[membership],
                        new EntityCreatedDto(user, DateTime.Now));
                    peanut.AddParticipators(new EntityChangedDto(user, DateTime.Now), newParticipation);
                }
            }

            return new ReadOnlyCollection<PeanutParticipation>(peanut.Participations);
        }

        /// <summary>
        ///     Rechnet den Peanut ab und speichert die Rechnung an diesem
        /// </summary>
        /// <param name="peanut">Der Peanut der abgerechnet wird</param>
        /// <param name="bill">Die Rechnung zum Peanut</param>
        /// <returns></returns>
        public Peanut ClearPeanut(Peanut peanut, Bill bill) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(bill, "bill");
            peanut.Clear(bill);
            return peanut;
        }

        /// <summary>
        ///     Erstellt einen neuen Peanut.
        /// </summary>
        /// <param name="userGroup"></param>
        /// <param name="peanutDto"></param>
        /// <param name="initialParticipators"></param>
        /// <param name="requirements"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [Transaction]
        public Peanut Create(
            UserGroup userGroup, PeanutDto peanutDto, IList<RequirementDto> requirements,
            IDictionary<UserGroupMembership, PeanutParticipationDto> initialParticipators, User user) {
            foreach (UserGroupMembership groupMembership in initialParticipators.Keys) {
                if (!userGroup.Equals(groupMembership.UserGroup)) {
                    throw new InvalidOperationException("Ein Peanut kann nur für Mitglieder der selben Gruppe erstellt werden.");
                }
            }
            Peanut peanut = new Peanut(userGroup, peanutDto, requirements, initialParticipators, new EntityCreatedDto(user, DateTime.Now));
            PeanutDao.Save(peanut);
            return peanut;
        }

        /// <summary>
        ///     Liefert alle Peanuts der Gruppe
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        public IPage<Peanut> FindAllPeanutsInGroup(IPageable pageRequest, UserGroup userGroup) {
            Require.NotNull(userGroup, "userGroup");
            Require.NotNull(pageRequest, "pageRequest");

            return PeanutDao.FindPeanutsInGroups(pageRequest, new List<UserGroup> { userGroup });
        }

        /// <summary>
        ///     Sucht nach Peanuts, an denen der Nutzer teilnehmen kann, da das Peanut in seiner Gruppe erstellt wurde und er
        ///     bisher nicht als Teilnehmer eingetragen ist bzw. seine Teilnahme nicht bestätigt hat.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IPage<Peanut> FindAttendablePeanutsForUser(IPageable pageRequest, User user, DateTime from, DateTime to) {
            return PeanutDao.FindAttendablePeanutsForUser(pageRequest, user, from, to);
        }

        /// <summary>
        ///     Sucht seitenweise nach abgerechneten Peanuts in einem bestimmten Zeitraum.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="userGroup">Die Gruppe, in welcher gesucht werden soll</param>
        /// <param name="from">
        ///     Der früheste Zeitpunkt für einen zu berücksichtigenden Peanut oder null, wenn ab dem ersten Peanut
        ///     gesucht werden soll.
        /// </param>
        /// <param name="to">
        ///     Der späteste Zeitpunkt für einen zu berücksichtigenden Peanut oder null, wenn bis zum letzten Peanut
        ///     gesucht werden soll.
        /// </param>
        /// <returns></returns>
        public IPage<Peanut> FindBilledPeanutsInGroup(IPageable pageRequest, UserGroup userGroup, DateTime? from = null, DateTime? to = null) {
            return PeanutDao.FindBilledPeanutsInGroup(pageRequest, userGroup, from, to);
        }

        /// <summary>
        ///     Sucht nach dem Peanut, aus dem die Rechnung erstellt wurde.
        ///     Wurde die Rechnung unabhängig von einem Peanut erstellt, wird null geliefert.
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        public Peanut FindFromBill(Bill bill) {
            return PeanutDao.FindFromBill(bill);
        }

        /// <summary>
        ///     Ruft alle Teilnahmen eines Nutzers an Peanuts in einem bestimmten Zeitraum seitenweise ab.
        ///     Die Sortierung erfolgt nach Datum des Peanuts.
        /// </summary>
        /// <param name="pageRequest">Seiteninformationen</param>
        /// <param name="forUser">Die Teilnahmen welches Nutzers sollen abgerufen werden?</param>
        /// <param name="from">Frühester Termin ab dem Peanuts gefunden werden</param>
        /// <param name="to">Spätester Termin bis zu dem Peanuts gefunden werden</param>
        /// <returns></returns>
        public IPage<PeanutParticipation> FindParticipationsOfUser(
            IPageable pageRequest, User forUser, DateTime from, DateTime to,
            IList<PeanutParticipationState> participationStates = null) {
            return PeanutDao.FindParticipationsOfUser(pageRequest, forUser, from, to, participationStates);
        }

        public PeanutsUserGroupMembershipStatistics GetPeanutsUserGroupMembershipStatistics(UserGroupMembership userGroupMembership) {
            Require.NotNull(userGroupMembership, "userGroupMembership");

            List<Peanut> allPeanutsInGroup =
                PeanutDao.FindPeanutsInGroups(
                    PageRequest.All,
                    new List<UserGroup> { userGroupMembership.UserGroup },
                    new DateTime(2000, 1, 1),
                    new DateTime(3000, 1, 1)).ToList();
            List<PeanutParticipation> allParticipationsOfMember =
                allPeanutsInGroup.SelectMany(p => p.Participations).Where(part => part.UserGroupMembership.Equals(userGroupMembership)).ToList();

            if (!allPeanutsInGroup.Any()) {
                return null;
            }

            return new PeanutsUserGroupMembershipStatistics(userGroupMembership, allPeanutsInGroup, allParticipationsOfMember);
        }

        public int GetUserGroupMemberKarma(UserGroupMembership userGroupMembership) {
            Require.NotNull(userGroupMembership, "userGroupMembership");
            IList<Peanut> allPeanutsInGroup =
                PeanutDao.FindPeanutsInGroups(
                    PageRequest.All,
                    new List<UserGroup> { userGroupMembership.UserGroup },
                    new DateTime(2000, 1, 1),
                    new DateTime(3000, 1, 1)).ToList();

            return GetUserGroupMemberKarma(userGroupMembership, allPeanutsInGroup);
        }

        /// <summary>
        ///     Ruft die Karmas der aktiven Gruppenmitglieder ab.
        /// </summary>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        public IDictionary<UserGroupMembership, int> GetUserGroupMembersKarma(UserGroup userGroup) {
            Require.NotNull(userGroup, "userGroup");

            List<UserGroupMembership> userGroupMemberships =
                UserGroupService.FindMembershipsByGroups(PageRequest.All, new List<UserGroup> { userGroup }, UserGroupMembership.ActiveTypes)
                    .ToList();
            IList<Peanut> allPeanutsInGroup =
                PeanutDao.FindPeanutsInGroups(
                    PageRequest.All,
                    new List<UserGroup> { userGroup },
                    new DateTime(2000, 1, 1),
                    new DateTime(3000, 1, 1)).ToList();

            return
                userGroupMemberships.ToDictionary(u => u, u => GetUserGroupMemberKarma(u, allPeanutsInGroup))
                    .OrderBy(b => b.Value)
                    .ToDictionary(d => d.Key, d => d.Value);
        }

        /// <summary>
        ///     Lädt alle Mitglieder der Gruppe zum Peanut ein
        /// </summary>
        /// <param name="peanut"></param>
        /// <param name="userGroup"></param>
        /// <param name="peanutParticipationType"></param>
        /// <param name="peanutInvitationNotificationOptions"></param>
        /// <param name="user"></param>
        [Transaction]
        public void InviteAllGroupMembers(
            Peanut peanut, UserGroup userGroup, PeanutParticipationType peanutParticipationType,
            PeanutInvitationNotificationOptions peanutInvitationNotificationOptions, User user) {
            IList<UserGroupMembership> members =
                UserGroupService.FindMembershipsByGroups(
                    PageRequest.All,
                    new List<UserGroup> { userGroup },
                    new List<UserGroupMembershipType> { UserGroupMembershipType.Administrator, UserGroupMembershipType.Member }).ToList();
            PeanutParticipationType participationType = peanutParticipationType;
            if (participationType != null) {
                PeanutParticipationDto peanutParticipationDto = new PeanutParticipationDto(participationType, PeanutParticipationState.Pending);
                /*Alle Nutzer einladen, die noch nicht als Teilnehmer am Peanut hinterlegt sind.*/
                IList<UserGroupMembership> inviteableMembers =
                    members.Except(peanut.Participations.Select(part => part.UserGroupMembership)).ToList();
                foreach (UserGroupMembership inviteableMember in inviteableMembers) {
                    InviteUser(peanut, inviteableMember, peanutParticipationDto, peanutInvitationNotificationOptions, user);
                }
            }
        }

        [Transaction]
        public void InviteUser(
            Peanut peanut, UserGroupMembership userGroupMembership, PeanutParticipationDto peanutParticipationDto,
            PeanutInvitationNotificationOptions peanutInvitationNotificationOptions, User user) {
            /*Nutzer/Teilnahme hinzufügen*/
            AddOrUpdateParticipations(
                peanut,
                new Dictionary<UserGroupMembership, PeanutParticipationDto> { { userGroupMembership, peanutParticipationDto } },
                user);

            /*Den Nutzer benachrichtigen*/
            NotificationService.SendPeanutInvitationNotification(peanut, userGroupMembership.User, peanutInvitationNotificationOptions, user);
        }

        [Transaction]
        public void RemoveParticipations(Peanut peanut, IList<UserGroupMembership> participators, User user) {
            Require.NotNull(participators, "participators");
            Require.NotNull(peanut, "peanut");
            Require.NotNull(user, "user");

            if (peanut.IsFixed) {
                throw new InvalidOperationException("Die Teilnahme an einem fixierten Peanut kann nicht abgesagt werden.");
            }

            peanut.RemoveParticipators(new EntityChangedDto(user, DateTime.Now), participators.ToArray());
        }

        [Transaction]
        public void Update(
            Peanut peanut, PeanutDto peanutDto, IList<RequirementDto> requirements, string updateComment,
            PeanutUpdateNotificationOptions notificationOptions, User user) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(peanutDto, "peanutDto");
            Require.NotNull(requirements, "requirements");
            Require.NotNull(user, "user");
            Require.NotNull(notificationOptions, "notificationOptions");

            AssertMaximumParticipationsIsNotViolatingConfirmedParticipations(peanutDto.MaximumParticipations, peanut.Participations);

            PeanutDto dtoBeforeUpdate = peanut.GetDto();
            IList<PeanutRequirement> requirementsBeforeUpdate = peanut.Requirements;
            if (HasPeanutChanged(peanut, peanutDto, requirements)) {
                /*Es wurden Änderungen am Peanut vorgenommen!*/
                peanut.Update(peanutDto, requirements, new EntityChangedDto(user, DateTime.Now));
                if (!string.IsNullOrWhiteSpace(updateComment)) {
                    /*Wenn es einen Änderungskommentar gibt, dann diesen hinzufügen.*/
                    peanut.AddComment(updateComment, new EntityCreatedDto(user, DateTime.Now));
                }

                if (notificationOptions.SendNotification) {
                    NotificationService.SendPeanutUpdateNotification(
                        peanut,
                        dtoBeforeUpdate,
                        requirementsBeforeUpdate,
                        updateComment,
                        notificationOptions,
                        user);
                }
                if (HaveRequirementsChanged(requirementsBeforeUpdate, requirements)) {
                    NotificationService.SendPeanutUpdateRequirementsNotification(
                        peanut,
                        updateComment,
                        new PeanutUpdateRequirementsNotificationOptions(notificationOptions.PeanutUrl),
                        user);
                }
            } else if (!string.IsNullOrWhiteSpace(updateComment)) {
                /*Es wurde nur ein Kommentar hinterlassen*/
                AddComment(peanut, updateComment, notificationOptions, user);
            }
        }

        /// <summary>
        ///     Aktualisiert den aktuellen Status des Peanuts.
        /// </summary>
        /// <param name="peanut"></param>
        /// <param name="peanutState"></param>
        /// <param name="notificationOptions"></param>
        /// <param name="user"></param>
        [Transaction]
        public void UpdateState(Peanut peanut, PeanutState peanutState, PeanutUpdateNotificationOptions notificationOptions, User user) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(user, "user");

            if (peanut.PeanutState == peanutState) {
                /*Keine Änderung am Status*/
                return;
            }

            if (peanutState >= PeanutState.SchedulingDone && !peanut.IsFixed) {
                RemoveParticipations(
                    peanut,
                    peanut.Participations.Where(s => s.ParticipationState == PeanutParticipationState.Pending)
                        .Select(s => s.UserGroupMembership)
                        .ToList(),
                    user);
            }
            peanut.UpdateState(peanutState, new EntityChangedDto(user, DateTime.Now));
            NotificationService.SendPeanutUpdateStateNotification(peanut, notificationOptions, user);
        }

        /// <summary>
        ///     Ermittelt das Karma eines Gruppenmitglieds.
        /// </summary>
        /// <param name="userGroupMembership"></param>
        /// <param name="allPeanuts"></param>
        /// <returns></returns>
        private static int GetUserGroupMemberKarma(UserGroupMembership userGroupMembership, IList<Peanut> allPeanuts) {
            IList<Peanut> relevantPeanuts =
                allPeanuts.Where(
                    p =>
                        p.PeanutState == PeanutState.Realized
                        && p.Participations.Any(part => part.UserGroupMembership.Equals(userGroupMembership))).ToList();

            int karma = 0;

            /*Für jedes erstelltes Peanut erhält man 2 Karma-Punkte.*/
            karma += 2 * relevantPeanuts.Count(p => p.CreatedBy.Equals(userGroupMembership.User));

            /*Für jedes mal Herstellen erhält man 2 Punkte*/
            karma += 3
                     * relevantPeanuts.SelectMany(p => p.Participations)
                         .Count(
                             part =>
                                 part.UserGroupMembership.Equals(userGroupMembership) && part.ParticipationType.IsProducer
                                 && !part.ParticipationType.IsCreditor);

            /*Für jedes mal Einkaufen erhält man 2 Punkte*/
            karma += 3
                     * relevantPeanuts.SelectMany(p => p.Participations)
                         .Count(
                             part =>
                                 part.UserGroupMembership.Equals(userGroupMembership) && !part.ParticipationType.IsProducer
                                 && part.ParticipationType.IsCreditor);

            /*Für jedes mal Einkaufen und Herstellen erhält man 1 Extra-Punkt*/
            karma += 1
                     * relevantPeanuts.SelectMany(p => p.Participations)
                         .Count(
                             part =>
                                 part.UserGroupMembership.Equals(userGroupMembership) && part.ParticipationType.IsProducer
                                 && part.ParticipationType.IsCreditor);

            /*Für jede normale Teilnahme ohne irgendwie behilflich zu sein, bekommt man einen Punkt Abzug*/
            karma -= 1
                     * relevantPeanuts.SelectMany(p => p.Participations)
                         .Count(
                             part =>
                                 part.UserGroupMembership.Equals(userGroupMembership) && !part.ParticipationType.IsProducer
                                 && !part.ParticipationType.IsCreditor);

            /*Jeder Euro des Kontostands ist ein Karma-Punkt.*/
            karma += (int)userGroupMembership.Account.Balance;

            return karma;
        }

        private void AssertMaximumParticipationsAreNotViolated(
            Peanut peanut, IDictionary<UserGroupMembership, PeanutParticipationDto> participations) {
            if (peanut.MaximumParticipations.HasValue) {

                int newNumberOfConfirmedParticipations = peanut.ConfirmedParticipations.Count(p => !participations.Keys.Contains(p.UserGroupMembership)) +
                    participations.Count(p => p.Value.ParticipationState == PeanutParticipationState.Confirmed);

                if (newNumberOfConfirmedParticipations > peanut.MaximumParticipations) {
                    throw new InvalidOperationException("Die maximale Anzahl an Teilnehmer würde überschritten werden.");
                }
            }
        }

        private void AssertMaximumParticipationsIsNotViolatingConfirmedParticipations(
            int? maximumParticipations, IList<PeanutParticipation> peanutParticipations) {
            if (maximumParticipations.HasValue) {
                if (maximumParticipations < peanutParticipations.Count(part => part.ParticipationState == PeanutParticipationState.Confirmed)) {
                    throw new InvalidOperationException("Es gibt bereits mehr Zusagen als die maximale Anzahl ein Teilnahmen.");
                }
            }
        }

        private bool HasPeanutChanged(Peanut peanut, PeanutDto peanutDto, IList<RequirementDto> requirements) {
            if (!peanut.GetDto().Equals(peanutDto)) {
                /*Es gab Änderungen an den allgemeinen Daten des Peanuts.*/
                return true;
            }

            IList<RequirementDto> requirementsBefore = peanut.Requirements.Select(r => r.GetDto()).ToList();
            if (requirementsBefore.Except(requirements).Any()) {
                /*Anforderungen wurden gelöscht*/
                return true;
            }
            if (requirements.Except(requirementsBefore).Any()) {
                /*Anforderungen wurden hinzugefügt*/
                return true;
            }

            return false;
        }

        private bool HaveRequirementsChanged(IList<PeanutRequirement> requirementsBeforeUpdate, IList<RequirementDto> requirements) {
            return ListHelper.AreEquivalent(requirementsBeforeUpdate.Select(r => r.GetDto()).ToList(), requirements);
        }
    }
}