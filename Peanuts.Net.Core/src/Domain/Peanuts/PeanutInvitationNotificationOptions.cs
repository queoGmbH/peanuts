using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts {
    /// <summary>
    /// Sammelt Benachrichtigungs-Informationen, für den Fall, dass ein Nutzer zu einem Peanut eingeladen wurde.
    /// </summary>
    public class PeanutInvitationNotificationOptions {
        public PeanutInvitationNotificationOptions(string peanutUrl, string attendPeanutUrl) {
            Require.NotNullOrWhiteSpace(peanutUrl, "peanutUrl");
            Require.NotNullOrWhiteSpace(attendPeanutUrl, "attendPeanutUrl");

            PeanutUrl = peanutUrl;
            AttendPeanutUrl = attendPeanutUrl;
        }

        /// <summary>
        /// Ruft die Url zum Peanut ab.
        /// </summary>
        public string PeanutUrl {
            get; private set;
        }


        /// <summary>
        /// Ruft die Url zur Seite ab, auf dem die Einladung zum Peanut bestätigt werden kann.
        /// </summary>
        public string AttendPeanutUrl {
            get; private set;
        }
    }
}