using System.Web;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Service;
using Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.Home;
using Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security;

using Microsoft.AspNet.Identity.Owin;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Controllers {
    [RouteArea("Admin")]
    [Authorization(new[] { Roles.Administrator })]
    public class HomeAdministrationController : Controller {
        private ApplicationUserManager _userManager;

        public IUserGroupService UserGroupService { get; set; }

        public ApplicationUserManager UserManager {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        /// <summary>
        ///     Gibt oder setzt den UserService
        /// </summary>
        public IUserService UserService { get; set; }

        /// <summary>
        ///     Controller-Methode für das Anzeigen des Admin Dashboards
        /// </summary>
        /// <returns></returns>
        [Route("Dashboard")]
        [HttpGet]
        public ActionResult Dashboard() {
            int userCount = UserService.GetActiveUsers();
            int estateCount = 0;
            int apartmentCount = 0;
            long customerCount = 0;
            long financialBrokerPoolCount = UserGroupService.GetCount();
            long houseBuilderCount = 0;
            long cityCount = 0;
            long parkingPositionTypeCount = 0;

            HomeAdministrationViewModel homeAdminModel = new HomeAdministrationViewModel(userCount, financialBrokerPoolCount);
            return View(homeAdminModel);
        }

        [Route("")]
        public ActionResult Index() {
            return RedirectToAction("Dashboard");
        }
    }
}