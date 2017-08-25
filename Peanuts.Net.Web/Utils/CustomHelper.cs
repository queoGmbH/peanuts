using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Com.QueoFlow.Peanuts.Net.Web.Utils {
    // FIXME: Dafür gibt es doch schon fertige und meines Erachtens bessere Lösungen. Warum werden nicht die übernommen?
    public static class CustomHelper {
        public static MvcHtmlString CustomDropdownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> list) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }
            string name = ExpressionHelper.GetExpressionText(expression);
            return CustomDropdownList(htmlHelper, name, list);
        }

        public static MvcHtmlString CustomListBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> list, int size = 4) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }
            string name = ExpressionHelper.GetExpressionText(expression);
            return CustomListBox(htmlHelper, name, list, size);
        }

        private static MvcHtmlString CustomDropdownList(this HtmlHelper htmlHelper, string name,
                IEnumerable<SelectListItem> list) {
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (String.IsNullOrEmpty(fullName)) {
                throw new ArgumentException("name");
            }

            return htmlHelper.Partial("Partials/HtmlHelpers/Select",
                    list,
                    new ViewDataDictionary { { "name", fullName } });
        }

        private static MvcHtmlString CustomListBox(this HtmlHelper htmlHelper, string name,
                IEnumerable<SelectListItem> list, int size = 4) {
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (String.IsNullOrEmpty(fullName)) {
                throw new ArgumentException("name");
            }

            return htmlHelper.Partial("Partials/HtmlHelpers/MultipleSelect",
                    list,
                    new ViewDataDictionary { { "name", fullName }, { "size", size } });
        }
    }
}