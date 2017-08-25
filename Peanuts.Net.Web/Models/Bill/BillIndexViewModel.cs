using System.Collections.Generic;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Bill {

    /// <summary>
    /// ViewModel für die Ansicht mit einer Rechnungsübersicht.
    /// </summary>
    public class BillIndexViewModel {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unsettledCreditorBills">Liste mit unbezahlten Ausgangsrechnungen</param>
        /// <param name="settledCreditorBills">Seite mit bezahlten Ausgangsrechnungen</param>
        /// <param name="unsettledDebitorBills">Liste mit unbezahlten Eingangsrechnungen</param>
        /// <param name="settledDebitorBills">Seite mit bezahlten Eingangsrechnungen</param>
        public BillIndexViewModel(IList<CreditorBillViewModel> unsettledCreditorBills, IPage<CreditorBillViewModel> settledCreditorBills, IList<DebitorBillViewModel> unsettledDebitorBills, IPage<DebitorBillViewModel> settledDebitorBills) {
            Require.NotNull(settledCreditorBills, "settledCreditorBills");
            Require.NotNull(settledDebitorBills, "settledDebitorBills");
            Require.NotNull(unsettledCreditorBills, "unsettledCreditorBills");
            Require.NotNull(unsettledDebitorBills, "unsettledDebitorBills");
            
            UnsettledCreditorBills = unsettledCreditorBills;
            SettledCreditorBills = settledCreditorBills;
            UnsettledDebitorBills = unsettledDebitorBills;
            SettledDebitorBills = settledDebitorBills;
        }
        
        /// <summary>
        /// Ruft alle unbezahlten Ausgangsrechnungen ab.
        /// </summary>
        public IList<CreditorBillViewModel> UnsettledCreditorBills { get; }

        /// <summary>
        /// Ruft die aktuelle Seite der Ausgangsrechnungen ab.
        /// Ausgangsrechnungen sind die Rechnungen, bei denen der Nutzer als Gläubiger/Kreditor auftritt. Also als der, der das Geld bekommt.
        /// </summary>
        public IPage<CreditorBillViewModel> SettledCreditorBills { get; private set; }

        /// <summary>
        /// Ruft alle unbezahlten Eingangsrechnungen ab.
        /// </summary>
        public IList<DebitorBillViewModel> UnsettledDebitorBills { get; }

        /// <summary>
        /// Ruft die aktuelle Seite der Eingangsrechnungen ab.
        /// Eingangsrechnungen sind die Rechnungen, bei denen der Nutzer der Schuldner ist, also der, der das Geld bezahlen muss.
        /// </summary>
        public IPage<DebitorBillViewModel> SettledDebitorBills {
            get; private set;
        }


    }

    /// <summary>
    /// ViewModel für die Anzeige einer Eingangsrechnung für einen Nutzer.
    /// </summary>
    public class DebitorBillViewModel {
        public DebitorBillViewModel(Core.Domain.Accounting.Bill bill, User user) {
            Require.NotNull(bill, "bill");
            Require.NotNull(user, "user");

            Bill = bill;
            User = user;
        }

        /// <summary>
        /// Ruft die anzuzeigende Rechnung ab.
        /// </summary>
        public Core.Domain.Accounting.Bill Bill { get; private set; }

        /// <summary>
        /// Ruft den Nutzer ab, für den die Rechnung angezeigt werden soll.
        /// </summary>
        public User User { get; private set; }

        /// <summary>
        /// Ruft den anteiligen Betrag ab, den der Nutzer an der Rechnung hat.
        /// </summary>
        public double Portion {
            get {
                BillUserGroupDebitor billUserGroupDebitor = Bill.UserGroupDebitors.SingleOrDefault(deb => deb.UserGroupMembership.User.Equals(User));
                if (billUserGroupDebitor != null) {
                    return Bill.GetPartialAmountByPortion(billUserGroupDebitor.Portion);
                } else {
                    return 0;
                }
            }
        }
    }

    /// <summary>
    /// ViewModel für die Anzeige einer Ausgangsrechnung für einen Nutzer.
    /// </summary>
    public class CreditorBillViewModel {
        public CreditorBillViewModel(Core.Domain.Accounting.Bill bill, User user) {
            Require.NotNull(bill, "bill");
            Require.NotNull(user, "user");

            Bill = bill;
            User = user;
        }

        /// <summary>
        /// Ruft die anzuzeigende Rechnung ab.
        /// </summary>
        public Core.Domain.Accounting.Bill Bill {
            get; private set;
        }

        /// <summary>
        /// Ruft den Nutzer ab, für den die Rechnung angezeigt werden soll.
        /// </summary>
        public User User {
            get; private set;
        }

        
    }
}