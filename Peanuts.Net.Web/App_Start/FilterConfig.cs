using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ErrorHandling;

namespace Com.QueoFlow.Peanuts.Net.Web {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            /* Fehlerhandling über Attribute. Filter mit einer höheren Order werden zuerst ausgeführt! */

            /*Als aller erstes muss der SecurityContext gesetzt werden.*/
            //filters.Add(new SecurityContextFilter(), int.MaxValue);

            // Wenn der Modelstate nicht valide ist, einen entsprechenden Statuscode in der Antwort setzen
            filters.Add(new ModelStateErrorActionFilter());

            // Die meisten Argument null Exceptions kommen, weil nichts gefunden wird. 
            filters.Add(new HandleArgumentNullExceptionAttribute(), 20);

            /*Filter für das behandeln von Http-Exceptions.*/
            filters.Add(new HttpExceptionFilterAttribute(), 10);

            /*Allgemeines Fehlerhandling*/
            filters.Add(new HandleErrorAttribute(), 1);
        }
    }
}