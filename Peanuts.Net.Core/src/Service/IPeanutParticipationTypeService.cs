using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {
    /// <summary>
    /// Schnittstelle, die einen Service zur Verwaltung von <see cref="PeanutParticipationType"/>s beschreibt.
    /// </summary>
    public interface IPeanutParticipationTypeService {

        /// <summary>
        /// Ruft seitenweise alle <see cref="PeanutParticipationType"/>s ab.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <returns></returns>
        IPage<PeanutParticipationType> GetAll(IPageable pageRequest);
    }
}