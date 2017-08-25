using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Manage
{
    /// <summary>
    /// ViewModel für die Anzeige des Nutzerprofils
    /// </summary>
    public class ShowMeViewModel
    {
        /// <summary>Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.</summary>
        public ShowMeViewModel(User user) {
            User = user;
        }

        public User User { get; set; }
    }
}
