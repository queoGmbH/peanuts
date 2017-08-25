using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;

namespace Com.QueoFlow.Peanuts.Net.Core.CreatorUtils {
    public class UserGroupCreator : EntityCreator {
        public UserCreator UserCreator { get; set; }

        public IUserGroupDao UserGroupDao { get; set; }

        public UserGroup Create(string additionalInformations = null, string name = "Gruppe 1", double? balanceOverdraftLimit = -10, User createdBy = null, DateTime? createdAt = null, bool persist = true) {
            
            UserGroupDto userGroupDto = new UserGroupDto(additionalInformations, name, balanceOverdraftLimit);
            EntityCreatedDto entityCreatedDto = GetEntityCreatedDto(createdBy, createdAt);
            UserGroup userGroup = new UserGroup(userGroupDto, entityCreatedDto);

            if (persist) {
                userGroup = UserGroupDao.Save(userGroup);
                UserGroupDao.Flush();
            }
            return userGroup;
        }

        public EntityCreatedDto GetEntityCreatedDto(User createdBy, DateTime? createdAt) {
            if (createdAt == null) {
                DateTime tempDateTime = DateTime.Now;
                tempDateTime = tempDateTime.AddMilliseconds(-tempDateTime.Millisecond);
                createdAt = tempDateTime;
            }
            if (createdBy == null) {
                createdBy = UserCreator.Create();
            }
            EntityCreatedDto entityCreatedDto = new EntityCreatedDto(createdBy, createdAt.Value);
            return entityCreatedDto;
        }
    }
}