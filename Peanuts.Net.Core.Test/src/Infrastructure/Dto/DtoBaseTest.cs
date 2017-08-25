using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Dto {
    /// <summary>
    ///     Basisklasse für all DTO-Tests.
    /// </summary>
    /// <typeparam name="T">Der Typ des DTOs welches getestet werden soll.</typeparam>
    [TestFixture]
    public abstract class DtoBaseTest<T> : PersistenceBaseTest
            where T : class {
        
        /// <summary>
        ///     Methode, welche die Equals-Methode des Dtos testet.
        /// </summary>
        [Test]
        public void TestEquals() {
            // Given: Zwei Objekte eines Typs die sich in alle Eigenschaften unterscheiden.
            T object1;
            T object2;
            OnRequestTwoDifferentDtos(out object1, out object2);

            // When: Ein Test gegen die Equals- und GetHashcode-Methode erfolgt
            TestEqualsAndGetHashCode(object1, object2);

            // Then: Dürfen keine Ausnahmen geworfen werden oder Tests fehlschlagen.
        }

        /// <summary>
        ///     Methode die aufgerufen wird, wenn zwei Instanzen des Types benötigt werden, deren sämtliche Eigenschaften
        ///     unterschiedlich sind.
        /// </summary>
        /// <param name="instance1">Instanz 1 die sich in allen Eigenschaften von Instanz 2 unterscheidet</param>
        /// <param name="instance2">Instanz 2 die sich in allen Eigenschaften von Instanz 1 unterscheidet</param>
        protected abstract void OnRequestTwoDifferentDtos(out T instance1, out T instance2);

        /// <summary>
        ///     Methode die für jedes Property einen Testfall erstellt, bei dem nur dieses Property abweicht
        /// </summary>
        /// <typeparam name="T">Eine beliebige Klasse</typeparam>
        /// <param name="object1">Ein Object vom Typ T</param>
        /// <param name="objectDifferingInAllProperies">
        ///     Ein Objekt beim dem alle lesbaren Properties vom denen des ersten
        ///     Parameters (instance1) abweichen müssen.
        /// </param>
        private void TestEqualsAndGetHashCode(T object1, T objectDifferingInAllProperies) {
            Type type = typeof(T);

            // Alle öffnetlichen lesbaren Parameter ermitteln.
            PropertyInfo[] publicGetProperties = type.GetProperties(BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.GetProperty);
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
            fields = fields.Concat(GetFieldsOfBaseType(type)).ToArray();


            if (!fields.Any() || !publicGetProperties.Any()) {
                throw new MissingBackingPropertiesException(fields.Count(), publicGetProperties.Count());
            }

            if (fields.Count() != publicGetProperties.Count()) {
                throw new DifferentNumberOfFieldsAndPropertiesException(fields.Count(), publicGetProperties.Count());
            }

            // Den Parameterlosen Konstruktor ermitteln.
            ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance, null, new Type[0], null);
            if (constructorInfo == null) {
                throw new MissingParameterlessConstructorException(type);
            }

            // Testen ob alle Properties unterschiedlich sind
            foreach (PropertyInfo expectedDifferentPublicReadableProperty in publicGetProperties) {
                Assert.AreNotEqual(expectedDifferentPublicReadableProperty.GetValue(object1, null), expectedDifferentPublicReadableProperty.GetValue(objectDifferingInAllProperies, null), "Die zwei Objekte unterscheiden sich nicht in alle öffentlichen, lesbaren Eigenschaften.");
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

                Assert.AreNotEqual(object1, copyWithOneDifferentProperty, string.Format("Obwohl das Feld [{0}] unterschiedlich ist, sind die beiden Instanzen Equal.", field.Name));
                Assert.AreNotEqual(object1.GetHashCode(), copyWithOneDifferentProperty.GetHashCode(), string.Format("Obwohl das Feld [{0}] unterschiedlich ist, haben die beiden Instanzen denselben HashCode.", field.Name));
            }

            object newTEqual = constructorInfo.Invoke(new object[0]);
            T copyExpectedEqual = (T)newTEqual;
            foreach (FieldInfo field in fields) {
                field.SetValue(copyExpectedEqual, field.GetValue(object1));
            }

            Assert.AreEqual(object1, copyExpectedEqual, "Obwohl die DTOs die gleichen Eigenschaften hatten, sind sie nicht Equal.");
            Assert.AreEqual(object1.GetHashCode(), copyExpectedEqual.GetHashCode());
        }

        private IEnumerable<FieldInfo> GetFieldsOfBaseType(Type type) {
            if (type.BaseType != null) {
                FieldInfo[] fields = type.BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
                fields = fields.Concat(GetFieldsOfBaseType(type.BaseType)).ToArray();
                return fields;
            }

            return new FieldInfo[]{};
        }
    }
}