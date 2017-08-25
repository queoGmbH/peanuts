using System;
using System.Collections.Generic;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate {
    public interface IGenericDao<T, in TKey> {
        /// <summary>
        ///     Leert die Session.
        /// </summary>
        /// <remarks>
        ///     Die Methode sollte nur in Testfällen verwendet werden.
        /// </remarks>
        void Clear();

        /// <summary>
        ///     Löscht die übergebene Entität
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        ///     Überprüft, ob es ein Entity mit dem Primärschlüssel gibt.
        /// </summary>
        /// <param name="primaryKey">Der Primärschlüssel.</param>
        /// <returns>
        ///     <code>true</code>, wenn ein Entity mit dem angegebenen Primärschlüssel existiert, sonst <code>false</code>
        /// </returns>
        bool Exists(TKey primaryKey);

        /// <summary>
        ///     Übernimmt alle offenen Änderungen in die Datenbank.
        /// </summary>
        /// <remarks>
        ///     Im Allgemeinen braucht diese Methode nicht aufgerufen werden, da die Steuerung
        ///     implizit über die Session bzw. die Transaktion und über den FlushMode erfolgt.
        ///     In bestimmten Fällen ist es aber hilfreich, wie z.B. bei Testfällen.
        /// </remarks>
        void Flush();

        /// <summary>
        ///     Führt ein Flush und ein Clear für die Session aus.
        /// </summary>
        void FlushAndClear();

        /// <summary>
        ///     Liefert das Entity mit dem angegebenen Primary Key.
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        T Get(TKey primaryKey);

        /// <summary>
        ///     Liefert eine Liste mit allen Entitäten.
        /// </summary>
        /// <returns>Liste mit allen Entities.</returns>
        IList<T> GetAll();

        /// <summary>
        ///     Liefert eine Liste mit allen Entitäten die entsprechend der Beschreibung sortiert sind.
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        IList<T> GetAll(Sort sort);

        /// <summary>
        ///     Liefert eine Liste mit Entities entsprechend der Pageinformationen
        ///     aus der Menge aller Entitäten.
        /// </summary>
        /// <param name="pageable">Beschreibung wleche Menge der Entitäten zurückgeliefert werden.</param>
        /// <returns>Liste mit Entitäten.</returns>
        IPage<T> GetAll(IPageable pageable);

        /// <summary>
        ///     Liefert eine Liste mit Entities entsprechend der Pageinformationen
        ///     aus der Menge aller Entitäten.
        /// </summary>
        /// <param name="pageable">Beschreibung wleche Menge der Entitäten zurückgeliefert werden.</param>
        /// <param name="sort"></param>
        /// <returns>Liste mit Entitäten.</returns>
        IPage<T> GetAll(IPageable pageable, Sort sort);

        /// <summary>
        ///     Liefert das Entity mit der entsprechenden BusinessId.
        /// </summary>
        /// <param name="businessId">die BusinessId</param>
        /// <returns>Das Entity</returns>
        T GetByBusinessId(Guid businessId);

        /// <summary>
        ///     Liefert ein Entity anhand seines Primäschlüssels.
        /// </summary>
        /// <param name="primaryKey">Der Primärschlüssel.</param>
        /// <returns>Das Entity mit dem angegebenen Primärschlüsel.</returns>
        T GetByPrimaryKey(TKey primaryKey);

        /// <summary>
        ///     Liefert die Anzahl aller Objekte.
        /// </summary>
        /// <returns>Anzahl der Objekte.</returns>
        long GetCount();

        /// <summary>
        ///     Speichert die übergebene Entität
        /// </summary>
        /// <param name="entity">Das zu speichernde Entity</param>
        /// <returns>Das gespeicherte Entity</returns>
        T Save(T entity);

        /// <summary>
        ///     Speichert alle Entitäten die in der übergebene Liste enthalten sind
        /// </summary>
        /// <param name="entities">Liste mit zu speichernden Entities.</param>
        /// <returns>Liste mit gespeicherten Entities</returns>
        IList<T> Save(IList<T> entities);
    }
}