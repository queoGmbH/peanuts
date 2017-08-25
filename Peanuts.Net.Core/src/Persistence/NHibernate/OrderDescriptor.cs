namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate {
    public class OrderDescriptor {
        private readonly Direction _direction;
        private readonly string _property;

        public OrderDescriptor(Direction direction, string property) {
            _direction = direction;
            _property = property;
        }

        /// <summary>
        ///     Liefert die Richtung der Sortierung.
        /// </summary>
        public Direction Direction {
            get { return _direction; }
        }

        /// <summary>
        ///     Liefert den Namen des Properties nach dem sortiert werden soll.
        /// </summary>
        public string Property {
            get { return _property; }
        }
    }
}