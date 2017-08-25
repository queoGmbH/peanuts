using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Dto {
    /// <summary>
    ///     DTO mit den Daten wer ein Entity geändert hat und wann es geändert wurde.
    /// </summary>
    public class EntityChangedDto {
        private DateTime _changedAt;
        private User _changedBy;

        /// <summary>
        ///     Ctor. Nur zur internen Verwendung.
        /// </summary>
        public EntityChangedDto() {
        }

        /// <summary>
        ///     Erzeugt eine neue Instanz von <see cref="EntityChangedDto" />
        /// </summary>
        /// <param name="changedBy">Wer hat das Entity geändert.</param>
        /// <param name="changedAt">Wann wurde das Entity geändert.</param>
        public EntityChangedDto(User changedBy, DateTime changedAt) {
            _changedBy = changedBy;
            _changedAt = changedAt;
        }

        /// <summary>
        ///     Liefert oder setzt das Datum, an dem ein Entity geändert wurde.
        /// </summary>
        public DateTime ChangedAt {
            get { return _changedAt; }
            set { _changedAt = value; }
        }

        /// <summary>
        ///     Liefert oder setzt den Nutzer der ein Entity geändert hat.
        /// </summary>
        public User ChangedBy {
            get { return _changedBy; }
            set { _changedBy = value; }
        }

        /// <inheritdoc />
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != GetType()) {
                return false;
            }
            return Equals((EntityChangedDto)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            unchecked {
                return (_changedBy != null ? _changedBy.GetHashCode() : 0) * 397 ^ _changedAt.GetHashCode();
            }
        }

        protected bool Equals(EntityChangedDto other) {
            return Equals(_changedBy, other._changedBy) && _changedAt.Equals(other._changedAt);
        }
    }
}