using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Manage
{

    /// <summary>
    /// ViewModel für die Ansicht zur Bearbeitung des eigenen Profils.
    /// </summary>
    public class UpdateMeViewModel
    {
        /// <summary>Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.</summary>
        public UpdateMeViewModel(User user, UpdateMeCommand updateMeCommand) {
            User = user;
            UpdateMeCommand = updateMeCommand;
        }

        /// <summary>Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.</summary>
        public UpdateMeViewModel(User user) : this(user, new UpdateMeCommand(user)) {
        }

        /// <summary>
        /// Ruft den zu ändernden Nutzer ab.
        /// </summary>
        public User User { get; private set; }

        /// <summary>
        /// Ruft das Command zum Ändern des (angemeldeten) Nutzers ab.
        /// </summary>
        public UpdateMeCommand UpdateMeCommand { get; private set; }
    }
}