using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.ResourceManagement;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

using Common.Logging;

using NHibernate;

using Spring.Transaction.Interceptor;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    public class UserService : IUserService {
        public IDocumentRepository DocumentRepository { get; set; }

        public EmailService EmailService { get; set; }

        /// <summary>
        ///     Liefert oder setzt den UserDao
        /// </summary>
        public IUserDao UserDao { get; set; }

        /// <summary>
        ///     Überprüft ob ein Nutzer gelöscht werden kann.
        ///     Ein Nutzer kann nur gelöscht werden, wenn er nicht mit Vorgängen (TODO: Verlinken mit Klasse(n)) verbunden ist.
        ///     Kann ein Nutzer nicht gelöscht werden, ist alternativ eine Archivierung möglich.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool CanUserBeDeleted(User user) {
            if (UserDao.IsUserReferencesWithIssues(user)) {
                /*Nutzer ist mit Vorgängen verknüpft und darf nicht gelöscht werden*/
                return false;
            }

            /*Nutzer kann gelöscht werden*/
            return true;
        }

        /// <summary>
        ///     Erzeugt einen neuen Nutzer.
        /// </summary>
        /// <param name="userContactDto">Kontaktdaten des Nutzers</param>
        /// <param name="userDataDto">Informationen über den Nutzer.</param>
        /// <param name="entityCreatedDto">Wer hat wann den Nutzer erstellt?</param>
        /// <param name="userPermissionDto">Zugangs-Informationen des Nutzers</param>
        /// <param name="passwordHash">Der Hash des vom Nutzer verwendeten Passworts.</param>
        /// <returns></returns>
        [Transaction]
        public User Create(string passwordHash, UserContactDto userContactDto, UserDataDto userDataDto, UserPaymentDto userPaymentDto,
            UserPermissionDto userPermissionDto, EntityCreatedDto entityCreatedDto) {
      Require.NotNull(userContactDto);
            Require.NotNull(userDataDto);
            Require.NotNull(entityCreatedDto, "entityCreatedDto");

            User user = new User(passwordHash,
                userContactDto,
                userDataDto,
                userPaymentDto,
                /*Standardmäßig alle Benachrichtigungen an*/
                UserNotificationOptionsDto.AllOn,
                userPermissionDto,
                new List<Document>(),
                entityCreatedDto);

            return UserDao.Save(user);
        }

        /// <summary>
        ///     Löscht oder archiviert einen Nutzer.
        ///     Ist der Nutzer mit Vorgängen verbunden, wird er archiviert. Eine Archivierung bedeutet, dass der Nutzer zwar noch
        ///     vorhanden ist, aber - falls nicht explizit anders aufgerufen - von Find-Methoden nicht mehr gefunden wird.
        ///     Eine Anmeldung an der Anwendung ist ebenfalls nicht mehr möglich.
        ///     Andernfalls wird er gelöscht und existiert in der Anwendung NICHT mehr.
        /// </summary>
        /// <param name="user">Der zu löschende Nutzer</param>
        [Transaction]
        public void Delete(User user) {
            if (CanUserBeDeleted(user)) {
                /*Nutzer aus DB löschen*/
                UserDao.Delete(user);
            } else {
                /*Nutzer archivieren (soft-delete)*/
                user.Delete();
            }
        }

        /// <summary>
        ///     Sucht nach Nutzern mit der angegebenen Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User FindByEmail(string email) {
            return UserDao.FindByEmail(email);
        }

        public User FindByPasswordResetCode(string code) {
            return UserDao.FindByPasswordResetCode(Guid.Parse(code));
        }

        /// <summary>
        ///     Sucht nach dem Nutzer anhand des Benutzernamen.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User FindByUserName(string userName) {
            User user = UserDao.FindByUserName(userName);
            return user;
        }

        /// <summary>
        ///     Ruft Seitenweise die Nutzer ohne Einschränkung ab.
        /// </summary>
        /// <param name="pageable">Welche Seite soll abgerufen werden?</param>
        /// <param name="searchTerm">Einschränkende Suchzeichenfolge für die Suche nach Nutzern.</param>
        /// <returns></returns>
        public IPage<User> FindUser(IPageable pageable, string searchTerm = null) {
            return UserDao.FindUser(pageable, searchTerm);
        }

        /// <summary>
        ///     Liefert die Anzahl der aktiven Nutzer.
        /// </summary>
        /// <returns></returns>
        public int GetActiveUsers() {
            return UserDao.GetActiveUserCount();
        }

        /// <summary>
        ///     Liefert eine Liste aller Nutzer.
        /// </summary>
        /// <returns></returns>
        public IList<User> GetAll() {
            return UserDao.GetAll();
        }

        /// <summary>
        ///     Sucht nach dem Nutzer anhand der BusinessId.
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        public User GetByBusinessId(Guid bid) {
            User user = UserDao.GetByBusinessId(bid);
            return user;
        }

        /// <summary>
        ///     Liefert den Nutzer mit der entsprechenden Id.
        /// </summary>
        /// <param name="id">ID des Nutzers.</param>
        /// <returns>Der Nutzer</returns>
        /// <exception cref="ObjectNotFoundException"> wenn der Nutzer mit der ID nicht existiert</exception>
        public User GetById(int id) {
            return UserDao.Get(id);
        }

        /// <summary>
        ///     Ruft die Anzahl aller Nutzer ab.
        /// </summary>
        /// <returns></returns>
        public long GetCount() {
            return UserDao.GetCount();
        }

        /// <summary>
        ///     Prüft ab ob für den Nutzer mit der angegebenen Id ein valides Passwort existiert.
        /// </summary>
        /// <param name="securityUserId"></param>
        /// <returns></returns>
        public bool HasPassword(string securityUserId) {
            Require.NotNull(securityUserId);

            int id = int.Parse(securityUserId);
            User user = UserDao.Get(id);
            if (!string.IsNullOrWhiteSpace(user.PasswordHash)) {
                return true;
            }

            return false;
        }

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
        [Transaction]
        public User Register(string username, string passwordHash, UserContactDto userContactDto, UserDataDto userDataDto,
            UserPaymentDto userPaymentDto, UserPermissionDto userPermissionDto, EntityCreatedDto entityCreatedDto, string baseLink) {
            Require.NotNullOrWhiteSpace(username, "username");
            Require.NotNull(userContactDto);
            Require.NotNull(userDataDto);
            Require.NotNull(entityCreatedDto, "entityCreatedDto");

            User user = new User(passwordHash,
                userContactDto,
                userDataDto,
                userPaymentDto,
                /*Standardmäßig alle Benachrichtigungen an*/
                UserNotificationOptionsDto.AllOn,
                userPermissionDto,
                new List<Document>(),
                entityCreatedDto);
            Guid.NewGuid();
            user = UserDao.Save(user);

            return user;
        }

        /// <summary>
        ///     Setzt das Password eines Benutzers und entfernt den PasswordResetCode
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="entityChangedDto"></param>
        [Transaction]
        public void ResetPassword(User user, string password, EntityChangedDto entityChangedDto) {
            Require.NotNull(entityChangedDto, "entityChangedDto");

            user.UpdatePassword(password, entityChangedDto);
            user.UpdatePasswordResetCode(Guid.Empty);
        }

        /// <summary>
        ///     Sendet die Mail mit dem Bestätigungslink
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmationUrl"></param>
        public void SendConfirmationMailMessage(User user, string confirmationUrl) {
            MailMessage mailMessage = EmailService.CreateMailMessage(user.Email,
                GetEmailModelForEmailConfirmation(user.DisplayName, confirmationUrl),
                "RegisterUser");
            _logger.Info($"Send Message to {mailMessage.To.First().Address}");
            EmailService.SendMessage(mailMessage);
        }

        private readonly ILog _logger = LogManager.GetLogger(typeof(UserService));

        /// <summary>
        /// Liefert alle Nutzer die Administratoren sind
        /// </summary>
        /// <returns></returns>
        public IList<User> FindAllAdmins() {
            
            return UserDao.FindByRole(Roles.Administrator);
        }

        /// <summary>
        ///     Setzt den Nutzer aktiv, die Email wurde bestätigt
        /// </summary>
        /// <param name="user"></param>
        /// <param name="editLink"></param>
        public void SetEmailConfirmed(User user, string editLink) {
            user.IsEnabled = true;
            NotificationService.SendNotificationAboutUserActivation(user,editLink);
        }

        /// <summary>
        ///    Liefert oder setzt den NotificationService
        /// </summary>
        public INotificationService NotificationService { get; set; }

        /// <summary>
        ///     Setzt den Code zum zurücksetzen des Passworts und schickt eine Email an den Nutzer mit dem Link.
        /// </summary>
        [Transaction]
        public void SetPasswordResetCodeAndSendEmail(User user, string baseLink) {
            Require.NotNull(user, "user");

            Guid passwordResetCode = Guid.NewGuid();
            user.UpdatePasswordResetCode(passwordResetCode);
            string fullLink = baseLink + "/Account/ResetPassword/" + passwordResetCode;
            MailMessage mailMessage = EmailService.CreateMailMessage(user.Email,
                GetEmailModelForPasswordReset(user, fullLink),
                "RequestPasswordReset");
            EmailService.SendMessage(mailMessage);
        }

        /// <summary>
        ///     Aktualisiert die Daten des Nutzers.
        /// </summary>
        /// <param name="user">Zu aktualisierender Nutzer</param>
        /// <param name="passwordHash">Der Hash des vom Nutzer verwendeten Passworts.</param>
        /// <param name="userContactDto"></param>
        /// <param name="userDataDto"></param>
        /// <param name="documentsToDelete"></param>
        /// <param name="entityChangedDto"></param>
        /// <param name="userPaymentDto"></param>
        /// <param name="userNotificationOptionsDto"></param>
        /// <param name="userPermissionDto"></param>
        /// <param name="newDocuments"></param>
        [Transaction]
        public void Update(User user, string passwordHash, UserContactDto userContactDto, UserDataDto userDataDto,
            UserPaymentDto userPaymentDto, UserNotificationOptionsDto userNotificationOptionsDto,
            UserPermissionDto userPermissionDto, IList<UploadedFile> newDocuments, IList<Document> documentsToDelete,
            EntityChangedDto entityChangedDto) {
            Require.NotNull(user, "user");
            Require.NotNull(userContactDto, "userContactDto");
            Require.NotNull(userDataDto, "userDataDto");
            Require.NotNull(userPermissionDto, "userPermissionDto");
            Require.NotNull(entityChangedDto, "entityChangedDto");
            Require.NotNull(documentsToDelete, "documentsToDelete");
            Require.NotNull(newDocuments, "newDocuments");

            IList<Document> documentsToAssign = newDocuments.Select(uf => DocumentRepository.Create(uf)).ToList();
            IList<Document> userDocuments = user.Documents.Except(documentsToDelete).Concat(documentsToAssign).ToList();

            if (IsDirty(user,
                passwordHash,
                userContactDto,
                userDataDto,
                userPaymentDto,
                userNotificationOptionsDto,
                userPermissionDto,
                userDocuments)) {
                user.Update(passwordHash,
                    userContactDto,
                    userDataDto,
                    userPaymentDto,
                    userNotificationOptionsDto,
                    userPermissionDto,
                    userDocuments,
                    entityChangedDto);
            }
        }

        /// <summary>
        ///     Aktualisiert die Personendaten und Kontaktdaten eines Nutzers
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userContactDto"></param>
        /// <param name="userDataDto"></param>
        /// <param name="userPaymentDto"></param>
        /// <param name="userNotificationOptionsDto"></param>
        /// <param name="entityChangedDto"></param>
        [Transaction]
        public void Update(User user, UserContactDto userContactDto, UserDataDto userDataDto, UserPaymentDto userPaymentDto,
            UserNotificationOptionsDto userNotificationOptionsDto, EntityChangedDto entityChangedDto) {
            Require.NotNull(user, "user");
            Require.NotNull(userContactDto, "userContactDto");
            Require.NotNull(userDataDto, "userDataDto");
            Require.NotNull(entityChangedDto, "entityChangedDto");
            Require.NotNull(userPaymentDto, "userPaymentDto");

            if (IsDirty(user,
                user.PasswordHash,
                userContactDto,
                userDataDto,
                userPaymentDto,
                userNotificationOptionsDto,
                user.GetUserPermissionDto(),
                user.Documents)) {
                user.Update(userContactDto, userDataDto, userPaymentDto, userNotificationOptionsDto, entityChangedDto);
            }
        }

        [Transaction]
        public void Update(User user, string username, string passwordHash) {
            Require.NotNull(user, "user");
            Require.NotNullOrWhiteSpace(username, "username");
            Require.NotNullOrWhiteSpace(passwordHash, "passwordHash");

            if (IsDirty(user,
                passwordHash,
                user.GetUserContactDto(),
                user.GetUserDataDto(),
                user.GetUserPaymentDto(),
                user.GetNotificationOptions(),
                user.GetUserPermissionDto(),
                user.Documents)) {
                EntityChangedDto changedDto = new EntityChangedDto(user, DateTime.Now);
                user.UpdatePassword(passwordHash, changedDto);
            }
        }

        private ModelMap GetEmailModelForEmailConfirmation(string name, string confirmationLink) {
            ModelMap emailModel = new ModelMap();
            emailModel.Add("name", name);
            emailModel.Add("confirmationLink", confirmationLink);
            return emailModel;
        }

      

        /// <summary>
        ///     Erstellt ein EmailModel für das PasswordReset und gibt dieses zurück
        /// </summary>
        /// <param name="user"></param>
        /// <param name="baseLink"></param>
        /// <returns></returns>
        private ModelMap GetEmailModelForPasswordReset(User user, string baseLink) {
            ModelMap emailModel = new ModelMap();
            emailModel.Add("user", user);
            emailModel.Add("baseLink", baseLink);
            return emailModel;
        }

        /// <summary>
        ///     Überprüft, ob es Änderungen am Nutzer gab.
        /// </summary>
        /// <param name="user">Der Nutzer in seiner ungeänderten Form.</param>
        /// <param name="passwordHash">Der neue PasswordHash</param>
        /// <param name="userContactDto">Die neuen Kontaktdaten</param>
        /// <param name="userDataDto">Die neuen Nutzerdaten</param>
        /// <param name="userPaymentDto">Die neuen Zahlungsinformationen</param>
        /// <param name="userPermissionDto">Die neuen Berechtigungsinformationen</param>
        /// <param name="userDocuments"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Wenn ein Dto oder eine Eigenschaft sowieso nicht geändert werden soll, dann den Wert nutzen, der am Nutzer
        ///     hinterlegt ist.
        /// </remarks>
        private bool IsDirty(User user, string passwordHash, UserContactDto userContactDto, UserDataDto userDataDto,
            UserPaymentDto userPaymentDto, UserNotificationOptionsDto userNotificationOptionsDto,
            UserPermissionDto userPermissionDto, IList<Document> userDocuments) {
            Require.NotNull(user, "user");
            
            Require.NotNull(userContactDto, "userContactDto");
            Require.NotNull(userDataDto, "userDataDto");
            Require.NotNull(userPermissionDto, "userPermissionDto");
            Require.NotNull(userPaymentDto, "userPaymentDto");
            Require.NotNull(userNotificationOptionsDto, "userNotificationOptionsDto");

           
            if (user.PasswordHash != passwordHash) {
                return true;
            }
            if (!Equals(user.GetUserContactDto(), userContactDto)) {
                return true;
            }
            if (!Equals(user.GetUserPaymentDto(), userPaymentDto)) {
                return true;
            }
            if (!Equals(user.GetUserPermissionDto(), userPermissionDto)) {
                return true;
            }
            if (!Equals(user.GetUserDataDto(), userDataDto)) {
                return true;
            }
            if (!Equals(user.GetNotificationOptions(), userNotificationOptionsDto)) {
                return true;
            }
            if (!ListHelper.AreEquivalent(user.Documents, userDocuments)) {
                return true;
            }

            return false;
        }
    }
}