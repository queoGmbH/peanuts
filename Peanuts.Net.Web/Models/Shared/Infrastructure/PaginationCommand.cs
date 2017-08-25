using System.Diagnostics;

using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Infrastructure {

    [DebuggerDisplay("Seite {_pageNumber} mit {_pageSize} Element(en) pro Seite")]
    public class PaginationCommand : IPageable {
        private int _pageNumber;
        private int _pageSize;

        public PaginationCommand() : this(1, 25) {
        }

        public PaginationCommand(int pageNumber, int pageSize) {
            _pageNumber = pageNumber;
            _pageSize = pageSize;
        }

        /// <summary>
        ///     Liefert das erste Element relativ zur Anzahl aller Elemente.
        /// </summary>
        public int FirstItem {
            get { return PageIndex * PageSize; }
        }

        /// <summary>
        ///     Liefert den Index der Seite die zurückgegeben werden soll.
        ///     Die erste Seite hat den Index 0!
        /// </summary>
        public int PageIndex {
            get { return PageNumber - 1; }
        }

        /// <summary>
        ///     Liefert die Nummer der Seite die zurückgegeben werden soll.
        ///     Die erste Seite hat die Seitennummer 1!
        /// </summary>
        public int PageNumber {
            get { return _pageNumber; }
            set { _pageNumber = value; }
        }

        /// <summary>
        ///     Liefert die Anzahl der Elemente die pro Seite zurückgegeben werden soll.
        /// </summary>
        public int PageSize {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
    }
}