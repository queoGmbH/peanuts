using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts {
    /// <summary>
    /// Sammelt Benachrichtigungsoptionen, für den Fall, dass die <see cref="Peanut.Requirements">Anforderungen</see> eines Peanuts geändert wurden.
    /// </summary>
    public class PeanutUpdateRequirementsNotificationOptions {
        public PeanutUpdateRequirementsNotificationOptions(string peanutUrl) {
            Require.NotNullOrWhiteSpace(peanutUrl, "peanutUrl");

            PeanutUrl = peanutUrl;
        }

        /// <summary>
        /// Ruft die Url zum PEanut ab.
        /// </summary>
        public string PeanutUrl {
            get; private set;
        }
    }
}