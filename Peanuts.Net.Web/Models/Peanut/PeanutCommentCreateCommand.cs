using System.ComponentModel.DataAnnotations;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut {
    /// <summary>
    /// Command zum Erstellen eines neuen Kommentars an einem Peanut.
    /// </summary>
    public class PeanutCommentCreateCommand {

        public PeanutCommentCreateCommand() {
            SendUpdateNotification = false;
        }

        /// <summary>
        /// Ruft ab oder legt fest, ob die Teilnehmer des Peanuts über die Änderung benachrichtigt werden sollen.
        /// </summary>
        public bool SendUpdateNotification {
            get; set;
        }

        /// <summary>
        /// Ruft die Nachricht bzw. den Kommentar ab, der am Peanuts hinterlegt wird oder - wenn <see cref="SendUpdateNotification"/> aktiv ist - per E-Mail an die Teilnehmer gesendet wird oder legt diesen fest.
        /// </summary>
        [StringLength(1000)]
        public string Comment {
            get; set;
        }

    }
}