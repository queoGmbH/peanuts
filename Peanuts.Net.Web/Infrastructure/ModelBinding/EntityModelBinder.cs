using System;
using System.Security.Principal;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

using Common.Logging;

using Microsoft.AspNet.Identity;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ModelBinding {
    public class EntityModelBinder<TEntity> : IModelBinder where TEntity: Entity {
        private readonly IGenericDao<TEntity, int> _dao;

        private readonly ILog _logger = LogManager.GetLogger(typeof(EntityModelBinder<>));

        public EntityModelBinder(IGenericDao<TEntity, int> dao) {
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
            object boundModel = null;
            string modelName = bindingContext.ModelName;
            
            if (IsSpecialNamedParameter(modelName)) {
                boundModel = BindSpecialValue(modelName, controllerContext);
            } else {
                ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
                if (valueProviderResult != null){
                    try {
                        boundModel = GetSingleValue(valueProviderResult.AttemptedValue);
                    } catch (Exception ex) {
                        _logger.ErrorFormat("Beim Binden eines Objektes vom Typ [{0}] mit der Id [{1}] ist ein Fehler aufgetreten.", ex, bindingContext.ModelType, valueProviderResult.AttemptedValue);
                        // TODO: Sinnvoller Fehlertext.
                        controllerContext.Controller.ViewData.ModelState.AddModelError(bindingContext.ModelName, "Ausgewählter Eintrag existiert nicht (mehr)!");
                    }
                    
                }
            }
            
            return boundModel;
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

        private object BindSpecialValue(string modelName, ControllerContext controllerContext) {
            TEntity entity = null;
            if (modelName.Equals("currentUser")) {
                try {
                    IIdentity currentIdentity = controllerContext.HttpContext.User.Identity;
                    string  userIdAsString = currentIdentity.GetUserId<string>();
                    Guid businessId = Guid.Parse(userIdAsString);
                    entity = _dao.GetByBusinessId(businessId);
                } catch (Exception ex) {
                    _logger.Error(ex);
                }
            }
            return entity;
        }

        private bool IsSpecialNamedParameter(string modelName) {
            return modelName.Equals("currentUser");
        }
    }
}