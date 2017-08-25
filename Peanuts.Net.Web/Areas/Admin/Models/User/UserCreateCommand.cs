using System.ComponentModel.DataAnnotations;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Resources;
using Com.QueoFlow.Peanuts.Net.Web.Resources;

namespace Com.QueoFlow.Peanuts.Net.Web.Areas.Admin.Models.User {
    /// <summary>
    ///     Das Command zum Erstellen eines Users
    /// </summary>
    public class UserCreateCommand {
        /// <summary>
        ///     Erzeugt eine neue Instanz von <see cref="UserCreateCommand" />
        /// </summary>
        public UserCreateCommand() {
            UserContactDto = new UserContactDto();
            UserDataDto = new UserDataDto();
            UserPermissionDto = new UserPermissionDto();
            UserPaymentDto = new UserPaymentDto();
        }

        public UserPaymentDto UserPaymentDto { get; set; }

        /// <summary>
        ///     Gibt oder setzt das UserContactDto
        /// </summary>
        public UserContactDto UserContactDto { get; set; }

        /// <summary>
        ///     Gibt oder setzt das UserDataDto
        /// </summary>
        public UserDataDto UserDataDto { get; set; }

        /// <summary>
        ///     Liefert oder setzt das UserPermissionDto
        /// </summary>
        public UserPermissionDto UserPermissionDto { get; set; }


        /// <summary>
        ///     Ruft das neue Passwort für den Nutzer ab oder legt dieses fest.
        ///     Ist kein Passwort gesetzt, wird es nicht geändert.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [Display(ResourceType = typeof(Resources_Web), Name = "common_label_NewPassword")]
        public string Password { get; set; }

        /// <summary>
        ///     Ruft die Bestätigung des zu ändernden Passwortes ab oder legt diese fest.
        ///     Muss mit der Eigenschaft <see cref="Password" /> übereinstimmen.
        /// </summary>
        [Compare("Password")]
        [Display(ResourceType = typeof(Resources_Web), Name = "common_label_NewPasswordConfirmation")]
        public string PasswordConfirmation { get; set; }
    }
}