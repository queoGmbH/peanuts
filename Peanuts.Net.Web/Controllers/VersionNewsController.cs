using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Controllers {
    [RoutePrefix("Version")]
    public class VersionNewsController : Controller {
        // GET
        [Route("Historie")]
        [Route("")]
        public ActionResult Index() {
            return View();
        }

        [Route("Aktuelle_Version")]
        public ActionResult CurrentVersionNews() {
            // TODO: Should be resolved automagically
            return PartialView("Version_1_1_0");
        }
    }
}