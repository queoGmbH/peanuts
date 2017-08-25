using System;
using System.Collections.Generic;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

using NHibernate;

using Spring.Data.NHibernate.Generic;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    public class PeanutDao : GenericDao<Peanut, int>, IPeanutDao {
        public IPage<Peanut> FindAttendablePeanutsForUser(IPageable pageRequest, User user, DateTime from, DateTime to) {
            Require.NotNull(user, "user");
            Require.NotNull(pageRequest, "pageRequest");

            HibernateDelegate<IPage<Peanut>> finder = delegate(ISession session) {
                IQueryOver<Peanut, Peanut> queryOver = session.QueryOver<Peanut>();

                /*Die aktiven Gruppen des Nutzers*/
                IList<UserGroup> userGroups =
                        session.QueryOver<UserGroupMembership>()
                                .Where(mem => mem.User == user)
                                .And(
                                    mem =>
                                        mem.MembershipType == UserGroupMembershipType.Administrator
                                        || mem.MembershipType == UserGroupMembershipType.Member).Select(mem => mem.UserGroup)
                                .List<UserGroup>();

                IList<Peanut> attendedPeanuts =
                        session.QueryOver<PeanutParticipation>()
                                .Where(part => part.ParticipationState == PeanutParticipationState.Confirmed)
                                .JoinQueryOver(part => part.UserGroupMembership).Where(mem => mem.User == user)
                                .Select(part => part.Peanut)
                                .List<Peanut>();

                queryOver
                        /*An bereits fixierten Tickets, kann sich kein neuer Teilnehmer anmelden.*/
                        .Where(peanut => peanut.PeanutState == PeanutState.Scheduling)
                        /*Am oder nach dem ab Datum*/
                        .And(peanut => peanut.Day >= from)
                        /*Nicht nach dem To Datum*/
                        .AndNot(peanut => peanut.Day > to)
                        /*Nur Peanuts aus den Gruppen des Nutzers*/
                        .WhereRestrictionOn(peanut => peanut.UserGroup).IsIn(userGroups.ToList())
                        /*Keine Peanuts an denen der Nutzer teilnimmt*/
                        .WhereRestrictionOn(peanut => peanut.Id).Not.IsIn(attendedPeanuts.Select(p => p.Id).ToList());

                return FindPage(queryOver, pageRequest);
            };
            return HibernateTemplate.Execute(finder);
        }

        public IPage<Peanut> FindBilledPeanutsInGroup(IPageable pageRequest, UserGroup userGroup, DateTime? from = null, DateTime? to = null) {
            Require.NotNull(userGroup, "userGroup");
            Require.NotNull(pageRequest, "pageRequest");

            HibernateDelegate<IPage<Peanut>> finder = delegate(ISession session) {
                IQueryOver<Peanut, Peanut> queryOver = session.QueryOver<Peanut>();

                queryOver.JoinQueryOver<Bill>(peanut => peanut.Bills).Where(peanutBill => peanutBill.IsSettled && peanutBill.UserGroup == userGroup);
                queryOver.Where(peanut => peanut.UserGroup == userGroup);

                if (from.HasValue) {
                    queryOver.And(p => p.Day > from.Value);
                }

                if (to.HasValue) {
                    queryOver.And(p => p.Day <= to.Value);
                }

                return FindPage(queryOver, pageRequest);
            };
            return HibernateTemplate.Execute(finder);
        }

        public Peanut FindFromBill(Bill bill) {
            Require.NotNull(bill, "bill");

            HibernateDelegate<Peanut> finder = delegate(ISession session) {
                IQueryOver<Peanut, Peanut> queryOver = session.QueryOver<Peanut>();
                queryOver.JoinQueryOver<Bill>(peanut => peanut.Bills).Where(peanutBill => peanutBill.Id == bill.Id);
                return queryOver.SingleOrDefault();
            };
            return HibernateTemplate.Execute(finder);
        }

        public IPage<PeanutParticipation> FindParticipationsOfUser(IPageable pageRequest, User forUser, DateTime from, DateTime to,
            IList<PeanutParticipationState> participationStates = null) {
            HibernateDelegate<IPage<PeanutParticipation>> finder = delegate(ISession session) {
                IQueryOver<PeanutParticipation, PeanutParticipation> queryOver = session.QueryOver<PeanutParticipation>();

                IQueryOver<PeanutParticipation, UserGroupMembership> joinUserGroupMembership =
                        queryOver.JoinQueryOver(participation => participation.UserGroupMembership);
                /*Nur für den Nutzer*/
                joinUserGroupMembership.Where(userGroupMembership => userGroupMembership.User == forUser);

                IQueryOver<PeanutParticipation, Peanut> joinPeanut = queryOver.JoinQueryOver(participation => participation.Peanut);
                joinPeanut
                        /*Am oder nach dem ab Datum*/
                        .And(peanut => peanut.Day >= from)
                        .And(peanut => peanut.PeanutState != PeanutState.Canceled)
                        /*Nicht nach dem To Datum*/
                        .AndNot(peanut => peanut.Day > to);

                if (participationStates != null && participationStates.Any()) {
                    /*Nur wenn die Teilnahme einen bestimmten Status hat.*/
                    queryOver.AndRestrictionOn(part => part.ParticipationState).IsIn(participationStates.ToArray());
                }

                return FindPage(queryOver, pageRequest);
            };
            return HibernateTemplate.Execute(finder);
        }

        public IPage<Peanut> FindPeanutsInGroups(IPageable pageRequest, IList<UserGroup> userGroups, DateTime? from, DateTime? to) {
            Require.NotNull(userGroups, "userGroups");
            Require.NotNull(pageRequest, "pageRequest");

            HibernateDelegate<IPage<Peanut>> finder = delegate(ISession session) {
                IQueryOver<Peanut, Peanut> queryOver = session.QueryOver<Peanut>();

                queryOver.WhereRestrictionOn(p => p.UserGroup).IsIn(userGroups.ToList());
                if (from != null) {
                    /*Am oder nach dem ab Datum*/
                    queryOver.And(peanut => peanut.Day >= from);
                }
                if (to != null) {
                    /*Nicht nach dem To Datum*/
                    queryOver.AndNot(peanut => peanut.Day > to);
                }

                return FindPage(queryOver, pageRequest);
            };
            return HibernateTemplate.Execute(finder);
        }
    }
}