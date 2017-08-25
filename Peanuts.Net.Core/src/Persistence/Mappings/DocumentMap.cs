using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    public class DocumentMap : EntityMap<Document> {
        public DocumentMap() {
            Map(document => document.ContentLength).Not.Nullable();
            Map(document => document.ContentType);
            Map(document => document.FileName).Not.Nullable();
            Map(document => document.OriginalFileName).Not.Nullable();
        }
    }
}