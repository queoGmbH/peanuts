using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    public class PeanutParticipationTypeMap : EntityMap<PeanutParticipationType> {
        protected PeanutParticipationTypeMap() {

            Map(type => type.Name).Not.Nullable().Length(255);
            Map(type => type.IsCreditor).Not.Nullable();
            Map(type => type.IsProducer).Not.Nullable();
            Map(type => type.MaxParticipatorsOfType).Nullable();

            References(peanut => peanut.CreatedBy).Nullable().NotFound.Ignore();
            Map(peanut => peanut.CreatedAt).Not.Nullable();
            References(peanut => peanut.ChangedBy).Nullable().NotFound.Ignore();
            Map(peanut => peanut.ChangedAt).Nullable();
            References(peanut => peanut.UserGroup).Not.Nullable().NotFound.Ignore();
        }
    }
}