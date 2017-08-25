using Com.QueoFlow.Peanuts.Net.Web.Resources;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Infrastructure.ErrorHandling {
    public class HttpExceptionViewModel {

        /// <summary>
        ///     Erzeugt ein ViewModel mit einem sehr allgemeinen Titel und einer sehr allgemeinen Fehlernachricht.
        /// </summary>
        public HttpExceptionViewModel() : this(Resources_Web.common_error_Message) {
        }

        /// <summary>
        ///     Erzeugt ein ViewModel mit einem spezifischen Titel und einer spezifischen Fehlernachricht.
        /// </summary>
        public HttpExceptionViewModel(string errorTitle, string errorMessage) {
            ErrorTitle = errorTitle;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        ///     Erzeugt ein ViewModel mit einem sehr allgemeinen Titel und einer spezifischen Fehlernachricht.
        /// </summary>
        public HttpExceptionViewModel(string errorMessage) : this(Resources_Web.common_error_Message, errorMessage) {
        }

        /// <summary>
        ///     Ruft eine detaillierte Beschreibung des Fehlers ab.
        /// </summary>
        public string ErrorMessage {
            get; private set;
        }

        /// <summary>
        ///     Ruft den Titel des Fehlers ab.
        /// </summary>
        public string ErrorTitle {
            get; private set;
        }


    }
}