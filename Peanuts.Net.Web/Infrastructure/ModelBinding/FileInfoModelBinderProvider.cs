using System;
using System.IO;
using System.Web.Mvc;

using Spring.Context.Support;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ModelBinding {
    /// <summary>
    /// Provider der einen ModelBinder für den Dateiupload bereitstellen kann.
    /// </summary>
    public class FileInfoModelBinderProvider:IModelBinderProvider {
        /// <summary>Gibt den Modellbinder für den angegebenen Typ zurück.</summary>
        /// <returns>Der Modellbinder für den angegebenen Typ.</returns>
        /// <param name="modelType">Der Typ des Modells.</param>
        public IModelBinder GetBinder(Type modelType) {
            if (modelType == typeof(FileInfo)) {
                return ContextRegistry.GetContext().GetObject<FileInfoModelBinder>();
            }

            return null;
        }
    }
}