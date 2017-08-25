using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Dto {
    /// <summary>
    ///     Das DTO enthält Daten an denen ein Entity erzeugt wurde.
    /// </summary>
    public class EntityCreatedDto {
        /// <summary>
        ///     Erzeugt eine neue Instanz von <see cref="EntityCreatedDto" />.
        /// </summary>
        public EntityCreatedDto() {
            CreatedBy = null;
            CreatedAt = DateTime.Now;
            
        }

        /// <summary>
        ///     Erzeugt eine neue Instanz von <see cref="EntityCreatedDto" />
        /// </summary>
        /// <param name="createdBy">Nutzer der das Entity erzeugt hat.</param>
        /// <param name="createdAt">Datum an dem das Entity erzeugt wurde.</param>
        public EntityCreatedDto(User createdBy, DateTime createdAt) {
            CreatedBy = createdBy;
            CreatedAt = createdAt;
        }

        /// <summary>
        ///     Liefert oder setzt das Datum, an dem ein Entity erzeugt wurde.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        ///     Liefert oder setzt den Nutzer der ein Entity erzeugt hat.
        /// </summary>
        public User CreatedBy { get; set; }

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
            return Equals((EntityCreatedDto)obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (CreatedBy != null ? CreatedBy.GetHashCode() : 0) * 397 ^ CreatedAt.GetHashCode();
            }
        }

        protected bool Equals(EntityCreatedDto other) {
            return Equals(CreatedBy, other.CreatedBy) && CreatedAt.Equals(other.CreatedAt);
        }
    }
}