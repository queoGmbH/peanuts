using System;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate {
    /// <summary>
    ///     Basisimplementierung von <see cref="IPageable" />.
    /// </summary>
    /// <remarks>
    ///     Die Seiten sind 1-basiert indiziert, d.h. eine 1 für die PageNumber liefert die erste Seite.
    /// </remarks>
    public class PageRequest : IPageable {
        /// <summary>
        ///     Erzeugt eine neue Instanz von <see cref="PageRequest" />.
        /// </summary>
        /// <param name="pageNumber">Die angeforderte Seite (Seitenzählung beginnt mit 1)</param>
        /// <param name="pageSize">Die Anzahl der Elemente pro Seite.</param>
        public PageRequest(int pageNumber, int pageSize) {
            if (pageNumber < 1) {
                throw new ArgumentException("PageNumber darf nicht kleiner als 1 sein.");
            }
            if (pageSize < 0) {
                /*Standard-Implementierung: Ist so gewollt, auch wenn eine Abfrage von 0 Elementen zunächst sinnfrei erscheint.*/
                throw new ArgumentException("PageSize darf nicht kleiner als 0 sein.");
            }
            PageNumber = pageNumber;
            PageIndex = pageNumber - 1;
            PageSize = pageSize;
        }

        /// <summary>
        ///     Ruft den Page-Request ab, der alle Einträge liefert.
        /// </summary>
        public static PageRequest All {
            get { return new PageRequest(1, int.MaxValue); }
        }

        /// <summary>
        ///     Get the first item relatively to the total number of items.
        /// </summary>
        public int FirstItem {
            get { return PageIndex * PageSize; }
        }

        /// <summary>
        ///     Liefert den Index der Seite die zurückgegeben werden soll.
        ///     Die erste Seite hat den Index 0!
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        ///     Get the page to be returned.
        /// </summary>
        public int PageNumber { get; private set; }

        /// <summary>
        ///     Get the number of items to be returned.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        ///     Ruft den Page-Request ab, der den ersten Eintrag liefert.
        /// </summary>
        public static PageRequest First {
            get {
                return new PageRequest(1,1);
            }
        }
    }
}