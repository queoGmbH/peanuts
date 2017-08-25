using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;

namespace Com.QueoFlow.Peanuts.Net.Core.CreatorUtils {
    public class UserGroupMembershipCreator : EntityCreator {


        public IUserGroupMembershipDao UserGroupMembershipDao { get; set; }

        public UserCreator UserCreator { get; set; }

        public UserGroupCreator UserGroupCreator { get; set; }

        public UserGroupMembership Create(UserGroup userGroup = null, User user = null, UserGroupMembershipType membershipType = UserGroupMembershipType.Member, User createdBy = null, DateTime? createdAt = null, bool persist = true) {

            if (createdBy == null) {
                createdBy = UserCreator.Create(persist: persist);
            }
            if (!createdAt.HasValue) {
                createdAt = DateTime.Now;
            }

            if (userGroup == null) {
                userGroup = UserGroupCreator.Create(createdBy: createdBy, persist: persist);
            }
            if (user == null) {
                user = UserCreator.Create(creationDto: new EntityCreatedDto(createdBy, createdAt.Value), persist: persist);
            }
            
            UserGroupMembership userGroupMembership = new UserGroupMembership(userGroup, user, membershipType, new EntityCreatedDto(createdBy, createdAt.Value));

            if (persist) {
                UserGroupMembershipDao.Save(userGroupMembership);
                UserGroupMembershipDao.Flush();
            }


            return userGroupMembership;
        }

    }
}