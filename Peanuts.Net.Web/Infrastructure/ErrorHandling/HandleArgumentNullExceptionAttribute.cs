using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Common.Logging;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ErrorHandling {
    /// <summary>
    ///     ExceptionFilter, der ArgumentNullExceptions behandelt.
    ///     Diese werden geworfen,  wenn ein Parameter für eine Action-Methode unerwartet NULL ist. Zum Beispiel wenn der
    ///     Nutzer die Id in der URL manipuliert.
    /// </summary>
    public class HandleArgumentNullExceptionAttribute : HandleErrorAttribute {
        private readonly ILog _log = LogManager.GetLogger(typeof(HandleArgumentNullExceptionAttribute));

        /// <summary>
        ///     Wird aufgerufen, wenn eine Ausnahme auftritt.
        /// </summary>
        /// <param name="filterContext">Der Aktionsfilterkontext.</param>
        /// <exception cref="T:System.ArgumentNullException">Der <paramref name="filterContext" />-Parameter ist null.</exception>
        public override void OnException(ExceptionContext filterContext) {

            if (filterContext.Exception is ArgumentNullException) {
                filterContext.Exception = new HttpException((int)HttpStatusCode.NotFound, "Ein Parameter des Requests war null, obwohl er das nicht sein darf.", filterContext.Exception);
            }
        }
    }
}