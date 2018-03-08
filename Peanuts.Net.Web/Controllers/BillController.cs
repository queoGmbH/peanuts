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
using Com.QueoFlow.Peanuts.Net.Web.Models.Bill;

namespace Com.QueoFlow.Peanuts.Net.Web.Controllers {
    [RoutePrefix("Bill")]
    [Authorization]
    public class BillController : Controller {
        public IBillService BillService { get; set; }

        /// <summary>
        ///     Liefert oder setzt den NotificationService
        /// </summary>
        public INotificationService NotificationService { get; set; }

        public IPeanutService PeanutService { get; set; }

        public IUserGroupService UserGroupService { get; set; }

        /// <summary>
        ///     Akzeptiert eine Rechnung und leitet zur Detailseite der Rechnung weiter.
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        [Route("{bill}/AcceptState")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptBill(Bill bill, User currentUser) {
            Require.NotNull(bill, "bill");
            Require.NotNull(currentUser, "currentUser");
            Require.IsTrue(() => bill.UserGroupDebitors.Any(deb => deb.UserGroupMembership.User.Equals(currentUser)), "bill");

            BillService.AcceptBill(bill,
                currentUser,
                currentUser,
                Url.Action("Show", "Bill", new { bill = bill.BusinessId }, Request.Url.Scheme),
                Url.Action("SettleForm", "Bill", new { bill = bill.BusinessId }, Request.Url.Scheme));

            return RedirectToAction("Show", new { bill = bill.BusinessId });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("")]
        public ActionResult Create(BillCreateCommand billCreateCommand, User currentUser) {
            if (!ModelState.IsValid) {
                IList<UserGroupMembership> userGroupMemberships =
                        UserGroupService.FindMembershipsByUser(PageRequest.All,
                            currentUser,
                            new List<UserGroupMembershipType> { UserGroupMembershipType.Administrator, UserGroupMembershipType.Member }).ToList();
                IList<UserGroup> userGroups = userGroupMemberships.Select(membership => membership.UserGroup).Distinct().ToList();
                IList<UserGroupMembership> members =
                        userGroups.SelectMany(
                                    userGroup =>
                                        UserGroupService.FindMembershipsByGroups(PageRequest.All,
                                            new List<UserGroup> { userGroup },
                                            new List<UserGroupMembershipType>
                                                    { UserGroupMembershipType.Administrator, UserGroupMembershipType.Member }))
                                .ToList();
                return View("Create", new BillCreateViewModel(userGroupMemberships, members, billCreateCommand));
            }

            UserGroupMembership creditor = UserGroupService.FindMembershipByUserAndGroup(currentUser, billCreateCommand.UserGroup);

            Bill bill = BillService.Create(billCreateCommand.UserGroup,
                billCreateCommand.BillDto,
                creditor,
                billCreateCommand.UserGroupDebitors.Values.ToList(),
                billCreateCommand.GuestDebitors.Values.ToList(),
                billCreateCommand.CreatedFromPeanut,
                currentUser);
            string billUrl = Url.Action("Show", "Bill", new { bill = bill.BusinessId }, Request.Url.Scheme);
            NotificationService.SendBillReceivedNotification(bill, billUrl);
            return RedirectToAction("Show", new { bill = bill.BusinessId });
        }

        [Route("CreateForm")]
        public ActionResult CreateForm(User currentUser) {
            IList<UserGroupMembership> usersUserGroupMemberships =
                    UserGroupService.FindMembershipsByUser(PageRequest.All,
                        currentUser,
                        new List<UserGroupMembershipType> { UserGroupMembershipType.Administrator, UserGroupMembershipType.Member }).ToList();
            IList<UserGroup> userGroups = usersUserGroupMemberships.Select(membership => membership.UserGroup).Distinct().ToList();
            IList<UserGroupMembership> members =
                    userGroups.SelectMany(
                                userGroup =>
                                    UserGroupService.FindMembershipsByGroups(PageRequest.All,
                                        new List<UserGroup> { userGroup },
                                        UserGroupMembership.ActiveTypes))
                            .ToList();

            return View("Create", new BillCreateViewModel(usersUserGroupMemberships, members));
        }

        [Route("CreateForUserGroup")]
        public ActionResult CreateForUserGroup(User currentUser, UserGroup selectedUserGroup) {
            IList<UserGroupMembership> usersUserGroupMemberships =
                    UserGroupService.FindMembershipsByUser(PageRequest.All,
                        currentUser,
                        new List<UserGroupMembershipType> { UserGroupMembershipType.Administrator, UserGroupMembershipType.Member }).ToList();
            IList<UserGroup> userGroups = usersUserGroupMemberships.Select(membership => membership.UserGroup).Distinct().ToList();
            IList<UserGroupMembership> members =
                    userGroups.SelectMany(
                                userGroup =>
                                    UserGroupService.FindMembershipsByGroups(PageRequest.All,
                                        new List<UserGroup> { userGroup },
                                        UserGroupMembership.ActiveTypes))
                            .ToList();
            return View("Create", new BillCreateViewModel(usersUserGroupMemberships, members, selectedUserGroup));
        }

        [Route("FromPeanut/{peanut}")]
        public ActionResult CreateFromPeanut(Peanut peanut, User currentUser) {
            Require.NotNull(currentUser, "currentUser");
            Require.NotNull(peanut, "peanut");

            UserGroupMembership currentUsersMembershipsInPeanutGroup = UserGroupService.FindMembershipByUserAndGroup(currentUser, peanut.UserGroup);
            IList<UserGroupMembership> availableUserGroupMemberships =
                    UserGroupService.FindMembershipsByGroups(PageRequest.All,
                        new List<UserGroup> { peanut.UserGroup },
                        UserGroupMembership.ActiveTypes).ToList();
            BillCreateViewModel billCreateViewModel = new BillCreateViewModel(new List<UserGroupMembership> { currentUsersMembershipsInPeanutGroup },
                availableUserGroupMemberships,
                peanut);

            billCreateViewModel.BillCreateCommand.UserGroup = peanut.UserGroup;
            billCreateViewModel.BillCreateCommand.BillDto.Subject = peanut.Name;
            IEnumerable<PeanutParticipation> participations =
                    peanut.Participations.Where(s => s.ParticipationState == PeanutParticipationState.Confirmed);

            IDictionary<string, BillUserGroupDebitorDto> userGroupDebitors = new Dictionary<string, BillUserGroupDebitorDto>();
            foreach (PeanutParticipation peanutParticipation in participations) {
                UserGroupMembership membership = peanutParticipation.UserGroupMembership;
                userGroupDebitors.Add(membership.BusinessId.ToString(), new BillUserGroupDebitorDto(membership, 1));
            }

            billCreateViewModel.BillCreateCommand.UserGroupDebitors = userGroupDebitors;

            return View("Create", billCreateViewModel);
        }

        [Route("")]
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Bill bill) {
            Require.NotNull(bill, "bill");

            if (!ModelState.IsValid) {
                return View("Delete", bill);
            }

            BillService.Delete(bill);
            return RedirectToAction("Index");
        }

        [Route("{bill}/DeleteForm")]
        [HttpGet]
        public ActionResult DeleteForm(Bill bill) {
            Require.NotNull(bill, "bill");

            return View("Delete", bill);
        }

        [Route("In")]
        public ActionResult In(User currentUser) {
            IPage<Bill> debitorBills = BillService.FindBillsWhereUserIsDebitor(PageRequest.All, currentUser, true);
            IPage<DebitorBillViewModel> debitorBillViewModels =
                    new Page<DebitorBillViewModel>(debitorBills.Select(b => new DebitorBillViewModel(b, currentUser)).ToList(),
                        new PageRequest(debitorBills.PageNumber, debitorBills.Size),
                        debitorBills.TotalElements);

            IPage<Bill> creditorBills = BillService.FindCreditorBillsForUser(PageRequest.First, currentUser, true);
            IPage<CreditorBillViewModel> creditorBillViewModels =
                    new Page<CreditorBillViewModel>(creditorBills.Select(b => new CreditorBillViewModel(b, currentUser)).ToList(),
                        new PageRequest(creditorBills.PageNumber, creditorBills.Size),
                        creditorBills.TotalElements);

            IList<CreditorBillViewModel> unsettledCreditorBills =
                    BillService.FindCreditorBillsForUser(PageRequest.All, currentUser, false)
                            .Select(b => new CreditorBillViewModel(b, currentUser))
                            .ToList();
            IList<DebitorBillViewModel> unsettledDebitorBills =
                    BillService.FindBillsWhereUserIsDebitor(PageRequest.All, currentUser, false)
                            .Select(b => new DebitorBillViewModel(b, currentUser))
                            .ToList();

            return View("In", new BillIndexViewModel(unsettledCreditorBills, creditorBillViewModels, unsettledDebitorBills, debitorBillViewModels));
        }

        [Route("")]
        public ActionResult Index(User currentUser) {
            IPage<Bill> unsettledCreditorBills = BillService.FindCreditorBillsForUser(PageRequest.All, currentUser, false);
            IPage<Bill> unsettledDebitorBills = BillService.FindBillsWhereUserIsDebitor(PageRequest.All, currentUser, false);

            if (unsettledDebitorBills.TotalElements > 0) {
                /*Es gibt Rechnungen, die ich noch bezahlen muss => zeigen*/
                return RedirectToAction("In");
            } else if (unsettledCreditorBills.TotalElements > 0) {
                /*Es gibt Rechnungen, auf deren Bezahlung ich warte => zeigen*/
                return RedirectToAction("Out");
            } else {
                /*Keine Offenen Rechnungen*/
                return RedirectToAction("In");
            }
        }

        [Route("Out")]
        public ActionResult Out(User currentUser) {
            /*Rechnungen die ich schon bezahlt habe*/
            IPage<Bill> debitorBills = BillService.FindBillsWhereUserIsDebitor(PageRequest.First, currentUser, true);
            IPage<DebitorBillViewModel> debitorBillViewModels =
                    new Page<DebitorBillViewModel>(debitorBills.Select(b => new DebitorBillViewModel(b, currentUser)).ToList(),
                        PageRequest.First,
                        debitorBills.TotalElements);
            /*Rechnungen die mir bereits bezahlt wurden und die ich abgerechnet habe*/
            IPage<Bill> creditorBills = BillService.FindCreditorBillsForUser(PageRequest.All, currentUser, true);
            IPage<CreditorBillViewModel> creditorBillViewModels =
                    new Page<CreditorBillViewModel>(creditorBills.Select(b => new CreditorBillViewModel(b, currentUser)).ToList(),
                        new PageRequest(creditorBills.PageNumber, creditorBills.Size),
                        creditorBills.TotalElements);
            /*Rechnungen die man mir noch bezahlen muss*/
            IList<CreditorBillViewModel> unsettledCreditorBills =
                    BillService.FindCreditorBillsForUser(PageRequest.All, currentUser, false)
                            .Select(b => new CreditorBillViewModel(b, currentUser))
                            .ToList();

            /*Rechnungen die ich noch bezahlen muss*/
            IList<DebitorBillViewModel> unsettledDebitorBills =
                    BillService.FindBillsWhereUserIsDebitor(PageRequest.All, currentUser, false)
                            .Select(b => new DebitorBillViewModel(b, currentUser))
                            .ToList();

            return View("Out", new BillIndexViewModel(unsettledCreditorBills, creditorBillViewModels, unsettledDebitorBills, debitorBillViewModels));
        }

        /// <summary>
        ///     Akzeptiert eine Rechnung und leitet zur Detailseite der Rechnung weiter.
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="billRefuseCommand"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        [Route("{bill}/AcceptState")]
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult RefuseBill(Bill bill, BillRefuseCommand billRefuseCommand, User currentUser) {
            Require.NotNull(bill, "bill");
            Require.NotNull(currentUser, "currentUser");
            Require.IsTrue(() => bill.UserGroupDebitors.Any(deb => deb.UserGroupMembership.User.Equals(currentUser)), "bill");

            if (!ModelState.IsValid) {
                return View("RefuseBill", new BillRefuseViewModel(bill, currentUser, billRefuseCommand));
            }

            BillService.DeclineBill(bill,
                currentUser,
                billRefuseCommand.RefuseComment,
                currentUser,
                Url.Action("Show", "Bill", new { bill = bill.BusinessId }, Request.Url.Scheme));
            return RedirectToAction("Index");
        }

        [Route("{bill}/AcceptState/RefuseForm")]
        public ActionResult RefuseBillForm(Bill bill, User currentUser) {
            return View("RefuseBill", new BillRefuseViewModel(bill, currentUser));
        }

        [Route("")]
        [HttpPatch]
        [ValidateAntiForgeryToken]
        public ActionResult Settle(Bill bill, User currentUser) {
            Require.NotNull(bill, "bill");
            Require.NotNull(currentUser, "currentUser");

            if (!ModelState.IsValid) {
                return View("Settle", bill);
            }

            BillService.Settle(bill, currentUser);

            return RedirectToAction("Index");
        }

        [Route("{bill}/SettleForm")]
        public ActionResult SettleForm(Bill bill) {
            Require.NotNull(bill, "bill");

            return View("Settle", bill);
        }

        [Route("{bill}")]
        public ActionResult Show(Bill bill, User currentUser) {
            Require.NotNull(bill, "bill");
            Require.NotNull(currentUser, "currentUser");

            Peanut createdFromPeanut = PeanutService.FindFromBill(bill);

            return View("Show", new BillShowViewModel(bill, new BillOptions(bill, currentUser), createdFromPeanut));
        }
    }
}