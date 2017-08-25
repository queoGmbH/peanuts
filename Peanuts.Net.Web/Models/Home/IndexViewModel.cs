using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Home {
    public class IndexViewModel {
        public IndexViewModel(IPage<PeanutParticipation> todaysPeanuts, IPage<UserGroupMembership> memberships, IList<Core.Domain.Accounting.Bill> pendingBills, IPage<Core.Domain.Accounting.Bill> declinedBills, IPage<Core.Domain.Accounting.Payment> pendingPayments, IPage<Core.Domain.Accounting.Payment> declinedPayments, string userName) {
            Require.NotNull(memberships, "memberships");
            Require.NotNull(pendingPayments, "pendingPayments");
            Require.NotNull(todaysPeanuts, "todaysPeanuts");
            Require.NotNull(pendingBills, "pendingBills");
            Require.NotNull(declinedBills, "declinedBills");
            Require.NotNull(declinedPayments, "declinedPayments");

            TodaysPeanuts = todaysPeanuts;
            Memberships = memberships;
            PendingBills = pendingBills;
            PendingPayments = pendingPayments;
            DeclinedPayments = declinedPayments;
            DeclinedBills = declinedBills;
            UserName = userName;
        }

        public IPage<Core.Domain.Accounting.Bill> DeclinedBills { get; set; }

        /// <summary>
        /// Ruft die offenen, abgelehnten Bezahlungen ab.
        /// </summary>
        public IPage<Core.Domain.Accounting.Payment> DeclinedPayments { get; set; }

        /// <summary>
        ///     Ruft die Mitgliedschaften des Nutzers ab.
        /// </summary>
        public IPage<UserGroupMembership> Memberships { get; private set; }

        /// <summary>
        ///     Ruft die aktuell offenen Rechnungen ab, egal ob Eingang oder Ausgang.
        /// </summary>
        public IList<Core.Domain.Accounting.Bill> PendingBills { get; private set; }

        /// <summary>
        ///     Ruft die aktuell offenen Zahlungen ab, egal ob Eingang oder Ausgang.
        /// </summary>
        public IPage<Core.Domain.Accounting.Payment> PendingPayments { get; private set; }

        /// <summary>
        ///     Ruft die heutigen Peanuts ab.
        /// </summary>
        public IPage<PeanutParticipation> TodaysPeanuts { get; private set; }

        /// <summary>
        /// Liefert den Namen des Benutzers
        /// </summary>
        public string UserName { get; private set; }
    }
}