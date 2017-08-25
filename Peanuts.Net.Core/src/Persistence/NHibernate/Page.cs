using System;
using System.Collections;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate {
    /// <summary>
    ///     Basisimplementierung für eine Seite beim Paging.
    /// </summary>
    /// <typeparam name="T">Der Typ der Elemente aus denen die Seite besteht.</typeparam>
    public class Page<T> : IPage<T> {
        private readonly IList<T> _content;
        private readonly IPageable _pageable;
        private readonly long _totalCount;

        /// <summary>
        ///     Erzeugt eine neue Instanz von <see cref="Page{T}" />
        /// </summary>
        /// <param name="content">Die Liste mit den Elementen für die Seite.</param>
        /// <param name="pageable">Die Informationen zur Seite.</param>
        /// <param name="totalCount">Die Anzahl aller Elemente.</param>
        public Page(IList<T> content, IPageable pageable, long totalCount) {
            _content = Require.NotNull(content, "content");
            _pageable = Require.NotNull(pageable, "pageable");
            _totalCount = totalCount;
        }

        /// <summary>
        ///     Erzeugt eine neue Instanz von <see cref="Page{T}" />
        /// </summary>
        /// <param name="content">Die Liste mit den Elementen für die Seite</param>
        /// <param name="totalCount">Die Anzahl aller Elemente.</param>
        public Page(IList<T> content, long totalCount)
            : this(content, new PageRequest(1, null == content ? 0 : content.Count), totalCount) {
        }

        /// <summary>
        ///     Gibt an ob es eine weitere Seite gibt.
        /// </summary>
        public bool HasNextPage {
            get { return PageNumber * Size < TotalElements; }
        }

        /// <summary>
        ///     Gibt an ob diese Seite einen vorhergehende Seite hat.
        /// </summary>
        public bool HasPreviousPage {
            get { return PageNumber > 1; }
        }

        /// <summary>
        ///     Liefert die Anzahl der Elemente auf dieser Seite.
        /// </summary>
        public int NumberOfElements {
            get { return _content.Count; }
        }

        /// <summary>
        ///     Liefert das zugrundeliegende <see cref="IPageable" /> mit dem die Seite generiert wurde.
        /// </summary>
        public IPageable Pageable {
            get { return _pageable; }
        }

        /// <summary>
        ///     Liefert die Seitenzahl. Die Seitenanzahl ist 0-basiert. und kleiner als die Gesamtseitenanzahl.
        /// </summary>
        public int PageNumber {
            get { return _pageable.PageNumber; }
        }

        /// <summary>
        ///     Liefert die Größe der Seite (Die maximale Anzahl an Einträgen pro Seite).
        /// </summary>
        public int Size {
            get { return _pageable.PageSize; }
        }

        /// <summary>
        ///     Liefert die Anzahl an Elementen der gesamten Liste
        /// </summary>
        public long TotalElements {
            get { return _totalCount; }
        }

        /// <summary>
        ///     Liefert die Gesamtanzahl an Seiten
        /// </summary>
        public int TotalPages {
            get { return (int)Math.Ceiling(TotalElements / (double)Size); }
        }

        /// <summary>
        ///     Gibt einen Enumerator zurück, der die Auflistung durchläuft.
        /// </summary>
        /// <returns>
        ///     Ein <see cref="T:System.Collections.Generic.IEnumerator`1" />, der zum Durchlaufen der Auflistung verwendet werden
        ///     kann.
        /// </returns>
        public IEnumerator<T> GetEnumerator() {
            return _content.GetEnumerator();
        }

        /// <summary>
        ///     Gibt einen Enumerator zurück, der eine Auflistung durchläuft.
        /// </summary>
        /// <returns>
        ///     Ein <see cref="T:System.Collections.IEnumerator" />-Objekt, das zum Durchlaufen der Auflistung verwendet werden
        ///     kann.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}