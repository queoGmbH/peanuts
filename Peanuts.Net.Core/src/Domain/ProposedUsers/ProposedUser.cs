using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers {
    /// <summary>
    ///     Beantragter Nutzer
    /// </summary>
    public class ProposedUser : Entity {
        private DateTime? _birthday;
        private DateTime? _changedAt;
        private User _changedBy;
        private string _city;
        private string _company;
        private Country _country;

        private DateTime _createdAt;
        private User _createdBy;
        private string _email;

        private string _firstName;
        private string _lastName;
        private string _mobile;
        private string _phone;
        private string _phonePrivate;
        private string _postalCode;
        private Salutation _salutation;
        private string _street;
        private string _streetNumber;
        private string _title;
        private string _url;
        private string _userName;

        /// <summary>
        ///     Ctor. für NHibernate
        /// </summary>
        public ProposedUser() {
        }

        public ProposedUser(string userName, ProposedUserDataDto proposedUserDataDto, ProposedUserContactDto proposedUserContactDto,
            EntityCreatedDto entityCreatedDto) {
            UpdateUsername(userName);
            Update(proposedUserDataDto);
            Update(proposedUserContactDto);
            Update(entityCreatedDto);
        }

        /// <summary>
        ///     Ruft das Geburtsdatum des Nutzers ab.
        /// </summary>
        public virtual DateTime? Birthday {
            get { return _birthday; }
        }

        /// <summary>
        ///     Ruft ab, zu welchem Zeitpunkt letztmalig Eigenschaften des Nutzers geändert wurden.
        /// </summary>
        public virtual DateTime? ChangedAt {
            get { return _changedAt; }
        }

        /// <summary>
        ///     Ruft ab, durch wen der Nutzer zuletzt geändert wurde.
        /// </summary>
        public virtual User ChangedBy {
            get { return _changedBy; }
        }

        /// <summary>
        ///     Ruft die Stadt des Nutzers ab.
        /// </summary>
        public virtual string City {
            get { return _city; }
        }

        /// <summary>
        ///     Ruft die Firma des Nutzers ab.
        /// </summary>
        public virtual string Company {
            get { return _company; }
        }

        /// <summary>
        ///     Ruft das Land des Nutzers ab.
        /// </summary>
        public virtual Country Country {
            get { return _country; }
        }

        /// <summary>
        ///     Ruft den Zeitpunkt der Erstellung des Nutzers ab.
        /// </summary>
        public virtual DateTime CreatedAt {
            get { return _createdAt; }
        }

        /// <summary>
        ///     Ruft ab, durch welchen Nutzer dieser Nutzer erstellt wurde.
        /// </summary>
        public virtual User CreatedBy {
            get { return _createdBy; }
        }

        /// <summary>
        ///     Gibt den vollen Namen des Nutzers aus.
        /// </summary>
        public virtual string DisplayName {
            get { return string.Format("{0} {1}", _firstName, _lastName); }
        }

        /// <summary>
        ///     Email-Adresse, die für Email-Benachrichtigungen an den Nutzer verwendet wird.
        /// </summary>
        public virtual string Email {
            get { return _email; }
        }

        /// <summary>
        ///     Ruft den Vornamen des Nutzers ab.
        /// </summary>
        public virtual string FirstName {
            get { return _firstName; }
        }

        /// <summary>
        ///     Ruft den Nachnamen des Nutzers ab.
        /// </summary>
        public virtual string LastName {
            get { return _lastName; }
        }

        /// <summary>
        ///     Ruft die mobile Telefonnummer des Nutzers ab.
        /// </summary>
        public virtual string Mobile {
            get { return _mobile; }
        }

        /// <summary>
        ///     Ruft die Telefonnummer des Nutzers ab.
        /// </summary>
        public virtual string Phone {
            get { return _phone; }
        }

        /// <summary>
        ///     Ruft die private Telefonnumer des Nutzers ab.
        /// </summary>
        public virtual string PhonePrivate {
            get { return _phonePrivate; }
        }

        /// <summary>
        ///     Ruft die Postleitzahl des Nutzers ab.
        /// </summary>
        public virtual string PostalCode {
            get { return _postalCode; }
        }

        public virtual Salutation Salutation {
            get { return _salutation; }
        }

        /// <summary>
        ///     Ruft die Straße des Nutzers ab.
        /// </summary>
        public virtual string Street {
            get { return _street; }
        }

        /// <summary>
        ///     Ruft die Hausnummer des Nutzers ab.
        /// </summary>
        public virtual string StreetNumber {
            get { return _streetNumber; }
        }

        /// <summary>
        ///     Ruft den Titel des Nutzers ab.
        /// </summary>
        public virtual string Title {
            get { return _title; }
        }

        /// <summary>
        ///     Ruft die Webseite des Nutzers ab.
        /// </summary>
        public virtual string Url {
            get { return _url; }
        }

        /// <summary>
        ///     Ruft den Usernamen des Nutzers ab.
        /// </summary>
        public virtual string UserName {
            get { return _userName; }
        }

        /// <summary>
        ///     Liefert den <see cref="ProposedUserContactDto" />
        /// </summary>
        /// <returns></returns>
        public virtual ProposedUserContactDto GetUserContactDto() {
            return new ProposedUserContactDto(_email,
                _street,
                _streetNumber,
                _postalCode,
                _city,
                _country,
                _company,
                _url,
                _phone,
                _phonePrivate,
                _mobile);
        }

        /// <summary>
        ///     Liefert den <see cref="ProposedUserDataDto" />
        /// </summary>
        /// <returns></returns>
        public virtual ProposedUserDataDto GetUserDataDto() {
            return new ProposedUserDataDto(FirstName, LastName, Title, Salutation, Birthday);
        }

        public override string ToString() {
            if (!string.IsNullOrWhiteSpace(_lastName) && !string.IsNullOrWhiteSpace(_firstName)) {
                return string.Format("{0} {1} {2}", Id, _firstName, _lastName);
            }

            return _userName;
        }

        public virtual void Update(string userName, ProposedUserDataDto proposedUserDataDto, ProposedUserContactDto proposedUserContactDto,
            EntityChangedDto entityChangedDto) {
            _userName = userName;
            Update(proposedUserDataDto);
            Update(proposedUserContactDto);
            Update(entityChangedDto);
        }

        public virtual void Update(string userName, ProposedUserDataDto proposedUserDataDto, ProposedUserContactDto proposedUserContactDto,
            EntityCreatedDto entityCreatedDto) {
            _userName = userName;
            Update(proposedUserDataDto);
            Update(proposedUserContactDto);
            Update(entityCreatedDto);
        }

        /// <summary>
        ///     Aktualisiert die Kontaktdaten des Nutzers
        /// </summary>
        /// <param name="userContactDto"></param>
        private void Update(ProposedUserContactDto userContactDto) {
            _email = userContactDto.Email;
            _city = userContactDto.City;
            _company = userContactDto.Company;
            _country = userContactDto.Country;
            _postalCode = userContactDto.PostalCode;
            _street = userContactDto.Street;
            _streetNumber = userContactDto.StreetNumber;
            _url = userContactDto.Url;

            _phone = userContactDto.Phone;
            _phonePrivate = userContactDto.PhonePrivate;
            _mobile = userContactDto.Mobile;
        }

        /// <summary>
        ///     Aktualisiert den Benutzernamen
        /// </summary>
        /// <param name="userDataDto"></param>
        private void Update(ProposedUserDataDto userDataDto) {
            _firstName = userDataDto.FirstName;
            _lastName = userDataDto.LastName;
            _title = userDataDto.Title;
            _salutation = userDataDto.Salutation;
            _birthday = userDataDto.Birthday;
        }

        /// <summary>
        ///     Aktualisiert den Zeitpunkt und die Person der Erstellung dieses Nutzers.
        /// </summary>
        /// <param name="entityCreatedDto"></param>
        private void Update(EntityCreatedDto entityCreatedDto) {
            Require.NotNull(entityCreatedDto, "entityCreatedDto");

            _createdBy = entityCreatedDto.CreatedBy;
            _createdAt = entityCreatedDto.CreatedAt;
        }

        /// <summary>
        ///     Aktualisiert den Zeitpunkt und den durchführenden Nutzer der letzten Änderung an diesem Nutzer.
        /// </summary>
        /// <param name="entityChangedDto"></param>
        private void Update(EntityChangedDto entityChangedDto) {
            Require.NotNull(entityChangedDto, "entityChangedDto");

            _changedBy = entityChangedDto.ChangedBy;
            _changedAt = entityChangedDto.ChangedAt;
        }

        private void UpdateUsername(string userName) {
            Require.NotNullOrWhiteSpace(userName, "userName");

            _userName = userName;
        }
    }
}