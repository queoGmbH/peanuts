using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence {
    /// <summary>
    ///     Konkrete Implementierung für einen DAO der ein <see cref="Document" /> persistieren und laden kann.
    /// </summary>
    public class DocumentDao : GenericDao<Document, int>, IDocumentDao {
    }
}