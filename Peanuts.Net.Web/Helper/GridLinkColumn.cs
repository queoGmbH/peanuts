using System.Collections.Generic;
using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Helper {
    
    
    /// <summary>
    /// Bildet eine spalte ab, die einen oder mehrere Links enthält.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TGridModel"></typeparam>
    public class GridLinkColumn<TModel, TGridModel> : IGridColumn<TModel, TGridModel> {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public GridLinkColumn(Grid<TModel, TGridModel> grid, string title) {
        }

        /// <summary>
        /// Ruft das Grid ab, dem die Spalte zugeordnet ist.
        /// </summary>
        public Grid<TModel, TGridModel> Grid { get; private set; }

        /// <summary>
        /// Ruft die eindeutige Id der Spalte ab.
        /// </summary>
        public string ColumnId { get; private set; }

        /// <summary>
        /// Ruft die Einträge der Tabelle in der Reihenfolge ab, wenn nach dieser Spalte aufsteigend sortiert würde.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<TGridModel> GetItemsOrderedByColumn(IEnumerable<TGridModel> model) {
            /*Items können nicht nach dieser Spalte sortiert werden.*/
            return model;
        }

        public string GetCellHtml(HtmlHelper<TGridModel> rowHtmlHelper, IEnumerable<TGridModel> itemsOrderedByColumn) {
            throw new System.NotImplementedException();
        }

        public string GetHeadHtml(HtmlHelper<IEnumerable<TGridModel>> htmlHelper) {
            throw new System.NotImplementedException();
        }
    }

    public class GridLink<TModel, TGridModel> {
        /// <summary>
        /// Erstellt ein neues Tag mit dem angegebenen Tagnamen.
        /// </summary>
        public GridLink() {
        }

        /// <summary>
        /// Ruft die Url des Links ab.
        /// </summary>
        public string Url { get; private set; }


    }
}