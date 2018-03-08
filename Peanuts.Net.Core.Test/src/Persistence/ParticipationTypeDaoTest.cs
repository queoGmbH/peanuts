using System.Collections.Generic;
using Com.QueoFlow.Peanuts.Net.Core.CreatorUtils;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence
{
    [TestFixture]
    public class ParticipationTypeDaoTest:PersistenceBaseTest
    {
        public IPeanutParticipationTypeDao PeanutParticipationTypeDao { get; set; }

        public PeanutParticipationTypeCreator PeanutParticipationTypeCreator { get; set; }

        public UserGroupCreator UserGroupCreator { get; set; }

        [Test]
        public void CreateParticipationType()
        {
            PeanutParticipationType participationType =  PeanutParticipationTypeCreator.Create(persist:false);
            PeanutParticipationTypeDao.Save(participationType);
            var peanutParticipationType = PeanutParticipationTypeDao.Get(participationType.Id);
            peanutParticipationType.UserGroup.Should().Be(participationType.UserGroup);
            peanutParticipationType.Name.Should().Be(participationType.Name);
            peanutParticipationType.IsProducer.Should().Be(participationType.IsProducer);
            peanutParticipationType.IsCreditor.Should().Be(participationType.IsCreditor);
            peanutParticipationType.MaxParticipatorsOfType.Should().Be(participationType.MaxParticipatorsOfType);
        }

        [Test]
        public void Test_FindByUserGroup_Should_Not_Return_ParticipationType_With_UserGroup_Null_When_Searching_By_Null() {
            /* Given: A Participationtype with assigned usergroup */
            UserGroup userGroup = UserGroupCreator.Create();
            PeanutParticipationType participationTypeWithUserGroupNull = PeanutParticipationTypeCreator.Create(userGroup: userGroup);

            /* When: trying to find participation types with usergroup=null  */
            IList<PeanutParticipationType> peanutParticipationTypes = PeanutParticipationTypeDao.FindForGroup(null);

            /* Then: the participation type must not be in the result set */
            peanutParticipationTypes.Should().NotContain(participationTypeWithUserGroupNull);
        }

        [Test]
        public void Test_FindByUserGroup_Should_Return_ParticipationType_With_UserGroup_Null() {
            /* Given: A Participationtype without assigned usergroup */
            PeanutParticipationType participationTypeWithUserGroupNull = PeanutParticipationTypeCreator.Create(userGroup: null);

            /* When: trying to find participation types with usergroup=null  */
            IList<PeanutParticipationType> peanutParticipationTypes = PeanutParticipationTypeDao.FindForGroup(null);

            /* Then: the participation type should be in the result set */
            peanutParticipationTypes.Should().Contain(participationTypeWithUserGroupNull);
        }

        [Test]
        public void Test_FindByUserGroup_Should_Not_Return_ParticipationType_Of_Other_UserGroup() {
            /* Given: A participation type without assigned usergroup */
            UserGroup userGroupOfParticipationType = UserGroupCreator.Create();
            UserGroup otherUserGroup = UserGroupCreator.Create();
            PeanutParticipationType participationTypeWithUserGroupNull = PeanutParticipationTypeCreator.Create(userGroup: userGroupOfParticipationType);

            /* When: searching participation types of other group */
            IList<PeanutParticipationType> peanutParticipationTypes = PeanutParticipationTypeDao.FindForGroup(otherUserGroup);

            /* Then: the participation type must not be in the result set */
            peanutParticipationTypes.Should().NotContain(participationTypeWithUserGroupNull);
        }
    }
}