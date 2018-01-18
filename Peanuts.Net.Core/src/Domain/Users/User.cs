using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Users {
    /// <summary>
    ///     Bildet einen Nutzer im System ab.
    /// </summary>
    [DebuggerDisplay("{Id}: {UserName}")]
    public class User : Entity {
        private readonly IList<Document> _documents = new List<Document>();
        private readonly IList<string> _roles = new List<string>();

 
        private bool _autoAcceptPayPalPayments;

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

        private bool _isDeleted;
        private bool _isEnabled = true;
        private string _lastName;
        private string _mobile;
        private bool _notifyMeAsCreditorOnDeclinedBills = true;
        private bool _notifyMeAsCreditorOnPeanutDeleted = true;
        private bool _notifyMeAsCreditorOnPeanutRequirementsChanged = true;
        private bool _notifyMeAsCreditorOnSettleableBills = true;
        private bool _notifyMeAsDebitorOnIncomingBills = true;
        private bool _notifyMeAsParticipatorOnPeanutChanged = true;
        private bool _notifyMeOnIncomingPayment = true;

        private bool _notifyMeOnPeanutInvitation = true;
        private string _passwordHash;
        private Guid? _passwordResetCode;

        private string _payPalBusinessName;

        private string _phone;
        private string _phonePrivate;
        private string _postalCode;
        private bool _sendMeWeeklySummaryAndForecast = true;
        private string _street;
        private string _streetNumber;
        private string _url;
        private string _userName;

        /// <summary>
        ///     Konstruktor für nHibernate.
        /// </summary>
        public User() {
        }

        public User(string passwordHash, UserContactDto userContactDto, UserDataDto userDataDto, UserPaymentDto userPaymentDto,
            UserNotificationOptionsDto userNotificationOptions,
            UserPermissionDto userPermissionDto, IList<Document> documents, EntityCreatedDto entityCreatedDto) {
            // TODO: Muss hier ein Passwort angegeben werden oder nicht?
            // Require.NotNullOrWhiteSpace(passwordHash, "passwordHash");
            Require.NotNull(userContactDto, "userContactDto");
            Require.NotNull(userDataDto, "userDataDto");
            Require.NotNull(userPermissionDto, "userPermissionDto");
            Require.NotNull(entityCreatedDto, "entityCreatedDto");
            Require.NotNull(userPaymentDto, "userPaymentDto");
            Require.NotNull(documents, "documents");
            Require.NotNull(userNotificationOptions, "userNotificationOptions");


            _passwordHash = passwordHash;

            Update(userContactDto);
            Update(userDataDto);
            Update(userPermissionDto);
            Update(entityCreatedDto);
            Update(userPaymentDto);
            Update(userNotificationOptions);
            Update(documents);
        }

        /// <summary>
        ///     Ruft ab, ob beim Erhalt einer Zahlung mit PayPal, diese automatisch akzeptiert und abgerechnet wird.
        /// </summary>
        public virtual bool AutoAcceptPayPalPayments {
            get { return _autoAcceptPayPalPayments; }
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
        ///     Ruft den Wohnort des Nutzers ab.
        /// </summary>
        public virtual string City {
            get { return _city; }
        }

        /// <summary>
        ///     Ruft optional das Unternehmen des Nutzers ab, für welches er tätig ist.
        ///     Ein Nutzer muss keinem Unternehmen zugewiesen sein.
        /// </summary>
        public virtual string Company {
            get { return _company; }
        }

        /// <summary>
        ///     Ruft den 2-Buchstaben-ISO-Code des Landes ab, in welchem sich der Wohnsitz des Nutzers befindet.
        ///     https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2
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
        ///     Hat sich der Nutzer registriert oder wurde er automatisiert erstellt ist die Eigenschaft NULL.
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
        ///     Ruft eine schreibgeschützte Kopie der Liste mit Dokumenten ab, die dem Nutzer zugeordnet sind.
        /// </summary>
        public virtual IList<Document> Documents {
            get { return new ReadOnlyCollection<Document>(_documents); }
        }

        /// <summary>
        ///     Email-Adresse, die für Email-Benachrichtigungen an den Nutzer verwendet wird.
        /// </summary>
        public virtual string Email {
            get { return _email; }
        }

        /// <summary>
        ///     Vorname des Nutzers
        /// </summary>
        public virtual string FirstName {
            get { return _firstName; }
        }

        /// <summary>
        ///     Ruft ab, ob der Nutzer gelöscht (bzw. archiviert) wurde.
        /// </summary>
        public virtual bool IsDeleted {
            get { return _isDeleted; }
        }

        /// <summary>
        ///     True, wenn die Anmeldung des Nutzers freigeschaltet ist
        /// </summary>
        public virtual bool IsEnabled {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        /// <summary>
        ///     Nachname des Nutzers
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
        ///     Ruft ab oder legt fest, ob der Nutzer als Kreditor eine Benachrichtigung erhalten möchte, wenn ein Schuldner die
        ///     Rechnung abgelehnt hat.
        /// </summary>
        public virtual bool NotifyMeAsCreditorOnDeclinedBills {
            get { return _notifyMeAsCreditorOnDeclinedBills; }
        }

        /// <summary>
        ///     Ruft ab oder legt fest, ob der Nutzer eine Benachrichtigung erhalten möchte, wenn ein Peanut an dem er teilnimmt
        ///     gelöscht wurde.
        /// </summary>
        public virtual bool NotifyMeAsCreditorOnPeanutDeleted {
            get { return _notifyMeAsCreditorOnPeanutDeleted; }
        }

        /// <summary>
        ///     Ruft ab oder legt fest, ob der Nutzer eine Benachrichtigung erhalten möchte, wenn er als Kreditor an einem Peanut
        ///     teilnimmt und die Anforderungsliste geändert wurde.
        /// </summary>
        public virtual bool NotifyMeAsCreditorOnPeanutRequirementsChanged {
            get { return _notifyMeAsCreditorOnPeanutRequirementsChanged; }
        }

        /// <summary>
        ///     Ruft ab oder legt fest, ob der Nutzer als Kreditor eine Benachrichtigung erhalten möchte, wenn eine Rechnung von
        ///     allen Schuldnern  akzeptiert wurde und abgerechnet werden kann.
        /// </summary>
        public virtual bool NotifyMeAsCreditorOnSettleableBills {
            get { return _notifyMeAsCreditorOnSettleableBills; }
        }

        /// <summary>
        ///     Ruft ab oder legt fest, ob der Nutzer eine Benachrichtigung erhalten möchte, wenn er eine neue Rechnung erhalten
        ///     hat.
        /// </summary>
        public virtual bool NotifyMeAsDebitorOnIncomingBills {
            get { return _notifyMeAsDebitorOnIncomingBills; }
        }

        /// <summary>
        ///     Ruft ab oder legt fest, ob der Nutzer eine Benachrichtigung erhalten möchte, wenn ein Peanut an dem er teilnimmt
        ///     geändert wurde.
        /// </summary>
        public virtual bool NotifyMeAsParticipatorOnPeanutChanged {
            get { return _notifyMeAsParticipatorOnPeanutChanged; }
        }

        /// <summary>
        ///     Ruft ab oder legt fest, ob der Nutzer eine Benachrichtigung erhalten möchte, wenn er den Eingang oder Ausgang einer
        ///     Bezahlung bestätigen muss.
        /// </summary>
        public virtual bool NotifyMeOnIncomingPayment {
            get { return _notifyMeOnIncomingPayment; }
        }

        /// <summary>
        ///     Ruft ab, ob der Nutzer eine Benachrichtigung erhalten möchte, wenn er zu einem Peanut eingeladen wurde.
        /// </summary>
        public virtual bool NotifyMeOnPeanutInvitation {
            get { return _notifyMeOnPeanutInvitation; }
        }

        /// <summary>
        ///     Passwort für die Authentifizierung des Nutzers.
        /// </summary>
        public virtual string PasswordHash {
            get { return _passwordHash; }
        }

        /// <summary>
        ///     Reset-Code, um das Passwort zurückzusetzen.
        /// </summary>
        public virtual Guid? PasswordResetCode {
            get { return _passwordResetCode; }
        }

        /// <summary>
        ///     Ruft den PayPal-Namen des Nutzers ab, an den Zahlungen getätigt werden.
        /// </summary>
        public virtual string PayPalBusinessName {
            get { return _payPalBusinessName; }
        }

        /// <summary>
        ///     Ruft die Telefonnummer des Nutzers ab.
        /// </summary>
        public virtual string Phone {
            get { return _phone; }
        }

        /// <summary>
        ///     Ruft die private Telefonnummer des Nutzers ab.
        /// </summary>
        public virtual string PhonePrivate {
            get { return _phonePrivate; }
        }

        /// <summary>
        ///     Ruft die Postleitzahl vom Wohnort des Nutzers ab.
        /// </summary>
        public virtual string PostalCode {
            get { return _postalCode; }
        }

        /// <summary>
        ///     Rollen, die dem Nutzer zugeordnet sind
        /// </summary>
        public virtual IList<string> Roles {
            get { return new ReadOnlyCollection<string>(_roles); }
        }

        /// <summary>
        ///     Ruft ab oder legt fest, ob der Nutzer wöchentliche eine Zusammenfassung der vergangenen Woche sowie eine Vorschau
        ///     auf die nächste Woche erhalten möchte.
        /// </summary>
        public virtual bool SendMeWeeklySummaryAndForecast {
            get { return _sendMeWeeklySummaryAndForecast; }
        }

        /// <summary>
        ///     Ruft den Namen des Straße ab, in welcher sich der Wohnsitz des Nutzers befindet.
        /// </summary>
        public virtual string Street {
            get { return _street; }
        }

        /// <summary>
        ///     Ruft die Hausnummer vom Wohnsitz des Nutzers ab.
        /// </summary>
        public virtual string StreetNumber {
            get { return _streetNumber; }
        }

        /// <summary>
        ///     Ruft optionale eine URL ab, über welche der Nutzer kontaktiert werden kann.
        /// </summary>
        public virtual string Url {
            get { return _url; }
        }

        /// <summary>
        ///     Benutzername, der für den Login verwendet wird.
        /// </summary>
        public virtual string UserName {
            get { return _userName; }
        }

        /// <summary>
        /// Ruft ab, ob es sich um einen aktiven Nutzer handelt. Also einen Nutzer der nicht archiviert ist und nicht disabled ist.
        /// </summary>
        public virtual bool IsActiveUser {
            get { return !IsDeleted && IsEnabled; } 
        }

        /// <summary>
        ///     Löscht (bzw. archiviert) den Nutzer. Dieser kann sich dann nicht mehr am System anmelden und wird auch in Listen
        ///     nicht mit angezeigt.
        /// </summary>
        public virtual void Delete() {
            _isDeleted = true;
        }

        public virtual UserNotificationOptionsDto GetNotificationOptions() {
            return new UserNotificationOptionsDto(
                _notifyMeAsCreditorOnPeanutDeleted,
                _notifyMeAsCreditorOnPeanutRequirementsChanged,
                _notifyMeAsParticipatorOnPeanutChanged,
                _notifyMeAsCreditorOnDeclinedBills,
                _notifyMeAsDebitorOnIncomingBills,
                _notifyMeOnIncomingPayment,
                _notifyMeAsCreditorOnSettleableBills,
                _sendMeWeeklySummaryAndForecast,
                _notifyMeOnPeanutInvitation);
        }

        /// <summary>
        ///     Liefert den <see cref="UserContactDto" />
        /// </summary>
        /// <returns></returns>
        public virtual UserContactDto GetUserContactDto() {
            return new UserContactDto(_email, _street, _streetNumber, _postalCode, _city, _country, _company, _url, _phone, _phonePrivate, _mobile);
        }

        /// <summary>
        ///     Liefert den <see cref="UserDataDto" />
        /// </summary>
        /// <returns></returns>
        public virtual UserDataDto GetUserDataDto() {
            return new UserDataDto(FirstName, LastName, Birthday, UserName);
        }

        public virtual UserPaymentDto GetUserPaymentDto() {
            return new UserPaymentDto(_payPalBusinessName, _autoAcceptPayPalPayments);
        }

        public virtual UserPermissionDto GetUserPermissionDto() {
            /*Kopie der Liste mit Rollen anlegen, damit diese nicht schreibgeschützt ist und bearbeitet werden kann.*/
            List<string> roles = Roles.ToList();
            return new UserPermissionDto(roles, IsEnabled);
        }

        /// <summary>
        ///     Überprüft, ob dem Nutzer mindestens eine der Rollen zugewiesen ist.
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public virtual bool HasAnyRole(string[] roles) {
            if (roles == null || !roles.Any()) {
                /*Wenn keine Rollen übergeben werden, false liefern*/
                return false;
            }

            /*Für sinnvolle Rollen (nicht null oder leer) überprüfen, ob mindestens eine davon dem Nutzer zugewiesen ist.*/
            return roles.Any(role => !string.IsNullOrWhiteSpace(role) && _roles.Contains(role));
        }

        /// <summary>
        ///     Überprüft, ob einem Nutzer die Rolle zugewiesen ist.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public virtual bool HasRole(string role) {
            if (string.IsNullOrWhiteSpace(role)) {
                return false;
            }
            return _roles.Contains(role);
        }

        public override string ToString() {
            if (!string.IsNullOrWhiteSpace(_lastName) && !string.IsNullOrWhiteSpace(_firstName)) {
                return string.Format("{0} {1} {2}", Id, _firstName, _lastName);
            }

            return _userName;
        }

        /// <summary>
        ///     Aktualisiert alle Eigenschaften des Nutzer und markiert ihn als geändert.
        /// </summary>
        /// <param name="passwordHash"></param>
        /// <param name="userContactDto"></param>
        /// <param name="userDataDto"></param>
        /// <param name="userPaymentDto"></param>
        /// <param name="userNotificationOptionsDto"></param>
        /// <param name="userPermissionDto"></param>
        /// <param name="documents">
        ///     Liste mit Dokumenten, die am Nutzer hinterlegt sein soll. Die Liste enthält sowohl neue, als
        ///     auch weiterhin hinterlegte Dokumente
        /// </param>
        /// <param name="entityChangedDto"></param>
        public virtual void Update(string passwordHash, UserContactDto userContactDto, UserDataDto userDataDto,
            UserPaymentDto userPaymentDto, UserNotificationOptionsDto userNotificationOptionsDto, UserPermissionDto userPermissionDto,
            IList<Document> documents, EntityChangedDto entityChangedDto) {

            Require.NotNullOrWhiteSpace(passwordHash, "passwordHash");
            Require.NotNull(userContactDto, "userContactDto");
            Require.NotNull(userDataDto, "userDataDto");
            Require.NotNull(userPermissionDto, "userPermissionDto");
            Require.NotNull(entityChangedDto, "entityChangedDto");
            Require.NotNull(documents, "documents");
            Require.NotNull(userPaymentDto, "userPaymentDto");
            Require.NotNull(userNotificationOptionsDto, "userNotificationOptionsDto");

     
            _passwordHash = passwordHash;

            Update(userContactDto);
            Update(userDataDto);
            Update(userPermissionDto);
            Update(documents);
            Update(userPaymentDto);
            Update(userNotificationOptionsDto);
            Update(entityChangedDto);
        }

        /// <summary>
        ///     Aktualisiert die Profildaten des Nutzers und markiert ihn als geändert.
        /// </summary>
        /// <param name="userContactDto"></param>
        /// <param name="userDataDto"></param>
        /// <param name="userPaymentDto"></param>
        /// <param name="userNotificationOptionsDto"></param>
        /// <param name="entityChangedDto"></param>
        public virtual void Update(UserContactDto userContactDto, UserDataDto userDataDto, UserPaymentDto userPaymentDto,
            UserNotificationOptionsDto userNotificationOptionsDto, EntityChangedDto entityChangedDto) {
            Require.NotNull(userContactDto, "userContactDto");
            Require.NotNull(userDataDto, "userDataDto");
            Require.NotNull(entityChangedDto, "entityChangedDto");
            Require.NotNull(userPaymentDto, "userPaymentDto");
            Require.NotNull(userNotificationOptionsDto, "userNotificationOptionsDto");

            Update(userContactDto);
            Update(userDataDto);
            Update(userPaymentDto);
            Update(userNotificationOptionsDto);
            Update(entityChangedDto);
        }

        /// <summary>
        ///     Aktualisiert das Passwort, bzw. den Hash des vom Nutzer verwendeten Passworts.
        /// </summary>
        /// <param name="passwordHash"></param>
        /// <param name="entityChangedDto">Wer hat das Passwort geändert und wann?</param>
        public virtual void UpdatePassword(string passwordHash, EntityChangedDto entityChangedDto) {
            _passwordHash = passwordHash;

            Update(entityChangedDto);
        }

        /// <summary>
        ///     Aktualisiert den Reset-Code für die Passwort-Vergessen-Funktion.
        /// </summary>
        /// <param name="passwordResetCode"></param>
        public virtual void UpdatePasswordResetCode(Guid? passwordResetCode) {
            _passwordResetCode = passwordResetCode;
        }

        private void Update(UserPaymentDto paymentDto) {
            _payPalBusinessName = paymentDto.PayPalBusinessName;
            _autoAcceptPayPalPayments = paymentDto.AutoAcceptPayPalPayments;
        }

        private void Update(UserNotificationOptionsDto dto) {
            _notifyMeAsCreditorOnPeanutDeleted = dto.NotifyMeAsCreditorOnPeanutDeleted;
            _notifyMeAsCreditorOnPeanutRequirementsChanged = dto.NotifyMeAsCreditorOnPeanutRequirementsChanged;
            _notifyMeAsParticipatorOnPeanutChanged = dto.NotifyMeAsParticipatorOnPeanutChanged;
            _notifyMeAsCreditorOnDeclinedBills = dto.NotifyMeAsCreditorOnDeclinedBills;
            _notifyMeAsDebitorOnIncomingBills = dto.NotifyMeAsDebitorOnIncomingBills;
            _notifyMeOnIncomingPayment = dto.NotifyMeOnIncomingPayment;
            _notifyMeAsCreditorOnSettleableBills = dto.NotifyMeAsCreditorOnSettleableBills;
            _notifyMeOnPeanutInvitation = dto.NotifyMeOnPeanutInvitation;
            _sendMeWeeklySummaryAndForecast = dto.SendMeWeeklySummaryAndForecast;
        }

        private void Update(IList<Document> documents) {
            /*gelöscht/nicht mehr zugeordnete Dokumente entfernen*/
            IList<Document> documentsToDelete = _documents.Except(documents).ToList();
            foreach (Document documentToDelete in documentsToDelete) {
                _documents.Remove(documentToDelete);
            }

            /*Neue Dokumente hinzufügen*/
            foreach (Document newDocument in documents.Except(_documents)) {
                _documents.Add(newDocument);
            }
        }

        /// <summary>
        ///     Aktualisiert die Kontaktdaten des Nutzers
        /// </summary>
        /// <param name="userContactDto"></param>
        private void Update(UserContactDto userContactDto) {
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
        ///     Aktualisiert die Zugangsberechtigungen
        /// </summary>
        /// <param name="userPermissionDto"></param>
        private void Update(UserPermissionDto userPermissionDto) {
            _roles.Clear();
            foreach (string userRole in userPermissionDto.Roles) {
                _roles.Add(userRole);
            }

            _isEnabled = userPermissionDto.IsEnabled;
        }

        /// <summary>
        ///     Aktualisiert den Benutzernamen
        /// </summary>
        /// <param name="userDataDto"></param>
        private void Update(UserDataDto userDataDto) {
            _firstName = userDataDto.FirstName;
            _lastName = userDataDto.LastName;
            _birthday = userDataDto.Birthday;
            _userName = userDataDto.UserName;

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
    }
}