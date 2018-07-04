using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;
using Com.QueoFlow.Peanuts.Net.Core.Service;
using Com.QueoFlow.Peanuts.Net.Web.Helper;
using Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security;
using Com.QueoFlow.Peanuts.Net.Web.Models.Home;
using Com.QueoFlow.Peanuts.Net.Web.Models.Menu;

namespace Com.QueoFlow.Peanuts.Net.Web.Controllers {
    [Authorization]
    public class HomeController : Controller {
        public IBillService BillService { get; set; }

        public IPaymentService PaymentService { get; set; }

        public IPeanutService PeanutService { private get; set; }

        public ISecurityExpressionRootFactory SecurityExpressionRootFactory { get; set; }

        /// <summary>
        ///     Legt den Service für den Zugriff auf Maklerpools fest.
        /// </summary>
        public IUserGroupService UserGroupService { private get; set; }

        /// <summary>
        ///     Legt den Service für den Zugriff auf Nutzer fest.
        /// </summary>
        public IUserService UserService { private get; set; }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<ActionResult> Index(User currentUser) {
            Require.NotNull(currentUser, "currentUser");

            DashboardInfos dashboardInfos = GetDashboardInfos(currentUser);
            return View("Index", new IndexViewModel(dashboardInfos, currentUser));
        }

        public ActionResult MenuContentPartial() {
            ActionUrl activeLink = ActionUrl.Current();
            IList<MenuItemViewModel> menuModel = new List<MenuItemViewModel>();

            /*Objekte*/
            menuModel.Add(
                new SimpleMenuItemViewModel(
                    "menu_item_home",
                    new ActionLink("Dashboard", "", "Home", "Index"),
                    activeLink.IsAction("", "Home", "Index"),
                    "icon-home"));

            /*Gruppen*/
            menuModel.Add(
                new SimpleMenuItemViewModel(
                    "menu_item_groups",
                    new ActionLink("Meine Gruppen", "", "UserGroup", "AllMemberships"),
                    activeLink.IsController("", "UserGroup"),
                    "icon-group"));

            /*Peanuts*/
            menuModel.Add(
                new SimpleMenuItemViewModel(
                    "menu_item_peanuts",
                    new ActionLink("Peanuts", "", "Peanut", "Index"),
                    activeLink.IsController("", "Peanut"),
                    "icon-calendar"));

            /*Rechnungen*/
            menuModel.Add(
                new SimpleMenuItemViewModel(
                    "menu_item_bills",
                    new ActionLink("Rechnungen", "", "Bill", "Index"),
                    activeLink.IsController("", "Bill"),
                    "icon-inbox"));

            /*Zahlungen*/
            menuModel.Add(
                new SimpleMenuItemViewModel(
                    "menu_item_payments",
                    new ActionLink("Zahlungen", "", "Payment", "Index"),
                    activeLink.IsController("", "Payment"),
                    "icon-eur"));

            if (SecurityExpressionRootFactory.CreateSecurityExpressionRoot().HasRole(Roles.Administrator)) {
                menuModel.Add(
                    new SimpleMenuItemViewModel(
                        "menu_item_administration",
                        new ActionLink("Administration", "Admin", "HomeAdministration", "Index"),
                        activeLink.IsArea("Admin") && !activeLink.IsController("Admin", "MyUsersAdministration"),
                        "icon-gears"));
            }

            MenuViewModel menuViewModel = new MenuViewModel(menuModel);

            return PartialView(menuViewModel);
        }

        private static List<PeanutParticipation> GetInvitationsFromUpcomingPeanuts(IPage<Peanut> upcomingPeanutsForUser, User currentUser) {
            IEnumerable<Peanut> unfixedPeanuts = upcomingPeanutsForUser.Where(peanut => !peanut.IsFixed);
            IEnumerable<PeanutParticipation> currentUsersParticipations = unfixedPeanuts.SelectMany(peanut => peanut.Participations)
                .Where(part => part.UserGroupMembership.User.Equals(currentUser));
            return currentUsersParticipations.Where(part => part.ParticipationState == PeanutParticipationState.Pending).ToList();
        }

        private static List<PeanutParticipation> GetUnobtrusiveParticipationsFromUpcomingPeanuts(IPage<Peanut> upcomingPeanutsForUser, User currentUser) {
            IEnumerable<PeanutParticipation> currentUsersParticipations = upcomingPeanutsForUser.SelectMany(peanut => peanut.Participations)
                .Where(part => part.UserGroupMembership.User.Equals(currentUser));
            return currentUsersParticipations.Where(part => part.ParticipationState == PeanutParticipationState.Confirmed && !(part.ParticipationType.IsCreditor || part.ParticipationType.IsProducer)).ToList();
        }

        private static List<PeanutParticipation> GetParticipationsAsCreditorFromUpcomingPeanuts(IPage<Peanut> upcomingPeanutsForUser, User currentUser) {
            IEnumerable<PeanutParticipation> currentUsersParticipations =
                upcomingPeanutsForUser.Where(peanut => peanut.PeanutState < PeanutState.PurchasingStarted)
                .SelectMany(peanut => peanut.Participations)
                .Where(part => part.UserGroupMembership.User.Equals(currentUser));
            return currentUsersParticipations.Where(part => part.ParticipationState == PeanutParticipationState.Confirmed && (part.ParticipationType.IsCreditor)).ToList();
        }

        private static List<PeanutParticipation> GetParticipationsAsProducerFromUpcomingPeanuts(IPage<Peanut> upcomingPeanutsForUser, User currentUser) {
            IEnumerable<PeanutParticipation> currentUsersParticipations = 
                upcomingPeanutsForUser.Where(peanut => peanut.PeanutState < PeanutState.Assembling)
                .SelectMany(peanut => peanut.Participations)
                .Where(part => part.UserGroupMembership.User.Equals(currentUser));
            return currentUsersParticipations.Where(part => part.ParticipationState == PeanutParticipationState.Confirmed && (part.ParticipationType.IsProducer)).ToList();
        }


        private static bool GetShowCurrentVersionNews(User currentUser) {
            if (currentUser.LatestReadVersionNews == null) {
                return true;
            }

            Version currentUserLatestReadVersionNews = currentUser.LatestReadVersionNews;
            Version currentApplicationVersion = VersionHelper.GetCurrentApplicationVersion();


            if (currentUserLatestReadVersionNews.Major < currentApplicationVersion.Major) {
                return true;
            }
            if (currentUserLatestReadVersionNews.Minor < currentApplicationVersion.Minor) {
                return true;
            }
            if (currentUserLatestReadVersionNews.Build < currentApplicationVersion.Build) {
                return true;
            }

            return false;
        }

        private DashboardInfos GetDashboardInfos(User currentUser) {
            IPage<UserGroupMembership> memberships = UserGroupService.FindMembershipsByUser(
                PageRequest.All,
                currentUser,
                new List<UserGroupMembershipType> { UserGroupMembershipType.Administrator, UserGroupMembershipType.Member });
            IPage<Payment> pendingPayments = PaymentService.FindPendingPaymentsByUser(PageRequest.First, currentUser);
            IPage<Payment> declinedPayments = PaymentService.FindDeclinedPaymentsByUser(PageRequest.First, currentUser);
            IPage<PeanutParticipation> peanutParticipations =
                PeanutService.FindParticipationsOfUser(PageRequest.All, currentUser, DateTime.Today, DateTime.Today);

            IPage<Bill> declinedBills = BillService.FindRefusedBillsWhereUserIsCreditor(PageRequest.All, currentUser);
            List<Bill> unsettledBills = BillService.FindCreditorBillsForUser(PageRequest.All, currentUser, false).ToList();
            unsettledBills.AddRange(BillService.FindBillsWhereUserIsDebitor(PageRequest.All, currentUser, false).ToList());
            DashboardInfos dashboardInfos = new DashboardInfos(
                peanutParticipations,
                memberships,
                unsettledBills,
                declinedBills,
                pendingPayments,
                declinedPayments);
            return dashboardInfos;
        }

        private List<Peanut> GetUnattendedPeanutsFromUpcomingPeanuts(IPage<Peanut> upcomingPeanutsForUser, User currentUser) {
            return upcomingPeanutsForUser.Where(
                peanut => !peanut.IsFixed).Where(peanut => !peanut.Participations.Any(part => part.UserGroupMembership.User.Equals(currentUser))).ToList();
        }

        public ActionResult DashboardNews(User currentUser) {

            IPage<UserGroupMembership> memberships = UserGroupService.FindMembershipsByUser(
                PageRequest.All,
                currentUser,
                new List<UserGroupMembershipType> { UserGroupMembershipType.Administrator, UserGroupMembershipType.Member });

            IPage<Peanut> upcomingPeanutsForUser = PeanutService.FindPeanutsInGroups(
                PageRequest.All,
                DateTime.Today,
                DateTime.Today.AddDays(5),
                memberships.Select(mem => mem.UserGroup).ToArray());

            IList<PeanutParticipation> upcomingPeanutParticipations = upcomingPeanutsForUser.SelectMany(p => p.Participations).Where(part => !part.Peanut.IsRealized &&  memberships.Contains(part.UserGroupMembership)).ToList();
            IList<Peanut> upcomingAttendablePeanuts = upcomingPeanutsForUser.Where(p => !p.IsFixed).Except(upcomingPeanutParticipations.Select(part => part.Peanut)).ToList();

            DashboardNews dashboardNews = new DashboardNews(currentUser, GetShowCurrentVersionNews(currentUser), upcomingPeanutParticipations, upcomingAttendablePeanuts);

            return PartialView("DashboardNews", dashboardNews);
        }
    }
}