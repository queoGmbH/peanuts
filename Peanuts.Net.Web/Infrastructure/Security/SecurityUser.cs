using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security {
    /// <summary>
    ///     Bildet den Teil des Nutzers ab der für die Sicherheitsbelange erforderlich ist.
    /// </summary>
    /// <remarks>
    ///     Der SecurityUser sollte die Teile enthalten, die von OWIN benötigt werden. Damit wird die Entkopplung der
    ///     OWIN Infrastruktur (und Nutzerverwaltung) von unserer eigenen Nutzerverwaltung - sprich dem UserService - erreicht
    ///     und
    ///     damit eine klare Trennung vorgenommen.
    /// </remarks>
    public class SecurityUser : IUser<string> {
#pragma warning disable 649
        private readonly string _id;
#pragma warning restore 649
        private readonly bool _isEnabled;
        private readonly IList<string> _roles = new List<string>();
        private int _accessFailedCount;
        private string _email;
        private bool _lockOutEnabled;
        private DateTimeOffset _lockOutEndDate;
        private string _passwordHash;
        private string _phoneNumber;
        private bool _twoFactorEnabled;
        private string _userName;

        public SecurityUser() {
        }

        /// <summary>Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.</summary>
        public SecurityUser(string id, string userName, string email, string passwordHash, bool isEnabled, IList<string> roles, string firstName, string lastName) {
            _email = email;
            _id = id;
            _userName = userName;
            _passwordHash = passwordHash;
            _isEnabled = isEnabled;
            _roles = roles;
            FirstName = firstName;
            LastName = lastName;
        }

        /// <summary>
        ///    Liefert oder setzt den FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///    Liefert oder setzt den LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     Liefert und setzt die fehlgeschlagenen Login-Versuche
        /// </summary>
        public int AccessFailedCount {
            get { return _accessFailedCount; }
            set { _accessFailedCount = value; }
        }

        /// <summary>
        ///     Liefert und setzt die Email-Adresse
        /// </summary>
        public string Email {
            get { return _email; }
            set { _email = value; }
        }

        /// <summary>
        ///     Liefert und setzt die BusinessId des Nutzers.
        /// </summary>
        public string Id {
            get { return _id; }
        }

        /// <summary>
        ///     Ruft ab, ob der Nutzer aktiv oder inaktiv gesetzt ist.
        /// </summary>
        public bool IsEnabled {
            get { return _isEnabled; }
        }

        public bool LockOutEnabled {
            get { return _lockOutEnabled; }
            set { _lockOutEnabled = value; }
        }

        public DateTimeOffset LockOutEndDate {
            get { return _lockOutEndDate; }
            set { _lockOutEndDate = value; }
        }

        /// <summary>
        ///     Liefert und setzt den Passwort-Hash
        /// </summary>
        public string PasswordHash {
            get { return _passwordHash; }
            set { _passwordHash = value; }
        }

        /// <summary>
        ///     Liefert und setzt die Telefonnummer
        /// </summary>
        public string PhoneNumber {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }

        /// <summary>
        ///     Ruft eine schreibgeschützte Kopie der Liste mit Rollen ab, die dem Nutzer zugeordnet sind.
        /// </summary>
        public IList<string> Roles {
            get { return new ReadOnlyCollection<string>(_roles); }
        }

        public bool TwoFactorEnabled {
            get { return _twoFactorEnabled; }
            set { _twoFactorEnabled = value; }
        }

        /// <summary>
        ///     Liefert und setzt den Benutzernamen
        /// </summary>
        public string UserName {
            get { return _userName; }
            set { _userName = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SecurityUser> manager) {
            // Beachten Sie, dass der "authenticationType" mit dem in "CookieAuthenticationOptions.AuthenticationType" definierten Typ übereinstimmen muss.
            ClaimsIdentity userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Benutzerdefinierte Benutzeransprüche hier hinzufügen
            // Die benutzerdefinierten Benutzeransprüche werden im ApplicationUserManager hinzugefügt, da dort die 
            // entsprechenden Services zur Verfügung stehen, um die Daten zu ermitteln.
            // dementsprechend sollten an dieser Stelle keine weiteren Claims hinzugefügt werden.
            return userIdentity;
        }
    }
}