using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

using NHibernate;
using NHibernate.Criterion;

using Spring.Data.NHibernate.Generic;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    /// <summary>
    ///     Dao der Methoden für die Persistierung von Rechnungen zur Verfügung stellt.
    /// </summary>
    public class BillDao : GenericDao<Bill, int>, IBillDao {
        public IPage<Bill> FindCreditorBillsForUser(IPageable pageRequest, User user, bool? isSettled) {
            Require.NotNull(pageRequest, "pageRequest");
            Require.NotNull(user, "user");

            HibernateDelegate<IPage<Bill>> finder = delegate(ISession session) {
                /*Mitgliedschaften des Nutzers laden, da die Rechnungen gegen die Mitgliedschaft gehen*/
                QueryOver<UserGroupMembership, UserGroupMembership> userGroupMembershipSubQuery =
                        QueryOver.Of<UserGroupMembership>().Where(mem => mem.User == user).Select(mem => mem.Id);

                IQueryOver<Bill, Bill> queryOver = session.QueryOver<Bill>();
                queryOver.WithSubquery.WhereProperty(bill => bill.Creditor).In(userGroupMembershipSubQuery);

                if (isSettled.HasValue) {
                    queryOver.And(bill => bill.IsSettled == isSettled.Value);
                }

                queryOver = queryOver.OrderBy(bill => bill.CreatedAt).Desc;

                return FindPage(queryOver, pageRequest);
            };
            return HibernateTemplate.Execute(finder);
        }

        public IPage<Bill> FindDebitorBillsForUser(IPageable pageRequest, User user, bool? isSettled) {
            HibernateDelegate<IPage<Bill>> finder = delegate(ISession session) {
                Require.NotNull(pageRequest, "pageRequest");
                Require.NotNull(user, "user");

                /*Mitgliedschaften des Nutzers laden, da die Rechnungen gegen die Mitgliedschaft gehen*/
                QueryOver<UserGroupMembership, UserGroupMembership> userGroupMembershipSubQuery =
                        QueryOver.Of<UserGroupMembership>().Where(mem => mem.User == user).Select(mem => mem.Id);

                /*Subquery für die Rechnungen, bei denen der Nutzer Schuldner ist*/
                IQueryOver<Bill, Bill> queryOver = session.QueryOver<Bill>();
                queryOver.Right.JoinQueryOver<BillUserGroupDebitor>(bill => bill.UserGroupDebitors)
                        .WithSubquery.WhereProperty(deb => deb.UserGroupMembership)
                        .In(userGroupMembershipSubQuery);

                if (isSettled.HasValue) {
                    queryOver.And(bill => bill.IsSettled == isSettled.Value);
                }

                queryOver = queryOver.OrderBy(bill => bill.CreatedAt).Desc;

                return FindPage(queryOver, pageRequest);
            };
            return HibernateTemplate.Execute(finder);
        }

        public IPage<Bill> FindDeclinedCreditorBillsByUser(PageRequest pageRequest, User user) {
            Require.NotNull(pageRequest, "pageRequest");
            Require.NotNull(user, "user");

            HibernateDelegate<IPage<Bill>> finder = delegate(ISession session) {
                /*Mitgliedschaften des Nutzers laden, da die Rechnungen gegen die Mitgliedschaft gehen*/
                QueryOver<UserGroupMembership, UserGroupMembership> userGroupMembershipSubQuery =
                        QueryOver.Of<UserGroupMembership>().Where(mem => mem.User == user).Select(mem => mem.Id);

                QueryOver<BillUserGroupDebitor, BillUserGroupDebitor> debitorSubQuery =
                        QueryOver.Of<BillUserGroupDebitor>().Where(deb => deb.BillAcceptState == BillAcceptState.Refused).Select(deb => deb.Id);
                IList<BillUserGroupDebitor> userGroupDebitorAlias = null;
                QueryOver<Bill, Bill> debitorBillsSubquery =
                        QueryOver.Of<Bill>()
                                .JoinAlias(bill => bill.UserGroupDebitors, () => userGroupDebitorAlias)
                                .WithSubquery.WhereExists(debitorSubQuery)
                                .Select(bill => bill.Id);

                IQueryOver<Bill, Bill> queryOver = session.QueryOver<Bill>();
                queryOver.Where(bill => bill.IsSettled == false);
                queryOver.WithSubquery.WhereProperty(bill => bill.Creditor).In(userGroupMembershipSubQuery);
                queryOver.WithSubquery.WhereProperty(x => x.Id).In(debitorBillsSubquery);

                return FindPage(queryOver, pageRequest);
            };
            return HibernateTemplate.Execute(finder);
        }

        public IPage<Bill> FindPendingBillsByUser(IPageable pageRequest, User user) {
            Require.NotNull(pageRequest, "pageRequest");
            Require.NotNull(user, "user");

            HibernateDelegate<IPage<Bill>> finder = delegate(ISession session) {
                QueryOver<UserGroupMembership, UserGroupMembership> userGroupMembershipSubQuery =
                        QueryOver.Of<UserGroupMembership>().Where(mem => mem.User == user).Select(mem => mem.Id);

                /*Subquery für die Rechnungen, bei denen der Nutzer Schuldner ist*/
                QueryOver<BillUserGroupDebitor, BillUserGroupDebitor> debitorSubQuery =
                        QueryOver.Of<BillUserGroupDebitor>().WithSubquery.WhereProperty(deb => deb.UserGroupMembership).In(userGroupMembershipSubQuery).Select(deb => deb.Id);
                IList<BillUserGroupDebitor> userGroupAlias = null;
                QueryOver<Bill, Bill> debitorBillsSubquery =
                        QueryOver.Of<Bill>()
                                .JoinAlias(bill => bill.UserGroupDebitors, () => userGroupAlias)
                                .WithSubquery.WhereProperty(bill => bill.Creditor).In(debitorSubQuery)
                                .Select(bill => bill.Id);

                /*Subquery für die Rechnungen, bei denen der Nutzer Gläubiger ist*/
                QueryOver<Bill, Bill> creditorSubQuery =
                        QueryOver.Of<Bill>().WithSubquery.WhereProperty(bill => bill.Creditor).In(userGroupMembershipSubQuery).Select(bill => bill.Id);

                /*Nur Rechnungen, die die Subqueries erfüllen*/
                IQueryOver<Bill, Bill> queryOver = session.QueryOver<Bill>();
                queryOver
                        .Where(Restrictions.Disjunction()
                                .Add(Subqueries.WhereProperty<Bill>(x => x.Id).In(debitorBillsSubquery))
                                .Add(Subqueries.WhereProperty<Bill>(x => x.Id).In(creditorSubQuery)));

                /*Rechnung muss offen sein.*/
                queryOver.And(bill => bill.IsSettled == false);

                return FindPage(queryOver, pageRequest);
            };
            return HibernateTemplate.Execute(finder);
        }

        /// <summary>
        ///     Sucht nach allen offenen Rechnungen
        /// </summary>
        /// <returns></returns>
        public IList<Bill> FindUnsettledBills() {
            FindHibernateDelegate<Bill> finder = delegate(ISession session) {
                ICriteria criteria = session.CreateCriteria(typeof(Bill));
                criteria.Add(Restrictions.Eq("IsSettled", false));

                return criteria.List<Bill>();
            };
            return HibernateTemplate.ExecuteFind(finder);
        }
    }
}