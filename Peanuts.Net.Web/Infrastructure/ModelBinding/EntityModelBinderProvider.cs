using System;
using System.Linq;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ModelBinding {
    public class EntityModelBinderProvider : IModelBinderProvider {
        /// <summary>
        ///     Gibt den Modellbinder für den angegebenen Typ zurück.
        /// </summary>
        /// <returns>
        ///     Der Modellbinder für den angegebenen Typ.
        /// </returns>
        /// <param name="modelType">Der Typ des Modells.</param>
        public IModelBinder GetBinder(Type modelType) {
            IModelBinder modelBinder = null;
            if (typeof(Entity).IsAssignableFrom(modelType)) {
                Type modelBinderType = typeof(EntityModelBinder<>).MakeGenericType(modelType);
                Type daoType = typeof(IGenericDao<,>).MakeGenericType(new[] { modelType, typeof(int) });
                object dao = DependencyResolver.Current.GetService(daoType);
                modelBinder = (IModelBinder)Activator.CreateInstance(modelBinderType, new[] { dao });
            } else if (TypeHelper.IsDomainEntityTypeList(modelType)) {
                Type entityType = modelType.GetGenericArguments().First();
                Type modelBinderType = typeof(EntityListModelBinder<>).MakeGenericType(entityType);
                Type daoType = typeof(IGenericDao<,>).MakeGenericType(new[] { entityType, typeof(int) });
                object dao = DependencyResolver.Current.GetService(daoType);
                modelBinder = (IModelBinder)Activator.CreateInstance(modelBinderType, new[] { dao });
            }

            return modelBinder;
        }
    }
}