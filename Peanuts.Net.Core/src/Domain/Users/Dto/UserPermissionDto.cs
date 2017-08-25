using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto {
    /// <summary>
    ///     DTO zur Übertragung der Zugangsberechtigungen/ Rollen
    /// </summary>
    [DtoFor(typeof(User))]
    public class UserPermissionDto {
        /// <summary>Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.</summary>
        public UserPermissionDto(IList<string> roles, bool isEnabled) {
            if (roles != null) {
                Roles = roles;
            } else {
                Roles = new List<string>();
            }

            IsEnabled = isEnabled;
        }

        /// <summary>Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.</summary>
        public UserPermissionDto() {
            Roles = new List<string>();
            IsEnabled = false;
        }

        /// <summary>
        ///     Ruft ab oder legt fest, ob der Nutzer aktiv ist oder nicht.
        /// </summary>
        [Required]
        public bool IsEnabled { get; set; }

        /// <summary>
        ///     Liefert die Rollen des Nutzers
        /// </summary>
        [Required]
        public IList<string> Roles { get; set; }

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
            return Equals((UserPermissionDto)obj);
        }

        public override int GetHashCode() {
            int hashCode = GetType().GetHashCode();
            hashCode = hashCode ^ (Roles == null ? 0 : Roles.GetHashCode());
            hashCode = hashCode ^ IsEnabled.GetHashCode();
            return hashCode;
        }

        protected bool Equals(UserPermissionDto other) {
            if (!CollectionEquals(Roles, other.Roles)) {
                return false;
            }
            if (!Equals(IsEnabled, other.IsEnabled)) {
                return false;
            }

            return true;
        }

        private bool CollectionEquals(IList<string> roles, IList<string> otherRoles) {
            if (roles == null || otherRoles == null) {
                if (otherRoles != roles) {
                    return false;
                } else {
                    return true;
                }
            }
            if (roles.Count != otherRoles.Count) {
                return false;
            }
            bool areEqual = roles.All(otherRoles.Contains) && otherRoles.All(roles.Contains);
            return areEqual;
        }
    }
}