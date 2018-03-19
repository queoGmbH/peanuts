using System.Net;
using System.Web.Mvc;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Service;
using Com.QueoFlow.Peanuts.Net.Web.Helper;

namespace Com.QueoFlow.Peanuts.Net.Web.Controllers {
    [RoutePrefix("Version")]
    public class VersionNewsController : Controller {

        /// <summary>
        /// Legt den Service für die Verwaltung von Nutzern fest.
        /// </summary>
        public IUserService UserService { private get; set; }

        // GET
        [Route("History")]
        [Route("")]
        public ActionResult Index() {
            return View();
        }

        [HttpGet]
        [Route("CurrentVersion")]
        public ActionResult CurrentVersionNews() {
            // TODO: Should be resolved automagically
            return PartialView("Version_1_1_1");
        }

        [HttpPost]
        [Route("CurrentVersion")]
        public ActionResult UserHasReadCurrentVersionNews(User currentUser) {
            Require.NotNull(currentUser, "currentUser");

            UserService.UpdateUserHasReadNewsOfVersion(currentUser, VersionHelper.GetCurrentApplicationVersion());

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}