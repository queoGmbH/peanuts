using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;
using Com.QueoFlow.Peanuts.Net.Core.Resources;
using Com.QueoFlow.Peanuts.Net.Core.Service;
using Com.QueoFlow.Peanuts.Net.Web.Helper;
using Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security;
using Com.QueoFlow.Peanuts.Net.Web.Models.Bill;
using Com.QueoFlow.Peanuts.Net.Web.Models.Home;
using Com.QueoFlow.Peanuts.Net.Web.Models.Menu;

namespace Com.QueoFlow.Peanuts.Net.Web.Controllers {
    [Authorization]
    public class HomeController : Controller {
        
        
        /// <summary>
        ///     Legt den Service für den Zugriff auf Maklerpools fest.
        /// </summary>
        public IUserGroupService UserGroupService { private get; set; }

        public IPeanutService PeanutService { private get; set; }

        public ISecurityExpressionRootFactory SecurityExpressionRootFactory { get; set; }

        public IPaymentService PaymentService { get; set; }

        public IBillService BillService { get; set; }

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
            
            IPage<UserGroupMembership> memberships = UserGroupService.FindMembershipsByUser(PageRequest.All, currentUser, new List<UserGroupMembershipType> { UserGroupMembershipType.Administrator, UserGroupMembershipType.Member });
            IPage<Payment> pendingPayments = PaymentService.FindPendingPaymentsByUser(PageRequest.First, currentUser);
            IPage<Payment> declinedPayments = PaymentService.FindDeclinedPaymentsByUser(PageRequest.First, currentUser);
            IPage<PeanutParticipation> peanutParticipations = PeanutService.FindParticipationsOfUser(PageRequest.All, currentUser, DateTime.Today, DateTime.Today);
   
            IPage<Bill> declinedBills = BillService.FindDeclinedCreditorBillsByUser(PageRequest.All, currentUser);
                   List<Bill> unsettledBills =
                    BillService.FindCreditorBillsForUser(PageRequest.All, currentUser, false).ToList();
             unsettledBills.AddRange(BillService.FindDebitorBillsForUser(PageRequest.All, currentUser, false).ToList());



            return View("Index", new IndexViewModel(peanutParticipations, memberships, unsettledBills, declinedBills, pendingPayments, declinedPayments, currentUser.DisplayName, GetShowCurrentVersionNews(currentUser)));
        }

        private static bool GetShowCurrentVersionNews(User currentUser) {

            if (currentUser.LatestReadVersionNews == null) {
                return true;
            }

            if (currentUser.LatestReadVersionNews < VersionHelper.GetCurrentApplicationVersion()) {
                return true;
            }

            return false;
        }

        public ActionResult MenuContentPartial() {
            ActionUrl activeLink = ActionUrl.Current();
            IList<MenuItemViewModel> menuModel = new List<MenuItemViewModel>();

            /*Objekte*/
            menuModel.Add(new SimpleMenuItemViewModel("menu_item_home",
                new ActionLink("Dashboard", "", "Home", "Index"),
                activeLink.IsAction("", "Home", "Index"),
                "icon-home"));

            /*Gruppen*/
            menuModel.Add(new SimpleMenuItemViewModel("menu_item_groups",
                new ActionLink("Meine Gruppen", "", "UserGroup", "AllMemberships"),
                activeLink.IsController("", "UserGroup"),
                "icon-group"));

            /*Peanuts*/
            menuModel.Add(new SimpleMenuItemViewModel("menu_item_peanuts",
                new ActionLink("Peanuts", "", "Peanut", "Index"),
                activeLink.IsController("", "Peanut"),
                "icon-calendar"));

            /*Rechnungen*/
            menuModel.Add(new SimpleMenuItemViewModel("menu_item_bills",
                new ActionLink("Rechnungen", "", "Bill", "Index"),
                activeLink.IsController("", "Bill"),
                "icon-inbox"));

            /*Zahlungen*/
            menuModel.Add(new SimpleMenuItemViewModel("menu_item_payments",
                new ActionLink("Zahlungen", "", "Payment", "Index"),
                activeLink.IsController("", "Payment"),
                "icon-eur"));

            if (SecurityExpressionRootFactory.CreateSecurityExpressionRoot().HasRole(Roles.Administrator)) {
                menuModel.Add(new SimpleMenuItemViewModel("menu_item_administration",
                    new ActionLink("Administration", "Admin", "HomeAdministration", "Index"),
                    activeLink.IsArea("Admin") && !activeLink.IsController("Admin", "MyUsersAdministration"),
                    "icon-gears"));
            }


            MenuViewModel menuViewModel = new MenuViewModel(menuModel);

            return PartialView(menuViewModel);
        }
    }
}