using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Helper {
    /// <summary>
    /// Modus für die Spalte.
    /// </summary>
    public enum GridColumnMode {
        /// <summary>
        /// In der Spalte werden Werte nur angezeigt.
        /// </summary>
        Display,
        /// <summary>
        /// In der Spalte können Werte bearbeitet werden.
        /// </summary>
        Editor
    }

    /// <summary>
    ///     Bildet eine Spalte in einer <see cref="Grid" /> ab.
    /// </summary>
    /// <typeparam name="TModel">Der Typ des Models der View, in dem das Grid angezeigt wird, dem dieses Spalte zugeordnet ist.</typeparam>
    /// <typeparam name="TGridModel">
    ///     Der Typ der Items, die in einer Zeile der Tabelle angezeigt werden, der diese Spalte
    ///     zugeordnet ist.
    /// </typeparam>
    public class GridColumn<TModel, TGridModel> : IGridColumn<TModel, TGridModel>, IHtmlString {
        protected const string FILTER_ALL_DEFAULT_TEXT = "Alle";
        protected const string FILTER_NULLOREMPTY_DEFAULT_TEXT = "(leer)";

        private readonly IDictionary<string, string> _attributes = new Dictionary<string, string>();
        private readonly IDictionary<string, IList<Func<TGridModel, object>>> _attributesBody = new Dictionary<string, IList<Func<TGridModel, object>>>();
        private readonly IDictionary<string, string> _attributesHead = new Dictionary<string, string>();
        private GridColumnMode _columnMode = GridColumnMode.Display;

        private Func<TGridModel, string> _filterExpression;
        private Func<TGridModel, IComparable> _orderByExpression;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public GridColumn(Grid<TModel, TGridModel> grid, string title) : this(grid) {
            Title = title;

            ColumnId = String.Format("{0}_column_{1}", Grid.GridId, Grid.Columns.Count);
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        protected GridColumn(Grid<TModel, TGridModel> grid) {
            Require.NotNull(grid, "grid");

            Grid = grid;
        }

        /// <summary>
        ///     Ruft ab, ob nach der Spalte sortiert werden kann.
        /// </summary>
        public bool CanOrderByColumn {
            get { return _orderByExpression != null; }
        }

        /// <summary>
        ///     Ruft die Id der Spalte ab.
        /// </summary>
        public string ColumnId { get; private set; }

        /// <summary>
        ///     Ruft die Tabelle ab, der diese Spalte zugeordnet ist.
        /// </summary>
        public Grid<TModel, TGridModel> Grid { get; private set; }

        /// <summary>
        ///     Ruft den Titel der Spalte ab.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        ///     Ruft die Attribute ab, die jede Zelle in dieser Spalte enthält.
        /// </summary>
        protected IReadOnlyDictionary<string, string> Attributes {
            get { return new ReadOnlyDictionary<string, string>(_attributes); }
        }

        /// <summary>
        ///     Ruft die Attribute ab, die jede Zelle in dieser Spalte enthält.
        /// </summary>
        protected IReadOnlyDictionary<string, IList<Func<TGridModel, object>>> BodyAttributes {
            get { return new ReadOnlyDictionary<string, IList<Func<TGridModel, object>>>(_attributesBody); }
        }

        /// <summary>
        ///     Ruft ab oder legt fest, ob nach dieser Spalte gefiltert werden kann.
        /// </summary>
        protected bool CanFilter {
            get { return _filterExpression != null; }
        }

        /// <summary>
        ///     Ruft ab, ob die Spalte nur zu Anzeige-Zwecken (<see cref="GridColumnMode.Display" />) dient oder zur Bearbeitung (
        ///     <see cref="GridColumnMode.Editor" />).
        /// </summary>
        protected GridColumnMode ColumnMode {
            get { return _columnMode; }
        }

        /// <summary>
        ///     Ruft den Text ab, der für die Checkbox zum Aktivieren oder Deaktivieren aller Filter angezeigt wird oder legt
        ///     diesen fest.
        /// </summary>
        protected string FilterAllText { get; set; }

        /// <summary>
        ///     Ruft den Schlüssel ab, unter welchem der Filter gespeichert wird.
        /// </summary>
        protected string FilterKey { get; private set; }

        /// <summary>
        ///     Ruft den Text ab, der zum Filtern von NULL oder empty verwendet wird oder legt diesen fest.
        /// </summary>
        protected string FilterNullOrEmptyText { get; set; }

        /// <summary>
        ///     Ruft die Attribute ab, die jede Zelle in dieser Spalte enthält.
        /// </summary>
        protected IReadOnlyDictionary<string, string> HeadAttributes {
            get { return new ReadOnlyDictionary<string, string>(_attributesHead); }
        }

        /// <summary>
        ///     Ruft einen evtl. angegebenen Namen für das Display-/Editor-Template ab, das für die Anzeige der Werte dieser Spalte
        ///     verwendet wird.
        /// </summary>
        protected string TemplateName { get; private set; }


        protected string Format { get; private set; }

        /// <summary>
        ///     Definiert für diese Spalte ein Attribute, das jede Zelle in dieser Spalte erhält.
        ///     Ist das Attribute bereits für die Spalte definiert, wird der neue Wert angehängt.
        /// </summary>
        /// <param name="attributeName">Der Attribute-Name. Dieser wird in Kleinbuchstaben umgewandelt.</param>
        /// <param name="attributeValue">Der Wert, den das Attribute erhalten soll.</param>
        /// <returns></returns>
        public GridColumn<TModel, TGridModel> Attribute(string attributeName, string attributeValue) {
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
        public GridColumn<TModel, TGridModel> AttributeBody(string attributeName, string attributeValue) {
            Require.NotNull(attributeValue, "attributeValue");
            Require.NotNullOrWhiteSpace(attributeName, "attributeName");

            
            return AttributeBody(attributeName, (model) => attributeValue);
        }

        public GridColumn<TModel, TGridModel> AttributeBody(string attributeName, Func<TGridModel, object> attributeExpression) {
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
        public GridColumn<TModel, TGridModel> AttributeHead(string attributeName, string attributeValue) {
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
        ///     Legt fest, dass diese Spalte Werte lediglich anzeigt.
        ///     Intern bedeutet das, dass die Methode Html.Display beim Rendern des Wertes verwendet wird.
        ///     Das DisplayTemplate wird dabei automatisch ermittelt.
        /// </summary>
        /// <returns></returns>
        public GridColumn<TModel, TGridModel> Display() {
            _columnMode = GridColumnMode.Display;
            Format = null;
            // TemplateName = "";

            return this;
        }

        /// <summary>
        ///     Legt fest, dass diese Spalte Werte lediglich anzeigt und dafür das übergebene DisplayTemplate verwendet.
        ///     Intern bedeutet das, dass die Methode Html.Display beim Rendern des Wertes verwendet wird.
        ///     Als DisplayTemplate wird das Template verwendet, was anhand des übergebenen Namens ermittelt werden kann.
        /// </summary>
        /// <returns></returns>
        public GridColumn<TModel, TGridModel> Display(string templateName) {
            Require.NotNullOrWhiteSpace(templateName, "templateName");

            _columnMode = GridColumnMode.Display;
            TemplateName = templateName;
            Format = null;

            return this;
        }

        /// <summary>
        ///     Legt fest, dass diese Spalte Werte lediglich anzeigt und dafür das übergebene DisplayTemplate verwendet.
        ///     Intern bedeutet das, dass die Methode Html.Display beim Rendern des Wertes verwendet wird.
        ///     Als DisplayTemplate wird das Template verwendet, was anhand des übergebenen Namens ermittelt werden kann.
        /// </summary>
        /// <returns></returns>
        public GridColumn<TModel, TGridModel> DisplayFormat(string format) {
            Require.NotNull(format, "format");

            _columnMode = GridColumnMode.Display;
            //TemplateName = "";
            Format = format;

            return this;
        }

        /// <summary>
        ///     Legt fest, dass diese Spalte zum Bearbeiten von Werten dient.
        ///     Intern bedeutet das, dass die Methode Html.Editor beim Rendern des Wertes verwendet wird.
        ///     Das EditorTemplate wird dabei automatisch ermittelt.
        /// </summary>
        /// <returns></returns>
        public GridColumn<TModel, TGridModel> Editor() {
            _columnMode = GridColumnMode.Editor;
            Format = null;
            //TemplateName = "";

            return this;
        }

        /// <summary>
        ///     Legt fest, dass diese Spalte zum Bearbeiten von Werten dient und dafür das übergebene EditorTemplate verwendet.
        ///     Intern bedeutet das, dass die Methode Html.Editor beim Rendern des Wertes verwendet wird.
        ///     Als EditorTemplate wird das Template verwendet, was anhand des übergebenen Namens ermittelt werden kann.
        /// </summary>
        /// <returns></returns>
        public GridColumn<TModel, TGridModel> Editor(string templateName) {
            Require.NotNullOrWhiteSpace(templateName, "templateName");

            _columnMode = GridColumnMode.Editor;
            Format = null;
            TemplateName = templateName;

            return this;
        }

        /// <summary>
        ///     Legt das Template für diese Spalte fest. Je nach <see cref="columnMode"/> können die Werte in der Spalte bearbeitet werden oder nur angezeigt.
        ///     Intern bedeutet das, dass entweder die Methode Html.Editor oder Html.Display beim Rendern des Wertes verwendet wird.
        /// </summary>
        /// <param name="columnMode">Dient die Spalte der Bearbeitung oder der Anzeige</param>
        /// <param name="editorTemplateName">Das zu verwendende Template bei <see cref="GridColumnMode.Editor"/></param>
        /// <param name="displayTemplateName">Das zu verwendende Template bei <see cref="GridColumnMode.Display"/></param>
        /// <returns></returns>
        public GridColumn<TModel, TGridModel> EditorOrDisplay(GridColumnMode columnMode, string editorTemplateName, string displayTemplateName) {
            Require.NotNullOrWhiteSpace(editorTemplateName, "editorTemplateName");
            Require.NotNullOrWhiteSpace(displayTemplateName, "displayTemplateName");

            _columnMode = columnMode;
            Format = null;
            if (columnMode == GridColumnMode.Display) {
                TemplateName = displayTemplateName;
            } else {
                TemplateName = editorTemplateName;
            }
            
            return this;
        }

        /// <summary>
        ///     Legt das Template für diese Spalte fest. Je nach <see cref="columnMode"/> können die Werte in der Spalte bearbeitet werden oder nur angezeigt.
        ///     Intern bedeutet das, dass entweder die Methode Html.Editor oder Html.Display beim Rendern des Wertes verwendet wird.
        /// </summary>
        /// <param name="columnMode">Dient die Spalte der Bearbeitung oder der Anzeige</param>
        /// <param name="templateName">Das zu verwendende Template</param>
        /// <returns></returns>
        public GridColumn<TModel, TGridModel> EditorOrDisplay(GridColumnMode columnMode, string templateName) {
            Require.NotNullOrWhiteSpace(templateName, "templateName");
            
            _columnMode = columnMode;
            TemplateName = templateName;
            Format = null;

            return this;
        }

        /// <summary>
        ///     Legt das Template für diese Spalte fest. Je nach <see cref="columnMode"/> können die Werte in der Spalte bearbeitet werden oder nur angezeigt.
        ///     Intern bedeutet das, dass entweder die Methode Html.Editor oder Html.Display beim Rendern des Wertes verwendet wird.
        /// 
        ///     Als Editor- bzw. Display-Template wird das Template verwendet, was anhand des übergebenen Namens ermittelt werden kann.
        /// </summary>
        /// <param name="columnMode">Dient die Spalte der Bearbeitung oder der Anzeige</param>
        /// <returns></returns>
        public GridColumn<TModel, TGridModel> EditorOrDisplay(GridColumnMode columnMode) {
            _columnMode = columnMode;
            Format = null;
            //TemplateName = "";

            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filterAllText">Der Text der für das Aktivieren bzw. Deaktivieren aller Filter angezeigt wird.</param>
        /// <param name="nullOrEmptyText">
        ///     Der Text der für das Aktivieren bzw. Deaktivieren des Filters für NULL oder empty Items
        ///     angezeigt wird.
        /// </param>
        /// <returns></returns>
        public GridColumn<TModel, TGridModel> Filter(Func<TGridModel, string> filter, string filterAllText = FILTER_ALL_DEFAULT_TEXT, string nullOrEmptyText = FILTER_NULLOREMPTY_DEFAULT_TEXT) {
            Require.NotNull(filter, "filter");

            _filterExpression = filter;
            FilterNullOrEmptyText = nullOrEmptyText;
            FilterAllText = filterAllText;

            return this;
        }

        /// <summary>
        ///     Ruft den zu verwendenden SaveFilterKey ab. Ist kein abweichernder Key definiert, wird der Fallback verwendet.
        /// </summary>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public string GetActualSaveFilterKey(string fallback) {
            if (string.IsNullOrWhiteSpace(FilterKey)) {
                return fallback;
            }

            return FilterKey;
        }

        /// <summary>
        ///     Ruft den Inhalt der Zelle ab.
        /// </summary>
        /// <param name="rowHtmlHelper"></param>
        /// <returns></returns>
        protected virtual string GetCellContent(HtmlHelper<TGridModel> rowHtmlHelper) {
            if (_columnMode == GridColumnMode.Display) {
                if (!String.IsNullOrWhiteSpace(TemplateName)) {
                    return rowHtmlHelper.DisplayForModel(TemplateName, rowHtmlHelper.ViewData.Model).ToString();
                } else {
                    if (Format != null) {
                        return string.Format(Format, rowHtmlHelper.ViewData.Model);
                    } else {
                        return rowHtmlHelper.DisplayForModel(rowHtmlHelper.ViewData.Model).ToString();
                    }
                }
            } else {
                if (!String.IsNullOrWhiteSpace(TemplateName)) {
                    return rowHtmlHelper.EditorForModel(TemplateName, rowHtmlHelper.ViewData.Model).ToString();
                } else {
                    return rowHtmlHelper.EditorForModel(rowHtmlHelper.ViewData.Model).ToString();
                }
            }
        }

        /// <summary>
        ///     Ruft das Html dieser Spalte für die Zeile ab, die durch das Model am übergebenen HtmlHelper definiert ist.
        /// </summary>
        /// <param name="rowHtmlHelper">
        ///     Der HtmlHelper zum Rendern der Zelle, der als Model das Objekt enthält, welches durch die
        ///     Zeile in der diese Zelle enthalten ist abgebildet wird.
        /// </param>
        /// <param name="itemsOrderedByColumn">
        ///     Die Objekte die in der Tabelle angezeigt werden, in der Reihenfolge, wenn nach
        ///     dieser Spalte aufsteigend sortiert wird.
        /// </param>
        /// <returns></returns>
        public virtual string GetCellHtml(HtmlHelper<TGridModel> rowHtmlHelper, IEnumerable<TGridModel> itemsOrderedByColumn) {
            TagBuilder cellTagBuilder = new TagBuilder("td");

            cellTagBuilder.Attributes.Add(Grid<TModel, TGridModel>.ATTRIBUTE_COLUMN_NAME, ColumnId);
            if (_filterExpression != null) {
                cellTagBuilder.Attributes.Add(Grid<TModel, TGridModel>.ATTRIBUTE_VALUE_TEXT, GetFilterValue(rowHtmlHelper));
            }
            cellTagBuilder.MergeAttribute(Grid<TModel, TGridModel>.ATTRIBUTE_COLUMN_NAME, ColumnId);

            /*Add Attributes*/
            foreach (var attribute in _attributes.Keys) {
                cellTagBuilder.Attributes.Add(attribute, _attributes[attribute]);
            }

            /*Add Attributes only for body-Cells*/
            foreach (var attribute in _attributesBody.Keys) {
                foreach (var expression in _attributesBody[attribute]) {
                    object expressionResult = expression.Invoke(rowHtmlHelper.ViewData.Model);
                    if (expressionResult != null) {
                        cellTagBuilder.MergeAttribute(attribute, expressionResult.ToString());
                    }
                }
                
            }

            cellTagBuilder.AddCssClass(Grid<TModel, TGridModel>.CLASS_QUEOWEBGRID_CELL);
            if (Grid.IsAllDataAvailable) {
                cellTagBuilder.MergeAttribute(Grid<TModel, TGridModel>.ATTRIBUTE_ROW_INDEX, itemsOrderedByColumn.ToList().IndexOf(rowHtmlHelper.ViewData.Model).ToString());
            }

            cellTagBuilder.InnerHtml = GetCellContent(rowHtmlHelper);

            return cellTagBuilder.ToString();
        }

        /// <summary>
        ///     Ruft das Html dieser Spalte für die Kopf-Zeile der Tabelle ab.
        /// </summary>
        /// <param name="helper">Der HtnmlHelper zum Render mit einer Liste der Einträge der Tabelle als Model.</param>
        /// <returns></returns>
        public string GetHeadHtml(HtmlHelper<IEnumerable<TGridModel>> helper) {
            TagBuilder headTagBuilder = new TagBuilder("th");

            /*Add Attributes*/
            foreach (var attribute in _attributes.Keys) {
                headTagBuilder.MergeAttribute(attribute, _attributes[attribute]);
            }
            foreach (var attribute in _attributesHead.Keys) {
                headTagBuilder.MergeAttribute(attribute, _attributesHead[attribute]);
            }

            headTagBuilder.MergeAttribute(Grid<TModel, TGridModel>.ATTRIBUTE_COLUMN_NAME, ColumnId);
            headTagBuilder.MergeAttribute(Grid<TModel, TGridModel>.ATTRIBUTE_COLUMN_CAN_SORT, CanOrderByColumn.ToString());
            headTagBuilder.MergeAttribute(Grid<TModel, TGridModel>.ATTRIBUTE_COLUMN_CAN_FILTER, CanFilter.ToString());
            // TODO
            headTagBuilder.MergeAttribute(Grid<TModel, TGridModel>.ATTRIBUTE_COLUMN_SAVE_FILTER, true.ToString());
            headTagBuilder.MergeAttribute(Grid<TModel, TGridModel>.ATTRIBUTE_COLUMN_SAVE_FILTER_KEY, string.Format("Filter#{0}", GetActualSaveFilterKey(Grid.GridId + "#" + ColumnId)));

            /*Der Inhalt entspricht dem Titel*/
            headTagBuilder.InnerHtml = GetHeadContent(helper, helper.ViewData.Model);

            return headTagBuilder.ToString();
        }

        /// <summary>
        ///     Ruft alle Einträge der Tabelle in der Reihenfolge ab, die entsteht wenn nach dieser Spalte aufsteigend sortiert
        ///     wird.
        /// </summary>
        /// <param name="gridItems">Die Einträge der Tabelle (in ihrer ursprünglichen Reihenfolge).</param>
        /// <returns></returns>
        public IEnumerable<TGridModel> GetItemsOrderedByColumn(IEnumerable<TGridModel> gridItems) {
            if (_orderByExpression != null && gridItems != null) {
                /*Es ist eine Sortierung angegeben => Items danach sortieren.*/
                return gridItems.OrderBy(_orderByExpression).ToList();
            } else {
                /*Es ist keine abweichende Sortierung angegeben => Reihenfolge beibehalten*/
                return gridItems;
            }
        }

        /// <summary>
        ///     Legt die Id dieser Spalte fest.
        ///     Diese wird aber nicht als Html-Id (#) verwendet, sondern lediglich um die Spalte eindeutig zu identifizieren.
        ///     Es wird beim Setzen überprüft, ob die Id wirklich eindeutig ist.
        /// </summary>
        /// <param name="id">Die in der Tabelle eindeutige Id dieser Spalte.</param>
        /// <returns></returns>
        public GridColumn<TModel, TGridModel> Id(string id) {
            /*Es muss eine Id angegeben werden.*/
            Require.NotNullOrWhiteSpace(id, "id");
            /*Prüfen, dass id in der Tabelle eindeutig ist.*/
            Require.IsFalse(() => Equals(Grid.Columns.SingleOrDefault(col => col.ColumnId == id)), "id");

            ColumnId = id;

            return this;
        }

        /// <summary>
        ///     Aktiviert für diese Spalte, dass der Nutzer nach dieser auf- oder absteigend sortieren kann.
        ///     Was als Grundlage für die Sortierung verwendet wird kann dabei beliebig über die Expression festgelegt werden.
        /// </summary>
        /// <param name="orderByExpression">Wie wird der Wert ermittelt, der für die Sortierung herangezogen wird.</param>
        /// <returns></returns>
        public GridColumn<TModel, TGridModel> OrderBy(Func<TGridModel, IComparable> orderByExpression) {
            _orderByExpression = orderByExpression;

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

            if (CanOrderByColumn) {
                TagBuilder sortTagBuilder = new TagBuilder("a");

                if (Grid.IsAllDataAvailable) {
                    /*Sortierung per Java-Script*/
                    sortTagBuilder.Attributes.Add("href", "#");
                } else {
                    /*Sortierung über neuladen der Seite*/
                    sortTagBuilder.Attributes.Add("href", "#");
                }

                sortTagBuilder.AddCssClass("webgrid-sort-by-column-link");

                headContentStringBuilder.AppendLine(sortTagBuilder.ToString());
            }

            if (CanFilter) {
                /*Icon/Link welcher den Filter toggled*/
                string filterControlId = String.Format("filter_by_column_{0}_{1}", Grid.GridId, ColumnId);
                TagBuilder filterTagBuilder = new TagBuilder("a");
                filterTagBuilder.Attributes.Add("href", "#");
                filterTagBuilder.AddCssClass("webgrid-filter-by-column-link");
                filterTagBuilder.Attributes.Add("data-toggle-target", filterControlId);
                headContentStringBuilder.AppendLine(filterTagBuilder.ToString());

                /*Filter mit einträgen*/
                headContentStringBuilder.AppendLine(GetFilterByColumnDiv(helper, filterControlId, items));
            }

            return headContentStringBuilder.ToString();
        }

        /// <summary>
        ///     Erzeugt das Html für die Checkbox zum Aktivieren bzw. Deaktivieren aller Filter für diese Spalte.
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        private string GetFilterAllCheckbox(HtmlHelper helper) {
            string filterAllCheckBoxId = helper.ViewDataContainer.ViewData.TemplateInfo.GetFullHtmlFieldId(String.Format("{0}_{1}_filter_all", Grid.GridId, ColumnId));
            TagBuilder selectAllLabelTagBuilder = new TagBuilder("label");
            selectAllLabelTagBuilder.Attributes.Add("for", filterAllCheckBoxId);
            selectAllLabelTagBuilder.InnerHtml = helper.CheckBox(filterAllCheckBoxId, true, new { @class = "filter-all-checkbox", webgrid_column_name = ColumnId }) + FilterAllText;
            string value = selectAllLabelTagBuilder.ToString();
            return value;
        }

        /// <summary>
        ///     Erzeugt das Html für das Div, dass die Filter für diese Spalte enthält.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="filterControlId"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        private string GetFilterByColumnDiv(HtmlHelper helper, string filterControlId, IEnumerable<TGridModel> items) {
            IList<string> distinctItemValuesAsText = items.Select(_filterExpression).Distinct().OrderBy(filterValue => filterValue).ToList();

            TagBuilder filterByColumnDivTagBuilder = new TagBuilder("div");
            filterByColumnDivTagBuilder.Attributes.Add("id", filterControlId);
            filterByColumnDivTagBuilder.AddCssClass("filter-by-column");

            StringBuilder filterByColumnDivContentStringBuilder = new StringBuilder();

            /*Checkbox zum Aktivieren/Deaktivieren aller Filter*/
            filterByColumnDivContentStringBuilder.AppendLine(GetFilterAllCheckbox(helper));

            foreach (string itemValueAsText in distinctItemValuesAsText) {
                string filterValue = itemValueAsText;
                if (string.IsNullOrEmpty(filterValue)) {
                    filterValue = FilterNullOrEmptyText;
                }
                filterByColumnDivContentStringBuilder.AppendLine(GetFilterByValueCheckbox(helper, filterValue));
            }

            filterByColumnDivTagBuilder.InnerHtml = filterByColumnDivContentStringBuilder.ToString();

            return filterByColumnDivTagBuilder.ToString();
        }

        /// <summary>
        ///     Ruft das Html für einen Filter-Eintrag ab.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="filterValue"></param>
        /// <returns></returns>
        private string GetFilterByValueCheckbox(HtmlHelper helper, string filterValue) {
            TagBuilder labelTagBuilder = new TagBuilder("label");

            string checkBoxId = helper.ViewData.TemplateInfo.GetFullHtmlFieldId(String.Format("{0}_{1}_{2}", Grid.GridId, ColumnId, filterValue));
            string checkBoxName = helper.ViewData.TemplateInfo.GetFullHtmlFieldName(String.Format("{0}_{1}_{2}", Grid.GridId, ColumnId, filterValue));
            labelTagBuilder.Attributes.Add("for", checkBoxId);

            TagBuilder checkboxTagBuilder = new TagBuilder("input");
            checkboxTagBuilder.AddCssClass("filter-single-checkbox");
            checkboxTagBuilder.Attributes.Add("checked", "checked");
            checkboxTagBuilder.Attributes.Add("data-filter-value", filterValue);
            checkboxTagBuilder.Attributes.Add("id", checkBoxId);
            checkboxTagBuilder.Attributes.Add("name", checkBoxName);
            checkboxTagBuilder.Attributes.Add("type", "checkbox");
            checkboxTagBuilder.Attributes.Add("value", "true");
            checkboxTagBuilder.Attributes.Add("webgrid-column-name", ColumnId);

            labelTagBuilder.InnerHtml = String.Format("{0} {1}", checkboxTagBuilder, filterValue);

            return labelTagBuilder.ToString();
        }

        /// <summary>
        ///     Ruft den Wert ab, der als Filterwert für die Zelle in dieser Zeile verwendet wird.
        /// </summary>
        /// <param name="rowHtmlHelper"></param>
        /// <returns></returns>
        private string GetFilterValue(HtmlHelper<TGridModel> rowHtmlHelper) {
            string filterText = _filterExpression(rowHtmlHelper.ViewData.Model);
            if (String.IsNullOrEmpty(filterText)) {
                filterText = FilterNullOrEmptyText;
            }
            return filterText;
        }

        /// <summary>Gibt eine HTML-codierte Zeichenfolge zurück.</summary>
        /// <returns>Eine HTML-codierte Zeichenfolge.</returns>
        public string ToHtmlString() {
            return Grid.ToHtmlString();
        }
    }

    /// <summary>
    ///     Spalte in einer Tabelle, deren Anzeige durch eine Expression auf den
    ///     <see cref="TGridModel">Typ eines Zeilen-Eintrages</see> abgebildet/ermittelt wird.
    /// </summary>
    /// <typeparam name="TModel">Der Typ des Models der View, in dem das Grid angezeigt wird, dem dieses Spalte zugeordnet ist.</typeparam>
    /// <typeparam name="TGridModel">
    ///     Der Typ der Items, die in einer Zeile der Tabelle angezeigt werden, der diese Spalte
    ///     zugeordnet ist.
    /// </typeparam>
    /// <typeparam name="TColumn">
    ///     Der Typ der Eigenschaft des <see cref="TGridModel" />s auf den die Expression dieser Spalte
    ///     zeigt.
    /// </typeparam>
    public class GridColumn<TModel, TGridModel, TColumn> : GridColumn<TModel, TGridModel> {
        public GridColumn(Grid<TModel, TGridModel> grid, Expression<Func<TGridModel, TColumn>> expression, string title) : base(grid, title) {
            Require.NotNull(expression, "expression");

            Expression = expression;
            CompiledExpression = Expression.Compile();
        }

        protected Func<TGridModel, TColumn> CompiledExpression { get; private set; }

        /// <summary>
        ///     Ruft die Expression für diese Spalte ab.
        /// </summary>
        public Expression<Func<TGridModel, TColumn>> Expression { get; private set; }

        /// <summary>
        ///     Fügt der Spalte ein Attribute hinzu.
        ///     Alle Zellen dieser Spalte erhalten dieses Attribute und seinen Wert.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public new GridColumn<TModel, TGridModel, TColumn> Attribute(string attributeName, string attributeValue) {
            base.Attribute(attributeName, attributeValue);
            return this;
        }

        public new GridColumn<TModel, TGridModel, TColumn> AttributeBody(string attributeName, string attributeValue) {
            base.AttributeBody(attributeName, attributeValue);
            return this;
        }

        public new GridColumn<TModel, TGridModel, TColumn> AttributeHead(string attributeName, string attributeValue) {
            base.AttributeHead(attributeName, attributeValue);
            return this;
        }

        /// <summary>
        ///     Legt fest, dass diese Spalte Werte lediglich anzeigt.
        ///     Intern bedeutet das, dass die Methode Html.Display beim Rendern des Wertes verwendet wird.
        ///     Das DisplayTemplate wird dabei automatisch ermittelt.
        /// </summary>
        /// <returns></returns>
        public new GridColumn<TModel, TGridModel, TColumn> Display() {
            base.Display();
            return this;
        }

        /// <summary>
        ///     Legt fest, dass diese Spalte Werte lediglich anzeigt und dafür das übergebene DisplayTemplate verwendet.
        ///     Intern bedeutet das, dass die Methode Html.Display beim Rendern des Wertes verwendet wird.
        ///     Als DisplayTemplate wird das Template verwendet, was anhand des übergebenen Namens ermittelt werden kann.
        /// </summary>
        /// <returns></returns>
        public new GridColumn<TModel, TGridModel, TColumn> Display(string templateName) {
            base.Display(templateName);
            return this;
        }

        /// <summary>
        ///     Legt fest, dass diese Spalte Werte lediglich anzeigt und dafür das übergebene DisplayTemplate verwendet.
        ///     Intern bedeutet das, dass die Methode Html.Display beim Rendern des Wertes verwendet wird.
        ///     Als DisplayTemplate wird das Template verwendet, was anhand des übergebenen Namens ermittelt werden kann.
        /// </summary>
        /// <returns></returns>
        public new GridColumn<TModel, TGridModel, TColumn> DisplayFormat(string format) {
            base.DisplayFormat(format);
            return this;
        }

        /// <summary>
        ///     Legt fest, dass diese Spalte zum Bearbeiten von Werten dient.
        ///     Intern bedeutet das, dass die Methode Html.Editor beim Rendern des Wertes verwendet wird.
        ///     Das EditorTemplate wird dabei automatisch ermittelt.
        /// </summary>
        /// <returns></returns>
        public new GridColumn<TModel, TGridModel, TColumn> Editor() {
            base.Editor();
            return this;
        }

        /// <summary>
        ///     Legt fest, dass diese Spalte zum Bearbeiten von Werten dient und dafür das übergebene EditorTemplate verwendet.
        ///     Intern bedeutet das, dass die Methode Html.Editor beim Rendern des Wertes verwendet wird.
        ///     Als EditorTemplate wird das Template verwendet, was anhand des übergebenen Namens ermittelt werden kann.
        /// </summary>
        /// <returns></returns>
        public new GridColumn<TModel, TGridModel, TColumn> Editor(string templateName) {
            base.Editor(templateName);
            return this;
        }


        /// <summary>
        ///     Legt das Template für diese Spalte fest. Je nach <see cref="columnMode"/> können die Werte in der Spalte bearbeitet werden oder nur angezeigt.
        ///     Intern bedeutet das, dass entweder die Methode Html.Editor oder Html.Display beim Rendern des Wertes verwendet wird.
        /// 
        ///     Als Editor- bzw. Display-Template wird das Template verwendet, was anhand des übergebenen Namens ermittelt werden kann.
        /// </summary>
        /// <param name="columnMode">Dient die Spalte der Bearbeitung oder der Anzeige</param>
        /// <param name="editorTemplateName">Das zu verwendende Template bei <see cref="GridColumnMode.Editor"/></param>
        /// <param name="displayTemplateName">Das zu verwendende Template bei <see cref="GridColumnMode.Display"/></param>
        /// <returns></returns>
        public new GridColumn<TModel, TGridModel, TColumn> EditorOrDisplay(GridColumnMode columnMode, string editorTemplateName, string displayTemplateName) {
            base.EditorOrDisplay(columnMode, editorTemplateName, displayTemplateName);
            return this;
        }

        /// <summary>
        ///     Legt das Template für diese Spalte fest. Je nach <see cref="columnMode"/> können die Werte in der Spalte bearbeitet werden oder nur angezeigt.
        ///     Intern bedeutet das, dass entweder die Methode Html.Editor oder Html.Display beim Rendern des Wertes verwendet wird.
        /// 
        ///     Als Editor- bzw. Display-Template wird das Template verwendet, was anhand des übergebenen Namens ermittelt werden kann.
        /// </summary>
        /// <param name="columnMode">Dient die Spalte der Bearbeitung oder der Anzeige</param>
        /// <param name="templateName">Das zu verwendende Template</param>
        /// <returns></returns>
        public new GridColumn<TModel, TGridModel, TColumn> EditorOrDisplay(GridColumnMode columnMode, string templateName) {
            base.EditorOrDisplay(columnMode, templateName);
            return this;
        }

        /// <summary>
        ///     Legt das Template für diese Spalte fest. Je nach <see cref="columnMode"/> können die Werte in der Spalte bearbeitet werden oder nur angezeigt.
        ///     Intern bedeutet das, dass entweder die Methode Html.Editor oder Html.Display beim Rendern des Wertes verwendet wird.
        /// 
        ///     Als Editor- bzw. Display-Template wird das Template verwendet, was anhand des übergebenen Namens ermittelt werden kann.
        /// </summary>
        /// <param name="columnMode">Dient die Spalte der Bearbeitung oder der Anzeige</param>
        /// <returns></returns>
        public new GridColumn<TModel, TGridModel, TColumn> EditorOrDisplay(GridColumnMode columnMode) {
            base.EditorOrDisplay(columnMode);
            return this;
        }

        /// <summary>
        ///     Definiert, dass nach der Spalte gefiltert werden kann.
        /// </summary>
        /// <param name="filterExpression">Wie wird der Filter-Wert ermittelt, der für diese Spalte verwendet wird.</param>
        /// <param name="filterAllText">Der Text der für den Eintrag verwendet wird, der alle Filter aktivierte oder deaktiviert.</param>
        /// <param name="nullOrEmptyText">
        ///     Der Text der für den Filter-Eintrag verwendet wird, wenn dort NULL oder Empty drin
        ///     stünde.
        /// </param>
        /// <returns></returns>
        public new GridColumn<TModel, TGridModel, TColumn> Filter(Func<TGridModel, string> filterExpression, string filterAllText = FILTER_ALL_DEFAULT_TEXT, string nullOrEmptyText = FILTER_NULLOREMPTY_DEFAULT_TEXT) {
            base.Filter(filterExpression, filterAllText, nullOrEmptyText);

            return this;
        }

        /// <summary>
        ///     Definiert, dass nach der Spalte gefiltert werden kann.
        /// </summary>
        /// <param name="filterAllText">Der Text der für den Eintrag verwendet wird, der alle Filter aktivierte oder deaktiviert.</param>
        /// <param name="nullOrEmptyText">
        ///     Der Text der für den Filter-Eintrag verwendet wird, wenn dort NULL oder Empty drin
        ///     stünde.
        /// </param>
        /// <returns></returns>
        public GridColumn<TModel, TGridModel, TColumn> Filter(string filterAllText = FILTER_ALL_DEFAULT_TEXT, string nullOrEmptyText = FILTER_NULLOREMPTY_DEFAULT_TEXT) {
            /*Standard-Filter mit ToString*/
            Func<TGridModel, string> filterExpression = delegate(TGridModel model) {
                TColumn cellValue = Expression.Compile().Invoke(model);
                if (cellValue != null) {
                    return cellValue.ToString();
                } else {
                    return null;
                }
            };

            base.Filter(filterExpression, filterAllText, nullOrEmptyText);

            return this;
        }

        /// <summary>
        ///     Ruft den Inhalt einer Zelle dieser Spalte ab.
        /// </summary>
        /// <param name="rowHtmlHelper"></param>
        /// <returns></returns>
        protected override string GetCellContent(HtmlHelper<TGridModel> rowHtmlHelper) {
            ViewDataDictionary viewDataDictionary = new ViewDataDictionary { TemplateInfo = new TemplateInfo { HtmlFieldPrefix = rowHtmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix } };

            if (ColumnMode == GridColumnMode.Display) {
                if (!string.IsNullOrWhiteSpace(TemplateName)) {
                    return rowHtmlHelper.DisplayFor(Expression, TemplateName, viewDataDictionary).ToString();
                } else {
                    if (Format != null) {
                        
                        TColumn cellvalue = CompiledExpression.Invoke(rowHtmlHelper.ViewData.Model);
                        return string.Format(Format, cellvalue);
                    } else {
                        return rowHtmlHelper.DisplayFor(Expression, viewDataDictionary).ToString();
                    }
                }
            } else {
                if (!string.IsNullOrWhiteSpace(TemplateName)) {
                    return rowHtmlHelper.EditorFor(Expression, TemplateName, viewDataDictionary).ToString();
                } else {
                    return rowHtmlHelper.EditorFor(Expression, viewDataDictionary).ToString();
                }
            }
        }

        /// <summary>
        ///     Aktiviert für diese Spalte, dass der Nutzer nach dieser auf- oder absteigend sortieren kann.
        ///     Als Wert der für die Sortierung herangezogen wird, dient entweder die ToString-Methode des Zellenwertes oder wenn
        ///     der Typ IComparable implementiert, dann dieser.
        /// </summary>
        /// <returns></returns>
        public GridColumn<TModel, TGridModel, TColumn> OrderBy() {
            Func<TGridModel, IComparable> orderByExpression;

            if (typeof(IComparable).IsAssignableFrom(typeof(TColumn))) {
                orderByExpression = delegate(TGridModel model) {
                    return Expression.Compile().Invoke(model) as IComparable;
                };
            } else {
                orderByExpression = delegate(TGridModel model) {
                    TColumn cellValue = Expression.Compile().Invoke(model);
                    if (cellValue != null) {
                        return cellValue.ToString();
                    } else {
                        return null;
                    }
                };
            }

            base.OrderBy(orderByExpression);

            return this;
        }

        /// <summary>
        ///     Aktiviert für diese Spalte, dass der Nutzer nach dieser auf- oder absteigend sortieren kann.
        ///     Was als Grundlage für die Sortierung verwendet wird kann dabei beliebig über die Expression festgelegt werden.
        /// </summary>
        /// <param name="orderByExpression">Wie wird der Wert ermittelt, der für die Sortierung herangezogen wird.</param>
        /// <returns></returns>
        public new GridColumn<TModel, TGridModel, TColumn> OrderBy(Func<TGridModel, IComparable> orderByExpression) {
            base.OrderBy(orderByExpression);

            return this;
        }
        
    }
}