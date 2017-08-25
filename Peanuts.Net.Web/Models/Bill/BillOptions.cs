using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Bill {
    /// <summary>
    ///     Sammelt Optionen die ein Nutzer für eine Rechnung hat.
    /// </summary>
    public class BillOptions {
        public BillOptions(Core.Domain.Accounting.Bill bill, User user) {
            Require.NotNull(bill, "bill");
            Require.NotNull(user, "user");

            CanAccept =
                    /*Die Rechnung ist noch nicht gebucht/abgerechnet*/
                    !bill.IsSettled &&
                    /*Der Nutzer hat die Rechnung bisher nicht akzeptiert*/
                    bill.UserGroupDebitors.Any(
                        debitor => debitor.UserGroupMembership.User.Equals(user) && debitor.BillAcceptState != BillAcceptState.Accepted);

            CanSettle =
                    /*Die Rechnung ist noch nicht gebucht/abgerechnet*/
                    !bill.IsSettled &&
                    /*Der Nutzer ist der Gläubiger der Rechnung*/
                    bill.Creditor.User.Equals(user) &&
                    /*Alle Nutzer müssen zugestimmt haben*/
                    (!bill.UserGroupDebitors.Any() || bill.UserGroupDebitors.All(deb => deb.BillAcceptState == BillAcceptState.Accepted));

            CanRefuse =
                    /*Die Rechnung ist noch nicht gebucht/abgerechnet*/
                    !bill.IsSettled &&
                    /*Der Nutzer hat die Rechnung bisher nicht abgelehnt*/
                    bill.UserGroupDebitors.Any(
                        debitor => debitor.UserGroupMembership.User.Equals(user) && debitor.BillAcceptState != BillAcceptState.Refused);

            CanDelete =
                    /*Die Rechnung ist noch nicht gebucht/abgerechnet*/
                    !bill.IsSettled &&
                    /*Der Nutzer hat die Rechnung bisher nicht akzeptiert*/
                    bill.Creditor.User.Equals(user);
        }

        /// <summary>
        ///     Kann der Nutzer die Rechnung Akzeptieren?
        /// </summary>
        public bool CanAccept { get; private set; }

        /// <summary>
        ///     Kann der Nutzer die Rechnung löschen?
        /// </summary>
        public bool CanDelete { get; private set; }

        /// <summary>
        ///     Kann der Nutzer die Rechnung Ablehnen?
        /// </summary>
        public bool CanRefuse { get; private set; }

        /// <summary>
        ///     Ruft ab, ob die Rechnung abgerechnet werden kann.
        /// </summary>
        public bool CanSettle { get; private set; }
    }
}