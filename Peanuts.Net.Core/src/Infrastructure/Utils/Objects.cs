using System;
using System.Linq.Expressions;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils {
    /// <summary>
    ///     Enthält Methoden für den allgemeinen Umgang mit Objekten.
    /// </summary>
    public static class Objects {
        /// <summary>
        ///     Ermittelt den Namen eines Property anhand der Expression.
        /// </summary>
        /// <typeparam name="T">Typ der Klasse von dem ein Propertyname bestimmt werden soll.</typeparam>
        /// <param name="expression">Der Ausdruck für das Property</param>
        /// <returns>Namen des Property</returns>
        /// <example>
        ///     string propertyName = Require.GetPropertyName{Foo}(x => x.Name);
        /// </example>
        /// <exception cref="ArgumentNullException">Wenn <paramref name="expression" /> <code>null</code> ist.</exception>
        /// <exception cref="InvalidOperationException">Wenn die Expression nicht zu einem Property ausgewertet werden kann.</exception>
        public static string GetPropertyName<T>(Expression<Func<T, object>> expression) {
            Require.NotNull(expression);
            MemberExpression memberExpression = expression.Body as MemberExpression;
            UnaryExpression unaryExpression = expression.Body as UnaryExpression;
            string propertyName;
            if (memberExpression != null) {
                propertyName = memberExpression.Member.Name;
            } else if (unaryExpression != null) {
                memberExpression = unaryExpression.Operand as MemberExpression;
                if (memberExpression == null) {
                    throw new InvalidOperationException("Can't determine a property from expression.");
                }
                propertyName = memberExpression.Member.Name;
            } else {
                throw new InvalidOperationException("Can't determine a property from expression.");
            }
            return propertyName;
        }

        /// <summary>
        ///     Ermittelt den Pfad eines Property anhand der Expression.
        /// </summary>
        /// <typeparam name="T">Typ der Klasse von dem ein Pfad bestimmt werden soll.</typeparam>
        /// <typeparam name="TProperty">Der Typ des Properties, auf welches die Expression zeigt.</typeparam>
        /// <param name="expression">Der Ausdruck für den Pfad</param>
        /// <returns>Pfad zum Property</returns>
        /// <example>
        ///     string propertyPath = Objects.GetPropertyPath{Foo}(x => x.Path1.Path2.Name);
        /// </example>
        /// <exception cref="ArgumentNullException">Wenn <paramref name="expression" /> <code>null</code> ist.</exception>
        /// <exception cref="InvalidOperationException">Wenn die Expression nicht zu einem Property ausgewertet werden kann.</exception>
        public static string GetPropertyPath<T, TProperty>(Expression<Func<T, TProperty>> expression) {
            Require.NotNull(expression);
            MemberExpression memberExpression = expression.Body as MemberExpression;
            UnaryExpression unaryExpression = expression.Body as UnaryExpression;

            if (memberExpression == null && unaryExpression != null) {
                memberExpression = unaryExpression.Operand as MemberExpression;
            }

            if (memberExpression == null) {
                throw new InvalidOperationException("Can't determine a property from expression.");
            }

            string[] strings = memberExpression.Expression.ToString().Split(new[] { "." }, 2, StringSplitOptions.None);
            if (strings.Length == 2) {
                return strings[1] + "." + memberExpression.Member.Name;
            } else {
                return memberExpression.Member.Name;
            }
        }

        /// <summary>
        ///     Ermittelt den Pfad eines Property anhand der Expression.
        /// </summary>
        /// <typeparam name="T">Typ der Klasse von dem ein Pfad bestimmt werden soll.</typeparam>
        /// <param name="expression">Der Ausdruck für den Pfad</param>
        /// <returns>Pfad zum Property</returns>
        /// <example>
        ///     string propertyPath = Objects.GetPropertyPath{Foo}(x => x.Path1.Path2.Name);
        /// </example>
        /// <exception cref="ArgumentNullException">Wenn <paramref name="expression" /> <code>null</code> ist.</exception>
        /// <exception cref="InvalidOperationException">Wenn die Expression nicht zu einem Property ausgewertet werden kann.</exception>
        public static string GetPropertyPath<T>(Expression<Func<T, object>> expression) {
            return GetPropertyPath<T, object>(expression);
        }
    }
}