using System.Collections.Specialized;
using System.Linq;
using System.Web.Routing;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Display {
    /// <summary>
    /// Model für die Anzeige eines Pagings.
    /// </summary>
    public class PaginationModel {
        private readonly string _paginationPrefix;
        private readonly string _pageNumberRouteParameter;
        private readonly string _pageSizeRouteParameter;

        public RouteValueDictionary RouteValueDictionary { get; set; }

        public NameValueCollection ParameterCollection { get; set; }

        public PaginationModel(RouteValueDictionary routeValueDictionary, NameValueCollection parameterCollection, bool hasNextPage, bool hasPreviousPage, int pageNumber, int totalPages, int size, string paginationRouteParamterPrefix = null, string pageNumberRouteParameter = "page", string pageSizeRouteParameter = "pageSize" ) {
            Require.NotNullOrWhiteSpace(pageNumberRouteParameter, "pageNumberRouteParameter");
            Require.NotNullOrWhiteSpace(pageSizeRouteParameter, "pageSizeRouteParameter");

            _paginationPrefix = paginationRouteParamterPrefix;
            _pageNumberRouteParameter = pageNumberRouteParameter;
            _pageSizeRouteParameter = pageSizeRouteParameter;

            RouteValueDictionary = routeValueDictionary;
            ParameterCollection = parameterCollection;
            HasNextPage = hasNextPage;
            HasPreviousPage = hasPreviousPage;
            PageNumber = pageNumber;
            TotalPages = totalPages;
            Size = size;
        }

        /// <summary>
        /// Liefert einen Wert der angibt, ob es eine vorherige Seite gibt.
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// Liefert die Nummer der aktuellen Seite (Seitennummern sind 1-basiert)
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Liefert die Anzahl aller Seiten.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Liefert die maximale Anzahl an Elementen die auf einer Seite dargestellt werden soll.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Liefert einen Wert der angibt, ob es eine weitere Seite gibt.
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Ruft einen optionalen Präfix für die RouteParameter für die Pagination ab.
        /// </summary>
        public string PaginationPrefix {
            get { return _paginationPrefix; }
        }

        /// <summary>
        /// Liefert das RouteValueDictionary für die Seite
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public RouteValueDictionary GetRouteValueDictionary(int pageNumber) {

            string pageNumberRouteParameterName = _pageNumberRouteParameter;
            string pageSizeRouteParameterName = _pageSizeRouteParameter;

            if (!string.IsNullOrWhiteSpace(_paginationPrefix)) {
                pageNumberRouteParameterName = string.Format("{0}.{1}", _paginationPrefix, pageNumberRouteParameterName);
                pageSizeRouteParameterName = string.Format("{0}.{1}", _paginationPrefix, pageSizeRouteParameterName);
            }


            RouteValueDictionary routeValueDictionary = new RouteValueDictionary(RouteValueDictionary);
            routeValueDictionary.Add(pageNumberRouteParameterName, pageNumber.ToString());
            if (routeValueDictionary.ContainsKey("RouteModels")) {
                routeValueDictionary.Remove("RouteModels");
            }
            // Muss nur angehängt werden, wenn es nicht die Standardanzahl ist.
            if (ParameterCollection.AllKeys.Contains(pageSizeRouteParameterName)) {
                routeValueDictionary.Add(pageSizeRouteParameterName, Size.ToString());
            }

            // Andere QueryParameter an die Url anfügen
            foreach (string key in ParameterCollection.AllKeys.Where(key => !key.StartsWith(pageNumberRouteParameterName))) {
                if (!routeValueDictionary.ContainsKey(key)) {
                    routeValueDictionary.Add(key, ParameterCollection[key]);
                }
            }

            return routeValueDictionary;
        }
    }
}