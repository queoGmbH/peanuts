using System.Linq;
using System.Web.Routing;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    
    public class DynamicListModel {


        private const string LIST_CLASS = "dynamic-list";

        public RouteValueDictionary HtmlAttributes { get; set; }

        public DynamicListModel(string expressionText, RouteValueDictionary htmlAttributes = null)
        {
            HtmlAttributes = htmlAttributes;
            ExpressionText = expressionText;
        }

        public string ExpressionText { get; private set; }

        /// <summary>
        /// Ruft eine Zeichenfolge mit den Attributen für das Panel ab.
        /// </summary>
        /// <returns></returns>
        public string GetHtmlAttributes() {
            var htmlAttributes = HtmlAttributes;
            if (HtmlAttributes == null) {
                htmlAttributes = new RouteValueDictionary();
            }

            /*Standard-Klasse für alle dynamischen Listen.*/
            if (!htmlAttributes.ContainsKey("class")) {
                htmlAttributes.Add("class", LIST_CLASS);
            } else {
                htmlAttributes["class"] += " " + LIST_CLASS;
            }
            
            return string.Join(" ", htmlAttributes.Where(htmlAttribute => htmlAttribute.Key != null).Select(htmlAttribute => string.Format("{0}=\"{1}\"", htmlAttribute.Key.Replace("_", "-"), htmlAttribute.Value)));
        }

    }
}