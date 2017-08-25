using System;
using System.Diagnostics;
using System.Globalization;
using System.Web.Mvc;

using Common.Logging;

using NHibernate.Util;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ModelBinding {
    /// <summary>
    /// ModelBinder, der einen Typen bindet, indem er den string-Wert, mit der <see cref="CultureInfo.InvariantCulture"/> als FormatProvider parsed.
    /// </summary>
    /// <typeparam name="TParseInvariant"></typeparam>
    public class InvariantCultureModelBinder<TParseInvariant> : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            try {
                string attemptedValue = valueProviderResult.AttemptedValue;
                if (attemptedValue == null) {
                    /*Wert ist sowieso null, deswegen braucht nichts geparsed werden*/
                    return null;
                }
                Type modelType = typeof(TParseInvariant);
                if (modelType.IsNullable()) {
                    /*Die ChangeType-Methode kann nicht mit Nullables umgehen, deswegen den zu Grunde liegenden Typ ermitteln und verwenden.*/
                    modelType = Nullable.GetUnderlyingType(modelType);
                }
                if (modelType != null) {
                    /*Mit der Invariant-Culture die Umwandlung durchführen*/
                    var model = Convert.ChangeType(attemptedValue, modelType, CultureInfo.InvariantCulture);
                    return model;
                }

                return null;

            } catch (Exception e) {
                LogManager.GetLogger("InvariantCultureModelBinder").InfoFormat("Der Wert {0} konnte nicht als Typ {1} gebunden werden.", e, valueProviderResult, typeof(TParseInvariant));
                Debug.WriteLine("Der Wert {0} konnte nicht als Typ {1} gebunden werden.", valueProviderResult, typeof(TParseInvariant));
                return null;
            }
            
        }
    }
}