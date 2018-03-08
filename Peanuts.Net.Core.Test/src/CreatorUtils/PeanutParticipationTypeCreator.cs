using System;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;

namespace Com.QueoFlow.Peanuts.Net.Core.CreatorUtils
{
    public class PeanutParticipationTypeCreator : EntityCreator
    {
        public UserGroupCreator UserGroupCreator { get; set; }

        public UserGroupMembershipCreator UserGroupMembershipCreator { get; set; }

        public IPeanutParticipationTypeDao ParticipationTypeDao { get; set; }

        public PeanutParticipationType Create(UserGroup userGroup = null, string name = "Test Typ", bool isCreditor = true, bool isProducer = false, int maxParticipators = 1, bool persist = true)
        {
            User user = UserGroupMembershipCreator.Create(userGroup, persist: persist).User;

            PeanutParticipationTypeDto peanutParticipationTypeDto = new PeanutParticipationTypeDto
            {
                IsCreditor = isCreditor,
                Name = name,
                IsProducer = isProducer,
                MaxParticipatorsOfType = maxParticipators
            };
            PeanutParticipationType peanutParticipationType = new PeanutParticipationType(peanutParticipationTypeDto, userGroup, new EntityCreatedDto(user, DateTime.Now));

            if (persist) {
                ParticipationTypeDao.Save(peanutParticipationType);
                ParticipationTypeDao.Flush();
            }

            return peanutParticipationType;
        }
    }
}