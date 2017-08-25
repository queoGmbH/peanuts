using System;
using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ModelBinding {
    public class SessionModelBinderProvider : IModelBinderProvider {
        /// <summary>
        ///     Gibt den Modellbinder für den angegebenen Typ zurück.
        /// </summary>
        /// <returns>
        ///     Der Modellbinder für den angegebenen Typ.
        /// </returns>
        /// <param name="modelType">Der Typ des Modells.</param>
        public IModelBinder GetBinder(Type modelType) {
            if (typeof(ISessionBindable).IsAssignableFrom(modelType)) {
                return new SessionModelBinder();
            } else {
                return null;
            }
        }
    }
}