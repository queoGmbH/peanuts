using System;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers;
using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    public interface IProposedUserService {
        /// <summary>
        ///     Erzeugt einen neuen beantragten Nutzer
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="proposedUserDataDto"></param>
        /// <param name="proposedUserContactDto"></param>
        /// <param name="entityCreatedDto"></param>
        /// <returns></returns>
        ProposedUser Create(string userName, ProposedUserDataDto proposedUserDataDto, ProposedUserContactDto proposedUserContactDto,
            EntityCreatedDto entityCreatedDto);

        /// <summary>
        ///     Löscht einen beantragten Nutzer.
        /// </summary>
        /// <param name="user"></param>
        void Delete(ProposedUser user);

        /// <summary>
        ///     Sucht nach beantragtem Nutzer anhand von einem Suchbegriff und/oder Gruppen.
        /// </summary>
        /// <param name="pageable"></param>
        /// <param name="searchTerm"></param>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        IPage<ProposedUser> FindUser(IPageable pageable, string searchTerm = null, UserGroup userGroup = null);

        /// <summary>
        ///     Gibt alle beantragten Nutzer zurück.
        /// </summary>
        /// <returns></returns>
        IList<ProposedUser> GetAll();

        /// <summary>
        ///     Sucht einen beantragten Nutzer anhand der BusinessId.
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        ProposedUser GetByBusinessId(Guid businessId);

        /// <summary>
        ///     Aktualisiert die Daten des beantragten Nutzers
        /// </summary>
        /// <param name="user"></param>
        /// <param name="username"></param>
        /// <param name="proposedUserDataDto"></param>
        /// <param name="proposedUserContactDto"></param>
        /// <param name="entityChangedDto"></param>
        void Update(ProposedUser user, string username, ProposedUserDataDto proposedUserDataDto, ProposedUserContactDto proposedUserContactDto,
            EntityChangedDto entityChangedDto);
    }
}