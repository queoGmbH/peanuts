using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure {
    public static class DtoAssert {
        /// <summary>
        ///     Methode die für jedes Property einen Testfall erstellt, bei dem nur dieses Property abweicht
        /// </summary>
        /// <typeparam name="T">Eine beliebige Klasse</typeparam>
        /// <param name="object1">Ein Object vom Typ T</param>
        /// <param name="objectDifferingInAllProperies">
        ///     Ein Objekt beim dem alle lesbaren Properties vom denen des ersten
        ///     Parameters (instance1) abweichen müssen.
        /// </param>
        public static void TestEqualsAndGetHashCode<T>(T object1, T objectDifferingInAllProperies) {
            Type type = typeof(T);

            // Alle öffentlichen lesbaren Parameter ermitteln.
            PropertyInfo[] publicGetProperties =
                    type.GetProperties(BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.GetProperty);
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
            fields = fields.Concat(GetFieldsOfBaseType(type)).ToArray();

            if (!fields.Any() || !publicGetProperties.Any()) {
                throw new MissingBackingPropertiesException(fields.Count(), publicGetProperties.Count());
            }

            if (fields.Count() != publicGetProperties.Count()) {
                throw new DifferentNumberOfFieldsAndPropertiesException(fields.Count(), publicGetProperties.Count());
            }

            // Den Parameterlosen Konstruktor ermitteln.
            ConstructorInfo constructorInfo =
                    type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance,
                        null,
                        new Type[0],
                        null);
            if (constructorInfo == null) {
                throw new MissingParameterlessConstructorException(type);
            }

            // Testen ob alle Properties unterschiedlich sind
            foreach (PropertyInfo expectedDifferentPublicReadableProperty in publicGetProperties) {
                
                Assert.AreNotEqual(expectedDifferentPublicReadableProperty.GetValue(object1, null),
                    expectedDifferentPublicReadableProperty.GetValue(objectDifferingInAllProperies, null),
                    string.Format("Für den Member: {0}. Die zwei Objekte unterscheiden sich nicht in allen öffentlichen, lesbaren Eigenschaften.", 
                    expectedDifferentPublicReadableProperty.Name));
            }

            // Testfälle mit jeweils einem unterschiedlichen Property erzeugen
            foreach (FieldInfo field in fields) {
                object newTDifferent = constructorInfo.Invoke(new object[0]);
                T copyWithOneDifferentProperty = (T)newTDifferent;

                foreach (FieldInfo fieldToChange in fields) {
                    if (field != fieldToChange) {
                        fieldToChange.SetValue(copyWithOneDifferentProperty, fieldToChange.GetValue(object1));
                    } else {
                        fieldToChange.SetValue(copyWithOneDifferentProperty, fieldToChange.GetValue(objectDifferingInAllProperies));
                    }
                }

                Assert.AreNotEqual(object1,
                    copyWithOneDifferentProperty,
                    string.Format("Obwohl das Feld [{0}] unterschiedlich ist, sind die beiden Instanzen Equal.", field.Name));
                Assert.AreNotEqual(object1.GetHashCode(),
                    copyWithOneDifferentProperty.GetHashCode(),
                    string.Format("Obwohl das Feld [{0}] unterschiedlich ist, haben die beiden Instanzen denselben HashCode.", field.Name));
            }

            object newTEqual = constructorInfo.Invoke(new object[0]);
            T copyExpectedEqual = (T)newTEqual;
            foreach (FieldInfo field in fields) {
                field.SetValue(copyExpectedEqual, field.GetValue(object1));
            }

            Assert.AreEqual(object1, copyExpectedEqual, "Obwohl die DTOs die gleichen Eigenschaften hatten, sind sie nicht Equal.");
            Assert.AreEqual(object1.GetHashCode(), copyExpectedEqual.GetHashCode());
        }

        private static IEnumerable<FieldInfo> GetFieldsOfBaseType(Type type) {
            if (type.BaseType != null) {
                FieldInfo[] fields = type.BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
                fields = fields.Concat(GetFieldsOfBaseType(type.BaseType)).ToArray();
                return fields;
            }

            return new FieldInfo[] { };
        }

        /// <summary>
        /// Überprüft, ob zwei DTOs Equal sind. 
        /// </summary>
        /// <typeparam name="TDto">Der Typ der Dtos.</typeparam>
        /// <param name="expected">Das erwartete Dto</param>
        /// <param name="actual">Das tatsächliche bzw. zu überprüfende Dto</param>
        public static void AreEqual<TDto>(TDto expected, TDto actual) where TDto : class {
            AreEqual(expected, actual, string.Format("Die Dtos vom Typ [{0}] sind nicht Equal", typeof(TDto)));
        }

        /// <summary>
        /// Überprüft, ob zwei DTOs Equal sind. 
        /// </summary>
        /// <typeparam name="TDto">Der Typ der Dtos.</typeparam>
        /// <param name="expected">Das erwartete Dto</param>
        /// <param name="actual">Das tatsächliche bzw. zu überprüfende Dto</param>
        /// <param name="message">Fehlermeldung wenn die DTOs nicht Equal sind.</param>
        public static void AreEqual<TDto>(TDto expected, TDto actual, string message) where TDto : class {

            if (Equals(expected, actual)) {
                /*Die Dtos sind Equal => alles gut*/
                return;
            }

            if (expected == null) {
                Assert.Fail("{0}\r\nEs wurde NULL erwartet.", message);
            }
            if (actual == null) {
                Assert.Fail("{0}\r\nDas DTO ist NULL.", message);
            }

            StringBuilder errorStringBuilder = new StringBuilder();
            PropertyInfo[] propertyInfos = typeof(TDto).GetProperties();

            bool areAllPropertiesEqual = true;
            foreach (PropertyInfo propertyInfo in propertyInfos.Where(property => property.CanRead)) {
                object expectedPropertyValue = propertyInfo.GetValue(expected);
                object actualPropertyValue = propertyInfo.GetValue(actual);
                if (!Equals(expectedPropertyValue, actualPropertyValue)) {
                    areAllPropertiesEqual = false;
                    errorStringBuilder.AppendLine(string.Format("Die Eigenschaft [{0}] unterscheidet sich => erwartet: [{1}] tatsächlich: [{2}]", propertyInfo.Name, expectedPropertyValue, actualPropertyValue));
                }
            }

            if (areAllPropertiesEqual) {
                Assert.Fail("{0}, obwohl alle Eigenschaften Equal sind.", message);
            } else {
                Assert.Fail("{0}:\r\n{1}", message, errorStringBuilder);
            }
        }
    }
}