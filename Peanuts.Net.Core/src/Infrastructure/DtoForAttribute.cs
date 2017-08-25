using System;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure {
    /// <summary>
    ///     Attribute, welches zur Kennzeichnung von Dtos verwendet wird, um den Bezug zur Domain-Klasse herzustellen.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class DtoForAttribute : Attribute {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Attribute" />-Klasse.
        /// </summary>
        /// <param name="domainType">Die Domain-Klasse, die ein Dto zur Erstellung und oder Aktualisierung verwendet.</param>
        public DtoForAttribute(Type domainType) {
            Require.NotNull(domainType, "domainType");

            DomainType = domainType;
        }

        /// <summary>
        ///     Ruft den Typen ab, der über das Attribute mit dem Typen verknüpft ist.
        /// </summary>
        public Type DomainType { get; private set; }

        /// <summary>
        ///     Ruft den Domain-Typen ab, der über das Attribute mit dem Dto verknüpft ist.
        /// </summary>
        /// <param name="dtoType">Der Typ, für den der über das Attribute zugeordnete Domain-Typ ermittelt werden soll.</param>
        /// <returns></returns>
        public static Type GetDomainType(Type dtoType) {
            object[] customAttributes = dtoType.GetCustomAttributes(typeof(DtoForAttribute), false);
            if (!customAttributes.Any()) {
                return null;
            }

            DtoForAttribute foundAttribute = customAttributes.Cast<DtoForAttribute>().First();
            return foundAttribute.DomainType;
        }

        /// <summary>
        ///     Versucht den Domain-Typen, der über das Attribute mit dem Dto verknüpft ist, zu ermitteln.
        /// </summary>
        /// <param name="dtoType">Das Dto</param>
        /// <param name="domainType">Der ermittelte Domain-Typ oder null wenn kein zugeordneter Typ gefunden wurde.</param>
        /// <returns>true wenn ein Typ gefunden wurde oder false wenn nicht.</returns>
        public static bool TryGetDomainType(Type dtoType, out Type domainType) {
            domainType = GetDomainType(dtoType);
            return domainType != null;
        }
    }
}