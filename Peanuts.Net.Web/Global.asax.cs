using System.Security.Claims;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Spring.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web
{
    public class MvcApplication : SpringMvcApplication
    {
        protected void Application_Start()
        {
            /* WebApi-Konfiguration */
            GlobalConfiguration.Configure(WebApiConfig.Register);

            /*Filter u.a. für FehlerHandling definieren*/
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            /*Routen-Konfiguration einbinden*/
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            /*Spezielle Modelbinder registrieren*/
            ModelBinderConfig.RegisterModelBindings(ModelBinders.Binders);

            /*Bundles definieren und registrieren*/
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            /* ??? */
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}
