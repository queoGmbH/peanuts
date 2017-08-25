using System.ComponentModel.DataAnnotations;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto {
    /// <summary>
    ///     DTO für die Verwaltung der allgemeinen Daten von <see cref="UserGroup" />.
    /// </summary>
    [DtoFor(typeof(UserGroup))]
    public class UserGroupDto {
        /// <summary>
        ///     Erzeugt eine neue
        /// </summary>
        public UserGroupDto() {
        }

        /// <summary>
        ///     Erzeugt eine neue Instanz von <see cref="UserGroup" />.
        /// </summary>
        /// <param name="additionalInformations">"Sonstige Informationen"</param>
        /// <param name="name">Name der Gruppe</param>
        /// <param name="balanceOverdraftLimit">Akzeptierte Schulden eines Teilnehmers</param>
        public UserGroupDto(string additionalInformations, string name, double? balanceOverdraftLimit) {
            AdditionalInformations = additionalInformations;
            Name = name;
            BalanceOverdraftLimit = balanceOverdraftLimit;
        }

        /// <summary>
        ///     Liefert oder setzt sonstige Informationen.
        /// </summary>
        [StringLength(4000)]
        public string AdditionalInformations { get; set; }

        /// <summary>
        ///     Liefert oder setzt den Namen der Gruppe.
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; }

        [Range(-1000,-20)]
        public double? BalanceOverdraftLimit { get; set; }

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
            return Equals((UserGroupDto)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            unchecked {
                int hashCode = AdditionalInformations != null ? AdditionalInformations.GetHashCode() : 0;
                hashCode = hashCode * 397 ^ BalanceOverdraftLimit.GetHashCode();
                hashCode = hashCode * 397 ^ (Name != null ? Name.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(UserGroupDto other) {
            return string.Equals(AdditionalInformations, other.AdditionalInformations) && string.Equals(Name, other.Name) && (BalanceOverdraftLimit == other.BalanceOverdraftLimit);
        }
    }
}