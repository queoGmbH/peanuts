using System;
using System.ComponentModel.DataAnnotations;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;
using Com.QueoFlow.Peanuts.Net.Core.Resources;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto {
    /// <summary>
    ///     Enthält alle Felder vom <see cref="User" /> die
    ///     bei der Vervollständigung der Benutzerdaten angegeben werden müssen.
    /// </summary>
    [DtoFor(typeof(User))]
    public class UserDataDto {
        private DateTime? _birthday;

        public UserDataDto() {
        }

        /// <summary>
        ///     Erzeugt eine neue Instanz von <see cref="UserDataDto" />
        /// </summary>
        /// <param name="firstName">Der Vorname</param>
        /// <param name="lastName">Der Nachname</param>
        /// <param name="birthday"></param>
        /// <param name="userName"></param>
        public UserDataDto(string firstName, string lastName, DateTime? birthday, string userName) {
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
            UserName = userName;
        }

        /// <summary>
        ///     Ruft das Geburtsdatum des Nutzer ab oder legt dieses fest.
        /// </summary>
        public DateTime? Birthday {
            get { return _birthday; }
            set {
                if (value.HasValue) {
                    /*Nur das Datum berücksichtigen*/
                    _birthday = value.Value.Date;
                } else {
                    _birthday = null;
                }
            }
        }

        /// <summary>
        ///     Liefert oder setzt den Vornamen.
        /// </summary>
        [StringLength(255)]
        public string FirstName { get; set; }

        /// <summary>
        ///     Liefert oder setzt den UserName.
        /// </summary>
        [StringLength(255)]
        public string LastName { get; set; }

        /// <summary>
        ///     Liefert oder setzt den Login.
        /// </summary>
        [StringLength(255)]
        [Required]
        [Display(ResourceType = typeof(Resources_Domain), Name = "label_Com_QueoFlow_Peanuts_Net_Core_Domain_Users_User_UserName")]
        public string UserName { get; set; }

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
            return Equals((UserDataDto)obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = _birthday.GetHashCode();
                hashCode = hashCode * 397 ^ (FirstName != null ? FirstName.GetHashCode() : 0);
                hashCode = hashCode * 397 ^ (LastName != null ? LastName.GetHashCode() : 0);
                hashCode = hashCode * 397 ^ (UserName != null ? UserName.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(UserDataDto other) {
            return _birthday.Equals(other._birthday) && string.Equals(FirstName, other.FirstName) && string.Equals(LastName, other.LastName)
                   && string.Equals(UserName, other.UserName);
        }
    }
}