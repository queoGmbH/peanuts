using System;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Bill {
    /// <summary>
    ///     Command zur Erstellung einer Rechnung.
    /// </summary>
    public class BillCreateCommand {
        /// <summary>
        ///     Erzeugt eine neue Instanz des <see cref="BillCreateCommand" />
        /// </summary>
        public BillCreateCommand() {
            BillDto = new BillDto();
            GuestDebitors = new Dictionary<string, BillGuestDebitorDto>();
            UserGroupDebitors = new Dictionary<string, BillUserGroupDebitorDto>();
        }

        public BillCreateCommand(Core.Domain.Peanuts.Peanut createdFromPeanut) : this() {
            Require.NotNull(createdFromPeanut, "createdFromPeanut");
            CreatedFromPeanut = createdFromPeanut;
        }

        /// <summary>
        ///     Erzeugt eine neue Instanz des <see cref="BillCreateCommand" /> und setzt die UserGroup
        /// </summary>
        public BillCreateCommand(Core.Domain.Users.UserGroup userGroup) : this() {
            Require.NotNull(userGroup, "userGroup");
            UserGroup = userGroup;
        }

        /// <summary>
        ///     Liefert oder setzt das BillDto mit den Informationen zur Rechnung
        /// </summary>
        public BillDto BillDto { get; set; }

        /// <summary>
        ///     Ruft den Peanut ab, aus dem die Rechnung erstellt wurde oder legt diesen fest.
        ///     Wird/wurde die Rechnung unabhängig von einem Peanut erstellt, dann NULL.
        /// </summary>
        public Core.Domain.Peanuts.Peanut CreatedFromPeanut { get; set; }

        /// <summary>
        ///     Liefert oder setzt das Dictionary der Gast Schuldner.
        ///     Der Key muss ein eindeutiger String sein.
        /// </summary>
        public IDictionary<string, BillGuestDebitorDto> GuestDebitors { get; set; }

        /// <summary>
        ///     Liefert oder setzt die Gruppe
        /// </summary>
        public Core.Domain.Users.UserGroup UserGroup { get; set; }

        /// <summary>
        ///     Liefert oder setzt das Dictionary der Schuldner. Der Key muss ein eindeutiger Schlüssel sein.
        ///     z.B. die BusinessId der UserGroupMembership.
        /// </summary>
        public IDictionary<string, BillUserGroupDebitorDto> UserGroupDebitors { get; set; }
    }
}