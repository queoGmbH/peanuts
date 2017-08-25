using System.ComponentModel.DataAnnotations;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto {
    /// <summary>
    ///     DTO zum Ändern der Kontaktdaten eines Nutzers.
    /// </summary>
    [DtoFor(typeof(User))]
    public class UserContactDto {
        public UserContactDto() {
            /*Default ist Deutschland*/
            Country = Country.DE;
        }

        /// <summary>
        ///     Konstruktor für k0omplette Initialisierung der Kontaktdaten eines Nutzers.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="street"></param>
        /// <param name="streetNumber"></param>
        /// <param name="postalCode"></param>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <param name="company"></param>
        /// <param name="url"></param>
        public UserContactDto(string email, string street, string streetNumber, string postalCode, string city, Country country, string company,
            string url, string phone, string phonePrivate, string mobile) {
            Email = email;
            Company = company;
            Street = street;
            StreetNumber = streetNumber;
            PostalCode = postalCode;
            City = city;
            Country = country;
            Url = url;
            Phone = phone;
            PhonePrivate = phonePrivate;
            Mobile = mobile;
        }

        /// <summary>
        ///     Ruft den Wohnort des Nutzers ab oder legt diesen fest.
        /// </summary>
        [StringLength(255)]
        public string City { get; set; }

        /// <summary>
        ///     Ruft den optionalen Namen des Unternehmens ab, für welches der Nutzer tätig ist oder legt diesen fest.
        /// </summary>
        [StringLength(255)]
        public string Company { get; set; }

        /// <summary>
        ///     Ruft den 2-Buchstaben-ISO-Code des Landes vom Wohnort des Nutzers ab oder legt diesen fest.
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        ///     Ruft die E-Mail-Adresse des Nutzers ab oder legt diese fest.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(30)]
        public string Mobile { get; set; }

        [StringLength(30)]
        public string Phone { get; set; }

        [StringLength(30)]
        public string PhonePrivate { get; set; }

        /// <summary>
        ///     Ruft die PLZ vom Wohnortes des Nutzers ab oder legt diese fest.
        /// </summary>
        [StringLength(10)]
        public string PostalCode { get; set; }

        /// <summary>
        ///     Ruft die Straße vom Wohnsitz des Nutzers ab oder legt diese fest.
        /// </summary>
        [StringLength(255)]
        public string Street { get; set; }

        /// <summary>
        ///     Ruft die Hausnummer vom Wohnsitz des Nutzers ab oder legt diese fest.
        /// </summary>
        [StringLength(20)]
        public string StreetNumber { get; set; }

        /// <summary>
        ///     Ruft die Kontakt-Url ab oder legt diese fest.
        /// </summary>
        [StringLength(255)]
        public string Url { get; set; }

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
            return Equals((UserContactDto)obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = Email != null ? Email.GetHashCode() : 0;
                hashCode = hashCode * 397 ^ (Company != null ? Company.GetHashCode() : 0);
                hashCode = hashCode * 397 ^ (Street != null ? Street.GetHashCode() : 0);
                hashCode = hashCode * 397 ^ (StreetNumber != null ? StreetNumber.GetHashCode() : 0);
                hashCode = hashCode * 397 ^ (PostalCode != null ? PostalCode.GetHashCode() : 0);
                hashCode = hashCode * 397 ^ (City != null ? City.GetHashCode() : 0);
                hashCode = hashCode * 397 ^ (int)Country;
                hashCode = hashCode * 397 ^ (Url != null ? Url.GetHashCode() : 0);
                hashCode = hashCode * 397 ^ (Phone != null ? Phone.GetHashCode() : 0);
                hashCode = hashCode * 397 ^ (PhonePrivate != null ? PhonePrivate.GetHashCode() : 0);
                hashCode = hashCode * 397 ^ (Mobile != null ? Mobile.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(UserContactDto other) {
            return
                    string.Equals(Email, other.Email) &&
                    string.Equals(Company, other.Company) &&
                    string.Equals(Street, other.Street) &&
                    string.Equals(StreetNumber, other.StreetNumber) &&
                    string.Equals(PostalCode, other.PostalCode) &&
                    string.Equals(City, other.City) &&
                    Country == other.Country &&
                    string.Equals(Url, other.Url) &&
                    string.Equals(Phone, other.Phone) &&
                    string.Equals(PhonePrivate, other.PhonePrivate) &&
                    string.Equals(Mobile, other.Mobile);
        }
    }
}