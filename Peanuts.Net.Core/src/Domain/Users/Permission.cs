using System.ComponentModel;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Users {
    /// <summary>
    ///     Mögliche Rechte der Applikation
    /// </summary>
    public enum Permission {
        [Description("Anzeigen eines Benutzers")]
        User_Show = 1,

        [Description("Bearbeiten eines Benutzers")]
        User_Edit = 2,

        [Description("Erstellen eines Benutzers")]
        User_Create = 3,

        [Description("Löschen eines Benutzers")]
        User_Delete = 4
    }
}