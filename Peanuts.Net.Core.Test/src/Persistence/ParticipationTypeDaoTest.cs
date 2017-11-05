using Com.QueoFlow.Peanuts.Net.Core.CreatorUtils;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
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
    }
}