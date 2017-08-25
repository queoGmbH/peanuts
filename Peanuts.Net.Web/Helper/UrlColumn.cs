using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Helper {
    public class UrlColumn<TModel, TGridModel> : IGridColumn<TModel, TGridModel>, IHtmlString {
        private readonly IDictionary<string, string> _attributes = new Dictionary<string, string>();

        private readonly IDictionary<string, IList<Func<TGridModel, object>>> _attributesBody =
                new Dictionary<string, IList<Func<TGridModel, object>>>();

        private readonly IDictionary<string, string> _attributesHead = new Dictionary<string, string>();

        private readonly IDictionary<string, string> _attributesUrl = new Dictionary<string, string>();

        private readonly Func<TGridModel, string> _expression;

        public UrlColumn(Grid<TModel, TGridModel> grid, Func<TGridModel, string> expression, string title) {
            Grid = grid;
            _expression = expression;
            Title = title;

            ColumnId = string.Format("{0}_column_{1}", Grid.GridId, Grid.Columns.Count);
        }

        public string ColumnId { get; }

        public Grid<TModel, TGridModel> Grid { get; }

        /// <summary>
        ///     Ruft den Text innerhalb des Links ab.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        ///     Ruft den Titel der Spalte ab.
        /// </summary>
        public string Title { get; }

        /// <summary>
        ///     Definiert für diese Spalte ein Attribute, das jede Zelle in dieser Spalte erhält.
        ///     Ist das Attribute bereits für die Spalte definiert, wird der neue Wert angehängt.
        /// </summary>
        /// <param name="attributeName">Der Attribute-Name. Dieser wird in Kleinbuchstaben umgewandelt.</param>
        /// <param name="attributeValue">Der Wert, den das Attribute erhalten soll.</param>
        /// <returns></returns>
        public UrlColumn<TModel, TGridModel> Attribute(string attributeName, string attributeValue) {
            Require.NotNullOrWhiteSpace(attributeName, "attributeName");

            attributeName = attributeName.ToLower();

            if (_attributes.ContainsKey(attributeName)) {
                _attributes[attributeName] += " " + attributeValue;
            } else {
                _attributes.Add(attributeName, attributeValue);
            }
            return this;
        }

        /// <summary>
        ///     Definiert für diese Spalte ein Attribute, das jede Zelle im Body (tbody) in dieser Spalte erhält.
        ///     Ist das Attribute bereits für die Spalte definiert, wird der neue Wert angehängt.
        /// </summary>
        /// <param name="attributeName">Der Attribute-Name. Dieser wird in Kleinbuchstaben umgewandelt.</param>
        /// <param name="attributeValue">Der Wert, den das Attribute erhalten soll.</param>
        /// <returns></returns>
        public UrlColumn<TModel, TGridModel> AttributeBody(string attributeName, string attributeValue) {
            Require.NotNull(attributeValue, "attributeValue");
            Require.NotNullOrWhiteSpace(attributeName, "attributeName");

            return AttributeBody(attributeName, model => attributeValue);
        }

        public UrlColumn<TModel, TGridModel> AttributeBody(string attributeName, Func<TGridModel, object> attributeExpression) {
            Require.NotNull(attributeExpression, "attributeExpression");
            Require.NotNullOrWhiteSpace(attributeName, "attributeName");

            attributeName = attributeName.ToLower();

            if (_attributesBody.ContainsKey(attributeName)) {
                _attributesBody[attributeName].Add(attributeExpression);
                ;
            } else {
                _attributesBody.Add(attributeName, new List<Func<TGridModel, object>> { attributeExpression });
            }

            return this;
        }

        /// <summary>
        ///     Definiert für diese Spalte ein Attribute, das jede Zelle im Head (thead) in dieser Spalte erhält.
        ///     Ist das Attribute bereits für die Spalte definiert, wird der neue Wert angehängt.
        /// </summary>
        /// <param name="attributeName">Der Attribute-Name. Dieser wird in Kleinbuchstaben umgewandelt.</param>
        /// <param name="attributeValue">Der Wert, den das Attribute erhalten soll.</param>
        /// <returns></returns>
        public UrlColumn<TModel, TGridModel> AttributeHead(string attributeName, string attributeValue) {
            Require.NotNullOrWhiteSpace(attributeName, "attributeName");

            attributeName = attributeName.ToLower();

            if (_attributesHead.ContainsKey(attributeName)) {
                _attributesHead[attributeName] += " " + attributeValue;
            } else {
                _attributesHead.Add(attributeName, attributeValue);
            }
            return this;
        }

        /// <summary>
        ///     Ruft den Inhalt der Zelle ab.
        /// </summary>
        /// <param name="rowHtmlHelper"></param>
        /// <returns></returns>
        public virtual string GetCellContent(HtmlHelper<TGridModel> rowHtmlHelper) {
            TagBuilder urlTagBuilder = new TagBuilder("a");
            urlTagBuilder.MergeAttributes(_attributesUrl);
            urlTagBuilder.MergeAttribute("href", _expression.Invoke(rowHtmlHelper.ViewData.Model));
            urlTagBuilder.InnerHtml = Text;

            return urlTagBuilder.ToString();
        }

        public string GetCellHtml(HtmlHelper<TGridModel> rowHtmlHelper, IEnumerable<TGridModel> itemsOrderedByColumn) {
            TagBuilder cellTagBuilder = new TagBuilder("td");

            cellTagBuilder.Attributes.Add(Grid<TModel, TGridModel>.ATTRIBUTE_COLUMN_NAME, ColumnId);
            cellTagBuilder.MergeAttribute(Grid<TModel, TGridModel>.ATTRIBUTE_COLUMN_NAME, ColumnId);

            /*Add Attributes*/
            foreach (string attribute in _attributes.Keys) {
                cellTagBuilder.Attributes.Add(attribute, _attributes[attribute]);
            }

            /*Add Attributes only for body-Cells*/
            foreach (string attribute in _attributesBody.Keys) {
                foreach (Func<TGridModel, object> expression in _attributesBody[attribute]) {
                    object expressionResult = expression.Invoke(rowHtmlHelper.ViewData.Model);
                    if (expressionResult != null) {
                        cellTagBuilder.MergeAttribute(attribute, expressionResult.ToString());
                    }
                }
            }

            cellTagBuilder.AddCssClass(Grid<TModel, TGridModel>.CLASS_QUEOWEBGRID_CELL);
            if (Grid.IsAllDataAvailable) {
                cellTagBuilder.MergeAttribute(Grid<TModel, TGridModel>.ATTRIBUTE_ROW_INDEX,
                    itemsOrderedByColumn.ToList().IndexOf(rowHtmlHelper.ViewData.Model).ToString());
            }

            cellTagBuilder.InnerHtml = GetCellContent(rowHtmlHelper);

            return cellTagBuilder.ToString();
        }

        public string GetHeadHtml(HtmlHelper<IEnumerable<TGridModel>> helper) {
            TagBuilder headTagBuilder = new TagBuilder("th");

            /*Add Attributes*/
            foreach (string attribute in _attributes.Keys) {
                headTagBuilder.Attributes.Add(attribute, _attributes[attribute]);
            }

            headTagBuilder.MergeAttribute(Grid<TModel, TGridModel>.ATTRIBUTE_COLUMN_NAME, ColumnId);

            /*Der Inhalt entspricht dem Titel*/
            headTagBuilder.InnerHtml = GetHeadContent(helper, helper.ViewData.Model);

            return headTagBuilder.ToString();
        }

        public IEnumerable<TGridModel> GetItemsOrderedByColumn(IEnumerable<TGridModel> model) {
            return model;
        }

        public string ToHtmlString() {
            return Grid.ToHtmlString();
        }

        /// <summary>
        ///     Definiert für diese Spalte ein Attribute, das der jeweilige Anchor in der Zelle in dieser Spalte erhält.
        ///     Ist das Attribute bereits für die Spalte definiert, wird der neue Wert angehängt.
        /// </summary>
        /// <param name="attributeName">Der Attribute-Name. Dieser wird in Kleinbuchstaben umgewandelt.</param>
        /// <param name="attributeValue">Der Wert, den das Attribute erhalten soll.</param>
        /// <returns></returns>
        public UrlColumn<TModel, TGridModel> UrlAttribute(string attributeName, string attributeValue) {
            Require.NotNullOrWhiteSpace(attributeName, "attributeName");

            attributeName = attributeName.ToLower();

            if (_attributesUrl.ContainsKey(attributeName)) {
                _attributesUrl[attributeName] += " " + attributeValue;
            } else {
                _attributesUrl.Add(attributeName, attributeValue);
            }
            return this;
        }

        public UrlColumn<TModel, TGridModel> UrlText(string text) {
            Text = text;

            return this;
        }

        /// <summary>
        ///     Ruft den Inhalt der Kopfzelle ab.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        protected virtual string GetHeadContent(HtmlHelper helper, IEnumerable<TGridModel> items) {
            StringBuilder headContentStringBuilder = new StringBuilder();

            TagBuilder labelTagBuilder = new TagBuilder("label");
            labelTagBuilder.InnerHtml = Title;

            headContentStringBuilder.AppendLine(labelTagBuilder.ToString());

            return headContentStringBuilder.ToString();
        }
    }
}