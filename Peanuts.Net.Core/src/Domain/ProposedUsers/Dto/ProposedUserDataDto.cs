using System;
using System.ComponentModel.DataAnnotations;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers.Dto {
    [DtoFor(typeof(ProposedUser))]
    public class ProposedUserDataDto {
        private DateTime? _birthday;

        public ProposedUserDataDto() {
        }

        /// <summary>
        ///     Erzeugt eine neue Instanz von <see cref="ProposedUserDataDto" />
        /// </summary>
        /// <param name="firstName">Der Vorname</param>
        /// <param name="lastName">Der Nachname</param>
        /// <param name="title">Der Titel</param>
        /// <param name="salutation">Die Anrede</param>
        /// <param name="birthday"></param>
        public ProposedUserDataDto(string firstName, string lastName, string title, Salutation salutation, DateTime? birthday) {
            FirstName = firstName;
            LastName = lastName;
            Title = title;
            Salutation = salutation;
            Birthday = birthday;
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
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        /// <summary>
        ///     Liefert oder setzt den Nachnamen.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string LastName { get; set; }

        /// <summary>
        ///     Liefert oder setzt die Ansprache.
        /// </summary>
        [Required]
        public Salutation Salutation { get; set; }

        /// <summary>
        ///     Liefert oder setzt den Titel.
        /// </summary>
        [StringLength(50)]
        public string Title { get; set; }

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
            return Equals((ProposedUserDataDto)obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = _birthday.GetHashCode();
                hashCode = hashCode * 397 ^ (FirstName != null ? FirstName.GetHashCode() : 0);
                hashCode = hashCode * 397 ^ (LastName != null ? LastName.GetHashCode() : 0);
                hashCode = hashCode * 397 ^ (int)Salutation;
                hashCode = hashCode * 397 ^ (Title != null ? Title.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(ProposedUserDataDto other) {
            return _birthday.Equals(other._birthday) && string.Equals(FirstName, other.FirstName) && string.Equals(LastName, other.LastName)
                   && Salutation == other.Salutation && string.Equals(Title, other.Title);
        }
    }
}