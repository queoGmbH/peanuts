using System;
using System.Collections.Generic;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;

namespace Com.QueoFlow.Peanuts.Net.Core.CreatorUtils {

    public class BillCreator : EntityCreator {
        const double DEFAULT_BILL_AMOUNT = 5.00;
        const string DEFAULT_BILL_SUBJECT = "Betreff";

        public IBillDao BillDao { get; set; }

        public UserGroupCreator UserGroupCreator {
            get; set;
        }

        public UserGroupMembershipCreator UserGroupMembershipCreator { get; set; }

        public Bill Create(UserGroup userGroup = null, UserGroupMembership creditor = null, string subject = DEFAULT_BILL_SUBJECT, double amount = DEFAULT_BILL_AMOUNT, IList<BillUserGroupDebitorDto> userGroupDebitorsDtos = null, IList<BillGuestDebitorDto> guestDebitorDtos = null, DateTime? settleDate = null, bool persist = true) {

            if (userGroupDebitorsDtos == null && guestDebitorDtos == null) {
                userGroupDebitorsDtos = new List<BillUserGroupDebitorDto>();
                guestDebitorDtos = new List<BillGuestDebitorDto> {new BillGuestDebitorDto("Guest Debitor", "guest@debitors.com", 1)};
            } else if(userGroupDebitorsDtos == null){
                userGroupDebitorsDtos = new List<BillUserGroupDebitorDto>();
            } else if (guestDebitorDtos == null) {
                guestDebitorDtos = new List<BillGuestDebitorDto>();
            }

            if (userGroup == null) {
                userGroup = UserGroupCreator.Create(persist: persist);
            }
            if (creditor == null) {
                creditor = UserGroupMembershipCreator.Create(userGroup, persist: persist);
            }

            BillDto billDto = CreateBillDto(subject, amount);
            IList<BillUserGroupDebitor> userGroupDebitors = userGroupDebitorsDtos.Select(dto => new BillUserGroupDebitor(dto.UserGroupMembership, dto.Portion)).ToList();
            IList<BillGuestDebitor> guestDebitors = guestDebitorDtos.Select(dto => new BillGuestDebitor(dto.Name, dto.Email, dto.Portion)).ToList();


            Bill bill = new Bill(userGroup, billDto, creditor, userGroupDebitors, guestDebitors, new EntityCreatedDto(creditor.User, DateTime.Now));
            if (settleDate.HasValue) {
                bill.Settle(new EntityChangedDto(bill.Creditor.User, settleDate.Value));
            }

            if (persist) {
                BillDao.Save(bill);
                BillDao.Flush();
            }

            return bill;
        }

        public BillDto CreateBillDto(string subject = DEFAULT_BILL_SUBJECT, double amount = DEFAULT_BILL_AMOUNT) {
            return new BillDto(subject, amount);
        }
    }
}