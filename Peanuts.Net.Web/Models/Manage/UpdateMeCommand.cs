using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Manage {
    public class UpdateMeCommand {
        /// <summary>Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.</summary>
        public UpdateMeCommand(User user) {
            Require.NotNull(user, "user");

            UserContactDto = user.GetUserContactDto();
            UserDataDto = user.GetUserDataDto();
            UserPaymentDto = user.GetUserPaymentDto();
            UserNotificationOptionsDto = user.GetNotificationOptions();
        }

        /// <summary>Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.</summary>
        public UpdateMeCommand() {
            UserContactDto = new UserContactDto();
            UserDataDto = new UserDataDto();
            UserPaymentDto = new UserPaymentDto();
            UserNotificationOptionsDto = new UserNotificationOptionsDto();
        }

        public UserContactDto UserContactDto { get; set; }

        public UserDataDto UserDataDto { get; set; }

        public UserPaymentDto UserPaymentDto { get; set; }

        public UserNotificationOptionsDto UserNotificationOptionsDto {
            get; set;
        }
    }
}