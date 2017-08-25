using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;
using Com.QueoFlow.Peanuts.Net.Core.Resources;

namespace Com.QueoFlow.Peanuts.Net.Web.Helper {
	/// <summary>
	/// </summary>
	/// <typeparam name="TGrid">Typ der Items die angezeigt werden sollen.</typeparam>
	/// <typeparam name="TModel">Typ des Models der View, in der das Grid angezeigt wird.</typeparam>
	public class Grid<TModel, TGrid> : IHtmlString {
		public const string ATTRIBUTE_COLUMN_CAN_FILTER = "webgrid-column-can-filter";
		public const string ATTRIBUTE_COLUMN_CAN_SORT = "webgrid-column-can-sort";
		public const string ATTRIBUTE_COLUMN_NAME = "webgrid-column-name";
		public const string ATTRIBUTE_COLUMN_SAVE_FILTER = "webgrid-column-save-filter";
		public const string ATTRIBUTE_COLUMN_SAVE_FILTER_KEY = "webgrid-column-save-filter-key";
		public const string ATTRIBUTE_COLUMN_SORT = "data-sort";
		public const string ATTRIBUTE_ROW_INDEX = "webgrid-row-index";
		public const string ATTRIBUTE_VALUE_TEXT = "webgrid-cell-value-as-text";

		public const string CLASS_QUEOWEBGRID = "webGrid";
		public const string CLASS_QUEOWEBGRID_CELL = "webGridCell";
		public const string CLASS_QUEOWEBGRID_HEADER_ROW = "webGridHeader";
		public const string CLASS_QUEOWEBGRID_ROW = "webGridRow";
		private readonly IDictionary<string, string> _attributes = new Dictionary<string, string>();

		private readonly IList<IGridColumn<TModel, TGrid>> _columns = new List<IGridColumn<TModel, TGrid>>();
		private readonly Expression<Func<TModel, IEnumerable<TGrid>>> _expression;
		private Func<TGrid, string> _rowIdExpression;
		private Func<TGrid, string> _rowIndexExpression;
		private readonly IList<Func<TGrid, KeyValuePair<string, string>?>> _rowAttributeExpressions = new List<Func<TGrid, KeyValuePair<string, string>?>>();

		/// <summary>
		/// Ruft den Präfix ab, der für alle Formularfelder innerhalb der Tabelle als Präfix verwendet werden soll.
		/// </summary>
		/// <remarks>
		/// Der tatsächlich verwendete Präfix kann dennoch abweichen, da zum Beispiel in verwendeten Templates der Präfix angepasst werden kann.
		/// Wird ein <see cref="RowIndex"/> angegeben, wird der Präfix pro Zeile angepasst. Er ergibt sich dann immer aus dem Tabellen-Präfix und dem Zeilen-Präfix.
		/// </remarks>
		public string GridHtmlFieldPrefix { get; private set; }

		public HtmlHelper HtmlHelper { get; set; }

		/// <summary>
		/// Ruft ab, ob alle Daten verfügbar sind und somit, das Paging, Sortieren und Filtern Client-Seitig durchgeführt werden kann.
		/// </summary>
		public bool IsAllDataAvailable { get; private set; }

		public Grid(HtmlHelper htmlHelper, Expression<Func<TModel, IEnumerable<TGrid>>> expression, bool isAllDataAvailable) {
			Require.NotNull(htmlHelper, "htmlHelper");
			Require.NotNull(expression, "expression");

			_expression = expression;
			HtmlHelper = htmlHelper;
			IsAllDataAvailable = isAllDataAvailable;

			// GridId = "tbl_" + Guid.NewGuid().ToString().Replace("-", "_");
		    GridId = "t_" + typeof(TGrid).FullName.Replace(".", "_");
		}

		/// <summary>
		///     Ruft eine schreibgeschützte Kopie der Liste mit Spalten ab.
		/// </summary>
		public IList<IGridColumn<TModel, TGrid>> Columns {
			get { return new ReadOnlyCollection<IGridColumn<TModel, TGrid>>(_columns); }
		}

		/// <summary>
		///     Ruft die Id des Grids ab.
		/// </summary>
		public string GridId { get; private set; }

		/// <summary>
		///     Ruft eine schreibgeschützte Kopie der Liste mit den Attributen der Tabelle ab.
		/// </summary>
		protected IReadOnlyDictionary<string, string> Attributes {
			get { return new ReadOnlyDictionary<string, string>(_attributes); }
		}

		/// <summary>
		///     Fügt der Tabelle ein Attribute hinzu.
		/// </summary>
		/// <param name="attributeName"></param>
		/// <param name="attributeValue"></param>
		/// <returns></returns>
		public Grid<TModel, TGrid> Attribute(string attributeName, string attributeValue) {
			Require.NotNullOrWhiteSpace(attributeName, "attributeName");

			if (_attributes.ContainsKey(attributeName)) {
				_attributes[attributeName] += " " + attributeValue;
			} else {
				_attributes.Add(attributeName, attributeValue);
			}

			return this;
		}

		/// <summary>
		/// Legt den HtmlFieldPrefix (fürs ModelBinding wichtig) fest, welcher für die Formularfelder innerhalb der Tabelle verwendet wird.
		/// </summary>
		/// <param name="htmlFieldPrefix">Der zu verwendende Präfix</param>
		/// <returns></returns>
		public Grid<TModel, TGrid> HtmlFieldPrefix(string htmlFieldPrefix) {
			GridHtmlFieldPrefix = htmlFieldPrefix;
			return this;
		}

		/// <summary>
		///     Fügt der Tabelle eine statische Spalte hinzu.
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		public GridColumn<TModel, TGrid> Column(string title) {
			GridColumn<TModel, TGrid> gridColumn = new GridColumn<TModel, TGrid>(this, title);
			_columns.Add(gridColumn);
			return gridColumn;
		}


        /// <summary>
		///     Fügt der Tabelle eine Spalte hinzu, die für jede Zeile den Wert ausgibt, der sich aus der festgelegten Expression
		///     ergibt.
		/// </summary>
		/// <typeparam name="TColumnProperty">
		///     Typ der Eigenschaft des <see cref="TGrid">Zeilen-Models</see>, welches in der Spalte
		///     angezeigt wird.
		/// </typeparam>
		/// <param name="expression">Expression über die für jede Zeile der anzuzeigende Wert ermittelt wird.</param>
		/// <param name="title">Titel der Spalte, der im Tabellenkopf angezeigt werden soll.</param>
		/// <returns>Die Spalte.</returns>
		public GridColumn<TModel, TGrid, TColumnProperty> ColumnFor<TColumnProperty>(Expression<Func<TGrid, TColumnProperty>> expression) {
            string labelByresource = LabelHelper.GetLabelFromResourceByPropertyName<Resources_Domain>(typeof(TGrid), expression.ToString().Split('.').Last());
            return ColumnFor(expression, labelByresource);
        }

        /// <summary>
        ///     Fügt der Tabelle eine Spalte hinzu, die für jede Zeile den Wert ausgibt, der sich aus der festgelegten Expression
        ///     ergibt.
        /// </summary>
        /// <typeparam name="TColumnProperty">
        ///     Typ der Eigenschaft des <see cref="TGrid">Zeilen-Models</see>, welches in der Spalte
        ///     angezeigt wird.
        /// </typeparam>
        /// <param name="expression">Expression über die für jede Zeile der anzuzeigende Wert ermittelt wird.</param>
        /// <param name="title">Titel der Spalte, der im Tabellenkopf angezeigt werden soll.</param>
        /// <returns>Die Spalte.</returns>
        public GridColumn<TModel, TGrid, TColumnProperty> ColumnFor<TColumnProperty>(Expression<Func<TGrid, TColumnProperty>> expression, string title) {
			GridColumn<TModel, TGrid, TColumnProperty> gridColumn = new GridColumn<TModel, TGrid, TColumnProperty>(this, expression, title);
			_columns.Add(gridColumn);
			return gridColumn;
		}

        public UrlColumn<TModel, TGrid> UrlColumn(Func<TGrid, string> expression, string title) {
            UrlColumn<TModel, TGrid> gridColumn = new UrlColumn<TModel, TGrid>(this, expression, title);
            _columns.Add(gridColumn);
            return gridColumn;
        }

        /// <summary>
        ///     Legt die Id des Grids fest.
        ///     &lt;table id=[Id]&gt;...&lt;/table id=[Id]&gt;
        /// </summary>
        /// <param name="id">Die festzulegende Id für das Grid.</param>
        /// <returns></returns>
        public Grid<TModel, TGrid> Id(string id) {
			Require.NotNullOrWhiteSpace(id, "id");

			GridId = id;

			return this;
		}

		/// <summary>
		///     Legt fest, wie die Id einer Zeile ermittelt wird.
		/// </summary>
		/// <param name="rowIdExpression"></param>
		/// <returns></returns>
		public Grid<TModel, TGrid> RowId(Func<TGrid, string> rowIdExpression) {
			_rowIdExpression = rowIdExpression;

			return this;
		}

		/// <summary>
		///     Legt fest, wie der Index für das Binding der Inputs und Selects einer Zeile ermittelt wird.
		/// </summary>
		/// <param name="rowIndexExpression"></param>
		/// <returns></returns>
		public Grid<TModel, TGrid> RowIndex(Func<TGrid, string> rowIndexExpression) {
			_rowIndexExpression = rowIndexExpression;

			return this;
		}

		/// <summary>
		/// Fügt einen Ausdruck hinzu, über den für jede Zeile ein Attribute vergeben werden kann.
		/// </summary>
		/// <param name="rowAttributeExpression"></param>
		/// <returns></returns>
		public Grid<TModel, TGrid> RowAttribute(Func<TGrid, KeyValuePair<string, string>?> rowAttributeExpression) {
			_rowAttributeExpressions.Add(rowAttributeExpression);

			return this;
		}

		/// <summary>
		///     Rendert das Html für die Tabelle.
		///     Darin ist alles enthalten, was innerhalb der table-Tags (&lt;table&gt;...&lt;/table&gt;) steht.
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <returns></returns>
		private MvcHtmlString ToHtml(HtmlHelper<TModel> htmlHelper) {
			string originalHtmlFieldPrefix = htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix;

			ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(_expression, htmlHelper.ViewData);

			string newHtmlFieldPrefix = "";
			if (_expression.Body.ToString().Contains(".")) {
				newHtmlFieldPrefix = Objects.GetPropertyPath(_expression);
			}
			htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = newHtmlFieldPrefix;
			htmlHelper.ViewDataContainer.ViewData.TemplateInfo.HtmlFieldPrefix = newHtmlFieldPrefix;

			TagBuilder tableTagBuilder = new TagBuilder("table");
			foreach (var attribute in _attributes.Keys) {
				tableTagBuilder.Attributes.Add(attribute, _attributes[attribute]);
			}
			tableTagBuilder.AddCssClass(CLASS_QUEOWEBGRID);
			tableTagBuilder.AddCssClass("init");
			tableTagBuilder.GenerateId(GridId);

			/*Items aus dem Model über die Expression ermitteln*/
			IEnumerable<TGrid> rowItems = modelMetadata.Model as IEnumerable<TGrid>;
			HtmlHelper<IEnumerable<TGrid>> rowsHtmlHelper = GetRowContextHtmlHelper(htmlHelper.ViewContext, htmlHelper.ViewData, htmlHelper.RouteCollection, rowItems);
			rowsHtmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix;
			rowsHtmlHelper.ViewDataContainer.ViewData.TemplateInfo.HtmlFieldPrefix = htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix;

			/*THead erstellen*/
			TagBuilder theadTagBuilder = new TagBuilder("thead");
			theadTagBuilder.InnerHtml = GetHeadHtml(rowsHtmlHelper);

			/*TBody erstellen*/
			TagBuilder tbodyTagBuilder = new TagBuilder("tbody");
			tbodyTagBuilder.InnerHtml = GetBodyHtml(rowsHtmlHelper);
			
			tableTagBuilder.InnerHtml = theadTagBuilder + " " + tbodyTagBuilder;

			/*Geänderten Binding-Präfix wieder zurücksetzen*/
			htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = originalHtmlFieldPrefix;

			return new MvcHtmlString(tableTagBuilder.ToString());
		}

		/// <summary>
		///     Ruft die Id ab, welche die Zeile bekommen soll.
		/// </summary>
		/// <param name="rowItem"></param>
		/// <returns></returns>
		protected string GetRowId(TGrid rowItem) {
			if (_rowIdExpression != null) {
				return _rowIdExpression.Invoke(rowItem);
			} else {
				return Guid.NewGuid().ToString();
			}
		}

		/// <summary>
		///     Ruft den Index ab, welche die Zeile bekommen soll.
		/// </summary>
		/// <param name="rowItem"></param>
		/// <returns></returns>
		protected string GetRowIndex(TGrid rowItem) {
			if (_rowIndexExpression != null) {
				return _rowIndexExpression.Invoke(rowItem);
			} else {
				return Guid.NewGuid().ToString();
			}
		}

		/// <summary>
		///     Hilfsmethode zur Erzeugung eines Html-Helpers mit dem selben Html-Kontext wie bisher, aber einem anderen
		///     Datenkontext.
		/// </summary>
		/// <typeparam name="THelper">Der Typ des Modells im Datenkontext</typeparam>
		/// <param name="viewContext">Der bisherige ViewContext</param>
		/// <param name="viewData">Die bisherigen viewData</param>
		/// <param name="routeCollection">Bisherige RouteCollection.</param>
		/// <param name="model">Die Instanz des Models, welches in der Zeile abgebildet werden soll.</param>
		/// <returns></returns>
		private static HtmlHelper<THelper> GetRowContextHtmlHelper<THelper>(ViewContext viewContext, ViewDataDictionary viewData, RouteCollection routeCollection, THelper model) {
			ViewDataDictionary newViewData = new ViewDataDictionary(viewData) {
				Model = model
			};

			ViewContext newViewContext = new ViewContext(viewContext.Controller.ControllerContext, viewContext.View, newViewData, viewContext.TempData, viewContext.Writer);
			IViewDataContainer viewDataContainer = new ViewPage();
			HtmlHelper<THelper> rowContextHtmlHelper = new HtmlHelper<THelper>(newViewContext, viewDataContainer, routeCollection);
			rowContextHtmlHelper.ViewData.Model = model;
            rowContextHtmlHelper.ViewContext.ViewData.Model = model;

            return rowContextHtmlHelper;
		}

		/// <summary>
		///     Das Html für den Table-Body &lt;tbody&gt; erzeugen.
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <returns></returns>
		private string GetBodyHtml(HtmlHelper<IEnumerable<TGrid>> htmlHelper) {
			StringBuilder allRowsStringBuilder = new StringBuilder();

			IDictionary<IGridColumn<TModel, TGrid>, IEnumerable<TGrid>> itemsOrderedByColumn = new Dictionary<IGridColumn<TModel, TGrid>, IEnumerable<TGrid>>();
			foreach (IGridColumn<TModel, TGrid> gridColumn in _columns) {
				itemsOrderedByColumn.Add(gridColumn, gridColumn.GetItemsOrderedByColumn(htmlHelper.ViewData.Model));
			}

			string originalHtmlFieldPrefix = htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix;
			string gridHtmlFieldPrefix = originalHtmlFieldPrefix;
			if (GridHtmlFieldPrefix != null) {
				gridHtmlFieldPrefix = GridHtmlFieldPrefix;
			}
			else {
				gridHtmlFieldPrefix = "";
			}

			htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = gridHtmlFieldPrefix;
			htmlHelper.ViewDataContainer.ViewData.TemplateInfo.HtmlFieldPrefix = gridHtmlFieldPrefix;

			foreach (TGrid rowItem in htmlHelper.ViewData.Model) {
				/*Für alle anzuzeigenden Items eine Zeile*/
				TagBuilder rowTagBuilder = new TagBuilder("tr");
				StringBuilder singleRowStringBuilder = new StringBuilder();
				HtmlHelper<TGrid> rowHtmlHelper = GetRowContextHtmlHelper(htmlHelper.ViewContext, htmlHelper.ViewData, htmlHelper.RouteCollection, rowItem);

				/*Metadaten holen*/
				string rowIndex = GetRowIndex(rowItem);
				string rowId = GetRowId(rowItem);

				if (!string.IsNullOrWhiteSpace(rowId)) {
					/*Wenn eine Id für die Zeile vergeben werden soll, dann die Id schreiben*/
					rowTagBuilder.GenerateId(rowId);
				}

				/*Evtl. Attribute an die Zeile anfügen*/
				foreach (Func<TGrid, KeyValuePair<string, string>?> rowAttributeExpression in _rowAttributeExpressions) {
					var attribute = rowAttributeExpression.Invoke(rowItem);
					if (attribute.HasValue && !string.IsNullOrWhiteSpace(attribute.Value.Key)) {
						rowTagBuilder.MergeAttribute(attribute.Value.Key, attribute.Value.Value);
					}
				}


				/*Für das Binding muss ein Hidden-Index-Feld gesetzt sein.*/
				singleRowStringBuilder.AppendLine(rowHtmlHelper.Hidden("Index", rowIndex).ToString());

				/*Für das Binding den Präfix, der im Index-Feld definiert wurde setzen.*/
				string newHtmlFieldPrefix = gridHtmlFieldPrefix + "[" + rowIndex + "]";

				rowHtmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = newHtmlFieldPrefix;
				rowHtmlHelper.ViewDataContainer.ViewData.TemplateInfo.HtmlFieldPrefix = newHtmlFieldPrefix;
				htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = newHtmlFieldPrefix;
				htmlHelper.ViewDataContainer.ViewData.TemplateInfo.HtmlFieldPrefix = newHtmlFieldPrefix;

				/*Alle Spalten anzeigen*/
				foreach (var gridColumn in _columns) {
				    string cellHtml = gridColumn.GetCellHtml(rowHtmlHelper, itemsOrderedByColumn[gridColumn]);
				    singleRowStringBuilder.AppendLine(cellHtml);
				}

				rowTagBuilder.InnerHtml = singleRowStringBuilder.ToString();

				rowTagBuilder.AddCssClass(CLASS_QUEOWEBGRID_ROW);
				if (IsAllDataAvailable) {
					rowTagBuilder.MergeAttribute(ATTRIBUTE_ROW_INDEX, htmlHelper.ViewData.Model.ToList().IndexOf(rowItem).ToString());
				}

				allRowsStringBuilder.AppendLine(rowTagBuilder.ToString());

				/*Den für das Binding geänderten Präfix der Zeile wieder zurücksetzen*/
				rowHtmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = gridHtmlFieldPrefix;
				rowHtmlHelper.ViewDataContainer.ViewData.TemplateInfo.HtmlFieldPrefix = gridHtmlFieldPrefix;
				htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = gridHtmlFieldPrefix;
				htmlHelper.ViewDataContainer.ViewData.TemplateInfo.HtmlFieldPrefix = gridHtmlFieldPrefix;
			}

			/*Den für das Binding geänderten Präfix im Grid wieder zurücksetzen*/
			htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = originalHtmlFieldPrefix;

			return allRowsStringBuilder.ToString();
		}

		/// <summary>
		///     Das Html für den Table-Head &lt;thead&gt;
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <returns></returns>
		private string GetHeadHtml(HtmlHelper<IEnumerable<TGrid>> htmlHelper) {
			TagBuilder headerRowTagBuilder = new TagBuilder("tr");
			StringBuilder headerRowStringBuilder = new StringBuilder();

			foreach (var gridColumn in _columns) {
				headerRowStringBuilder.AppendLine(gridColumn.GetHeadHtml(htmlHelper));
			}

			headerRowTagBuilder.InnerHtml = headerRowStringBuilder.ToString();
			headerRowTagBuilder.AddCssClass(CLASS_QUEOWEBGRID_HEADER_ROW);
			return headerRowTagBuilder.ToString();
		}

		/// <summary>Gibt eine HTML-codierte Zeichenfolge zurück.</summary>
		/// <returns>Eine HTML-codierte Zeichenfolge.</returns>
		public string ToHtmlString() {

			var htmlHelper = new HtmlHelper<TModel>(new ViewContext(HtmlHelper.ViewContext, HtmlHelper.ViewContext.View, HtmlHelper.ViewData, HtmlHelper.ViewContext.TempData, HtmlHelper.ViewContext.Writer), new ViewPage(), HtmlHelper.RouteCollection);
			
			htmlHelper.ViewData.Model = (TModel)HtmlHelper.ViewData.Model;

			return ToHtml(htmlHelper).ToString();
		}

		
	}
}