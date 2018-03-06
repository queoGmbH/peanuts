using System;
using System.Collections.Generic;
using System.Linq;
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

        [Test]
        public void Test_Find_DeclindedBills_Should_Not_Find_Pending_Bills() {
            /* Given: A Bill with pending usergroup-debitors */
            UserGroupMembership creditorUserGroupMembership = UserGroupMembershipCreator.Create();
            UserGroup userGroup = creditorUserGroupMembership.UserGroup;
            UserGroupMembership debitorUserGroupMembership = UserGroupMembershipCreator.Create(userGroup: userGroup);
            User creditor = creditorUserGroupMembership.User;

            Bill bill = BillCreator.Create(
                userGroup,
                creditorUserGroupMembership,
                userGroupDebitorsDtos: new List<BillUserGroupDebitorDto> {
                    new BillUserGroupDebitorDto(debitorUserGroupMembership, 1),
                    new BillUserGroupDebitorDto(creditorUserGroupMembership, 1)
                });

            /* When: searching for declinded bills */
            IPage<Bill> foundBills = BillDao.FindDeclinedCreditorBillsByUser(PageRequest.All, creditor);

            /* Then: the bill must not be found */
            foundBills.Should().NotContain(bill);
        }

        [Test]
        public void Test_Find_DeclindedBills_Should_Not_Find_Bills_When_One_Debitor_Accepted() {
            /* Given: A Bill with pending and accepted usergroup-debitors */
            UserGroupMembership creditorUserGroupMembership = UserGroupMembershipCreator.Create();
            UserGroup userGroup = creditorUserGroupMembership.UserGroup;
            UserGroupMembership debitorUserGroupMembership1 = UserGroupMembershipCreator.Create(userGroup: userGroup);
            UserGroupMembership debitorUserGroupMembership2 = UserGroupMembershipCreator.Create(userGroup: userGroup);
            User creditor = creditorUserGroupMembership.User;

            Bill bill = BillCreator.Create(
                userGroup,
                creditorUserGroupMembership,
                userGroupDebitorsDtos: new List<BillUserGroupDebitorDto> {
                    new BillUserGroupDebitorDto(debitorUserGroupMembership1, 1),
                    new BillUserGroupDebitorDto(debitorUserGroupMembership2, 1),
                    new BillUserGroupDebitorDto(creditorUserGroupMembership, 1)
                });

            bill.UserGroupDebitors[0].Accept();
            bill.UserGroupDebitors[2].Accept();
            BillDao.Flush();

            /* When: searching for declinded bills */
            IPage<Bill> foundBills = BillDao.FindDeclinedCreditorBillsByUser(PageRequest.All, creditor);

            /* Then: the bill must not be found */
            foundBills.Should().NotContain(bill);
        }

        [Test]
        public void Test_Find_DeclindedBills_Should_Find_Bill_When_One_Debitor_Refused() {
            /* Given: A Bill with pending usergroup-debitors */
            UserGroupMembership creditorUserGroupMembership = UserGroupMembershipCreator.Create();
            UserGroup userGroup = creditorUserGroupMembership.UserGroup;
            UserGroupMembership debitorUserGroupMembership = UserGroupMembershipCreator.Create(userGroup: userGroup);
            User creditor = creditorUserGroupMembership.User;

            Bill refusedBill = BillCreator.Create(
                userGroup,
                creditorUserGroupMembership,
                userGroupDebitorsDtos: new List<BillUserGroupDebitorDto> { new BillUserGroupDebitorDto(debitorUserGroupMembership, 1) });
            refusedBill.UserGroupDebitors.First().Refuse("Kommentar");
            BillDao.Flush();

            /* When: searching for declinded bills */
            IPage<Bill> foundBills = BillDao.FindDeclinedCreditorBillsByUser(PageRequest.All, creditor);

            /* Then: the bill must be found */
            foundBills.Should().Contain(refusedBill);
        }
    }
}