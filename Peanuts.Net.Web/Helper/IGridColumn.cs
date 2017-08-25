using System.Collections.Generic;
using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Helper {
    public interface IGridColumn<TModel, TGridModel> {
        
        /// <summary>
        /// Ruft das Grid ab, dem die Spalte zugeordnet ist.
        /// </summary>
        Grid<TModel, TGridModel> Grid { get; }

        /// <summary>
        /// Ruft die eindeutige Id der Spalte ab.
        /// </summary>
        string ColumnId { get; }

        /// <summary>
        /// Ruft die Einträge der Tabelle in der Reihenfolge ab, wenn nach dieser Spalte aufsteigend sortiert würde.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IEnumerable<TGridModel> GetItemsOrderedByColumn(IEnumerable<TGridModel> model);

        /// <summary>
        /// Ruft das Html dieser Spalte für die Zeile ab, die durch das Model am übergebenen HtmlHelper definiert ist.
        /// </summary>
        /// <param name="rowHtmlHelper">Der HtmlHelper zum Rendern der Zelle, der als Model das Objekt enthält, welches durch die Zeile in der diese Zelle enthalten ist abgebildet wird.</param>
        /// <param name="itemsOrderedByColumn">Die Objekte die in der Tabelle angezeigt werden, in der Reihenfolge, wenn nach dieser Spalte aufsteigend sortiert wird.</param>
        /// <returns></returns>
        string GetCellHtml(HtmlHelper<TGridModel> rowHtmlHelper, IEnumerable<TGridModel> itemsOrderedByColumn);

        /// <summary>
        /// Ruft das Html dieser Spalte für die Kopf-Zeile der Tabelle ab.
        /// </summary>
        /// <param name="helper">Der HtnmlHelper zum Render mit einer Liste der Einträge der Tabelle als Model.</param>
        /// <returns></returns>
        string GetHeadHtml(HtmlHelper<IEnumerable<TGridModel>> helper);

    }
}