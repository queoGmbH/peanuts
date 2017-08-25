using System.ComponentModel.DataAnnotations;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts {
    /// <summary>
    /// Dto für eine Voraussetzung eines Peanuts.
    /// </summary>
    public class RequirementDto {
        public RequirementDto() {
        }

        public RequirementDto(string name, double quantity, string unit, string url) {
            Require.NotNullOrWhiteSpace(name, "name");

            Quantity = quantity;
            Name = name;
            Unit = unit;
            Url = url;
        }

        /// <summary>
        /// Ruft die dieser Voraussetzung ab.
        /// </summary>
        public double Quantity {
            get; set;
        }

        /// <summary>
        /// Ruft den Namen dieser Voraussetzung ab.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name {
            get; set;
        }

        /// <summary>
        /// Ruft die Einheit der Menge dieser Voraussetzung ab.
        /// </summary>
        [StringLength(15)]
        public string Unit {
            get; set;
        }

        /// <summary>
        /// Ruft eine URL ab, auf der die Voraussetzung beschrieben oder ein Bild o.ä. enthalten ist oder legt diese fest.
        /// </summary>
        public string Url {
            get; set;
        }

        /// <summary>
        /// Ruft einen DTO ab, der mit Vorbelegungsdaten initialisiert ist.
        /// </summary>
        public static RequirementDto Default {
            get {
                return new RequirementDto() {
                    Quantity = 1,
                    Unit = "x"
                };
            }
        }

        protected bool Equals(RequirementDto other) {
            return Quantity.Equals(other.Quantity) && string.Equals(Name, other.Name) && string.Equals(Unit, other.Unit) && string.Equals(Url, other.Url);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RequirementDto)obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = Quantity.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Unit != null ? Unit.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Url != null ? Url.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}