using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.ProposedUsers;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

using NHibernate;
using NHibernate.Criterion;

using Spring.Data.NHibernate.Generic;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    public class ProposedUserDao : GenericDao<ProposedUser, int>, IProposedUserDao {
        /// <summary>
        ///     Sucht einen beantragten Nutzer anhand seiner Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ProposedUser FindByEmail(string email) {
            HibernateDelegate<ProposedUser> finder = delegate(ISession session) {
                ICriteria criteria = session.CreateCriteria(typeof(ProposedUser));
                criteria.Add(Restrictions.Eq(Objects.GetPropertyName<ProposedUser>(user => user.Email), email));

                ProposedUser userByEmail = criteria.UniqueResult<ProposedUser>();
                return userByEmail;
            };
            return HibernateTemplate.Execute(finder);
        }

        /// <summary>
        ///     Sucht einen beantragten Nutzer anhand des Benutzernamen aus.
        /// </summary>
        /// <param name="userName">Benutzername</param>
        /// <returns></returns>
        public ProposedUser FindByUserName(string userName) {
            HibernateDelegate<ProposedUser> finder = delegate(ISession session) {
                ICriteria criteria = session.CreateCriteria(typeof(ProposedUser));
                criteria.Add(Restrictions.Eq(Objects.GetPropertyName<ProposedUser>(user => user.UserName), userName));

                ProposedUser userByUserName = criteria.UniqueResult<ProposedUser>();
                return userByUserName;
            };
            return HibernateTemplate.Execute(finder);
        }

        /// <summary>
        ///     Ruft Seitenweise die beantragten Nutzer der Plattform ab.
        /// </summary>
        /// <param name="pageable">Informationen über aufzurufende Seite und Seitengröße</param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public IPage<ProposedUser> FindProposedUser(IPageable pageable, string searchTerm) {
            Require.NotNull(pageable, "pageable");

            Action<ICriteria> criterionsDelegate = delegate(ICriteria criteria) {
                if (!string.IsNullOrEmpty(searchTerm)) {
                    /*Die Prüfung ob die Properties die Suchzeichenfolge enthalten per Oder verknüpfen*/
                    Disjunction orCriterias = new Disjunction();
                    orCriterias.Add(Restrictions.Like(Objects.GetPropertyName<ProposedUser>(user => user.UserName),
                        searchTerm,
                        MatchMode.Anywhere));
                    orCriterias.Add(Restrictions.Like(Objects.GetPropertyName<ProposedUser>(user => user.Email),
                        searchTerm,
                        MatchMode.Anywhere));
                    orCriterias.Add(Restrictions.Like(Objects.GetPropertyName<ProposedUser>(user => user.FirstName),
                        searchTerm,
                        MatchMode.Anywhere));
                    orCriterias.Add(Restrictions.Like(Objects.GetPropertyName<ProposedUser>(user => user.LastName),
                        searchTerm,
                        MatchMode.Anywhere));
                    criteria.Add(orCriterias);
                }
            };

            /*Sortierung hinzufügen*/
            Action<ICriteria> ordersDelegate = delegate(ICriteria criteria) {
                criteria.AddOrder(Order.Asc("LastName"));
                criteria.AddOrder(Order.Asc("FirstName"));
            };

            return Find(pageable, criterionsDelegate, ordersDelegate);
        }
    }
}