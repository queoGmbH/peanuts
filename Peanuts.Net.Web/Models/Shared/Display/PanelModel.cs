using System;
using System.Linq;
using System.Web.Routing;
using System.Web.WebPages;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Display {
    
    /// <summary>
    /// Model mit den Eigenschaften eines Panels.
    /// </summary>
    public class PanelModel {
        public string PanelId { get; set; }

        /// <summary>
        /// Ruft den Typen des Panels ab.
        /// 
        /// Siehe http://getbootstrap.com/components/#panels
        /// </summary>
        public PanelType PanelType { get; private set; }

        public Func<object, HelperResult> Heading { get; set; }

        /// <summary>
        /// Ruft das Collapsing-Verhalten des Panels ab.
        /// </summary>
        public PanelCollapsing Collapsing { get; private set; }

        /// <summary>
        /// Ruft ab, ob das Panel prinzipiell Collapsing unterstützt.
        /// </summary>
        public bool CanCollapse {
            get {
                return Collapsing == PanelCollapsing.CanCollapse || Collapsing == PanelCollapsing.IsCollapsed;
            } 
        }

        /// <summary>
        /// Ruft ab, ob das [div class="panel-body"][/div]-Tag gerendert werden soll oder nicht.
        /// </summary>
        public bool IncludePanelBody { get; private set; }

        public RouteValueDictionary HtmlAttributes { get; set; }

        /// <summary>
        /// Ruft einen Text ab, der im Heading als Badge (http://getbootstrap.com/components/#badges) angezeigt wird.
        /// </summary>
        public string Badge { get; private set; }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="T:System.Object"/>-Klasse.
        /// </summary>
        /// <param name="panelType">Der Typ (Aussehen) des Panels</param>
        /// <param name="heading">Der Inhalt des Headers</param>
        /// <param name="collapsing">Kann das Panel zusammengeklappt werden und wenn ja ist es zusammengeklappt?</param>
        /// <param name="includePanelBody">Soll das [div class="panel-body"][/div]-Tag gerendert werden?</param>
        /// <param name="badge">Zeichenfolge die im Heading als Badge (http://getbootstrap.com/components/#badges) angezeigt wird. Wenn NULL oder Whitespace wird kein Badge angezeigt.</param>
        /// <param name="htmlAttributes"></param>
        public PanelModel(PanelType panelType, Func<object, HelperResult> heading, PanelCollapsing collapsing, bool includePanelBody, string badge = null, RouteValueDictionary htmlAttributes = null)
        {
            Require.NotNull(panelType, "panelType");
            Require.NotNull(heading, "heading");
            
            PanelType = panelType;
            Heading = heading;
            Collapsing = collapsing;
            IncludePanelBody = includePanelBody;
            HtmlAttributes = htmlAttributes;

            Badge = badge;

            PanelId = string.Format("panel_{0}", Guid.NewGuid().ToString().Replace("-", "_"));
        }

        public string GetCollapseClass() {
            string canCollapseString = "";
            if (Collapsing == PanelCollapsing.IsCollapsed || Collapsing == PanelCollapsing.CanCollapse) {
                canCollapseString = "collapse";
            }

            string isCollapsedString = "";
            if (Collapsing == PanelCollapsing.CanCollapse) {
                isCollapsedString = "in";
            }

            return string.Format("{0} {1}", canCollapseString, isCollapsedString);
        }

        /// <summary>
        /// Ruft eine Zeichenfolge mit den Attributen für das Panel ab.
        /// </summary>
        /// <returns></returns>
        public string GetHtmlAttributes() {
            var htmlAttributes = HtmlAttributes;
            if (HtmlAttributes == null) {
                htmlAttributes = new RouteValueDictionary();
            }
            if (!htmlAttributes.ContainsKey("class")) {
                htmlAttributes.Add("class", PanelType.CssClass);
            } else {
                htmlAttributes["class"] += " " + PanelType.CssClass;
            }

            return string.Join(" ", htmlAttributes.Where(htmlAttribute => htmlAttribute.Key != null).Select(htmlAttribute => string.Format("{0}=\"{1}\"", htmlAttribute.Key.Replace("_", "-"), htmlAttribute.Value)));
        }

        public string GetCollapseTarget() {
            return string.Format("{0}_collapse", PanelId);
        }

        public string GetCollapsedClass() {
            if (Collapsing == PanelCollapsing.IsCollapsed) {
                return "collapsed";
            }

            return "";
        }
    }
}