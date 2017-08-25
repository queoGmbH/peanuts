using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Web.Resources;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.User {
    /// <summary>
    ///     Das Command zum bearbeiten eines Users
    /// </summary>
    public class UserUpdateCommand {
        /// <summary>
        ///     Erzeugt eine neue Instanz von <see cref="UserUpdateCommand" />
        /// </summary>
        public UserUpdateCommand() {
            UserContactDto = new UserContactDto();
            UserDataDto = new UserDataDto();
            UserPermissionDto = new UserPermissionDto();
            UserPaymentDto = new UserPaymentDto();
            NewDocuments = new List<UploadedFile>();
            DeleteDocuments = new List<Document>();
            UserNotificationOptionsDto = new UserNotificationOptionsDto();
        }

        /// <summary>
        ///     Erzeugt eine neue Instanz vom <see cref="UserUpdateCommand" />
        /// </summary>
        /// <param name="user">Der zu ändernde Nutzer</param>
        public UserUpdateCommand(Core.Domain.Users.User user) {
            Require.NotNull(user, "user");

            Username = user.UserName;
            UserContactDto = user.GetUserContactDto();
            UserDataDto = user.GetUserDataDto();
            UserPermissionDto = user.GetUserPermissionDto();
            UserPaymentDto = user.GetUserPaymentDto();
            UserNotificationOptionsDto = user.GetNotificationOptions();
        }

        /// <summary>
        ///     Ruft die Liste mit zu löschenden Dokumenten ab oder legt diese fest.
        /// </summary>
        public IList<Document> DeleteDocuments { get; set; }

        public UserPaymentDto UserPaymentDto {
            get; set;
        }

        public UserNotificationOptionsDto UserNotificationOptionsDto { get; set; }

        /// <summary>
        ///     Ruft die liste mit Dokumenten ab, die dem Nutzer neu zugeordnet werden sollen oder legt diese fest.
        /// </summary>
        public List<UploadedFile> NewDocuments { get; set; }

        /// <summary>
        ///     Ruft das neue Passwort für den Nutzer ab oder legt dieses fest.
        ///     Ist kein Passwort gesetzt, wird es nicht geändert.
        /// </summary>
        [Display(ResourceType = typeof(Resources_Web), Name = "common_label_NewPassword")]
        public string NewPassword { get; set; }

        /// <summary>
        ///     Ruft die Bestätigung des zu ändernden Passwortes ab oder legt diese fest.
        ///     Muss mit der Eigenschaft <see cref="NewPassword" /> übereinstimmen.
        /// </summary>
        [Display(ResourceType = typeof(Resources_Web), Name = "common_label_NewPasswordConfirmation")]
        [Compare("NewPassword")]
        public string NewPasswordConfirmation { get; set; }

        /// <summary>
        ///     Gibt oder Setzt das UserContactDto
        /// </summary>
        public UserContactDto UserContactDto { get; set; }

        /// <summary>
        ///     Gibt oder Setzt das UserDataDto
        /// </summary>
        public UserDataDto UserDataDto { get; set; }

        /// <summary>
        ///     Ruft den <see cref="User.UserName" /> des Nutzers ab oder legt diesen fest.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        ///     Liefert oder setzt das PermissionDto für die Berechtigungseinstellungen
        /// </summary>
        public UserPermissionDto UserPermissionDto { get; set; }
    }
}