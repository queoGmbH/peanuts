using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    public class PeanutParticipationMap : EntityMap<PeanutParticipation> {
        protected PeanutParticipationMap() {

            References(participation => participation.Peanut).ForeignKey("FK_PEANUT_PARTICIPATION").Not.Nullable();
            References(participation => participation.ParticipationType).ForeignKey("FK_PEANUT_PARTICIPATION_TYPE").Not.Nullable();
            References(participation => participation.UserGroupMembership).ForeignKey("FK_PEANUT_PARTICIPATION_MEMBERSHIP").Not.Nullable();
            Map(participation => participation.ParticipationState).Not.Nullable();


            References(peanut => peanut.CreatedBy).Not.Nullable().ForeignKey("FK_CREATOR_OF_PEANUT_PARTICIPATION");
            Map(peanut => peanut.CreatedAt).Not.Nullable();
            References(peanut => peanut.ChangedBy).Nullable().NotFound.Ignore();
            Map(peanut => peanut.ChangedAt).Nullable();
        }
    }
}