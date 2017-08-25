using System.Collections.Generic;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate {
    /// <summary>
    ///     Eine Seite ist eine Teilliste von Objekten.
    ///     Die Seite enthält Informationen über die Position der Teilliste innerhalb der gesamten Liste.
    /// </summary>
    /// <typeparam name="T">type of contained objects</typeparam>
    public interface IPage<out T> : IEnumerable<T> {
        /// <summary>
        ///     Gibt an ob es eine weitere Seite gibt.
        /// </summary>
        bool HasNextPage { get; }

        /// <summary>
        ///     Gibt an ob diese Seite einen vorhergehende Seite hat.
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        ///     Liefert die Anzahl der Elemente auf dieser Seite.
        /// </summary>
        int NumberOfElements { get; }

        /// <summary>
        ///     Liefert die Seitenzahl. Die Seitenanzahl ist 0-basiert. und kleiner als die Gesamtseitenanzahl.
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        ///     Liefert die Größe der Seite (Anzahl an Einträgen);
        /// </summary>
        int Size { get; }

        /// <summary>
        ///     Liefert die Anzahl an Elementen der gesamten Liste
        /// </summary>
        long TotalElements { get; }

        /// <summary>
        ///     Liefert die Gesamtseitenanzahl an Seiten
        /// </summary>
        int TotalPages { get; }

        
    }
    
}