using Com.QueoFlow.Peanuts.Net.Core.Infrastructure;

namespace Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts
{
    [DtoFor(typeof(PeanutParticipationType))]
    public class PeanutParticipationTypeDto
    {
        public string Name { get; set; }

        public bool IsProducer { get; set; }

        public bool IsCreditor { get; set; }

        public int MaxParticipatorsOfType { get; set; }
    }
}