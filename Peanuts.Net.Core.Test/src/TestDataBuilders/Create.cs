using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

using Spring.Context.Support;

namespace Com.QueoFlow.Peanuts.Net.Core.TestDataBuilders {
    public static class Create {
        public static class A {
            public static PaymentBuilder Payment() {
                return ContextRegistry.GetContext().GetObject<PaymentBuilder>();
            }

            public static UserBuilder User() {
                return ContextRegistry.GetContext().GetObject<UserBuilder>();
            }

            public static UserGroupBuilder UserGroup() {
                return ContextRegistry.GetContext().GetObject<UserGroupBuilder>();
            }

            public static UserGroupMemberShipBuilder UserGroupMembership() {
                return ContextRegistry.GetContext().GetObject<UserGroupMemberShipBuilder>();
            }
        }

        public static class An {
            public static AccountBuilder Account() {
                return ContextRegistry.GetContext().GetObject<AccountBuilder>();
            }

            /// <summary>
            /// </summary>
            public static UserBuilder Administrator() {
                return A.User().WithRole(Roles.Administrator);
            }
        }
    }
}