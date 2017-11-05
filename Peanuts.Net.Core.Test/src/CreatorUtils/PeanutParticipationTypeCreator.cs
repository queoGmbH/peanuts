using System;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Core.CreatorUtils
{
    public class PeanutParticipationTypeCreator : EntityCreator
    {
        public UserGroupCreator UserGroupCreator { get; set; }

        public UserGroupMembershipCreator UserGroupMembershipCreator { get; set; }

        public PeanutParticipationType Create(UserGroup userGroup = null, string name = "Test Typ", bool isCreditor = true, bool isProducer = false, int maxParticipators = 1, bool persist = true)
        {
            if (userGroup==null)
            {
                userGroup = UserGroupCreator.Create();
            }
            User user = UserGroupMembershipCreator.Create(userGroup, persist: persist).User;

            var peanutParticipationTypeDto = new PeanutParticipationTypeDto
            {
                IsCreditor = isCreditor,
                Name = name,
                IsProducer = isProducer,
                MaxParticipatorsOfType = maxParticipators
            };
            PeanutParticipationType peanutParticipationType = new PeanutParticipationType(peanutParticipationTypeDto, userGroup, new EntityCreatedDto(user, DateTime.Now));
            return peanutParticipationType;
        }
    }
}