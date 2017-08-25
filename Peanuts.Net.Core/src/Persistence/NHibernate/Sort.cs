using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate {
    public class Sort : IEnumerable<OrderDescriptor> {
        private readonly IList<OrderDescriptor> _orders;

        public Sort(IList<OrderDescriptor> orders) {
            if (orders == null || !orders.Any()) {
                throw new ArgumentOutOfRangeException("orders", "Es muss mindestens ein Property für die Sortierung angegeben werden!");
            }
            _orders = orders;
        }

        public Sort(Direction sortDirection, Expression<Func<object, object>> propertyExpression) {
            string propertyName = Objects.GetPropertyName(propertyExpression);

            OrderDescriptor orderDescriptor = new OrderDescriptor(sortDirection, propertyName);
            List<OrderDescriptor> orderDescriptors = new List<OrderDescriptor>() { orderDescriptor };
            _orders = orderDescriptors;
        }

        public Sort(Direction sortDirection, params string[] propertyNames) {
            if (propertyNames == null || !propertyNames.Any()) {
                throw new ArgumentOutOfRangeException("propertyNames", "Es muss mindestens ein Property für die Sortierung angegeben werden!");
            }
            List<OrderDescriptor> orders = new List<OrderDescriptor>();
            foreach (string propertyName in propertyNames) {
                orders.Add(new OrderDescriptor(sortDirection, propertyName));
            }
            _orders = orders;
        }

        public OrderDescriptor this[int index] {
            get { return _orders[index]; }
        }

        /// <summary>
        ///     Gibt einen Enumerator zurück, der die Auflistung durchläuft.
        /// </summary>
        /// <returns>
        ///     Ein <see cref="T:System.Collections.Generic.IEnumerator`1" />, der zum Durchlaufen der Auflistung verwendet werden
        ///     kann.
        /// </returns>
        public IEnumerator<OrderDescriptor> GetEnumerator() {
            return _orders.GetEnumerator();
        }

        /// <summary>
        ///     Gibt einen Enumerator zurück, der eine Auflistung durchläuft.
        /// </summary>
        /// <returns>
        ///     Ein <see cref="T:System.Collections.IEnumerator" />-Objekt, das zum Durchlaufen der Auflistung verwendet werden
        ///     kann.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}