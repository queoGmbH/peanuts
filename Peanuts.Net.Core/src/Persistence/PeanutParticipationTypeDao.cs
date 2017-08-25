using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    public class PeanutParticipationTypeDao : GenericDao<PeanutParticipationType, int>, IPeanutParticipationTypeDao {
        
    }

    public interface IPeanutParticipationTypeDao : IGenericDao<PeanutParticipationType, int> {
    }
}