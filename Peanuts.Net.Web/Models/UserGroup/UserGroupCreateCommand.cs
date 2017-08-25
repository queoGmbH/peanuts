using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    /// <summary>
    /// Command zum Erstellen einer neuen UserGroup.
    /// </summary>
    [DtoFor(typeof(Core.Domain.Users.UserGroup))]
    public class UserGroupCreateCommand {

        /// <summary>
        /// Erzeugt eine neue Instanz von <see cref="UserGroupCreateCommand"/>.
        /// </summary>
        /// <param name="canUserChangePassword">Darf der Nutzer das Passwort ändern.</param>
        /// <param name="commission">Die Maklerprovision in %.</param>
        /// <param name="userGroupDto">Die Daten für den Pool.</param>
        public UserGroupCreateCommand(bool canUserChangePassword, double commission, UserGroupDto userGroupDto) {
            CanUserChangePassword = canUserChangePassword;
            Commission = commission;
            UserGroupDto = userGroupDto;
        }

        /// <summary>
        /// Erzeugt eine neue Instanz von <see cref="UserGroupCreateCommand"/>.
        /// </summary>
        public UserGroupCreateCommand() {
            UserGroupDto = new UserGroupDto();
            Commission = 0.0;
            CanUserChangePassword = false;
        }

        /// <summary>
        /// Liefert oder setzt den <see cref="UserGroupDto"/>.
        /// </summary>
        public UserGroupDto UserGroupDto { get; set; }

        /// <summary>
        /// Liefert oder setzt die Maklerprovision in %.
        /// </summary>
        public double Commission { get; set; }

        /// <summary>
        /// Liefert oder setzt einen Wert der angibt, ob die Nutzer ihr Password ändern dürfen.
        /// </summary>
        public bool CanUserChangePassword { get; set; }

    }
}