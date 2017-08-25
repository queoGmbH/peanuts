using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;


namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils {
    /// <summary>
    ///     Klasse mit Hilfsmethoden für Typen.
    /// </summary>
    public static class TypeHelper {
        /// <summary>
        ///     Ruft den Enum-Typen eines Typen ab.
        ///     Dazu werden folgende Fälle überprüft:
        ///     - der Parameter ist ein enum?
        ///     - der Parameter ist ein Nullable-enum
        ///     - der Parameter ist eine generische Liste eines enum-Typen
        ///     - der Parameter ist eine generische Liste eines Nullable-enum-Typen
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetBaseEnumType(Type type) {
            if (type.IsEnum) {
                /*Single-Select für eine Enum*/
                return type;
            } else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && type.GenericTypeArguments[0].IsEnum) {
                /*Single-Select => Nullable<Enum>*/
                return type.GenericTypeArguments[0];
            } else if (IsListType(type) && type.GenericTypeArguments[0].IsEnum) {
                /*Multi-Select für eine Enum*/
                return type.GenericTypeArguments[0];
            } else if (IsListType(type) && type.GenericTypeArguments[0].IsGenericType
                       && type.GenericTypeArguments[0].GetGenericTypeDefinition() == typeof(Nullable<>)
                       && type.GenericTypeArguments[0].GenericTypeArguments[0].IsEnum) {
                /*Multi-Select für eine Nullable-Enum*/
                return type.GenericTypeArguments[0].GetGenericArguments()[0];
            } else {
                /*Keine Enum => Error*/
                throw new ArgumentException("T must be an enumerated type");
            }
        }

        /// <summary>
        ///     Ruft die Beschreibung, die mit dem Description-Attribute definiert wurde, für ein Enum-Member ab.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetEnumMemberDescription(Enum val) {
            DescriptionAttribute[] customAttributes =
                    (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (customAttributes.Length > 0) {
                return customAttributes[0].Description;
            }
            return string.Empty;
        }

        /// <summary>
        ///     Überprüft ob der Typ von DomainEntity abgeleitet ist.
        /// </summary>
        /// <param name="typeToCheck"></param>
        /// <returns></returns>
        public static bool IsDomainEntityType(Type typeToCheck) {
            if (typeToCheck.IsSubclassOf(typeof(Entity))) {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Überprüft ob es sich bei einem Typ, um eine generische Liste eines DomainEntity-Types handelt.
        /// </summary>
        /// <param name="typeToCheck"></param>
        /// <returns></returns>
        public static bool IsDomainEntityTypeList(Type typeToCheck) {
            if (!IsListType(typeToCheck)) {
                return false;
            }

            Type[] genericTypes = typeToCheck.GetGenericArguments();
            if (!genericTypes.Any()) {
                return false;
            }

            return IsDomainEntityType(genericTypes[0]);
        }

        /// <summary>
        ///     Überprüft, ob es sich bei einem bestimmten Typ um einen handelt, der eine Mehrfachauswahl zulässt.
        /// </summary>
        /// <param name="typeToCheck">Der zu prüfende Typ. Bei NULL wird false geliefert.</param>
        /// <returns></returns>
        public static bool IsListType(Type typeToCheck) {
            if (typeToCheck == null) {
                return false;
            }

            IList<Type> listTypes = new List<Type>
                    { typeof(IList), typeof(IList<>), typeof(ICollection), typeof(ICollection<>), typeof(List<>), typeof(Collection<>) };

            if (typeToCheck.IsGenericType) {
                return listTypes.Contains(typeToCheck.GetGenericTypeDefinition());
            } else if (typeToCheck.BaseType != null) {
                return IsListType(typeToCheck.BaseType);
            }

            return false;
        }
    }
}