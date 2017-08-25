namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate {
    /// <summary>
    ///     Interface for pagination information.
    /// </summary>
    public interface IPageable {
        /// <summary>
        ///     Liefert das erste Element relativ zur Anzahl aller Elemente.
        /// </summary>
        int FirstItem { get; }

        /// <summary>
        ///     Liefert den Index der Seite die zurückgegeben werden soll.
        ///     Die erste Seite hat den Index 0!
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        ///     Liefert die Nummer der Seite die zurückgegeben werden soll.
        ///     Die erste seite hat die Seitennummer 1!
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        ///     Liefert die Anzahl der Elemente die pro Seite zurückgegeben werden soll.
        /// </summary>
        int PageSize { get; }
    }
}