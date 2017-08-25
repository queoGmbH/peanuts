using System;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils {
    public static class EnumHelper {
        /// <summary>
        ///     Versucht das Label über den Propertynamen eines Dtos und seinen zugehörigen Domain-Typen zu ermitteln.
        ///     Kann keine Domain-Typ zugeordnet oder keine Resource gefunden werden, wird null geliefert.
        ///     Der Name der Resources muss dabei folgende Konvention erfüllen:
        ///     label_[kompletter Namespace]_[Domain-Typ]_[Property-Name]
        /// </summary>
        /// <typeparam name="TResource"></typeparam>
        /// <typeparam name="TEnum">Der Enum-Typ für dessen Member eine Ressource bestimmt werden soll.</typeparam>
        /// <param name="value">Der Enum-Member für welchen die Übersetzung ermittelt werden soll.</param>
        /// <returns></returns>
        public static string GetLabelFromResource<TResource, TEnum>(TEnum value) where TEnum : struct, IConvertible {
            string fullTypeNameWithUnderscore = typeof(TEnum).FullName.Replace(".", "_");
            string resourceKey = string.Format("label_{0}_{1}", fullTypeNameWithUnderscore, value);
            string labelFromResource = ResourcesHelper.GetByResourceKey<TResource>(resourceKey);
            if (labelFromResource != null) {
                return labelFromResource;
            }
            return value.ToString();
        }
    }
}