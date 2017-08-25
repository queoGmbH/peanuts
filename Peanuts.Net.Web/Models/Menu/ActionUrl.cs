using System;
using System.Web;
using System.Web.Routing;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Menu {
    public class ActionUrl {
        /// <summary>
        ///     Ruft den Namen der RootArea ab.
        /// </summary>
        public const string ROOT_AREA_NAME = "NoArea";

        public ActionUrl(string area, string controller, string action, object routeParameters) : this (area, controller, action, new RouteValueDictionary(routeParameters)) {
        }

        public ActionUrl(string area, string controller, string action, RouteValueDictionary routeParameters) {
            Area = area;
            Controller = controller;
            Action = action;
            
            if (routeParameters == null) {
                RouteParameters = new RouteValueDictionary();
            } else {
                RouteParameters = routeParameters;
            }
            if (!RouteParameters.ContainsKey("area")) {
                RouteParameters.Add("area", area);
            }
        }

        /// <summary>
        ///     Ruft den Namen der ActionMethode ab, die der Link aufruft.
        /// </summary>
        public string Action { get; private set; }

        /// <summary>
        /// Ruft die Routen-Parameter ab.
        /// </summary>
        public RouteValueDictionary RouteParameters { get; private set; }

        /// <summary>
        ///     Ruft den Namen der Area ab, auf die der Link verweisen soll.
        /// </summary>
        public string Area { get; private set; }

        /// <summary>
        ///     Ruft den Namen des Controllers ab, auf den gezeigt wird.
        /// </summary>
        public string Controller { get; private set; }

        /// <summary>
        /// Liefert die Action-Url für den übergebenen Request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ActionUrl FromRequest(HttpRequest request) {

            RouteData routeData = request.RequestContext.RouteData;
            string area = ROOT_AREA_NAME;
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

            return new ActionUrl(area, controller, action, request.RequestContext.RouteData.Values);
        }

        /// <summary>
        /// Liefert die Action-Url für den aktuellen Request.
        /// </summary>
        /// <returns></returns>
        public static ActionUrl Current() {
            return FromRequest(HttpContext.Current.Request);
        }

        /// <summary>
        ///     Ruft ab, ob dieser Link auf die entsprechende Action-Methode eines bestimmten Controllers in einer bestimmten Area
        ///     geht.
        /// </summary>
        /// <param name="area">Die Area in welcher der Controller liegen soll</param>
        /// <param name="controller">Der Controller der die Action enthalten soll</param>
        /// <param name="action">Die Action-Methode auf welche der Link gehen soll.</param>
        /// <returns></returns>
        public bool IsAction(string area, string controller, string action) {
            return IsController(area, controller) && StringComparer.InvariantCultureIgnoreCase.Compare(Action, action) == 0;
        }

        /// <summary>
        ///     Ruft ab, ob dieser Link auf einen entsprechenden Controller geht.
        /// </summary>
        /// <param name="area">Die Area in welcher der Controller liegt.</param>
        /// <param name="controller">Der Controller.</param>
        /// <returns></returns>
        public bool IsController(string area, string controller) {

            return IsArea(area) && StringComparer.InvariantCultureIgnoreCase.Compare(Controller, controller) == 0;
        }

        /// <summary>
        /// Überprüft, ob der Link auf eine Seite in der aktuellen Area verweist.
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public bool IsArea(string area) {

            if (string.IsNullOrWhiteSpace(area)) {
                area = ROOT_AREA_NAME;
            }
            
            return StringComparer.InvariantCultureIgnoreCase.Compare(Area, area) == 0;
        }
    }
}