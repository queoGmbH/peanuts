using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms {
    public class DropDownModel : FormControlModel {
        private readonly IList<string> _selectedItemsValues = new List<string>();
        private readonly string _selectedItemValue;

        /// <summary>
        ///     Initialisiert eine Instanz, bei der die ToString() Methode der SelectableItems sowohl als Value, als auch als Text
        ///     für einen Listeneintrag verwendet werden.
        ///     Zum Beispiel bei String-Listen.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="modelMetaData"></param>
        /// <param name="propertyPath"></param>
        /// <param name="selectableItems"></param>
        /// <param name="label"></param>
        /// <param name="placeholder"></param>
        public DropDownModel(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath, IList selectableItems, string label,
            string placeholder, string lazyUrl = null)
            : base(htmlHelper, modelMetaData, propertyPath, label) {
            Require.NotNull(selectableItems);

            SelectableItems = selectableItems;
            Placeholder = placeholder;

            IsMultiSelect = TypeHelper.IsListType(modelMetaData.ModelType);
            if (IsMultiSelect) {
                /*Es können mehrere Items ausgewählt sein.*/

                /* !!! Muss nach dem setzen von _valueProperty aufgerufen werden !!!*/
                if (modelMetaData.Model != null) {
                    ICollection selectedItems = modelMetaData.Model as ICollection;
                    if (selectedItems != null) {
                        foreach (object selectedItem in selectedItems) {
                            _selectedItemsValues.Add(GetSelectableItemValue(selectedItem));
                        }
                    }
                }
            } else {
                /*Es kann maximal ein Item ausgewählt sein.*/
                /* !!! Muss nach dem setzen von _valueProperty aufgerufen werden !!!*/
                _selectedItemValue = GetSelectableItemValue(modelMetaData.Model);
            }

            LazyUrl = lazyUrl;
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public DropDownModel(HtmlHelper htmlHelper, ModelMetadata modelMetaData, string propertyPath, IList selectableItems, string valuePropertyName,
            string textPropertyName, string label, string placeholder, DependingDropDownOptions dependingDropDownOptions = null)
            : base(htmlHelper, modelMetaData, propertyPath, label) {
            Require.NotNull(selectableItems);

            SelectableItems = selectableItems;
            ValuePropertyName = valuePropertyName;

            TextPropertyName = textPropertyName;
            Placeholder = placeholder;

            DependingDropDownOptions = dependingDropDownOptions;

            IsMultiSelect = TypeHelper.IsListType(modelMetaData.ModelType);
            if (IsMultiSelect) {
                /*Es können mehrere Items ausgewählt sein.*/

                /* !!! Muss nach dem setzen von _valueExpression aufgerufen werden !!!*/
                if (modelMetaData.Model != null) {
                    ICollection selectedItems = modelMetaData.Model as ICollection;
                    if (selectedItems != null) {
                        foreach (object selectedItem in selectedItems) {
                            _selectedItemsValues.Add(GetSelectableItemValue(selectedItem));
                        }
                    }
                }
            } else {
                /*Es kann maximal ein Item ausgewählt sein.*/

                /* !!! Muss nach dem setzen von _valueExpression aufgerufen werden !!!*/
                _selectedItemValue = GetSelectableItemValue(modelMetaData.Model);
            }
        }

        public DependingDropDownOptions DependingDropDownOptions {
            get; set;
        }

        /// <summary>
        ///     Ruft ab, ob in dem DropDown ein Element oder mehrere Elemente ausgewählt werden können.
        /// </summary>
        public bool IsMultiSelect {
            get;
        }

        /// <summary>
        ///     Ruft ab, ob in dem DropDown auch kein Element ausgewählt werden kann.
        ///     Für diese Auswahl wird der Text <see cref="Placeholder" /> verwendet.
        ///     Geht nur für SingleSelect-DropDown.
        /// </summary>
        public bool IsNullable {
            get {
                return Placeholder != null && !IsMultiSelect;
            }
        }

        /// <summary>
        ///     Ruft die URL ab, über welche die auswählbaren Einträge nachgeladen werden können.
        /// </summary>
        public string LazyUrl {
            get; private set;
        }

        /// <summary>
        ///     Ruft den Platzhalter ab, der in der DropDown angezeigt werden soll, wenn kein Element durch den Nutzer ausgewählt
        ///     wurde.
        /// </summary>
        public string Placeholder {
            get;
        }

        /// <summary>
        ///     Ruft die auswählbaren Werte in der DropDown ab.
        /// </summary>
        public IList SelectableItems {
            get; private set;
        }

        /// <summary>
        ///     Ruft den ausgewählten Wert ab.
        /// </summary>
        public object SelectedValue {
            get {
                return ModelMetaData.Model;
            }
        }

        public bool IsDepending {
            get {
                return DependingDropDownOptions != null;
            }
        }



        public string TextPropertyName {
            get; set;
        }

        public string ValuePropertyName {
            get; set;
        }

        public string GetDependsOnAttribute<TModel>(ViewDataDictionary<TModel> viewData) {
            if (DependingDropDownOptions != null) {
                return string.Format("js-depends-on={0}", DependingDropDownOptions.GetDependsOnId());
            } else {
                return "";
            }
        }

        public string GetDependingAttributeMarkup(object dependsOnValue) {
            return string.Format("js-depends-on-value={0}", dependsOnValue);

        }

        public string GetDependingAttribute(object selectableItem) {
            if (DependingDropDownOptions != null) {
                return GetDependingAttributeMarkup(DependingDropDownOptions.GetDependsOnValue(selectableItem));
            } else {
                return "";
            }
        }

        /// <summary>
        ///     Ruft ab, ob die DropDownBox eine Mehrfachauswahl zu lässt und liefert das entsprechende Html-Attribute.
        /// </summary>
        /// <returns></returns>
        public string GetMultipleDropDown() {
            if (IsMultiSelect) {
                /*Es können mehrere Elemente ausgewählt werden*/
                return "multiple=\"multiple\"";
            }

            /*Es kann maximal ein Element ausgewählt werden*/
            return "";
        }

        /// <summary>
        ///     Überprüft für ein Auswählbares Item, ob es ausgewählt ist.
        ///     Ist es ausgewählt wird das von HTML benötigte Keyword "selected" geliefert, andernfalls <see cref="string.Empty" />
        ///     .
        /// </summary>
        /// <param name="selectableItem"></param>
        /// <returns></returns>
        public string GetSelectableItemSelected(object selectableItem) {
            if (IsItemSelected(selectableItem)) {
                /*Das Element ist ausgewählt.*/
                return "selected";
            }

            /*Das Element ist nicht ausgewählt*/
            return string.Empty;
        }

        /// <summary>
        ///     Ruft die Zeichenfolge für das auswählbare Element ab, die als angezeigter Text für das [option]-Tag verwendet wird.
        /// </summary>
        /// <param name="selectableItem">Das auswählbare Element, für welches der Anzeigetext ermittelt werden soll.</param>
        /// <returns></returns>
        public string GetSelectableItemText(object selectableItem) {
            try {
                object value = GetText(selectableItem, TextPropertyName);
                if (value != null) {
                    return value.ToString();
                }
            } catch (Exception) {
                //
            }

            return string.Empty;
        }

        /// <summary>
        ///     Ruft den Wert für das auswählbare Element ab, der als Schlüssel (value) für das [option]-Tag verwendet wird.
        /// </summary>
        /// <param name="selectableItem">Das auswählbare Element, für welches der Schlüssel ermittelt werden soll.</param>
        /// <returns></returns>
        public string GetSelectableItemValue(object selectableItem) {
            try {
                object value = GetValue(selectableItem);
                if (value != null) {
                    return value.ToString();
                }
            } catch (Exception) {
                //
            }

            return string.Empty;
        }

        private static object GetText(object parent, string propertyNameOrPath) {
            if (parent == null) {
                return null;
            }

            if (propertyNameOrPath == null) {
                return parent;
            }

            string[] pathStrings = propertyNameOrPath.Split(new[] { "." }, 2, StringSplitOptions.RemoveEmptyEntries);


            if (pathStrings.Length == 0) {
                return parent;
            }

            PropertyInfo propertyInfo = parent.GetType().GetProperty(pathStrings[0]);
            object propertyValue = propertyInfo.GetValue(parent);

            if (pathStrings.Length == 1) {
                return propertyValue;
            } else {
                return GetText(propertyValue, pathStrings[1]);
            }
        }

        private object GetValue(object item) {
            if (item == null) {
                /*Null bleibt null*/
                return null;
            }

            if (string.IsNullOrWhiteSpace(ValuePropertyName)) {
                /*Kein Name für Value-Property angegeben. Item direkt zurückgeben.*/
                return item;
            }

            /*Wert anhand des PropertyName ermitteln.*/
            Type type = item.GetType();

            if (type.IsEnum) {
                /*Enum ist ein Sonderfall.*/
                return item;
            } else {
                PropertyInfo textProperty = type.GetProperty(ValuePropertyName);
                if (textProperty == null) {
                    return null;
                }

                return textProperty.GetValue(item);
            }
        }

        private bool IsItemSelected(object selectableItem) {
            if (IsMultiSelect) {
                return _selectedItemsValues.Contains(GetSelectableItemValue(selectableItem));
            }

            return GetSelectableItemValue(selectableItem) == _selectedItemValue;
        }

        public IDictionary<string, string> GetDependingPlaceholders() {
            if (DependingDropDownOptions != null) {
                return DependingDropDownOptions.GetDependingPlaceholders();
            } else {
                return new Dictionary<string, string>();
            }
        }
    }
}