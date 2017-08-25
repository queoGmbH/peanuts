using System;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.CreatorUtils;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

using FluentAssertions;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {

    [TestFixture]
    public class BillDaoTest : PersistenceBaseTest {


        public BillDao BillDao { get; set; }

        public BillCreator BillCreator { get; set; }

        public UserGroupMembershipCreator UserGroupMembershipCreator { get; set; }

        /// <summary>
        /// FindUnsettledBilss
        /// </summary>
        [Test]
        public void TestFindUnsettledBills() {
            UserGroupMembership userGroupMembership = UserGroupMembershipCreator.Create();
            UserGroupMembership userGroupMembership2 = UserGroupMembershipCreator.Create(userGroup: userGroupMembership.UserGroup);

            //Given: Mehrere Rechnungen
            Bill settledBill = BillCreator.Create(userGroup: userGroupMembership.UserGroup, settleDate: DateTime.Now, userGroupDebitorsDtos: new List<BillUserGroupDebitorDto> {new BillUserGroupDebitorDto(userGroupMembership, 1), new BillUserGroupDebitorDto(userGroupMembership2, 1) });
            Bill settledBill2 = BillCreator.Create(settleDate: DateTime.Now, userGroupDebitorsDtos: new List<BillUserGroupDebitorDto> { new BillUserGroupDebitorDto(userGroupMembership, 1), new BillUserGroupDebitorDto(userGroupMembership2, 1) });

            Bill unsettledBill1 = BillCreator.Create(settleDate: null, userGroupDebitorsDtos: new List<BillUserGroupDebitorDto> { new BillUserGroupDebitorDto(userGroupMembership, 1), new BillUserGroupDebitorDto(userGroupMembership2, 1)} );
            Bill unsettledBill2 = BillCreator.Create(settleDate: null, userGroupDebitorsDtos: new List<BillUserGroupDebitorDto> { new BillUserGroupDebitorDto(userGroupMembership, 1), new BillUserGroupDebitorDto(userGroupMembership2, 1)} );

            //When: Nach Rechnungen gesucht wird, die bisher nicht abgerechnet wurden
            IList<Bill> unsettledBills = BillDao.FindUnsettledBills();

            //Then: Dürfen auch nur diese geliefert werden.
            unsettledBills.Should().Contain(new List<Bill> { unsettledBill1, unsettledBill2});
            unsettledBills.Should().NotContain(new List<Bill> { settledBill, settledBill2 });
        }

        /// <summary>
        /// FindUnsettledBilss
        /// </summary>
        [Test]
        public void TestFindUnsettledBillsSingleResult() {
            UserGroupMembership userGroupMembership = UserGroupMembershipCreator.Create();
            UserGroupMembership userGroupMembership2 = UserGroupMembershipCreator.Create(userGroup: userGroupMembership.UserGroup);

            //Given: Mehrere Rechnungen von denen nur eine nicht abgerechnet ist.
            Bill settledBill = BillCreator.Create(userGroup: userGroupMembership.UserGroup, settleDate: DateTime.Now, userGroupDebitorsDtos: new List<BillUserGroupDebitorDto> { new BillUserGroupDebitorDto(userGroupMembership, 1), new BillUserGroupDebitorDto(userGroupMembership2, 1) });
            Bill settledBill2 = BillCreator.Create(settleDate: DateTime.Now, userGroupDebitorsDtos: new List<BillUserGroupDebitorDto> { new BillUserGroupDebitorDto(userGroupMembership, 1), new BillUserGroupDebitorDto(userGroupMembership2, 1) });

            Bill unsettledBill1 = BillCreator.Create(settleDate: null, userGroupDebitorsDtos: new List<BillUserGroupDebitorDto> { new BillUserGroupDebitorDto(userGroupMembership, 1), new BillUserGroupDebitorDto(userGroupMembership2, 1) });

            //When: Nach Rechnungen gesucht wird, die bisher nicht abgerechnet wurden
            IList<Bill> unsettledBills = BillDao.FindUnsettledBills();

            //Then: Darf nur die eine geliefert werden.
            unsettledBills.Should().Contain(new List<Bill> { unsettledBill1 });
            unsettledBills.Should().NotContain(new List<Bill> { settledBill, settledBill2 });
        }
    }
}