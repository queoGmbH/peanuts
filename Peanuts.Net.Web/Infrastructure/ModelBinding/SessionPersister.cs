using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ModelBinding {
    /// <summary>
    /// Persister der Methoden bereitstellt, um Werte in eine Session zu schreiben oder daraus zu lesen.
    /// </summary>
    public static class SessionPersister {

        /// <summary>
        /// Schreibt die Werte an die Session.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="values"></param>
        public static void WriteToSession(this HttpSessionStateBase session, SessionValueProvider values) {
            Require.NotNull(session, "session");
            Require.NotNull(values, "values");

            /*Alle Werte aus dem Value-Provider an die Session schreiben.*/
            foreach (string key in values.Keys) {
                ValueProviderResult valueProviderResult = values.GetValue(key);
                if (valueProviderResult != null) {
                    /*Neu gesetzten Wert an die Session schreiben*/
                    session[key] = valueProviderResult;
                }
                else {
                    /*Wenn der Wert nicht mehr gesetzt ist, dann leeren, damit keine alten Werte an der Session zurückbleiben*/
                    session[key] = null;
                }
            }
        }

        /// <summary>
        /// Ließt Werte aus der Session.
        /// </summary>
        /// <param name="session">Die Session aus der gelesen werden soll.</param>
        /// <param name="bindingContext">Der BindingContext, für welchen die Werte aus der Session gelesen werden sollen.</param>
        public static SessionValueProvider ReadFromSession(this HttpSessionStateBase session, ModelBindingContext bindingContext) {
            IDictionary<string, ValueProviderResult> valueProviderResults = new Dictionary<string, ValueProviderResult>();

            if (session != null) {
                foreach (KeyValuePair<string, ModelMetadata> modelMetadata in bindingContext.PropertyMetadata) {
                    string key = string.Format("{0}.{1}", bindingContext.ModelName, modelMetadata.Key);
                    if (session[key] != null) {
                        /*Wert für das Property steht an der Session => in ValueProvider schreiben.*/
                        valueProviderResults.Add(key, session[key] as ValueProviderResult);
                    }
                }
            }

            return new SessionValueProvider(valueProviderResults);
        }

        /// <summary>
        /// Löscht Werte aus der Session, deren Key mit dem übergebenen übereinstimmt oder damit beginnt.
        /// </summary>
        /// <param name="session">Die Session aus der gelöscht werden soll.</param>
        /// <param name="prefix">Der Schlüssel bzw. der Beginn der Schlüssel die gelöscht werden sollen.</param>
        public static IList<string> RemoveFromSession(this HttpSessionStateBase session, string prefix) {
            IList<string> keysToRemove = new List<string>();

            if (session != null) {
                foreach (string sessionKey in session.Keys) {
                    if (sessionKey == prefix || sessionKey.StartsWith(sessionKey)) {
                        keysToRemove.Add(sessionKey);
                    }
                }
                foreach (string keyToRemove in keysToRemove) {
                    session[keyToRemove] = null;
                }
            }

            return keysToRemove;
        }

        /// <summary>
        /// Löscht einen Wert aus der Session, dessen Key mit dem übergebenen übereinstimmt.
        /// </summary>
        /// <param name="session">Die Session aus der gelöscht werden soll.</param>
        /// <param name="key">Der Schlüssel der gelöscht werden sollen.</param>
        public static void RemoveKeyFromSession(this HttpSessionStateBase session, string key) {
            IDictionary<string, ValueProviderResult> valueProviderResults = new Dictionary<string, ValueProviderResult>();
            if (session != null) {
                foreach (string sessionKey in session.Keys) {
                    if (sessionKey == key) {
                        session[sessionKey] = null;
                    }
                }
            }
        }
    }
}