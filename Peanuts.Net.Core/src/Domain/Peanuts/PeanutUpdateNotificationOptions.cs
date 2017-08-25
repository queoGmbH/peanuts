using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts {
    /// <summary>
    ///     Sammelt Benachrichtigungsoptionen, für den Fall, dass ein Peanut geändert wurde.
    /// </summary>
    public class PeanutUpdateNotificationOptions {
        public PeanutUpdateNotificationOptions(bool sendNotification, string peanutUrl) {
            Require.NotNullOrWhiteSpace(peanutUrl, "peanutUrl");

            PeanutUrl = peanutUrl;
            SendNotification = sendNotification;
        }

        /// <summary>
        ///     Ruft die Url zum PEanut ab.
        /// </summary>
        public string PeanutUrl { get; private set; }

        /// <summary>
        ///     Ruft ab, ob eine Benachrichtigung an die Teilnehmer des Peanuts gesendet werden soll.
        /// </summary>
        public bool SendNotification { get; private set; }
    }
}