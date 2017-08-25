using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {

    /// <summary>
    /// Service der MEthoden zur Verwaltung von <see cref="PeanutParticipationType"/>s anbietet.
    /// </summary>
    public class PeanutParticipationTypeService : IPeanutParticipationTypeService {
        public IPeanutParticipationTypeDao PeanutParticipationTypeDao { private get; set; }

        public IPage<PeanutParticipationType> GetAll(IPageable pageRequest) {
            return PeanutParticipationTypeDao.GetAll(pageRequest);
        }
    }
}