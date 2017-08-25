using System;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    public interface IUserDao : IGenericDao<User, int> {
        /// <summary>
        ///     Sucht einen Benutzer anhand seiner Email
        /// </summary>
        /// <param name="email">Die Email des Benutzers</param>
        /// <returns></returns>
        User FindByEmail(string email);

        User FindByPasswordResetCode(Guid bid);

        /// <summary>
        ///     Sucht einen Nutzer anhand des Benutzernamen aus.
        /// </summary>
        /// <param name="userName">Benutzername</param>
        /// <returns></returns>
        User FindByUserName(string userName);

        /// <summary>
        ///     Ruft Seitenweise die Nutzer der Plattform ab.
        ///     ///
        /// </summary>
        /// <param name="pageable">Informationen über aufzurufende Seite und Seitengröße</param>
        /// <param name="searchTerm">
        ///     Einschränkende Suchzeichenfolge für die Suche nach Nutzern.
        ///     Wenn NullOrEmpty, wird keine explizite Suche durchgeführt.
        /// </param>
        /// <returns></returns>
        IPage<User> FindUser(IPageable pageable, string searchTerm = null);

        /// <summary>
        ///     Gibt die Anzahl der freigeschalteten Makler aus.
        /// </summary>
        /// <returns></returns>
        int GetActiveUserCount();

        /// <summary>
        ///     Überprüft, ob ein Nutzer mit Vorgängen verbunden ist.
        /// </summary>
        /// <param name="user">Der Nutzer für den überprüft werden soll, ob er mit Vorgängen verbunden ist.</param>
        /// <returns>
        ///     true => wenn Vorgängen mit diesem Nutzer verknüpft sind
        ///     false => wenn keine Vorgänge mit dem Nutzer verknüpft sind
        /// </returns>
        bool IsUserReferencesWithIssues(User user);

        /// <summary>
        /// Liefert alle Nutzer welche die übergebenen Rollen haben.
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        IList<User> FindByRole(params string[] roles);
    }
}