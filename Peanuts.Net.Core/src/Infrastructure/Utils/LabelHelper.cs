using System;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils {
    /// <summary>
    ///     Klasse mit Hilfsmethoden für die Ermittlung von Labels.
    /// </summary>
    public static class LabelHelper {
        /// <summary>
        ///     Versucht das Label über den Propertynamen eines Dtos und seinen zugehörigen Domain-Typen zu ermitteln.
        ///     Kann keine Domain-Typ zugeordnet oder keine Resource gefunden werden, wird null geliefert.
        ///     Der Name der Resources muss dabei folgende Konvention erfüllen:
        ///     label_[kompletter Namespace]_[Domain-Typ]_[Property-Name]
        /// </summary>
        /// <typeparam name="TResource"></typeparam>
        /// <param name="dtoType"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetLabelFromResourceByPropertyName<TResource>(Type dtoType, string propertyName) {
            Require.NotNull(dtoType, "dtoType");
            Require.NotNullOrWhiteSpace(propertyName, "propertyName");

            Type domainType;

            if (DtoForAttribute.TryGetDomainType(dtoType, out domainType)) {
                string resourceKey = string.Format("label_{0}_{1}", domainType.FullName.Replace(".", "_"), propertyName);
                return ResourcesHelper.GetByResourceKey<TResource>(resourceKey);
            } else {
                string resourceKey = string.Format("label_{0}_{1}", dtoType.FullName.Replace(".", "_"), propertyName);
                return ResourcesHelper.GetByResourceKey<TResource>(resourceKey);
            }
        }
    }
}