using System.Linq;
using System.Web.Routing;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    /// <summary>
    /// Model mit den Eigenschaften eines List-Items.
    /// </summary>
    public class DynamicListItemModel {

        private const string LIST_ITEM_CLASS = "dynamic-list-item";

        public RouteValueDictionary HtmlAttributes { get; set; }

        public DynamicListItemModel(string listExpressionText, RouteValueDictionary htmlAttributes = null)
        {
            if (htmlAttributes == null) {
                HtmlAttributes = new RouteValueDictionary();
            } else {
                HtmlAttributes = htmlAttributes;
            }

            ListExpressionText = listExpressionText;
        }

        public string ListExpressionText { get; private set; }

        /// <summary>
        /// Ruft eine Zeichenfolge mit den Attributen für das Panel ab.
        /// </summary>
        /// <returns></returns>
        public string GetHtmlAttributes() {
            var htmlAttributes = HtmlAttributes;
            if (HtmlAttributes == null) {
                htmlAttributes = new RouteValueDictionary();
            }

            /*Standard-Klasse für alle Items in dynamischen Listen.*/
            if (!htmlAttributes.ContainsKey("class")) {
                htmlAttributes.Add("class", LIST_ITEM_CLASS);
            } else {
                htmlAttributes["class"] += " " + LIST_ITEM_CLASS;
            }
            
            return string.Join(" ", htmlAttributes.Where(htmlAttribute => htmlAttribute.Key != null).Select(htmlAttribute => string.Format("{0}=\"{1}\"", htmlAttribute.Key.Replace("_", "-"), htmlAttribute.Value)));
        }

    }
}