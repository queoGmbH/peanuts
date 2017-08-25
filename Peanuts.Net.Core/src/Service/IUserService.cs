using System;
using System.Collections;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

using NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    /// <summary>
    ///     Schnittstelle, die einen Service zur Verwaltung von Nutzern beschreibt.
    /// </summary>
    public interface IUserService {
        /// <summary>
        ///     Liefert oder setzt den UserDao
        /// </summary>
        IUserDao UserDao { get; set; }

        /// <summary>
        ///     Erzeugt einen neuen Nutzer.
        /// </summary>
        /// <param name="userContactDto">Kontaktdaten des Nutzers</param>
        /// <param name="userDataDto">Informationen über den Nutzer.</param>
        /// <param name="entityCreatedDto">Wer hat wann den Nutzer erstellt?</param>
        /// <param name="userPaymentDto">Informationen und Einstellungen im Zusammenhang mit Zahlungen.</param>
        /// <param name="userPermissionDto">Zugangs-Informationen des Nutzers</param>
        /// <param name="passwordHash">Der Hash des vom Nutzer verwendeten Passworts.</param>
        /// <returns></returns>
        User Create(string passwordHash, UserContactDto userContactDto, UserDataDto userDataDto, UserPaymentDto userPaymentDto,
            UserPermissionDto userPermissionDto, EntityCreatedDto entityCreatedDto);

        /// <summary>
        ///     Löscht einen Nutzer.
        /// </summary>
        /// <param name="user">Der zu löschende Nutzer</param>
        void Delete(User user);

        /// <summary>
        ///     Sucht nach Nutzern mit der angegebenen Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        User FindByEmail(string email);

        /// <summary>
        ///     Findet einen User anhand des PasswortResetSchlüssels
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        User FindByPasswordResetCode(string code);

        /// <summary>
        ///     Sucht nach dem Nutzer anhand des Benutzernamen.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        User FindByUserName(string userName);

        /// <summary>
        ///     Ruft Seitenweise die Nutzer ohne Einschränkung ab.
        /// </summary>
        /// <param name="pageable">Welche Seite soll abgerufen werden?</param>
        /// <param name="searchTerm">Einschränkende Suchzeichenfolge für die Suche nach Nutzern.</param>
        /// <returns></returns>
        IPage<User> FindUser(IPageable pageable, string searchTerm = null);

        /// <summary>
        ///     Liefert die Anzahl der aktiven Nutzer.
        /// </summary>
        /// <returns></returns>
        int GetActiveUsers();

        /// <summary>
        ///     Liefert eine Liste aller Nutzer.
        /// </summary>
        /// <returns></returns>
        IList<User> GetAll();

        /// <summary>
        ///     Sucht nach dem Nutzer anhand der BusinessId.
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        User GetByBusinessId(Guid bid);

        /// <summary>
        ///     Liefert den Nutzer mit der entsprechenden Id.
        /// </summary>
        /// <param name="id">ID des Nutzers.</param>
        /// <returns>Der Nutzer</returns>
        /// <exception cref="ObjectNotFoundException"> wenn der Nutzer mit der ID nicht existiert</exception>
        User GetById(int id);

        /// <summary>
        ///     Ruft die Anzahl aller Nutzer ab.
        /// </summary>
        /// <returns></returns>
        long GetCount();

        /// <summary>
        ///     Prüft ab ob für den Nutzer mit der angegebenen Id ein valides Passwort existiert.
        /// </summary>
        /// <param name="securityUserId"></param>
        /// <returns></returns>
        bool HasPassword(string securityUserId);

        /// <summary>
        ///     Setzt das Password eines Benutzers und entfernt den PasswordResetCode
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="entityChangedDto">Welcher Nutzer ändert das Passwort und wann? Das kann auch der Nutzer selbst sein.</param>
        void ResetPassword(User user, string password, EntityChangedDto entityChangedDto);

        /// <summary>
        ///     Setzt den Code zum zurücksetzen des Passworts und schickt eine Email an den Nutzer mit dem Link.
        /// </summary>
        void SetPasswordResetCodeAndSendEmail(User user, string baseLink);

        /// <summary>
        ///     Aktualisiert die Daten des Nutzers, darunter auch Zugangsberechtigungen (für Administration)
        /// </summary>
        /// <param name="user">Zu aktualisierender Nutzer</param>
        /// <param name="passwordHash">Der Hash des vom Nutzer verwendeten Passworts.</param>
        /// <param name="userContactDto"></param>
        /// <param name="userDataDto"></param>
        /// <param name="userPaymentDto">Informationen und Einstellungen im Zusammenhang mit Zahlungen.</param>
        /// <param name="userPermissionDto"></param>
        /// <param name="newDocuments">Liste mit hochgeladenen Dokumenten, die dem Nutzer neu zugeordnet werden sollen.</param>
        /// <param name="documentsToDelete">Liste mit Dokumenten, die vom Nutzer entfernt werden sollen.</param>
        /// <param name="userNotificationOptionsDto">Benachrichtigungseinstellungen für den Nutzer</param>
        /// <param name="entityChangedDto">Wer hat den Nutzer und wann zuletzt geändert?</param>
        void Update(User user, string passwordHash, UserContactDto userContactDto, UserDataDto userDataDto,
            UserPaymentDto userPaymentDto, UserNotificationOptionsDto userNotificationOptionsDto, UserPermissionDto userPermissionDto, IList<UploadedFile> newDocuments, IList<Document> documentsToDelete,
            EntityChangedDto entityChangedDto);

        /// <summary>
        ///     Aktualisiert die Personendaten und Kontaktdaten eines Nutzers
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userContactDto"></param>
        /// <param name="userDataDto"></param>
        /// <param name="userPaymentDto">Informationen und Einstellungen im Zusammenhang mit Zahlungen</param>
        /// <param name="userNotificationOptionsDto">Benachrichtigungseinstellungen des Nutzers.</param>
        /// <param name="entityChangedDto">Wer hat den Nutzer und wann zuletzt geändert?</param>
        void Update(User user, UserContactDto userContactDto, UserDataDto userDataDto, UserPaymentDto userPaymentDto, UserNotificationOptionsDto userNotificationOptionsDto, EntityChangedDto entityChangedDto);

        /// <summary>
        ///     Aktualisiert Nutzername und Passwort eines Nutzers
        /// </summary>
        /// <param name="user"></param>
        /// <param name="username"></param>
        /// <param name="passwordHash"></param>
        void Update(User user, string username, string passwordHash);

        /// <summary>
        ///     Erzeugt einen neuen Nutzer.
        /// </summary>
        /// <param name="username">Der Name des Nutzers</param>
        /// <param name="passwordHash">Der Hash des vom Nutzer verwendeten Passworts.</param>
        /// <param name="userContactDto">Kontaktdaten des Nutzers</param>
        /// <param name="userDataDto">Informationen über den Nutzer.</param>
        /// <param name="userPaymentDto"></param>
        /// <param name="userPermissionDto">Zugangs-Informationen des Nutzers</param>
        /// <param name="entityCreatedDto">Wer hat wann den Nutzer erstellt?</param>
        /// <param name="baseLink"></param>
        /// <returns></returns>
        User Register(string username, string passwordHash, UserContactDto userContactDto, UserDataDto userDataDto, UserPaymentDto userPaymentDto, UserPermissionDto userPermissionDto, EntityCreatedDto entityCreatedDto, string baseLink);

        /// <summary>
        /// Setzt den Nutzer aktiv, die Email wurde bestätigt
        /// </summary>
        /// <param name="user"></param>
        /// <param name="editLink"></param>
        void SetEmailConfirmed(User user, string editLink);

        /// <summary>
        ///     Sendet die Mail mit dem Bestätigungslink
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmationUrl"></param>
        void SendConfirmationMailMessage(User user, string confirmationUrl);

        /// <summary>
        /// Liefert alle Nutzer die Administratoren sind
        /// </summary>
        /// <returns></returns>
        IList<User> FindAllAdmins();
    }
}