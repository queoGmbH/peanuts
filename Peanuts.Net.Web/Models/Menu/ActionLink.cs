using System.Web.Routing;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Menu {
    /// <summary>
    ///     Bildet einen Link ab,
    /// </summary>
    public class ActionLink : ActionUrl {
        public ActionLink(string text, string area, string controller, string action, object routeParameters = null, bool isModal = false) : base(area, controller, action, routeParameters) {
            Text = text;
            IsModal = isModal;
        }

        public ActionLink(string text, string area, string controller, string action, RouteValueDictionary routeParameters, bool isModal = false) : base(area, controller, action, routeParameters) {
            Text = text;
            IsModal = isModal;
        }

        /// <summary>
        /// Ruft den Text ab, der für den Link angezeigt wird.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Ruft ab , ob der Link modal geöffnet werden soll oder nicht.
        /// </summary>
        public bool IsModal { get; private set; }

    }
}