using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;
using NHibernate;
using Spring.Data.NHibernate.Generic;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    public class PeanutParticipationTypeDao : GenericDao<PeanutParticipationType, int>, IPeanutParticipationTypeDao
    {
        public IList<PeanutParticipationType> Find(UserGroup userGroup)
        {
            HibernateDelegate<IPage<PeanutParticipationType>> finder = delegate(ISession session)
            {
                IQueryOver<PeanutParticipationType, PeanutParticipationType> queryOver = session.QueryOver<PeanutParticipationType>();
                queryOver.Where(p => p.UserGroup.Id == userGroup.Id);
                return FindPage(queryOver, PageRequest.All);
            };
            return HibernateTemplate.Execute(finder).ToList();
        }
    }

    public interface IPeanutParticipationTypeDao : IGenericDao<PeanutParticipationType, int> {
        IList<PeanutParticipationType> Find(UserGroup userGroup);
    }
}