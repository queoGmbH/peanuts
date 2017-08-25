using System;
using System.Linq;
using System.Linq.Expressions;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;
using Com.QueoFlow.Peanuts.Net.Core.Resources;

namespace Com.QueoFlow.Peanuts.Net.Web.Helper {
    public static class GridExtension {

        public static GridColumn<TModel, TGrid, TColumn> ColumnFor<TModel, TGrid, TColumn>(this Grid<TModel, TGrid> grid, Expression<Func<TGrid, TColumn>> expression, string title) {
            return grid.ColumnFor(expression, title);
        }

        /// <summary>
        ///     Fügt der Tabelle der diese Spalte zugeordnet ist eine weitere Spalte hinzu, die für jede Zeile den Wert ausgibt,
        ///     der sich aus der festgelegten Expression
        ///     ergibt.
        /// </summary>
        /// <typeparam name="TColumn">
        ///     Typ der Eigenschaft des <see cref="TGrid">Zeilen-Models</see>, welches in der Spalte
        ///     angezeigt wird.
        /// </typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TGrid"></typeparam>
        /// <param name="column"></param>
        /// <param name="expression">Expression über die für jede Zeile der anzuzeigende Wert ermittelt wird.</param>
        /// <param name="title">Titel der Spalte, der im Tabellenkopf angezeigt werden soll.</param>
        /// <returns>Die Spalte.</returns>
        public static GridColumn<TModel, TGrid, TColumn> ColumnFor<TModel, TGrid, TColumn>(this IGridColumn<TModel, TGrid> column, Expression<Func<TGrid, TColumn>> expression, string title) {
            return column.Grid.ColumnFor(expression, title);
        }

        public static UrlColumn<TModel, TGrid> UrlColumn<TModel, TGrid>(this IGridColumn<TModel, TGrid> column, Func<TGrid, string> expression) {
            return column.Grid.UrlColumn(expression, "");
        }

        public static UrlColumn<TModel, TGrid> UrlColumn<TModel, TGrid>(this IGridColumn<TModel, TGrid> column, Func<TGrid, string> expression, string title) {
            return column.Grid.UrlColumn(expression, title);
        }

        /// <summary>
        ///     Fügt der Tabelle der diese Spalte zugeordnet ist eine weitere Spalte hinzu, die für jede Zeile den Wert ausgibt,
        ///     der sich aus der festgelegten Expression
        ///     ergibt.
        /// </summary>
        /// <typeparam name="TColumn">
        ///     Typ der Eigenschaft des <see cref="TGrid">Zeilen-Models</see>, welches in der Spalte
        ///     angezeigt wird.
        /// </typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TGrid"></typeparam>
        /// <param name="column"></param>
        /// <param name="expression">Expression über die für jede Zeile der anzuzeigende Wert ermittelt wird.</param>
        /// <param name="title">Titel der Spalte, der im Tabellenkopf angezeigt werden soll.</param>
        /// <returns>Die Spalte.</returns>
        public static GridColumn<TModel, TGrid, TColumn> ColumnFor<TModel, TGrid, TColumn>(this IGridColumn<TModel, TGrid> column,
                Expression<Func<TGrid, TColumn>> expression) {
            
            string labelByresource = LabelHelper.GetLabelFromResourceByPropertyName<Resources_Domain>(typeof(TGrid), expression.ToString().Split('.').Last());
            return column.Grid.ColumnFor(expression, labelByresource);
        }



        /// <summary>
        ///     Legt fest, wie die Id einer Zeile ermittelt wird.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="rowIdExpression"></param>
        /// <returns></returns>
        public static Grid<TModel, TGrid> RowId<TModel, TGrid>(this IGridColumn<TModel, TGrid> column, Func<TGrid, string> rowIdExpression) {
            return column.Grid.RowId(rowIdExpression);
        }

        /// <summary>
        ///     Legt fest, wie der Index für das Binding der Inputs und Selects einer Zeile ermittelt wird.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="rowIndexExpression"></param>
        /// <returns></returns>
        public static Grid<TModel, TGrid> RowIndex<TModel, TGrid>(this IGridColumn<TModel, TGrid> column,
                Func<TGrid, string> rowIndexExpression) {
            return column.Grid.RowIndex(rowIndexExpression);
        }
    }
}