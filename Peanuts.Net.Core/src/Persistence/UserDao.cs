using System;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;
using FluentNHibernate.Utils;
using NHibernate;
using NHibernate.Criterion;

using Spring.Data.NHibernate.Generic;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    public class UserDao : GenericDao<User, int>, IUserDao {
        /// <summary>
        ///     Sucht einen Nutzer anhand seiner Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User FindByEmail(string email) {
            HibernateDelegate<User> finder = delegate(ISession session) {
                ICriteria criteria = session.CreateCriteria(typeof(User));
                criteria.Add(Restrictions.Eq(Objects.GetPropertyName<User>(user => user.Email), email));

                User userByEmail = criteria.UniqueResult<User>();
                return userByEmail;
            };
            return HibernateTemplate.Execute(finder);
        }

        public User FindByPasswordResetCode(Guid guid) {
            HibernateDelegate<User> finder = delegate(ISession session) {
                ICriteria criteria = session.CreateCriteria(typeof(User));
                criteria.Add(Restrictions.Eq(Objects.GetPropertyName<User>(user => user.PasswordResetCode), guid));

                User userByPasswordResetCode = criteria.UniqueResult<User>();
                return userByPasswordResetCode;
            };
            return HibernateTemplate.Execute(finder);
        }

        /// <summary>
        ///     Liefert alle Nutzer welche die übergebenen Rollen haben.
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public IList<User> FindByRole(params string[] roles) {
            FindHibernateDelegate<User> finder = delegate(ISession session) {
                ICriteria criteria = session.CreateCriteria<User>();

                ICriteria rolesCriteria = criteria.CreateCriteria(Objects.GetPropertyName<User>(user => user.Roles));
                rolesCriteria.Add(Restrictions.In("elements", roles));

                return criteria.List<User>();
            };
            return HibernateTemplate.ExecuteFind(finder);
        }
        
        /// <summary>
        ///     Sucht einen Nutzer anhand des Benutzernamen aus.
        /// </summary>
        /// <param name="userName">Benutzername</param>
        /// <returns></returns>
        public User FindByUserName(string userName) {
            HibernateDelegate<User> finder = delegate(ISession session) {
                ICriteria criteria = session.CreateCriteria(typeof(User));
                criteria.Add(Restrictions.Eq(Objects.GetPropertyName<User>(user => user.UserName), userName));

                User userByUserName = criteria.UniqueResult<User>();
                return userByUserName;
            };
            return HibernateTemplate.Execute(finder);
        }

        /// <summary>
        ///     Ruft Seitenweise die Nutzer der Plattform ab.
        ///     ///
        /// </summary>
        /// <param name="pageable">Informationen über aufzurufende Seite und Seitengröße</param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public IPage<User> FindUser(IPageable pageable, string searchTerm) {
            Require.NotNull(pageable, "pageable");

            Action<ICriteria> criterionsDelegate = delegate(ICriteria criteria) {
                if (!string.IsNullOrEmpty(searchTerm)) {
                    /*Die Prüfung ob die Properties die Suchzeichenfolge enthalten per Oder verknüpfen*/
                    Disjunction orCriterias = new Disjunction();
                    orCriterias.Add(Restrictions.Like(Objects.GetPropertyName<User>(user => user.UserName), searchTerm, MatchMode.Anywhere));
                    orCriterias.Add(Restrictions.Like(Objects.GetPropertyName<User>(user => user.Email), searchTerm, MatchMode.Anywhere));
                    orCriterias.Add(Restrictions.Like(Objects.GetPropertyName<User>(user => user.FirstName), searchTerm, MatchMode.Anywhere));
                    orCriterias.Add(Restrictions.Like(Objects.GetPropertyName<User>(user => user.LastName), searchTerm, MatchMode.Anywhere));
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

        /// <summary>
        ///     Gibt die Anzahl der freigeschalteten Makler aus.
        /// </summary>
        public int GetActiveUserCount() {
            HibernateDelegate<int> finder = delegate(ISession session) {
                ICriteria criteria = session.CreateCriteria(typeof(User));
                criteria.Add(Restrictions.Eq(Objects.GetPropertyName<User>(user => user.IsEnabled), true));

                IList<User> activeUsers = criteria.List<User>();
                return activeUsers.Count;
            };
            return HibernateTemplate.Execute(finder);
        }

        /// <summary>
        ///     Sucht einen Nutzer anhand der BusinessId aus.
        /// </summary>
        /// <param name="businessId">BuisnessId</param>
        /// <returns></returns>
        public new User GetByBusinessId(Guid businessId) {
            HibernateDelegate<User> finder = delegate(ISession session) {
                ICriteria criteria = session.CreateCriteria(typeof(User));
                criteria.Add(Restrictions.Eq(Objects.GetPropertyName<User>(user => user.BusinessId), businessId));

                User userByBId = criteria.UniqueResult<User>();
                return userByBId;
            };
            return HibernateTemplate.Execute(finder);
        }

        /// <summary>
        ///     Überprüft, ob ein Nutzer mit Vorgängen verbunden ist.
        /// </summary>
        /// <param name="user">Der Nutzer für den überprüft werden soll, ob er mit Vorgängen verbunden ist.</param>
        /// <returns>
        ///     true => wenn Vorgängen mit diesem Nutzer verknüpft sind
        ///     false => wenn keine Vorgänge mit dem Nutzer verknüpft sind
        /// </returns>
        public bool IsUserReferencesWithIssues(User user) {
            Require.NotNull(user, "user");

            // TODO: An dieser Stelle wird die Überprüfung auf referenzierte Vorgänge implementiert, wenn die Vorgangskomponente implementiert ist.

            return false;
        }
    }
}