using System.Net;
using System.Web;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Infrastructure.ErrorHandling;
using Com.QueoFlow.Peanuts.Net.Web.Resources;

using Common.Logging;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ErrorHandling {
    /// <summary>
    ///     ExceptionFilter, der HTTP-Exception behandelt.
    /// </summary>
    public class HttpExceptionFilterAttribute : HandleErrorAttribute {
        private readonly ILog _log = LogManager.GetLogger(typeof(HttpExceptionFilterAttribute));

        /// <summary>
        ///     Wird aufgerufen, wenn eine Ausnahme auftritt.
        /// </summary>
        /// <param name="filterContext">Der Aktionsfilterkontext.</param>
        /// <exception cref="T:System.ArgumentNullException">Der <paramref name="filterContext" />-Parameter ist null.</exception>
        public override void OnException(ExceptionContext filterContext) {
            if (filterContext.Exception is HttpException) {
                if (!IsAjaxRequest(filterContext.HttpContext.Request)) {
                    HandleHttpException(filterContext);
                }
            }
        }

        /// <summary>
        ///     Behandle die HttpException je nach Status-Code.
        /// </summary>
        /// <param name="exceptionContext"></param>
        private void HandleHttpException(ExceptionContext exceptionContext) {
            exceptionContext.ExceptionHandled = true;
            exceptionContext.HttpContext.Response.Clear();
            exceptionContext.HttpContext.Response.TrySkipIisCustomErrors = true;

            HttpException exception = (HttpException)exceptionContext.Exception;

            HttpExceptionViewModel errorViewModel;
            switch (exception.GetHttpCode()) {
                case (int)HttpStatusCode.NotFound: {
                    errorViewModel = new HttpExceptionViewModel(Resources_Web.error_NotFound_Title, Resources_Web.error_NotFound_Message);
                    exceptionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                }
                case (int)HttpStatusCode.Unauthorized: {
                    /*Die Seite steht nur für angemeldete Nutzer bereit.*/
                    errorViewModel = new HttpExceptionViewModel(Resources_Web.error_Unauthorized_Title, Resources_Web.error_Unauthorized_Message);
                    exceptionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                }
                case (int)HttpStatusCode.Forbidden: {
                    /*Der Nutzer ist nicht berechtigt die Seite aufzurufen.*/
                    errorViewModel = new HttpExceptionViewModel(Resources_Web.error_Forbidden_Title, Resources_Web.error_Forbidden_Message);
                    exceptionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;
                }
                default: {
                    /*Für nicht näher spezifizierte Fehler, wird eine allgemeine Fehlerseite angezeigt.*/
                    errorViewModel = new HttpExceptionViewModel(Resources_Web.common_error_Title, Resources_Web.common_error_Message);
                    exceptionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                }
            }

            /*Fehlerhaften Request in Log-Dateien schreiben*/
            LogRequestError(exceptionContext);

            if (IsAjaxRequest(exceptionContext.HttpContext.Request)) {
                /*Wenn es sich um einen Ajax-Request handelt, wird nur eine Teilansicht ohne Html-Head zurückgegeben.*/
                exceptionContext.Result = new PartialViewResult() {
                    ViewName = "Infrastructure/ErrorHandling/HttpExceptionPartial",
                    ViewData = new ViewDataDictionary<HttpExceptionViewModel>(errorViewModel),
                    TempData = exceptionContext.Controller.TempData
                };
            } else {
                exceptionContext.Result = new ViewResult {
                    ViewName = "Infrastructure/ErrorHandling/HttpException",
                    MasterName = Master,
                    ViewData = new ViewDataDictionary<HttpExceptionViewModel>(errorViewModel),
                    TempData = exceptionContext.Controller.TempData
                };
            }
        }

        /// <summary>
        ///     Ist der Request ein Ajax-Request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool IsAjaxRequest(HttpRequestBase request) {
            // if the request is AJAX return JSON else view.
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private void LogRequestError(ExceptionContext exceptionContext) {
            string controllerName = (string)exceptionContext.RouteData.Values["controller"];
            string actionName = (string)exceptionContext.RouteData.Values["action"];
            _log.ErrorFormat("Fehler beim Verarbeiten der URL: {0}", exceptionContext.HttpContext.Request.RawUrl);
            _log.ErrorFormat("Fehler beim Verarbeiten eines Request {0} {1}", exceptionContext.Exception, controllerName, actionName);
        }
    }
}