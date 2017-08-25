using System;
using System.Collections.Generic;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

using NHibernate;
using NHibernate.Criterion;

using Spring.Data.NHibernate.Generic;
using Spring.Data.NHibernate.Generic.Support;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate {
    /// <summary>
    ///     Konkrete Implementierung für einen DAO unter Verwendung von nHibernate und spring.net.
    /// </summary>
    /// <typeparam name="T">Typ des Entities</typeparam>
    /// <typeparam name="TKey">Typ des Primärschlüssels den das Entity verwendet.</typeparam>
    public class GenericDao<T, TKey> : HibernateDaoSupport, IGenericDao<T, TKey> where T : class {
        /// <summary>
        ///     Leert die Session und verwirft alle noch anstehenden Änderungen.
        /// </summary>
        /// <remarks>
        ///     Die Methode sollte nur in Testfällen verwendet werden.
        /// </remarks>
        public void Clear() {
            HibernateTemplate.Clear();
        }

        /// <summary>
        ///     Löscht die übergebene Entität
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity) {
            Require.NotNull(entity, "entity");
            HibernateTemplate.Delete(entity);
        }

        /// <summary>
        ///     Überprüft, ob es eine Entität mit dem Primärschlüssel gibt.
        /// </summary>
        /// <param name="primaryKey">Der Primärschlüssel.</param>
        /// <returns>
        ///     <code>true</code>, wenn ein Entity mit dem angegebenen Primärschlüssel existiert, sonst <code>false</code>
        /// </returns>
        public virtual bool Exists(TKey primaryKey) {
            if (HibernateTemplate.Get<T>(primaryKey) != null) {
                return true;
            }
            return false;
        }

        public IPage<TResult> FindPage<TResult>(IQueryOver<TResult, TResult> queryOver, IPageable pageRequest) {
            IQueryOver<TResult, TResult> countQuery = queryOver.Clone().ClearOrders().ToRowCountInt64Query();
            countQuery.Skip(0).Take(int.MaxValue);
            IFutureValue<long> rowCount = countQuery.Select(Projections.RowCountInt64()).FutureValue<long>();

            queryOver.Skip(pageRequest.FirstItem);
            queryOver.Take(pageRequest.PageSize);

            IEnumerable<TResult> enumerable = queryOver.Future<TResult>();
            return new Page<TResult>(enumerable.ToList(), pageRequest, rowCount.Value);
        }

        /// <summary>
        ///     Übernimmt alle offenen Änderungen in die Datenbank.
        /// </summary>
        /// <remarks>
        ///     Im Allgemeinen braucht diese Methode nicht aufgerufen werden, da die Steuerung
        ///     implizit über die Session bzw. die Transaktion und über den FlushMode erfolgt.
        ///     In bestimmten Fällen ist es aber hilfreich, wie z.B. bei Testfällen.
        /// </remarks>
        public void Flush() {
            HibernateTemplate.Flush();
        }

        /// <summary>
        ///     Führt ein Flush und ein Clear für die Session aus.
        /// </summary>
        public void FlushAndClear() {
            Flush();
            Clear();
        }

        /// <summary>
        ///     Liefert das Entity mit dem angegebenen Primary Key.
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public T Get(TKey primaryKey) {
            return GetByPrimaryKey(primaryKey);
        }

        public IPage<T> GetAll(IPageable pageable, Sort sort) {
            Require.NotNull(pageable, "pageable");
            Require.NotNull(sort, nameof(sort));

            HibernateDelegate<IPage<T>> finder = delegate(ISession session) {
                ICriteria elementsCriteria = session.CreateCriteria(typeof(T));
                ApplySort(elementsCriteria, sort);
                ApplyPaging(pageable, elementsCriteria);

                ICriteria countCriteria = session.CreateCriteria(typeof(T));
                countCriteria.SetProjection(Projections.RowCountInt64());

                IFutureValue<long> futureTotalCount = countCriteria.FutureValue<long>();
                IEnumerable<T> futureElements = elementsCriteria.Future<T>();
                Page<T> page = new Page<T>(futureElements.ToList(), pageable, futureTotalCount.Value);
                return page;
            };
            return HibernateTemplate.Execute(finder);
        }

        /// <summary>
        ///     Liefert eine Liste mit allen Entitäten.
        /// </summary>
        /// <returns>Liste mit allen Entities.</returns>
        public virtual IList<T> GetAll() {
            return HibernateTemplate.LoadAll<T>();
        }

        /// <summary>
        ///     Liefert eine Liste mit allen Entitäten die entsprechend der Beschreibung sortiert sind.
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IList<T> GetAll(Sort sort) {
            FindHibernateDelegate<T> finder = delegate(ISession session) {
                ICriteria criteria = session.CreateCriteria(typeof(T));
                criteria = ApplySort(criteria, sort);
                IList<T> results = criteria.List<T>();
                return results;
            };
            IList<T> resultObjects = HibernateTemplate.ExecuteFind(finder);
            return resultObjects;
        }

        /// <summary>
        ///     Liefert eine Liste mit Entities entsprechend der Pageinformationen
        ///     aus der Menge aller Entitäten.
        /// </summary>
        /// <param name="pageable">Beschreibung welche Menge der Entitäten zurückgeliefert werden.</param>
        /// <returns>Liste mit Entitäten.</returns>
        public virtual IPage<T> GetAll(IPageable pageable) {
            Require.NotNull(pageable, "pageable");

            HibernateDelegate<IPage<T>> finder = delegate(ISession session) {
                ICriteria elementsCriteria = session.CreateCriteria(typeof(T));
                elementsCriteria.AddOrder(Order.Asc("Id"));
                ApplyPaging(pageable, elementsCriteria);

                ICriteria countCriteria = session.CreateCriteria(typeof(T));
                countCriteria.SetProjection(Projections.RowCountInt64());

                IFutureValue<long> futureTotalCount = countCriteria.FutureValue<long>();
                IEnumerable<T> futureElements = elementsCriteria.Future<T>();
                Page<T> page = new Page<T>(futureElements.ToList(), pageable, futureTotalCount.Value);
                return page;
            };
            return HibernateTemplate.Execute(finder);
        }

        /// <summary>
        ///     Liefert das Entity mit der entsprechenden BusinessId.
        /// </summary>
        /// <param name="businessId">die BusinessId</param>
        /// <returns>Das Entity</returns>
        /// <exception cref="ObjectNotFoundException">Wenn kein Entity mit der BusinessId existiert</exception>
        public T GetByBusinessId(Guid businessId) {
            HibernateDelegate<T> find = delegate(ISession session) {
                ICriteria criteria = session.CreateCriteria(typeof(T));
                criteria.Add(Restrictions.Eq("BusinessId", businessId));
                return criteria.UniqueResult<T>();
            };
            T entity = HibernateTemplate.Execute(find);
            if (entity == null) {
                throw new ObjectNotFoundException(businessId, typeof(T));
            }
            return entity;
        }

        /// <summary>
        ///     Liefert ein Entity anhand seines Primäschlüssels.
        /// </summary>
        /// <param name="primaryKey">Der Primärschlüssel.</param>
        /// <returns>Das Entity mit dem angegebenen Primärschlüssel.</returns>
        public virtual T GetByPrimaryKey(TKey primaryKey) {
            T loadedEntity = HibernateTemplate.Get<T>(primaryKey);
            if (loadedEntity == null) {
                throw new ObjectNotFoundException(primaryKey, typeof(T));
            }
            return loadedEntity;
        }

        /// <summary>
        ///     Liefert die Anzahl aller Objekte.
        /// </summary>
        /// <returns>Anzahl der Objekte.</returns>
        public long GetCount() {
            HibernateDelegate<long> find = delegate(ISession session) {
                ICriteria criteria = session.CreateCriteria(typeof(T));
                criteria.SetProjection(Projections.RowCountInt64());
                return criteria.UniqueResult<long>();
            };
            return HibernateTemplate.Execute(find);
        }

        /// <summary>
        ///     Speichert die übergebene Entität
        /// </summary>
        /// <param name="entity">Das zu speichernde Entity</param>
        /// <returns>Das gespeicherte Entity</returns>
        /// <exception cref="ArgumentNullException">Wenn <paramref name="entity" /> <code>null</code> ist.</exception>
        public virtual T Save(T entity) {
            Require.NotNull(entity, "entity");
            HibernateTemplate.SaveOrUpdate(entity);
            return entity;
        }

        /// <summary>
        ///     Speichert alle Entitäten die in der übergebene Liste enthalten sind.
        /// </summary>
        /// <param name="entities">Liste mit zu speichernden Entities.</param>
        /// <exception cref="NullReferenceException">Wenn in der Liste ein Eintrag null ist</exception>
        /// <returns>Liste mit gespeicherten Entities</returns>
        /// <exception cref="ArgumentNullException">Wenn <paramref name="entities" /> <code>null</code> ist.</exception>
        public virtual IList<T> Save(IList<T> entities) {
            Require.NotNull(entities, "entities");
            for (int i = 0; i < entities.Count; i++) {
                T entity = entities[i];
                if (entity == null) {
                    throw new NullReferenceException(
                        string.Format("Die Entität am Index {0} war null und konnte nicht gespeichert werden.", i));
                }
                HibernateTemplate.SaveOrUpdate(entity);
            }
            return entities;
        }

        protected static void ApplyPaging(IPageable pageable, ICriteria elementsCriteria) {
            elementsCriteria.SetFirstResult(pageable.FirstItem);
            elementsCriteria.SetMaxResults(pageable.PageSize);
        }

        /// <summary>
        /// </summary>
        /// <param name="pageable"></param>
        /// <param name="criteriaBuilder">
        ///     Delegate zum Hinzufügen der "Where-Klauseln". Wird sowohl in der eigentlichen Abfrage,
        ///     als auch in der Count-Abfrage verwendet.
        /// </param>
        /// <param name="ordersBuilder">
        ///     Delegate zum Hinzufügen der "Order-Klauseln". Wird nur in der eigentlichen Abfrage, nicht
        ///     aber in der Count-Abfrage verwendet.
        /// </param>
        /// <returns></returns>
        protected virtual IPage<T> Find(IPageable pageable, Action<ICriteria> criteriaBuilder, Action<ICriteria> ordersBuilder = null) {
            Require.NotNull(pageable, "pageable");
            Require.NotNull(criteriaBuilder, "criteriaBuilder");

            HibernateDelegate<IPage<T>> finder = delegate(ISession session) {
                ICriteria elementsCriteria = session.CreateCriteria(typeof(T));

                criteriaBuilder(elementsCriteria);
                if (ordersBuilder != null) {
                    ordersBuilder(elementsCriteria);
                }
                ApplyPaging(pageable, elementsCriteria);

                ICriteria countCriteria = session.CreateCriteria(typeof(T));
                criteriaBuilder(countCriteria);
                countCriteria.SetProjection(Projections.RowCountInt64());

                IFutureValue<long> futureTotalCount = countCriteria.FutureValue<long>();
                IEnumerable<T> futureElements = elementsCriteria.Future<T>();
                Page<T> page = new Page<T>(futureElements.ToList(), pageable, futureTotalCount.Value);
                return page;
            };
            return HibernateTemplate.Execute(finder);
        }

        /// <summary>
        ///     Versuch über ein Detached Criteria die Where-Klauseln zu verarbeiten, damit es mit dem Order nicht zu
        ///     komplikationen kommt.
        /// </summary>
        protected virtual IPage<T> Find(IPageable pageable, Action<DetachedCriteria> criteriaBuilder, Action<ICriteria> ordersBuilder = null) {
            Require.NotNull(pageable, "pageable");
            Require.NotNull(criteriaBuilder, "criteriaBuilder");

            HibernateDelegate<IPage<T>> finder = delegate(ISession session) {
                DetachedCriteria whereCriteria = DetachedCriteria.For(typeof(T));
                criteriaBuilder(whereCriteria);
                whereCriteria.SetProjection(Projections.Property("Id"));

                ICriteria elementsCriteria = session.CreateCriteria(typeof(T));
                elementsCriteria.Add(Subqueries.PropertyIn("Id", whereCriteria));

                if (ordersBuilder != null) {
                    ordersBuilder(elementsCriteria);
                }
                ApplyPaging(pageable, elementsCriteria);

                ICriteria countCriteria = session.CreateCriteria(typeof(T));
                countCriteria.Add(Subqueries.PropertyIn("Id", whereCriteria));
                countCriteria.SetProjection(Projections.RowCountInt64());

                IFutureValue<long> futureTotalCount = countCriteria.FutureValue<long>();
                IEnumerable<T> futureElements = elementsCriteria.Future<T>();
                Page<T> page = new Page<T>(futureElements.ToList(), pageable, futureTotalCount.Value);
                return page;
            };
            return HibernateTemplate.Execute(finder);
        }

        /// <summary>
        ///     Versuch über ein Detached Criteria die Where-Klauseln zu verarbeiten, damit es mit dem Order nicht zu
        ///     komplikationen kommt.
        /// </summary>
        protected virtual IPage<T> Find(IPageable pageable, Action<IQueryOver<T, T>> criteriaBuilder, Action<IQueryOver<T, T>> ordersBuilder = null) {
            Require.NotNull(pageable, "pageable");
            Require.NotNull(criteriaBuilder, "criteriaBuilder");

            HibernateDelegate<IPage<T>> finder = delegate(ISession session) {
                IQueryOver<T, T> query = session.QueryOver<T>();
                criteriaBuilder(query);
                if (ordersBuilder != null) {
                    ordersBuilder(query);
                }

                IQueryOver<T, T> countQuery = session.QueryOver<T>();
                criteriaBuilder(countQuery);

                query.Skip(pageable.FirstItem);
                query.Take(pageable.PageSize);

                long futureTotalCount = countQuery.RowCountInt64();
                IEnumerable<T> futureElements = query.Future<T>();
                Page<T> page = new Page<T>(futureElements.ToList(), pageable, futureTotalCount);
                return page;
            };
            return HibernateTemplate.Execute(finder);
        }

        private ICriteria ApplySort(ICriteria criteria, Sort sort) {
            foreach (OrderDescriptor orderDescription in sort) {
                if (orderDescription.Direction.Equals(Direction.Ascending)) {
                    criteria.AddOrder(Order.Asc(orderDescription.Property));
                } else {
                    criteria.AddOrder(Order.Desc(orderDescription.Property));
                }
            }
            return criteria;
        }
    }
}