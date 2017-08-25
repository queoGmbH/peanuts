using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ErrorHandling {
    /// <summary>
    ///     ActionFilter der bei einem invaliden ModelState den StatusCode auf 422 setzt.
    ///     https://de.wikipedia.org/wiki/HTTP-Statuscode => 422 = "Unprocessable Entity"
    ///     Anfrage wegen semantischer Fehler abgelehnt
    /// </summary>
    public class ModelStateErrorActionFilter : IResultFilter {
        /// <summary>
        ///     Wird aufgerufen, nachdem ein Aktionsergebnis ausgeführt wurde.
        /// </summary>
        /// <param name="filterContext">Der Filterkontext.</param>
        public void OnResultExecuted(ResultExecutedContext filterContext) {
        }

        /// <summary>
        ///     Wird aufgerufen, bevor ein Aktionsergebnis ausgeführt wird.
        /// </summary>
        /// <param name="filterContext">Der Filterkontext.</param>
        public void OnResultExecuting(ResultExecutingContext filterContext) {
            if (!filterContext.Controller.ViewData.ModelState.IsValid) {
                /* https://de.wikipedia.org/wiki/HTTP-Statuscode => 422 = "Unprocessable Entity"
                 * Verwendet, wenn weder die Rückgabe von Statuscode 415 noch 400 gerechtfertigt wäre, 
                 * eine Verarbeitung der Anfrage jedoch zum Beispiel wegen semantischer Fehler abgelehnt wird. */
                filterContext.HttpContext.Response.StatusCode = 422;
            }
        }
    }
}