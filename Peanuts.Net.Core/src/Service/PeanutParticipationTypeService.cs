using System;
using System.Collections;
using System.Collections.Generic;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Persistence;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Service {

    /// <summary>
    /// Service der Methoden zur Verwaltung von <see cref="PeanutParticipationType"/>s anbietet.
    /// </summary>
    public class PeanutParticipationTypeService : IPeanutParticipationTypeService {
        public IPeanutParticipationTypeDao PeanutParticipationTypeDao { private get; set; }

        public IPage<PeanutParticipationType> GetAll(IPageable pageRequest) {
            return PeanutParticipationTypeDao.GetAll(pageRequest);
        }
        public IList<PeanutParticipationType> Find( UserGroup userGroup) {
            return PeanutParticipationTypeDao.Find(userGroup);
        }

        public PeanutParticipationType Create(PeanutParticipationTypeDto participationTypeDto, UserGroup userGroup, User createdBy)
        {
            PeanutParticipationType peanutParticipationType = new PeanutParticipationType(participationTypeDto, userGroup, new EntityCreatedDto(createdBy,DateTime.Now));
            PeanutParticipationTypeDao.Save(peanutParticipationType);
            return peanutParticipationType;
        }
    }
}