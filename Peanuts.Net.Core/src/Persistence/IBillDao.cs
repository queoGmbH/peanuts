using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    /// <summary>
    /// Beschreibt einen Dao, der Methoden zur Persistierung von Rechnungen bereitstellt.
    /// </summary>
    public interface IBillDao : IGenericDao<Bill, int> {

        /// <summary>
        /// Ruft die Eingangsrechnungen eines Nutzers ab.
        /// Also die, bei denen der Nutzer Geld schuldet.
        /// </summary>
        /// <param name="pageRequest">Seiteninformationen für die Abfrage</param>
        /// <param name="user">Für welchen Nutzer sollen die Rechnungen abgerufen werden.</param>
        /// <param name="isSettled">
        ///     Die Rechnungen welchen Status sollen abgerufen werden?
        ///     true => Es werden nur <see cref="Bill.IsSettled">abgerechnete Rechnungen</see> abgerufen.
        ///     false => Es werden nur <see cref="Bill.IsSettled">NICHT abgerechnete Rechnungen</see> abgerufen.
        ///     null => Ob die Rechnung <see cref="Bill.IsSettled">abgerechnet ist</see> spielt keine Rolle.
        /// </param>
        /// <returns></returns>
        IPage<Bill> FindBillsWhereUserIsDebitor(IPageable pageRequest, User user, bool? isSettled);

        /// <summary>
        /// Ruft die Ausgangsrechnungen eines Nutzers ab.
        /// Also die, bei denen der Nutzer Geld erhält.
        /// </summary>
        /// <param name="pageRequest">Seiteninformationen für die Abfrage</param>
        /// <param name="user">Für welchen Nutzer sollen die Rechnungen abgerufen werden.</param>
        /// <param name="isSettled">
        ///     Die Rechnungen welchen Status sollen abgerufen werden?
        ///     true => Es werden nur <see cref="Bill.IsSettled">abgerechnete Rechnungen</see> abgerufen.
        ///     false => Es werden nur <see cref="Bill.IsSettled">NICHT abgerechnete Rechnungen</see> abgerufen.
        ///     null => Ob die Rechnung <see cref="Bill.IsSettled">abgerechnet ist</see> spielt keine Rolle.
        /// </param>
        /// <returns></returns>
        IPage<Bill> FindCreditorBillsForUser(IPageable pageRequest, User user, bool? isSettled);

        /// <summary>
        /// Ruft alle offenen Rechnungen für einen Nutzer ab, egal ob Eingans- oder Ausgangsrechnungen.
        /// </summary>
        /// <param name="pageRequest">Seiteninformationen</param>
        /// <param name="user">Die Rechnungen welches Nutzers sollen abgerufen werden?</param>
        /// <returns></returns>
        IPage<Bill> FindPendingBillsByUser(IPageable pageRequest, User user);

        /// <summary>
        /// Sucht nach nicht abgerechneten Rechnungen, bei denen der Nutzer der Kreditor ist (der Gläubiger) und bei denen mindestens 1 Nutzer die Zahlung der Rechnung abgelehnt hat.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        IPage<Bill> FindRefusedBillsWhereUserIsCreditor(PageRequest pageRequest, User user);

        /// <summary>
        /// Sucht nach allen offenen Rechnungen
        /// </summary>
        /// <returns></returns>
        IList<Bill> FindUnsettledBills();
    }
}