using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;
using Com.QueoFlow.Peanuts.Net.Core.Service;
using Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security;
using Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup;

namespace Com.QueoFlow.Peanuts.Net.Web.Controllers {
    [RoutePrefix("UserGroup")]
    [Authorization]
    public class UserGroupController : Controller {
        public IBookingService BookingService { get; set; }

        /// <summary>
        ///     Liefert oder setzt den NotificationService
        /// </summary>
        public INotificationService NotificationService { get; set; }

        public IPaymentService PaymentService { get; set; }

        public IPeanutService PeanutService { get; set; }

        public IUserGroupService UserGroupService { get; set; }

        public IUserService UserService { get; set; }

        [Route("{userGroup}/UserGroupMembership/{userGroupMembership}/Invitation")]
        [ValidateAntiForgeryToken]
        [HttpPatch]
        public ActionResult AcceptMembershipInvitation(UserGroup userGroup, UserGroupMembership userGroupMembership, User currentUser) {
            Require.NotNull(userGroupMembership, "userGroupMembership");
            Require.NotNull(userGroup, "userGroup");
            Require.IsTrue(() => userGroupMembership.MembershipType == UserGroupMembershipType.Invited, "userGroupMembership");
            Require.IsTrue(() => currentUser.Equals(userGroupMembership.User), "userGroupMembership");

            UserGroupService.UpdateMembershipTypes(
                new Dictionary<UserGroupMembership, UserGroupMembershipType> { { userGroupMembership, UserGroupMembershipType.Member } },
                currentUser);

            return RedirectToAction("Membership", new { userGroup = userGroup.BusinessId, userGroupMembership = userGroupMembership.BusinessId });
        }

        [ValidateAntiForgeryToken]
        [HttpPatch]
        [Route("{userGroup}/UserGroupMembership/{userGroupMembership}/Request")]
        public ActionResult AcceptMembershipRequest(UserGroup userGroup, UserGroupMembership userGroupMembership, User currentUser) {
            Require.NotNull(userGroupMembership, "userGroupMembership");
            Require.NotNull(userGroup, "userGroup");
            Require.IsTrue(() => userGroupMembership.MembershipType == UserGroupMembershipType.Request, "userGroupMembership");

            UserGroupMembership currentUsersMembership = UserGroupService.FindMembershipByUserAndGroup(currentUser, userGroupMembership.UserGroup);
            Require.IsTrue(() => currentUsersMembership != null && currentUsersMembership.MembershipType == UserGroupMembershipType.Administrator,
                "userGroupMembership");

            UserGroupService.UpdateMembershipTypes(
                new Dictionary<UserGroupMembership, UserGroupMembershipType> { { userGroupMembership, UserGroupMembershipType.Member } },
                currentUser);
            string urlToUserGroup = Url.Action("Membership",
                "UserGroup",
                new { userGroup = userGroup.BusinessId, userGroupMembership = userGroupMembership.BusinessId },
                Request.Url.Scheme);
            NotificationService.SendConfirmMembershipNotification(userGroupMembership.User, userGroupMembership.UserGroup, urlToUserGroup);
            return RedirectToAction("AllMemberships");
        }

        [Route("{userGroup}/Membership/Account")]
        public ActionResult Account(UserGroup userGroup, User currentUser, int pageNumber = 1, int pageSize = 25) {
            Require.NotNull(userGroup, "userGroup");
            UserGroupMembership currentUsersMembershipInGroup;
            AssertCurrentUserIsActiveMemberInGroup(userGroup, currentUser, out currentUsersMembershipInGroup);

            IPage<BookingEntry> bookingEntries = BookingService.FindByAccount(new PageRequest(pageNumber, pageSize), currentUsersMembershipInGroup.Account);
            return View("UserGroupMembershipAccount", new UserGroupMemberShipAccountViewModel(currentUsersMembershipInGroup, bookingEntries, UserGroupMembershipOptions.ForCurrentUser(currentUsersMembershipInGroup)));
        }

        [Route("All/Membership")]
        public ActionResult AllMemberships(User currentUser) {
            IList<UserGroupMembership> currentMemberships =
                    UserGroupService.FindMembershipsByUser(PageRequest.All,
                        currentUser,
                        new List<UserGroupMembershipType> { UserGroupMembershipType.Administrator, UserGroupMembershipType.Member }).ToList();
            IList<UserGroupMembership> myRequestedMemberships =
                    UserGroupService.FindMembershipsByUser(PageRequest.All,
                        currentUser,
                        new List<UserGroupMembershipType> { UserGroupMembershipType.Request }).ToList();
            IList<UserGroupMembership> invitations =
                    UserGroupService.FindMembershipsByUser(PageRequest.All,
                        currentUser,
                        new List<UserGroupMembershipType> { UserGroupMembershipType.Invited }).ToList();

            IList<UserGroupMembership> myGroups =
                    UserGroupService.FindMembershipsByUser(PageRequest.All,
                        currentUser,
                        new List<UserGroupMembershipType> { UserGroupMembershipType.Administrator }).ToList();
            IList<UserGroupMembership> requestedMembershipsInMyGroup =
                    UserGroupService.FindMembershipsByGroups(PageRequest.All,
                        myGroups.Select(mem => mem.UserGroup).ToList(),
                        new List<UserGroupMembershipType> { UserGroupMembershipType.Request }).ToList();

            return View(new UserGroupIndexViewModel(currentMemberships, myRequestedMemberships, invitations, requestedMembershipsInMyGroup));
        }

        [Route("")]
        [HttpPost]
        public ActionResult Create(UserGroupCreateCommand userGroupCreateCommand, User currentUser) {
            Require.NotNull(currentUser, nameof(currentUser));
            if (!ModelState.IsValid) {
                UserGroupCreateViewModel userGroupCreateViewModel = new UserGroupCreateViewModel(userGroupCreateCommand);
                return View(userGroupCreateViewModel);
            }

            /*Der Ersteller der Gruppe ist initial Administrator*/
            Dictionary<User, UserGroupMembershipType> initialUsers = new Dictionary<User, UserGroupMembershipType>();
            initialUsers.Add(currentUser, UserGroupMembershipType.Administrator);

            UserGroupService.Create(userGroupCreateCommand.UserGroupDto, initialUsers, currentUser);
            return RedirectToAction("AllMemberships");
        }

        [Route("Create")]
        [HttpGet]
        public ActionResult CreateForm() {
            UserGroupCreateCommand userGroupCreateCommand = new UserGroupCreateCommand();
            UserGroupCreateViewModel userGroupCreateViewModel = new UserGroupCreateViewModel(userGroupCreateCommand);
            return View("Create", userGroupCreateViewModel);
        }

        [HttpPost]
        [Route("{userGroup}/UserGroupMembership/Invitation")]
        public ActionResult Invite(UserGroup userGroup, User user, User currentUser) {
            Require.NotNull(userGroup, "userGroup");

            if (!ModelState.IsValid) {
                IPage<UserGroupMembership> membersOfGroup = UserGroupService.FindMembershipsByGroups(PageRequest.All,
                    new List<UserGroup> { userGroup },
                    new List<UserGroupMembershipType> { UserGroupMembershipType.Administrator, UserGroupMembershipType.Member });
                IList<User> invitableUsers = UserService.GetAll().Except(membersOfGroup.Select(mem => mem.User)).ToList();
                return View("Invite", new UserGroupMembershipInvitationViewModel(userGroup, invitableUsers));
            }
            string allMembershipsUrl = Url.Action("AllMemberships", "UserGroup", null, Request.Url.Scheme);
            UserGroupService.Invite(userGroup, user, currentUser, allMembershipsUrl);

            return RedirectToAction("AllMemberships");
        }

        [HttpGet]
        [Route("{userGroup}/UserGroupMembership/CreateForm")]
        public ActionResult InviteForm(UserGroup userGroup) {
            Require.NotNull(userGroup, "userGroup");

            IPage<UserGroupMembership> membersOfGroup = UserGroupService.FindMembershipsByGroups(PageRequest.All,
                new List<UserGroup> { userGroup },
                new List<UserGroupMembershipType> { UserGroupMembershipType.Administrator, UserGroupMembershipType.Member });
            IList<User> invitableUsers = UserService.GetAll().Except(membersOfGroup.Select(mem => mem.User)).ToList();
            return View("Invite", new UserGroupMembershipInvitationViewModel(userGroup, invitableUsers));
        }

        [Route("{userGroup}/Membership/{userGroupMembership}")]
        public ActionResult Membership(UserGroup userGroup, UserGroupMembership userGroupMembership, User currentUser) {
            return MembershipDetails(userGroup, userGroupMembership, currentUser);
        }

        [Route("{userGroup:guid}/Membership/{userGroupMembership:guid}/Details")]
        [Route("{userGroup:guid}/Membership/")]
        public ActionResult MembershipDetails(UserGroup userGroup, UserGroupMembership userGroupMembership, User currentUser) {
            Require.NotNull(userGroup, "userGroup");
            UserGroupMembership currentUsersMembershipInGroup;
            AssertCurrentUserIsActiveMemberInGroup(userGroup, currentUser, out currentUsersMembershipInGroup);

            if (userGroupMembership != null) {
                Require.IsTrue(() => userGroupMembership.UserGroup.Equals(userGroup), "userGroupMembership");
                Require.IsFalse(() => userGroupMembership.MembershipType == UserGroupMembershipType.Quit, "userGroupMembership");
            } else {
                userGroupMembership = currentUsersMembershipInGroup;
            }
            
            UserGroupMembershipOptions userGroupMembershipOptions = UserGroupMembershipOptions.ForOtherUser(userGroupMembership, currentUsersMembershipInGroup);
            UserGroupMembershipDetailsViewModel userGroupMembershipDetailsViewModel = new UserGroupMembershipDetailsViewModel(userGroupMembership, currentUsersMembershipInGroup, userGroupMembershipOptions);
            return View("UserGroupMembershipDetails", userGroupMembershipDetailsViewModel);
        }

        [Route("{userGroup}/Statistics")]
        public ActionResult Statistics(UserGroup userGroup, User currentUser) {
            Require.NotNull(userGroup, "userGroup");
            UserGroupMembership currentUsersMembershipInGroup;
            AssertCurrentUserIsActiveMemberInGroup(userGroup, currentUser, out currentUsersMembershipInGroup);
            
            IList<UserGroupMembership> members = UserGroupService.FindMembershipsByGroups(PageRequest.All, new List<UserGroup> { userGroup }, UserGroupMembership.ActiveTypes).ToList();


            IDictionary<UserGroupMembership, int> userGroupMembersKarma = PeanutService.GetUserGroupMembersKarma(userGroup);

            UserGroupMembershipOptions userGroupMembershipOptions = UserGroupMembershipOptions.ForCurrentUser(currentUsersMembershipInGroup);
            UserGroupMembershipStatisticsViewModel userGroupMembershipStatisticsViewModel = new UserGroupMembershipStatisticsViewModel(
                userGroup, 
                currentUsersMembershipInGroup,
                members,
                userGroupMembershipOptions,
                PeanutService.GetPeanutsUserGroupMembershipStatistics(currentUsersMembershipInGroup), 
                userGroupMembersKarma
            );
            return View("UserGroupStatistics", userGroupMembershipStatisticsViewModel);
        }

        private void AssertCurrentUserIsActiveMemberInGroup(UserGroup userGroup, User currentUser, out UserGroupMembership currentUsersMembershipInGroup) {
            UserGroupMembership foundCurrentUsersMembershipInGroup = UserGroupService.FindMembershipByUserAndGroup(currentUser, userGroup);
            Require.NotNull(foundCurrentUsersMembershipInGroup, "currentUsersMembershipInGroup");
            Require.IsFalse(() => foundCurrentUsersMembershipInGroup.MembershipType == UserGroupMembershipType.Quit, "userGroupMembership");

            currentUsersMembershipInGroup = foundCurrentUsersMembershipInGroup;
        }

        [Route("{userGroup}/UserGroupMembership/{userGroupMembership}")]
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult QuitMembership(UserGroup userGroup, UserGroupMembership userGroupMembership, User currentUser) {
            Require.NotNull(userGroup, "userGroup");
            Require.NotNull(userGroupMembership, "userGroupMembership");
            Require.IsTrue(() => userGroupMembership.UserGroup.Equals(userGroup), "userGroupMembership");
            Require.IsFalse(() => userGroupMembership.MembershipType == UserGroupMembershipType.Quit, "userGroupMembership");

            if (!ModelState.IsValid) {
                return QuitMembershipForm(userGroup, userGroupMembership, currentUser);
            }

            UserGroupService.QuitOrRemoveMemberships(new[] { userGroupMembership }, currentUser);
            return RedirectToAction("AllMemberships", "UserGroup");
        }

        [Route("{userGroup}/UserGroupMembership/{userGroupMembership}/QuitMembershipForm")]
        [HttpGet]
        public ActionResult QuitMembershipForm(UserGroup userGroup, UserGroupMembership userGroupMembership, User currentUser) {
            Require.NotNull(userGroup, "userGroup");
            Require.NotNull(userGroupMembership, "userGroupMembership");
            Require.IsTrue(() => userGroupMembership.UserGroup.Equals(userGroup), "userGroupMembership");
            Require.IsFalse(() => userGroupMembership.MembershipType == UserGroupMembershipType.Quit, "userGroupMembership");

            if (userGroupMembership.User.Equals(currentUser)) {
                return View("QuitMyMembership", userGroupMembership);
            } else {
                return View("QuitOtherMembership", userGroupMembership);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("{userGroup}/UserGroupMembership/Request")]
        public ActionResult RequestMembership(UserGroup userGroup, User currentUser) {
            Require.NotNull(userGroup, "userGroup");

            string allMembershipsUrl = Url.Action("AllMemberships", "UserGroup", null, Request.Url.Scheme);
            UserGroupService.RequestMembership(userGroup, currentUser, allMembershipsUrl);

            return RedirectToAction("AllMemberships");
        }

        [Route("RequestMembershipForm")]
        public ActionResult RequestMembershipForm(User currentUser, int pageNumber = 1, int pageSize = 25) {
            IPage<UserGroup> groups = UserGroupService.FindUserGroupsWhereUserIsNoMember(new PageRequest(pageNumber, pageSize), currentUser);

            return View("Request", groups);
        }

        [ValidateAntiForgeryToken]
        [HttpPut]
        public ActionResult Update(UserGroup userGroup, UserGroupUpdateCommand userGroupUpdateCommand, User currentUser) {
            Require.NotNull(userGroup, "userGroup");
            Require.NotNull(userGroupUpdateCommand, "userGroupUpdateCommand");
            Require.NotNull(currentUser, "currentUser");

            if (!ModelState.IsValid) {
                return View("Update", new UserGroupUpdateViewModel(userGroup, userGroupUpdateCommand));
            }

            UserGroupService.Update(userGroup, userGroupUpdateCommand.UserGroupDto, currentUser);
            return RedirectToAction("AllMemberships");
        }

        [HttpGet]
        public ActionResult UpdateForm(UserGroup userGroup) {
            Require.NotNull(userGroup, "userGroup");

            return View("Update", new UserGroupUpdateViewModel(userGroup));
        }

        [Route("{userGroup}/Peanuts")]
        public ActionResult Peanuts(UserGroup userGroup, User currentUser, int pageNumber = 1, int pageSize = 20) {
            Require.NotNull(userGroup, "userGroup");
            UserGroupMembership currentUsersMembershipInGroup;
            AssertCurrentUserIsActiveMemberInGroup(userGroup, currentUser, out currentUsersMembershipInGroup);

            IPage<Peanut> peanuts = PeanutService.FindAllPeanutsInGroup(new PageRequest(pageNumber, pageSize), userGroup);
            UserGroupMembershipOptions userGroupMembershipOptions = UserGroupMembershipOptions.ForCurrentUser(currentUsersMembershipInGroup);
            UserGroupPeanutsViewModel userGroupPeanutsViewModel = new UserGroupPeanutsViewModel(userGroup, currentUsersMembershipInGroup, peanuts, userGroupMembershipOptions);

            return View("UserGroupPeanuts", userGroupPeanutsViewModel);
        }

        [Route("{userGroup}/Administration")]
        public ActionResult Administration(UserGroup userGroup, User currentUser) {
            Require.NotNull(userGroup, "userGroup");
            UserGroupMembership currentUsersMembershipInGroup;
            AssertCurrentUserIsActiveMemberInGroup(userGroup, currentUser, out currentUsersMembershipInGroup);

            IList<UserGroupMembership> members = UserGroupService.FindMembershipsByGroups(PageRequest.All, new List<UserGroup> { userGroup }, UserGroupMembership.ActiveTypes).ToList();
            UserGroupMembershipOptions userGroupMembershipOptions = UserGroupMembershipOptions.ForCurrentUser(currentUsersMembershipInGroup);
            UserGroupAdministrationViewModel userGroupMembershipDetailsViewModel = new UserGroupAdministrationViewModel(userGroup, currentUsersMembershipInGroup, members, userGroupMembershipOptions);
            return View("UserGroupAdministration", userGroupMembershipDetailsViewModel);
        }

        [Route("{userGroup}/Members")]
        public ActionResult Members(UserGroup userGroup, User currentUser) {
            Require.NotNull(userGroup, "userGroup");
            UserGroupMembership currentUsersMembershipInGroup;
            AssertCurrentUserIsActiveMemberInGroup(userGroup, currentUser, out currentUsersMembershipInGroup);
            
            IList<UserGroupMembership> members = UserGroupService.FindMembershipsByGroups(PageRequest.All, new List<UserGroup> { userGroup }, UserGroupMembership.ActiveTypes).ToList();

            IList<UserGroupMembership> formerMembers =
                members.Where(mem => mem.MembershipType == UserGroupMembershipType.Quit || !mem.User.IsActiveUser).ToList();

            IList<UserGroupMembership> pendingMembers =
                members.Where(mem => UserGroupMembership.PendingTypes.Contains(mem.MembershipType) && mem.User.IsActiveUser).ToList();

            IList<UserGroupMembership> currentMembers =
                members.Where(mem => UserGroupMembership.ActiveTypes.Contains(mem.MembershipType) && mem.User.IsActiveUser).ToList();

            UserGroupMembershipOptions userGroupMembershipOptions = UserGroupMembershipOptions.ForOtherUser(currentUsersMembershipInGroup, currentUsersMembershipInGroup);
            UserGroupMembersViewModel userGroupMembershipDetailsViewModel = new UserGroupMembersViewModel(userGroup, currentUsersMembershipInGroup, currentMembers, pendingMembers, formerMembers, userGroupMembershipOptions);
            return View("UserGroupMembers", userGroupMembershipDetailsViewModel);
        }
    }
}