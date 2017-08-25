using System.Web.Mvc;
using System.Web.Routing;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Web.Helper {
    /// <summary>
    ///     Helper-Klasse zur Erzeugung der Helper für einzelne Html-Elemente.
    /// </summary>
    public static class MvcHelperExtensions {
        public static MvcDisplayExtension Display(this HtmlHelper helper) {
            return new MvcDisplayExtension(helper);
        }

        public static MvcDisplayExtension DisplayEx<TModel>(this HtmlHelper<TModel> helper) {
            return new MvcDisplayExtension(helper);
        }

        public static MvcDisplayExtension<TModel> DisplayFor<TModel>(this HtmlHelper<TModel> helper) {
            return new MvcDisplayExtension<TModel>(helper);
        }

        /// <summary>
        ///     Erzeugt eine Instanz der Hilfsklasse zur Erzeugung von HTML-Formular-Elementen.
        /// </summary>
        /// <typeparam name="TModel">Der Typ des Models für den Html-Helper.</typeparam>
        /// <param name="helper">Die Instanz des Html-Helpers, der für das Rendern des HTMLs </param>
        /// <returns></returns>
        public static MvcFormExtension<TModel> Form<TModel>(this HtmlHelper<TModel> helper) {
            return new MvcFormExtension<TModel>(helper);
        }

        /// <summary>
        ///     Liefert die PageId für die aktuell anzuzeigende Seite.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="idForElement"></param>
        /// <param name="idForAction"></param>
        /// <returns></returns>
        public static string GenerateId<TEntity>(this HtmlHelper htmlHelper, IdForElement idForElement, IdForAction idForAction) where TEntity : Entity {
            Require.NotNull(idForElement, "@for");
            Require.NotNull(idForAction, "action");

            return idForElement + "_" + typeof(TEntity).FullName.Replace(".", "_") + "_" + idForAction;
        }

        //public static MvcLinksExtension<TModel> Links<TModel>(this HtmlHelper<TModel> helper)
        //{
        //    return new MvcLinksExtension<TModel>(helper);
        //}

        //public static MvcPanelExtension<TModel> Panel<TModel>(this HtmlHelper<TModel> helper)
        //{
        //    return new MvcPanelExtension<TModel>(helper);
        //}

        /// <summary>
        ///     Liefert die PageId für die aktuell anzuzeigende Seite.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="view">Instanz der View.</param>
        /// <returns></returns>
        public static string GetPageId(this HtmlHelper htmlHelper, WebViewPage view) {
            if (view.IsSectionDefined("PageId")) {
                return view.RenderSection("PageId").ToString().Trim();
            }

            RouteData routeData = htmlHelper.ViewContext.HttpContext.Request.RequestContext.RouteData;
            string area = "NoArea";
            if (routeData.DataTokens.ContainsKey("area")) {
                area = routeData.DataTokens["area"].ToString();
            }
            string controller = "Home";
            if (routeData.Values.ContainsKey("controller")) {
                controller = routeData.Values["controller"].ToString();
            }

            string action = "Index";
            if (routeData.Values.ContainsKey("action")) {
                action = routeData.Values["action"].ToString();
            }

            return area + "_" + controller + "_" + action;
        }

        public static MvcSecurityExtension Security(this HtmlHelper helper) {
            return new MvcSecurityExtension(helper);
        }
    }

    public class IdForElement {
        /// <summary>
        ///     Die Id soll für ein Formular generiert werden.
        /// </summary>
        public static IdForElement Form = new IdForElement("f");

        public static IdForElement Table = new IdForElement("t");
        private readonly string _prefix;

        private IdForElement(string prefix) {
            Require.NotNullOrWhiteSpace(prefix, "prefix");

            _prefix = prefix;
        }

        public override string ToString() {
            return _prefix;
        }
    }

    public class IdForAction {
        public static IdForAction Create = new IdForAction("create");
        public static IdForAction Delete = new IdForAction("delete");
        public static IdForAction Index = new IdForAction("index");

        /// <summary>
        ///     Die Id soll für ein Formular generiert werden.
        /// </summary>
        public static IdForAction Update = new IdForAction("update");

        private readonly string _suffix;

        public IdForAction(string suffix) {
            Require.NotNullOrWhiteSpace(suffix, "suffix");

            _suffix = suffix;
        }

        public override string ToString() {
            return _suffix;
        }
    }
}