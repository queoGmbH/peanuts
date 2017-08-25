using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ModelBinding {
    /// <summary>
    ///     Value Provider, der die Werte aus der Session bindet.
    ///     Dazu müssen die Werte als ValueProviderResult an der Session stehen.
    /// </summary>
    public class SessionValueProvider : IValueProvider {
        private readonly IDictionary<string, ValueProviderResult> _sessionValues;

        public SessionValueProvider(IDictionary<string, ValueProviderResult> sessionValues) {
            Require.NotNull(sessionValues, "sessionValues");

            _sessionValues = sessionValues;
        }

        /// <summary>
        /// Ruft die eindeutigen Schlüssel ab, für welche Werte hinterlegt sind.
        /// </summary>
        public IList<string> Keys {
            get { return new ReadOnlyCollection<string>(_sessionValues.Keys.ToList()); }
        }

        /// <summary>
        /// Erzeugt einen ValueProvider aus dem BindingContext. 
        /// 
        /// Anwendungsfall: 
        /// Die Werte sollen aus dem Request gelesen werden.
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public static SessionValueProvider FromBindingContext(ModelBindingContext bindingContext) {
            IDictionary<string, ValueProviderResult> valueProviderResults = new Dictionary<string, ValueProviderResult>();
            foreach (KeyValuePair<string, ModelMetadata> modelMetadata in bindingContext.PropertyMetadata) {
                string key = string.Format("{0}.{1}", bindingContext.ModelName, modelMetadata.Key);
                ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(key);
                valueProviderResults.Add(key, valueProviderResult);
            }
            SessionValueProvider sessionValueProvider = new SessionValueProvider(valueProviderResults);
            return sessionValueProvider;
        }

        /// <summary>
        /// Erzeugt einen ValueProvider aus der Session für den übergebenen BindingContext.
        /// 
        /// Anwendungsfall:
        /// Die Werte sollen aus der Session gelesen werden.
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public static SessionValueProvider FromSession(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            return FromSession(controllerContext.HttpContext.Session, bindingContext);
        }


        /// <summary>
        /// Erzeugt einen ValueProvider aus der Session für den übergebenen BindingContext.
        /// 
        /// Anwendungsfall:
        /// Die Werte sollen aus der Session gelesen werden.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public static SessionValueProvider FromSession(HttpSessionStateBase session, ModelBindingContext bindingContext) {
            Require.NotNull(bindingContext, "bindingContext");

            return SessionPersister.ReadFromSession(session, bindingContext);
        }


        /// <summary>
        ///     Bestimmt, ob die Auflistung das angegebene Präfix enthält.
        /// </summary>
        /// <returns>
        ///     True, wenn die Auflistung das angegebene Präfix enthält, andernfalls False.
        /// </returns>
        /// <param name="prefix">Das Präfix, nach dem gesucht werden soll.</param>
        public bool ContainsPrefix(string prefix) {
            return _sessionValues.Keys.Any(key => key.StartsWith(prefix));
        }

        /// <summary>
        ///     Ruft mit dem angegebenen Schlüssel ein Wertobjekt ab.
        /// </summary>
        /// <returns>
        ///     Das gefundene Wertobjekt für den angegebenen Schlüssel oder null, wenn der Schlüssel nicht gefunden wurde.
        /// </returns>
        /// <param name="key">Der Schlüssel für das Wertobjekt, das abgerufen werden soll.</param>
        public ValueProviderResult GetValue(string key) {
            if (_sessionValues.ContainsKey(key)) {
                return _sessionValues[key];
            }

            return null;
        }
    }
}