using System;
using System.Collections.Generic;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

using Common.Logging;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ModelBinding {
    /// <summary>
    ///     Klasse implementiert das Binden von Listen von Entities.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityListModelBinder<TEntity> : IModelBinder where TEntity : Entity {
        private readonly IGenericDao<TEntity, int> _dao;
        private readonly ILog _logger = LogManager.GetLogger(typeof(EntityModelBinder<>));

        public EntityListModelBinder(IGenericDao<TEntity, int> dao) {
            _dao = dao;
        }

        /// <summary>
        ///     Bindet das Modell mithilfe des angegebenen Controllerkontexts und Bindungskontexts an einen Wert.
        /// </summary>
        /// <returns>
        ///     Der gebundene Wert.
        /// </returns>
        /// <param name="controllerContext">Der Controllerkontext.</param>
        /// <param name="bindingContext">Der Bindungskontext.</param>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            /*Es handelt sich um eine Liste die gebunden werden muss*/
            List<TEntity> domainEntities = new List<TEntity>();
            if (valueProviderResult == null) {
                return domainEntities;
            }
            IList<object> listOfDomainEntityKeys = valueProviderResult.ConvertTo(typeof(IList<object>)) as IList<object>;
            if (listOfDomainEntityKeys != null) {
                foreach (object domainEntityId in listOfDomainEntityKeys) {
                    TEntity domainEntityValue;
                    try {
                        domainEntityValue = GetSingleValue(domainEntityId);
                        if (!domainEntities.Contains(domainEntityValue)) {
                            /*TODO: Ist null ein valider Wert?*/
                            domainEntities.Add(domainEntityValue);
                        }
                    } catch (Exception ex) {
                        _logger.ErrorFormat("Beim Binden eines Objektes für eine Liste vom Typ [{0}] mit der Id [{1}] ist ein Fehler aufgetreten.", ex, bindingContext.ModelType, valueProviderResult.AttemptedValue);
                        // TODO: Sinnvoller Fehlertext.
                        controllerContext.Controller.ViewData.ModelState.AddModelError(bindingContext.ModelName, "Ausgewählter Eintrag existiert nicht (mehr)!");
                    }
                }
            }

            return domainEntities;
        }

        private TEntity GetSingleValue(object modelValue) {
            if (modelValue == null) {
                return null;
            }
            Guid businessId;
            int id;
            TEntity entity = null;
            if (Guid.TryParse(modelValue.ToString(), out businessId)) {
                /*Domain-Entity wird anhand der BusinessId gebunden*/
                entity = _dao.GetByBusinessId(businessId);
            } else if (int.TryParse(modelValue.ToString(), out id)) {
                /*Domain-Entity wird anhand der Id gebunden*/
                entity = _dao.GetByPrimaryKey(id);
            }
            return entity;
        }
    }
}