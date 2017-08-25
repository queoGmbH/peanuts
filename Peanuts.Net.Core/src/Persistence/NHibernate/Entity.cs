using System;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate {
    /// <summary>
    ///     Eine beispielhafte Implementierung für ein Entity mit einem Integer als ID.
    ///     Diese kann auch als Grundlage für konkrete Implementierungen von Domainobjekten
    ///     verwendet werden.
    ///     TODO: Folgende Punkte noch überlegen und klären
    ///     - soll die BusinessId auf das Entity getypt werden (also: BusinessId{T}) - dann kann man BusinessIds nicht
    ///     verwechseln, aber sie werden auch nur sehr selten verwendet
    ///     - GetHashCode und AreEquivalent korrekt implementieren. Beachten bei GetType, dass da ein Hibernate Proxy dahinter stecken
    ///     kann!!!
    ///     Also per Hilfsmethode erst mal den richtigen Typ bestimmen!
    ///     -
    /// </summary>
    public class Entity {
        private Guid _businessId;
        private int _id;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="Entity" />-Klasse.
        /// </summary>
        public Entity() {
            _businessId = Guid.NewGuid();
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="Entity" />-Klasse.
        /// </summary>
        public Entity(Guid businessId) {
            _businessId = businessId;
        }

        /// <summary>
        ///     Liefert die BusinessId des Objekts.
        /// </summary>
        public virtual Guid BusinessId {
            get { return _businessId; }
            protected set { _businessId = value; }
        }

        /// <summary>
        ///     Liefert die ID des Objekts.
        /// </summary>
        public virtual int Id {
            get { return _id; }
            protected set { _id = value; }
        }

        /// <summary>
        ///     Bestimmt, ob das angegebene Objekt mit dem aktuellen Objekt identisch ist.
        ///     Es wird auf Typgleichheit und die Gleichheit der BusinessId geprüft.
        /// </summary>
        /// <returns>
        ///     true, wenn das angegebene Objekt und das aktuelle Objekt gleich sind, andernfalls false.
        /// </returns>
        /// <param name="obj">Das Objekt, das mit dem aktuellen Objekt verglichen werden soll.</param>
        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj == null) {
                return false;
            }
            if (!(obj is Entity)) {
                return false;
            }
            if (GetTypeUnproxied() != ((Entity)obj).GetTypeUnproxied()) {
                return false;
            }

            Entity other = (Entity)obj;
            if (!BusinessId.Equals(other.BusinessId)) {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Fungiert als Hashfunktion für einen bestimmten Typ.
        /// </summary>
        /// <returns>
        ///     Ein Hashcode für das aktuelle Objekt.
        /// </returns>
        public override int GetHashCode() {
            return GetType().GetHashCode() ^ BusinessId.GetHashCode();
        }

        public virtual Type GetTypeUnproxied() {
            return GetType();
        }

        /// <summary>
        ///     Gibt eine Zeichenfolge zurück, die das aktuelle Objekt darstellt.
        /// </summary>
        /// <returns>
        ///     Eine Zeichenfolge, die das aktuelle Objekt darstellt.
        /// </returns>
        public override string ToString() {
            string toString = string.Format("{0}: Id: {1}", GetType().Name, Id);
            return toString;
        }
    }
}