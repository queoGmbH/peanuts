using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    public interface IProposedUserDao : IGenericDao<ProposedUser, int> {
        /// <summary>
        ///     Sucht einen Benutzer anhand seiner Email
        /// </summary>
        /// <param name="email">Die Email des Benutzers</param>
        /// <returns></returns>
        ProposedUser FindByEmail(string email);

        /// <summary>
        ///     Sucht einen beantragten Nutzer anhand des Benutzernamen aus.
        /// </summary>
        /// <param name="userName">Benutzername</param>
        /// <returns></returns>
        ProposedUser FindByUserName(string userName);

        /// <summary>
        ///     Ruft Seitenweise die beantragten Nutzer der Plattform ab.
        /// </summary>
        /// <param name="pageable">Informationen über aufzurufende Seite und Seitengröße</param>
        /// <param name="searchTerm">
        ///     Einschränkende Suchzeichenfolge für die Suche nach Nutzern.
        ///     Wenn NullOrEmpty, wird keine explizite Suche durchgeführt.
        /// </param>
        /// <returns></returns>
        IPage<ProposedUser> FindProposedUser(IPageable pageable, string searchTerm = null);
    }
}