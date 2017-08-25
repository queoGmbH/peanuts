namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security {
    public class SimpleGrantedAuthority : IGrantedAuthority {
        private readonly string _authority;

        public SimpleGrantedAuthority(string authority) {
            _authority = authority;
        }

        public string Authority {
            get { return _authority; }
        }

        /// <summary>
        ///     Bestimmt, ob das angegebene Objekt mit dem aktuellen Objekt identisch ist.
        /// </summary>
        /// <returns>
        ///     true, wenn das angegebene Objekt und das aktuelle Objekt gleich sind, andernfalls false.
        /// </returns>
        /// <param name="obj">Das Objekt, das mit dem aktuellen Objekt verglichen werden soll.</param>
        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj is SimpleGrantedAuthority) {
                return _authority.Equals(((SimpleGrantedAuthority)obj).Authority);
            }
            return false;
        }

        /// <summary>
        ///     Fungiert als Hashfunktion für einen bestimmten Typ.
        /// </summary>
        /// <returns>
        ///     Ein Hashcode für das aktuelle Objekt.
        /// </returns>
        public override int GetHashCode() {
            return GetType().GetHashCode() ^ _authority.GetHashCode();
        }

        /// <summary>
        ///     Gibt eine Zeichenfolge zurück, die das aktuelle Objekt darstellt.
        /// </summary>
        /// <returns>
        ///     Eine Zeichenfolge, die das aktuelle Objekt darstellt.
        /// </returns>
        public override string ToString() {
            return _authority;
        }
    }
}