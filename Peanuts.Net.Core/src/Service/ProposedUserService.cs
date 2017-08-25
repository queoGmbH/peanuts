using System;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers;
using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

using Spring.Transaction.Interceptor;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    public class ProposedUserService : IProposedUserService {
        public IProposedUserDao ProposedUserDao { get; set; }

        /// <summary>
        ///     Erzeugt einen neuen beantragten Nutzer
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="proposedUserDataDto"></param>
        /// <param name="proposedUserContactDto"></param>
        /// <param name="entityCreatedDto"></param>
        /// <returns></returns>
        [Transaction]
        public ProposedUser Create(string userName, ProposedUserDataDto proposedUserDataDto, ProposedUserContactDto proposedUserContactDto,
            EntityCreatedDto entityCreatedDto) {
            Require.NotNullOrWhiteSpace(userName, nameof(userName));
            Require.NotNull(proposedUserDataDto, nameof(proposedUserDataDto));
            Require.NotNull(proposedUserContactDto, nameof(proposedUserContactDto));
            Require.NotNull(entityCreatedDto, nameof(entityCreatedDto));

            ProposedUser user = new ProposedUser(userName, proposedUserDataDto, proposedUserContactDto, entityCreatedDto);

            return ProposedUserDao.Save(user);
        }

        /// <summary>
        ///     Löscht einen beantragten Nutzer.
        /// </summary>
        /// <param name="user"></param>
        [Transaction]
        public void Delete(ProposedUser user) {
            Require.NotNull(user, nameof(user));
            ProposedUserDao.Delete(user);
        }

        /// <summary>
        ///     Sucht nach beantragtem Nutzer anhand von einem Suchbegriff und/oder Gruppen.
        /// </summary>
        /// <param name="pageable"></param>
        /// <param name="searchTerm"></param>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        public IPage<ProposedUser> FindUser(IPageable pageable, string searchTerm = null,
            UserGroup userGroup = null) {
            Require.NotNull(pageable, nameof(pageable));
            return ProposedUserDao.FindProposedUser(pageable, searchTerm);
        }

        /// <summary>
        ///     Gibt alle beantragten Nutzer zurück.
        /// </summary>
        /// <returns></returns>
        public IList<ProposedUser> GetAll() {
            return ProposedUserDao.GetAll();
        }

        /// <summary>
        ///     Sucht einen beantragten Nutzer anhand der BusinessId.
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public ProposedUser GetByBusinessId(Guid businessId) {
            return ProposedUserDao.GetByBusinessId(businessId);
        }

        /// <summary>
        ///     Aktualisiert die Daten des beantragten Nutzers
        /// </summary>
        /// <param name="user"></param>
        /// <param name="username"></param>
        /// <param name="proposedUserDataDto"></param>
        /// <param name="proposedUserContactDto"></param>
        /// <param name="entityChangedDto"></param>
        [Transaction]
        public void Update(ProposedUser user, string username, ProposedUserDataDto proposedUserDataDto, ProposedUserContactDto proposedUserContactDto,
            EntityChangedDto entityChangedDto) {
            Require.NotNull(user, "user");
            Require.NotNullOrWhiteSpace(username, "username");
            Require.NotNull(proposedUserDataDto, "proposedUserDataDto");
            Require.NotNull(proposedUserContactDto, "proposedUserContactDto");
            Require.NotNull(entityChangedDto, "entityChangedDto");

            user.Update(username, proposedUserDataDto, proposedUserContactDto, entityChangedDto);
        }
    }
}