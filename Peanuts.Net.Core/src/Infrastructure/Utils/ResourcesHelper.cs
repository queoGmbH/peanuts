using System;
using System.Reflection;
using System.Resources;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils {
    public static class ResourcesHelper {
        /// <summary>
        ///     Versucht den String-Wert für einen Schlüssel aus dem Resourcen-Typ zu laden.
        /// </summary>
        /// <typeparam name="TResource"></typeparam>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public static string GetByResourceKey<TResource>(string resourceKey) {
            Type resourceType = typeof(TResource);
            Assembly resourceAssembly = resourceType.Assembly;
            ResourceManager resourceManager = new ResourceManager(resourceType.FullName, resourceAssembly);
            try {
                return resourceManager.GetString(resourceKey);
            } catch (Exception) {
            }
            if (resourceType.Name.Contains("_")) {
                /*Wenn die Resources-Datei mit einem "." angelegt wird, der beim Resourcen-Typ durch ein "_" ersetzt wird, funktioniert die Standard-Initialisierung nicht. Deswegen dieser Workaround.*/
                ResourceManager resourceManagerDot =
                        new ResourceManager(String.Format("{0}.{1}", resourceType.Namespace, resourceType.Name.Replace("_", ".")),
                            resourceAssembly);
                try {
                    return resourceManagerDot.GetString(resourceKey);
                } catch (Exception) {
                }
            }

            return null;
        }
    }
}