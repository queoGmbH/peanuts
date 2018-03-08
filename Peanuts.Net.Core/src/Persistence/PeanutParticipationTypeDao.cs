using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using Spring.Data.NHibernate.Generic;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    public class PeanutParticipationTypeDao : GenericDao<PeanutParticipationType, int>, IPeanutParticipationTypeDao
    {
        public IList<PeanutParticipationType> FindForGroup(UserGroup userGroup)
        {
            HibernateDelegate<IPage<PeanutParticipationType>> finder = delegate(ISession session)
            {
                IQueryOver<PeanutParticipationType, PeanutParticipationType> queryOver = session.QueryOver<PeanutParticipationType>();

                if (userGroup != null) {
                    queryOver.Where(Restrictions.Or(
                        Restrictions.Where<PeanutParticipationType>(participationType => participationType.UserGroup == userGroup),
                        Restrictions.Where<PeanutParticipationType>(participationType => participationType.UserGroup == null)));
                } else {
                    /*Participation types without assigned usergroup are for all usergroups*/
                    queryOver.Where(part => part.UserGroup == null);
                }
                
                return FindPage(queryOver, PageRequest.All);
            };
            return HibernateTemplate.Execute(finder).ToList();
        }
    }

    public interface IPeanutParticipationTypeDao : IGenericDao<PeanutParticipationType, int> {
        IList<PeanutParticipationType> FindForGroup(UserGroup userGroup);
    }
}