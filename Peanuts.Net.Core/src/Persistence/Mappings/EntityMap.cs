using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

using FluentNHibernate.Mapping;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    public class EntityMap<T> : ClassMap<T> where T : Entity {
        protected EntityMap() {
            Id(x => x.Id);
            Map(x => x.BusinessId).Unique().Not.Nullable();
        }
    }
}