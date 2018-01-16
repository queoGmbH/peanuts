using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Extensions;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;
using Com.QueoFlow.Peanuts.Net.Core.Service;
using Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security;
using Com.QueoFlow.Peanuts.Net.Web.Models.Peanut;

namespace Com.QueoFlow.Peanuts.Net.Web.Controllers {
    [Authorization]
    [RoutePrefix("Peanut")]
    public class PeanutController : Controller {
        private static readonly List<UserGroupMembershipType> ActiveUsergroupMembershipTypes = new List<UserGroupMembershipType> {
            UserGroupMembershipType.Administrator, UserGroupMembershipType.Member
        };

        /// <summary>
        ///     Liefert oder setzt den PeanutParticipationTypeService
        /// </summary>
        public IPeanutParticipationTypeService PeanutParticipationTypeService { get; set; }

        /// <summary>
        ///     Liefert oder setzt den PeanutService
        /// </summary>
        public IPeanutService PeanutService { get; set; }

        /// <summary>
        ///     Liefert oder setzt den UserGroupService
        /// </summary>
        public IUserGroupService UserGroupService { get; set; }

        [HttpPost]
        [Route("{peanut:guid}/Comments")]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(Peanut peanut, PeanutCommentCreateCommand peanutCommentCreateCommand, User currentUser) {
            if (!ModelState.IsValid) {
                return RedirectToAction("Show", new { peanut = peanut.BusinessId });
            }

            PeanutService.AddComment(peanut,
                peanutCommentCreateCommand.Comment,
                new PeanutUpdateNotificationOptions(peanutCommentCreateCommand.SendUpdateNotification,
                    Url.Action("Show", "Peanut", new { peanut = peanut.BusinessId }, Request.Url.Scheme)),
                currentUser);
            return RedirectToAction("Show", new { peanut = peanut.BusinessId });
        }

        [Route("{peanut:guid}/Participation")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Attend(Peanut peanut, User currentUser, PeanutParticipationCreateCommand peanutParticipationCreateCommand) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(currentUser, "currentUser");
            Require.IsFalse(() => peanut.IsFixed, "peanut");
            
            UserGroupMembership userGroupMembership = UserGroupService.FindMembershipsByUserAndGroup(currentUser, peanut.UserGroup);
            Require.NotNull(userGroupMembership, "userGroupMembership");


            if(!UserGroupService.IsUserSolvent(userGroupMembership)) {
                return View("CanNotParticipate",
                    new PeanutParticipationRejectedViewModel(peanut));
            }

            if (!ModelState.IsValid) {
                IList<PeanutParticipationType> peanutParticipationTypes = PeanutParticipationTypeService.FindForGroup(peanut.UserGroup);
                return View("CreateParticipation",
                    new PeanutParticipationCreateFormViewModel(peanut, peanutParticipationTypes.ToList(), peanutParticipationCreateCommand));
            }

            PeanutService.AddOrUpdateParticipations(peanut,
                new Dictionary<UserGroupMembership, PeanutParticipationDto> {
                    {
                        userGroupMembership,
                        new PeanutParticipationDto(peanutParticipationCreateCommand.PeanutParticipationType, PeanutParticipationState.Confirmed)
                    }
                },
                currentUser);

            return RedirectToAction("Show", new { peanut = peanut.BusinessId });
        }

        [Route("{peanut:guid}/Participation/ParticipationForm")]
        [HttpGet]
        public ActionResult AttendForm(Peanut peanut, User currentUser) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(currentUser, "currentUser");
            Require.IsFalse(() => peanut.IsFixed, "peanut");

            IList<PeanutParticipationType> peanutParticipationTypes = PeanutParticipationTypeService.FindForGroup(peanut.UserGroup);
            return View("CreateParticipation", new PeanutParticipationCreateFormViewModel(peanut, peanutParticipationTypes.ToList()));
        }

        /// <summary>
        ///     Erstellt einen neuen Peanut
        /// </summary>
        /// <param name="peanutCreateCommand"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("")]
        public ActionResult Create(PeanutCreateCommand peanutCreateCommand, User currentUser) {
            Require.NotNull(peanutCreateCommand, "peanutCreateCommand");
            Require.NotNull(currentUser, "currentUser");

            if (!ModelState.IsValid) {
                List<UserGroupMembership> userGroupMemberships =
                        UserGroupService.FindMembershipsByUser(PageRequest.All, currentUser, ActiveUsergroupMembershipTypes).ToList();
                List<UserGroup> userGroups = userGroupMemberships.Select(membership => membership.UserGroup).ToList();
                List<PeanutParticipationType> participationTypes = PeanutParticipationTypeService.GetAll(PageRequest.All).ToList();
                return View("Create", new PeanutCreateViewModel(userGroups));
            }

            /*Initiale Teilnehmer ermitteln.*/
            IDictionary<UserGroupMembership, PeanutParticipationDto> initialParticators = new Dictionary<UserGroupMembership, PeanutParticipationDto>();
            Peanut peanut = PeanutService.Create(peanutCreateCommand.UserGroup,
                peanutCreateCommand.PeanutDto,
                peanutCreateCommand.Requirements.Values.ToList(),
                initialParticators,
                currentUser);

            return RedirectToAction("Show",new {peanut = peanut.BusinessId});
        }

        /// <summary>
        ///     Liefert das Form zum Erstellen eines Peanuts
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        [Route("CreateForm/{day}")]
        [Route("CreateForm")]
        public ActionResult CreateForm(User currentUser, DateTime? day) {
            List<UserGroupMembership> userGroupMemberships =
                    UserGroupService.FindMembershipsByUser(PageRequest.All, currentUser, ActiveUsergroupMembershipTypes).ToList();
            List<UserGroup> userGroups = userGroupMemberships.Select(membership => membership.UserGroup).ToList();
            PeanutCreateViewModel peanutCreateViewModel = new PeanutCreateViewModel(userGroups);
            if (day.HasValue) {
                peanutCreateViewModel.PeanutCreateCommand.PeanutDto.Day = day.Value;
            }
            return View("Create", peanutCreateViewModel);
        }

        /// <summary>
        ///     Liefert eine Liste der Peanuts für den übergebenen Tag
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        [Route("{year:int}/{month:int}/{day:int}", Order = 1)]
        public ActionResult Day(int year, int month, int day, User currentUser) {
            DateTime date = new DateTime(year, month, day);

            IPage<PeanutParticipation> peanutParticipations = PeanutService.FindParticipationsOfUser(PageRequest.All, currentUser, date, date, new [] { PeanutParticipationState.Confirmed });
            IPage<Peanut> attendablePeanuts = PeanutService.FindAttendablePeanutsForUser(PageRequest.All, currentUser, date, date);

            if (peanutParticipations.TotalElements == 1 && attendablePeanuts.TotalElements == 0) {
                /*Wenn es an diesem Tag nur diesen einen Peanut gibt, dann diesen anzeigen*/
                return RedirectToAction("Show", "Peanut", new { peanut = peanutParticipations.Single().Peanut.BusinessId });
            } else {
                return View("List", new PeanutsListViewModel(date, date, peanutParticipations.ToList(), attendablePeanuts.ToList()));
            }
        }

        [Route("{peanut:guid}/Participation/{peanutParticipation}")]
        [HttpDelete]
        public ActionResult DeclineParticipation(Peanut peanut, PeanutParticipation peanutParticipation, User currentUser) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(peanutParticipation, "peanutParticipation");
            Require.IsTrue(() => peanutParticipation.Peanut.Equals(peanut), "peanutParticipation");

            PeanutService.RemoveParticipations(peanut, new List<UserGroupMembership> { peanutParticipation.UserGroupMembership }, currentUser);

            return RedirectToAction("Day", new { year = peanut.Day.Year, month = peanut.Day.Month, day = peanut.Day.Day });
        }

        /// <summary>
        ///     Liefert die Übersichtsseite der Peanuts für den aktuellen Monat
        /// </summary>
        /// <returns></returns>
        [Route("", Order = 2)]
        public ActionResult Index() {
            return RedirectToAction("Index", new { year = DateTime.Now.Year, month = DateTime.Now.Month });
        }

        /// <summary>
        ///     Liefert die Übersichtsseite der Peanuts für den aktuellen Monat
        /// </summary>
        /// <returns></returns>
        [Route("{year:int}/{month:int}", Order = 1)]
        public ActionResult Index(User currentUser, int year, int month) {
            Require.NotNull(currentUser, "currentUser");
            Require.Ge(year, 2000, "year");
            Require.Ge(month, 1, "month");
            Require.Le(month, 12, "month");

            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            /*Ab Montag der Woche des ersten Tages des Monats.*/
            DateTime from = firstDayOfMonth.GetFirstDayOfWeek();
            /*Bis Sonntag der Woche des letzten Tages Monats*/
            DateTime to = firstDayOfMonth
                    .AddMonths(1) // 1. des nächsten Monats
                    .AddDays(-1) // letzter des aktuellen Monats
                    .GetFirstDayOfWeek() // Montag der letzten Woche
                    .AddDays(6); // Sonntag der letzten Woche

            IPage<PeanutParticipation> peanutParticipations = PeanutService.FindParticipationsOfUser(PageRequest.All, currentUser, from, to);
            IPage<Peanut> attendablePeanuts = PeanutService.FindAttendablePeanutsForUser(PageRequest.All, currentUser, from, to);
            IDictionary<DateTime, IList<PeanutParticipation>> peanutParticipationsByDate =
                    peanutParticipations.GroupBy(participation => participation.Peanut.Day)
                            .ToDictionary(g => g.Key, g => (IList<PeanutParticipation>)g.ToList());
            IDictionary<DateTime, IList<Peanut>> attendablePeanutsByDate = attendablePeanuts.GroupBy(peanut => peanut.Day)
                    .ToDictionary(g => g.Key, g => (IList<Peanut>)g.ToList());

            return View("Index", new PeanutsIndexViewModel(year, month, peanutParticipationsByDate, attendablePeanutsByDate));
        }

        //[Route("GetParticipationTypes/{userGroup:guid}")]
        //public ActionResult GetParticipationTypes(UserGroup userGroup)
        //{
        //    var peanutParticipationTypes = PeanutParticipationTypeService.Find(userGroup);
        //    PeanutParticipationTypeSelectionModel participationTypeSelectionModel = new PeanutParticipationTypeSelectionModel();
        //    participationTypeSelectionModel.SelectableParticipationTypes = peanutParticipationTypes;
        //    return PartialView("EditorTemplates/ParticipationType", participationTypeSelectionModel);
        //}

        [HttpPost]
        [Route("{peanut:guid}/Invitation")]
        [ValidateAntiForgeryToken]
        public ActionResult InviteUser(Peanut peanut, PeanutInvitationCreateCommand peanutInvitationCreateCommand, User currentUser) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(peanutInvitationCreateCommand, "peanutInvitationCreateCommand");
            Require.NotNull(currentUser, "currentUser");

            if (peanut.IsFixed) {
                return View("CanNotInvite",new PeanutParticipationRejectedViewModel(peanut));
            }

            string peanutUrl = Url.Action("Show", "Peanut", new { peanut = peanut.BusinessId }, Request.Url.Scheme);
            string attendPeanutUrl = Url.Action("AttendForm", "Peanut", new { peanut = peanut.BusinessId }, Request.Url.Scheme);
            if (peanutInvitationCreateCommand.UserGroupMembership != null) {
                PeanutService.InviteUser(peanut,
                    peanutInvitationCreateCommand.UserGroupMembership,
                    new PeanutParticipationDto(peanutInvitationCreateCommand.PeanutParticipationType, PeanutParticipationState.Pending),
                    new PeanutInvitationNotificationOptions(peanutUrl, attendPeanutUrl),
                    currentUser);
            } else {
                PeanutService.InviteAllGroupMembers(peanut,
                    peanut.UserGroup,
                    peanutInvitationCreateCommand.PeanutParticipationType,
                    new PeanutInvitationNotificationOptions(peanutUrl, attendPeanutUrl),
                    currentUser);
            }

            return RedirectToAction("Show", new { peanut = peanut.BusinessId });
        }

        /// <summary>
        ///     Zeigt den Peanut an
        /// </summary>
        /// <param name="peanut"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        [Route("{peanut:guid}")]
        public ActionResult Show(Peanut peanut, User currentUser) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(currentUser, "currentUser");

            IList<PeanutParticipationType> peanutParticipationTypes = PeanutParticipationTypeService.FindForGroup(peanut.UserGroup);
            List<UserGroupMembership> userGroupMemberships =
                    UserGroupService.FindMembershipsByGroups(PageRequest.All,
                        new List<UserGroup> { peanut.UserGroup },
                        UserGroupMembership.ActiveTypes).ToList();
            /*Es können alle Nutzer eingeladen werden, die in der Gruppe aktives Mitglied sind und noch nicht am Peanut teilnehmen oder ihre Teilnahme abgesagt haben*/
            List<UserGroupMembership> invitableUsers =
                    userGroupMemberships.Except(
                        peanut.Participations.Where(part => part.ParticipationState != PeanutParticipationState.Refused)
                                .Select(part => part.UserGroupMembership)).ToList();

            return View("Show",
                new PeanutShowViewModel(peanut,
                    peanut.Participations.SingleOrDefault(part => part.UserGroupMembership.User.Equals(currentUser)),
                    invitableUsers,
                    peanutParticipationTypes,
                    new PeanutEditOptions(peanut, currentUser)));
        }

        [HttpPut]
        [Route("{peanut:guid}")]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Peanut peanut, PeanutUpdateCommand peanutUpdateCommand, User currentUser) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(peanutUpdateCommand, "peanutUpdateCommand");
            Require.NotNull(currentUser, "currentUser");

            if (MaximumParticipationsIsLowerThanConfirmedParticipations(peanutUpdateCommand.PeanutDto.MaximumParticipations, peanut.Participations)) {
                ModelState.AddModelError(Objects.GetPropertyPath<PeanutUpdateViewModel>(vm => vm.PeanutUpdateCommand.PeanutDto.MaximumParticipations), "Es gibt bereits mehr Zusagen als maximale Teilnehmer!");
            }

            if (!ModelState.IsValid) {
                IList<PeanutParticipationType> peanutParticipationTypes = PeanutParticipationTypeService.FindForGroup(peanut.UserGroup);
                return View("Update", new PeanutUpdateViewModel(peanut, peanutUpdateCommand, peanutParticipationTypes));
            }

            PeanutService.Update(peanut,
                peanutUpdateCommand.PeanutDto,
                peanutUpdateCommand.Requirements.Values.ToList(),
                peanutUpdateCommand.PeanutCommentCreateCommand.Comment,
                new PeanutUpdateNotificationOptions(peanutUpdateCommand.PeanutCommentCreateCommand.SendUpdateNotification,
                    Url.Action("Show", "Peanut", new { peanut = peanut.BusinessId }, Request.Url.Scheme)),
                currentUser);

            return RedirectToAction("Show", new { peanut = peanut.BusinessId });
        }

        private bool MaximumParticipationsIsLowerThanConfirmedParticipations(int? maximumParticipations, IList<PeanutParticipation> peanutParticipations) {
            if (maximumParticipations < peanutParticipations.Count(part => part.ParticipationState == PeanutParticipationState.Confirmed)) {
                return true;
            } else {
                return false;
            }
        }

        [Route("{peanut:guid}/UpdateForm")]
        public ActionResult UpdateForm(Peanut peanut, User currentUser) {
            IList<PeanutParticipationType> peanutParticipationTypes = PeanutParticipationTypeService.FindForGroup(peanut.UserGroup);
            return View("Update", new PeanutUpdateViewModel(peanut, peanutParticipationTypes));
        }

        [ValidateAntiForgeryToken]
        [Route("{peanut:guid}/Participation/{peanutParticipation}")]
        [HttpPut]
        public ActionResult UpdateParticipation(Peanut peanut, PeanutParticipation peanutParticipation,
            PeanutParticipationUpdateCommand peanutParticipationUpdateCommand, User currentUser) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(peanutParticipation, "peanutParticipation");
            Require.NotNull(peanutParticipationUpdateCommand, "peanutParticipationUpdateCommand");
            Require.IsTrue(() => peanut.Equals(peanutParticipation.Peanut), "peanutParticipation");

            if (!ModelState.IsValid) {
                return RedirectToAction("Show", new { peanut = peanut.BusinessId });
            }

            Dictionary<UserGroupMembership, PeanutParticipationDto> participationUpdates =
                    new Dictionary<UserGroupMembership, PeanutParticipationDto> {
                        {
                            peanutParticipation.UserGroupMembership,
                            new PeanutParticipationDto(peanutParticipationUpdateCommand.PeanutParticipationType,
                                peanutParticipation.ParticipationState)
                        }
                    };
            PeanutService.AddOrUpdateParticipations(peanut,
                participationUpdates,
                currentUser);

            return RedirectToAction("Show", new { peanut = peanut.BusinessId });
        }

        [Route("{peanut:guid}/PeanutState")]
        [HttpPatch]
        public ActionResult UpdateState(Peanut peanut, PeanutState peanutState, User currentUser) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(currentUser, "currentUser");

            string peanutUrl = Url.Action("Show", "Peanut", new { peanut = peanut.BusinessId }, Request.Url.Scheme);
            PeanutUpdateNotificationOptions peanutUpdateNotificationOptions = new PeanutUpdateNotificationOptions(true, peanutUrl);

            PeanutService.UpdateState(peanut, peanutState, peanutUpdateNotificationOptions, currentUser);

            return RedirectToAction("Show", new { peanut = peanut.BusinessId });
        }
    }
}