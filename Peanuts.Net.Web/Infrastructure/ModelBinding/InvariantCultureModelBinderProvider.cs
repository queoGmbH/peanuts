using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.ModelBinding {

    /// <summary>
    /// ModelBinder-Provider, der Parameter für die im zugewiesenen Typen über den <see cref="InvariantCultureModelBinder{TParseInvariant}"/> bindet.
    /// </summary>
    public class InvariantCultureModelBinderProvider : IModelBinderProvider {
        private IList<Type> _invariantBindingTypes;

        public InvariantCultureModelBinderProvider(IList<Type> invariantBindingTypes) {
            _invariantBindingTypes = invariantBindingTypes;
        }

        public InvariantCultureModelBinderProvider(params Type[] invariantBindingTypes) {
            _invariantBindingTypes = invariantBindingTypes;
        }

        public IList<Type> InvariantBindingTypes {
            get { return new ReadOnlyCollection<Type>(_invariantBindingTypes); }
        }

        public IModelBinder GetBinder(Type modelType) {
            if (_invariantBindingTypes.Contains(modelType)) {
                Type modelBinderType = typeof(InvariantCultureModelBinder<>).MakeGenericType(modelType);
                object instance = Activator.CreateInstance(modelBinderType);
                return instance as IModelBinder;
            }

            return null;
        }
    }
}