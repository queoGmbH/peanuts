using System;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    public interface IPeanutDao : IGenericDao<Peanut, int> {
        /// <summary>
        /// Sucht nach Peanuts, an denen ein Nutzer in einem bestimmten Zeitraum teilgenommen hat.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="forUser"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="participationStates"></param>
        /// <returns></returns>
        IPage<PeanutParticipation> FindParticipationsOfUser(IPageable pageRequest, User forUser, DateTime @from, DateTime to, IList<PeanutParticipationState> participationStates = null);

        /// <summary>
        /// Sucht in einem bestimmten Zeitraum nach Peanuts in den übergeben Gruppen.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="userGroups"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        IPage<Peanut> FindPeanutsInGroups(IPageable pageRequest, IList<UserGroup> userGroups, DateTime? @from=null, DateTime? to=null);



        /// <summary>
        /// Sucht nach Peanuts, an denen der Nutzer teilnehmen kann, da das Peanut in seiner Gruppe erstellt wurde und er bisher nicht als Teilnehmer eingetragen ist bzw. seine Teilnahme nicht bestätigt hat.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        IPage<Peanut> FindAttendablePeanutsForUser(IPageable pageRequest, User user, DateTime @from, DateTime to);

        /// <summary>
        /// Sucht nach dem Peanut, aus dem die Rechnung erstellt wurde.
        /// Wurde die Rechnung unabhängig von einem Peanut erstellt, wird null geliefert.
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        Peanut FindFromBill(Bill bill);

        /// <summary>
        /// Sucht seitenweise nach abgerechneten Peanuts in einem bestimmten Zeitraum.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="userGroup">Die Gruppe, in welcher gesucht werden soll</param>
        /// <param name="from">Der früheste Zeitpunkt für einen zu berücksichtigenden Peanut oder null, wenn ab dem ersten Peanut gesucht werden soll.</param>
        /// <param name="to">Der späteste Zeitpunkt für einen zu berücksichtigenden Peanut oder null, wenn bis zum letzten Peanut gesucht werden soll.</param>
        /// <returns></returns>
        IPage<Peanut> FindBilledPeanutsInGroup(IPageable pageRequest, UserGroup userGroup, DateTime? from = null, DateTime? to = null);

  

    }
}