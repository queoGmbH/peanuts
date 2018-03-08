using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Documents;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;
using Com.QueoFlow.Peanuts.Net.Core.Resources;
using Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms;
using Com.QueoFlow.Peanuts.Net.Web.Resources;

using Array = System.Array;

namespace Com.QueoFlow.Peanuts.Net.Web.Helper {
    public class MvcFormExtension<TModel> {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public MvcFormExtension(HtmlHelper<TModel> helper) {
            Helper = helper;
        }

        /// <summary>
        ///     Ruft die Instanz des HtmlHelpers ab.
        /// </summary>
        public HtmlHelper<TModel> Helper { get; private set; }

        public MvcDynamicList<TList> BeginDynamicList<TList>(Expression<Func<TModel, IDictionary<string, TList>>> expression, object htmlAttributes) {
            return BeginDynamicList(expression, new RouteValueDictionary(htmlAttributes));
        }

        public MvcDynamicList<TList> BeginDynamicList<TList>(Expression<Func<TModel, IDictionary<string, TList>>> expression, RouteValueDictionary htmlAttributes = null) {
            DynamicListModel listModel = new DynamicListModel(ExpressionHelper.GetExpressionText(expression), htmlAttributes);
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);

            return new MvcDynamicList<TList>(Helper, listModel, modelMetadata.Model as IDictionary<string, TList>);
        }

        public MvcHtmlString Checkbox(Expression<Func<TModel, bool>> expression, string label = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            CheckboxInputModel checkboxInputModel = new CheckboxInputModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                label);
            return Helper.Partial("EditorTemplates/Forms/ChoiceInput", checkboxInputModel);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="selectableItems"></param>
        /// <param name="keyProperty"></param>
        /// <param name="textProperty"></param>
        /// <param name="placeholder">
        ///     Der Placeholder gibt an, welcher Wert angezeigt wird, wenn keine Auswahl getroffen ist/wurde.
        ///     Ist der Platzhalter null, ist der erste Wert der Box vorausgewählt.
        ///     Wenn der Platzhalter gesetzt ist, symbolisiert dieser den Text, der angezeigt wird, wenn null ausgewählt ist.
        /// </param>
        /// <param name="label"></param>
        /// <param name="lazyUrl">
        ///     Die Url über welche die auswählbaren Einträge nachgeladen werden können. Ist keine Url angegeben,
        ///     werden die Einträge nicht nachgeladen.
        /// </param>
        /// <returns></returns>
        public MvcHtmlString Select<TProperty>(Expression<Func<TModel, TProperty>> expression, string keyProperty, string textProperty,
            IList selectableItems, string placeholder, string label = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            DropDownModel dropDownModel = new DropDownModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                selectableItems,
                keyProperty,
                textProperty,
                label,
                placeholder,
                null);
            return Helper.Partial("EditorTemplates/Forms/Select", dropDownModel);
        }

        /// <summary>
        /// Zeigt als alternative für eine Checkbox ein DropDown für ein bool?-Property an.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="textTrue">Der Text der für den Wert true angezeigt wird oder null wenn <see cref="Resources_Web.label_True"/> angezeigt werden soll.</param>
        /// <param name="textFalse">Der Text der für den Wert false angezeigt wird oder null wenn <see cref="Resources_Web.label_False"/> angezeigt werden soll.</param>
        /// <param name="placeholder"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public MvcHtmlString Select(Expression<Func<TModel, bool?>> expression, string textTrue = null, string textFalse = null, string placeholder = null, string label = null) {
            if (textFalse == null) {
                textFalse = Resources_Web.label_False;
            }
            if (textTrue == null) {
                textTrue = Resources_Web.label_True;
            }
            IList selectableItems;
            if (placeholder != null) {
                selectableItems = new[] { new { value = default(bool?), text = placeholder }, new { value = (bool?)true, text = textTrue }, new { value = (bool?)false, text = textFalse } };
            } else {
                selectableItems = new[] { new { value = true, text = textTrue }, new { value = false, text = textFalse } };
            }

            return Select(expression, "value", "text", selectableItems, placeholder, label);
        }

        /// <summary>
        /// Zeigt als alternative für eine Checkbox ein DropDown für ein bool-Property an.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="textTrue">Der Text der für den Wert true angezeigt wird oder null wenn <see cref="Resources_Web.label_True"/> angezeigt werden soll.</param>
        /// <param name="textFalse">Der Text der für den Wert false angezeigt wird oder null wenn <see cref="Resources_Web.label_False"/> angezeigt werden soll.</param>
        /// <param name="placeholder"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public MvcHtmlString Select(Expression<Func<TModel, bool>> expression, string textTrue = null, string textFalse = null, string placeholder = null, string label = null) {
            if (textFalse == null) {
                textFalse = Resources_Web.label_False;
            }
            if (textTrue == null) {
                textTrue = Resources_Web.label_True;
            }
            IList selectableItems;
            if (placeholder != null) {
                selectableItems = new[] { new { value = default(bool?), text = placeholder }, new { value = (bool?)true, text = textTrue }, new { value = (bool?)false, text = textFalse } };
            } else {
                selectableItems = new[] { new { value = true, text = textTrue }, new { value = false, text = textFalse } };
            }

            return Select(expression, "value", "text", selectableItems, placeholder, label);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="selectableItems"></param>
        /// <param name="placeholder">
        ///     Der Placeholder gibt an, welcher Wert angezeigt wird, wenn keine Auswahl getroffen ist/wurde.
        ///     Ist der Platzhalter null, ist der erste Wert der Box vorausgewählt.
        ///     Wenn der Platzhalter gesetzt ist, symbolisiert dieser den Text, der angezeigt wird, wenn null ausgewählt ist.
        /// </param>
        /// <param name="label"></param>
        /// <param name="lazyUrl">
        ///     Die Url über welche die auswählbaren Einträge nachgeladen werden können. Ist keine Url angegeben,
        ///     werden die Einträge nicht nachgeladen.
        /// </param>
        /// <returns></returns>
        public MvcHtmlString Select<TProperty>(Expression<Func<TModel, TProperty>> expression, IList selectableItems, string placeholder, string label = null, string lazyUrl = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            DropDownModel dropDownModel = new DropDownModel(Helper, modelMetadata, ExpressionHelper.GetExpressionText(expression), selectableItems, label, placeholder, lazyUrl);
            return Helper.Partial("EditorTemplates/Forms/Select", dropDownModel);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TPropertyAndEnum">>Der Typ der Enum, deren Member in der Auswahlbox angezeigt werden sollen.</typeparam>
        /// <param name="expression"></param>
        /// <param name="placeholder">
        ///     Der Placeholder gibt an, welcher Wert angezeigt wird, wenn keine Auswahl getroffen ist/wurde.
        ///     Ist der Platzhalter null, ist der erste Wert der Box vorausgewählt.
        ///     Wenn der Platzhalter gesetzt ist, symbolisiert dieser den Text, der angezeigt wird, wenn null ausgewählt ist.
        /// </param>
        /// <param name="selectableEnumMembers"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public MvcHtmlString Select<TPropertyAndEnum>(Expression<Func<TModel, TPropertyAndEnum>> expression, string placeholder, IList selectableEnumMembers, string label = null) {
            Type baseEnumType = TypeHelper.GetBaseEnumType(typeof(TPropertyAndEnum));

            Array values;
            if (selectableEnumMembers != null) {
                foreach (object selectableEnumMember in selectableEnumMembers) {
                    if (selectableEnumMember.GetType() != baseEnumType) {
                        throw new ArgumentException("TEnum must be of same type as enum Property!");
                    }
                }

                values = new object[selectableEnumMembers.Count];
                selectableEnumMembers.CopyTo(values, 0);
            } else {
                values = Enum.GetValues(baseEnumType);
            }

            IList selectableItems = new List<dynamic>();
            foreach (object value in values) {
                object item = new {
                    Value = value,
                    Text = GetEnumMemberText(baseEnumType, value)
                };
                selectableItems.Add(item);
            }

            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            DropDownModel dropDownModel = new DropDownModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                selectableItems,
                "Value",
                "Text",
                label,
                placeholder);
            return Helper.Partial("EditorTemplates/Forms/Select", dropDownModel);
        }

        public MvcHtmlString SelectDepending<TProperty, TDependsOn>(Expression<Func<TModel, TProperty>> expression, string keyProperty, string textProperty, DependingDropDownOptions<TModel, TProperty, TDependsOn> dependingOptions, string placeholder, string label = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            DropDownModel dropDownModel = new DropDownModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                dependingOptions.DependingValues.SelectMany(dict => dict.Value).Distinct().ToList(),
                keyProperty,
                textProperty,
                label,
                placeholder,
                dependingOptions);
            return Helper.Partial("EditorTemplates/Forms/Select", dropDownModel);
        }

        public MvcHtmlString Date<TProperty>(Expression<Func<TModel, TProperty>> expression, string placeholder = null, string label = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            DateInputModel dateInputModel = new DateInputModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                label,
                placeholder);
            return Helper.Partial("EditorTemplates/Forms/DateTime", dateInputModel);
        }

        public MvcHtmlString DateTime<TProperty>(Expression<Func<TModel, TProperty>> expression, string placeholder = null,
            string label = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            DateTimeInputModel dateTimeInputModel = new DateTimeInputModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                label,
                placeholder);
            return Helper.Partial("EditorTemplates/Forms/DateTime", dateTimeInputModel);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TPropertyAndEnum">>Der Typ der Enum, deren Member in der Auswahlbox angezeigt werden sollen.</typeparam>
        /// <param name="expression"></param>
        /// <param name="placeholder">
        ///     Der Placeholder gibt an, welcher Wert angezeigt wird, wenn keine Auswahl getroffen ist/wurde.
        ///     Ist der Platzhalter null, ist der erste Wert der Box vorausgewählt.
        ///     Wenn der Platzhalter gesetzt ist, symbolisiert dieser den Text, der angezeigt wird, wenn null ausgewählt ist.
        /// </param>
        /// <param name="label"></param>
        /// <returns></returns>
        public MvcHtmlString Select<TPropertyAndEnum>(Expression<Func<TModel, TPropertyAndEnum>> expression, string placeholder,
            string label = null) /*where TPropertyAndEnum : struct, IConvertible*/ {
            if (!typeof(TPropertyAndEnum).IsEnum && !(typeof(TPropertyAndEnum).IsValueType && typeof(TPropertyAndEnum).GenericTypeArguments[0].IsEnum)) {
                throw new ArgumentException("T must be an enumerated type");
            }
            Type baseEnumType;
            if (typeof(TPropertyAndEnum).IsEnum) {
                baseEnumType = typeof(TPropertyAndEnum);
            } else {
                baseEnumType = typeof(TPropertyAndEnum).GenericTypeArguments[0];
            }
            Array values = Enum.GetValues(baseEnumType);
            IList selectableItems = new List<dynamic>();
            foreach (object value in values) {
                object item = new {
                    Value = value,
                    Text = LabelHelper.GetLabelFromResourceByPropertyName<Resources_Domain>(
                        baseEnumType,
                        value.ToString())
                };
                selectableItems.Add(item);
            }

            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            DropDownModel dropDownModel = new DropDownModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                selectableItems,
                "Value",
                "Text",
                label,
                placeholder);
            return Helper.Partial("EditorTemplates/Forms/Select", dropDownModel);
        }

        public MvcHtmlString Email<TProperty>(Expression<Func<TModel, TProperty>> expression, string placeholder = null, string label = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            EmailInputModel emailInputModel = new EmailInputModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                label,
                placeholder);
            return Helper.Partial("EditorTemplates/Forms/Input", emailInputModel);
        }

        public MvcHtmlString FileUpload<TProperty>(Expression<Func<TModel, TProperty>> expression, string placeholder = null, string label = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            FileUploadModel fileUploadModel = new FileUploadModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                label,
                placeholder);
            return Helper.Partial("EditorTemplates/Forms/File", fileUploadModel);
        }

        public MvcHtmlString FileUploadAsync<TProperty>(Expression<Func<TModel, TProperty>> expression, string uploadUrl, string placeholder = null, string label = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            FileAsyncUploadModel fileUploadModel = new FileAsyncUploadModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                label,
                placeholder, uploadUrl);
            return Helper.Partial("EditorTemplates/Forms/FileAsync", fileUploadModel);
        }

        public MvcHtmlString DeleteDocuments(Expression<Func<TModel, IList<Document>>> expression, IList<Document> documents) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);

            return Helper.Partial("EditorTemplates/Forms/DeleteDocuments", new DeleteDocumentsModel(Helper, modelMetadata, ExpressionHelper.GetExpressionText(expression), documents));
        }

        /// <summary>
        ///     Erzeugt ein Hidden-Field für den Ausdruck.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="valueProperty">Die Eigenschaft des Models, die als Wert verwendet werden soll.</param>
        /// <param name="model">
        ///     Überschreibt den Wert, der über den Ausdruck ermittelt wird. Das bietet die Möglichkeit für nicht initialisiert
        ///     ChildModels den Wert zu definieren.
        ///     Ist der Wert NULL, wird der Wert aus dem Model verwendet, der über die <see cref="expression" />geholt wird.
        ///     Der Parameter <see cref="valueProperty" /> wird dann ignoriert.
        /// </param>
        /// <returns></returns>
        public MvcHtmlString Hidden<TProperty>(Expression<Func<TModel, TProperty>> expression, string valueProperty, object model = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);

            if (model == null) {
                /*Model wurde nicht überschrieben, es wird also der Wert über den Pfad ermittelt.*/
                model = modelMetadata.Model;
                if (!string.IsNullOrWhiteSpace(valueProperty) && model != null) {
                    /*Property des Models verwenden*/
                    PropertyInfo propertyInfo = typeof(TProperty).GetProperty(valueProperty);
                    if (propertyInfo != null) {
                        model = propertyInfo.GetValue(model);
                    }
                }
            }

            return Hidden(ExpressionHelper.GetExpressionText(expression), model);
        }

        public MvcHtmlString Hidden<TProperty>(string name, TProperty value) {
            HiddenInputModel textInputModel = new HiddenInputModel(Helper, name, value);
            return Helper.Partial("EditorTemplates/Forms/Hidden", textInputModel);
        }

        public MvcHtmlString Month<TProperty>(Expression<Func<TModel, TProperty>> expression, string placeholder = null, string label = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            MonthInputModel monthInputModel = new MonthInputModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                label,
                placeholder);
            return Helper.Partial("EditorTemplates/Forms/DateTime", monthInputModel);
        }

        public MvcHtmlString Number<TProperty>(Expression<Func<TModel, TProperty>> expression, string placeholder = null,
            string label = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            NumberInputModel numberInputModel = new NumberInputModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                label,
                placeholder);
            return Helper.Partial("EditorTemplates/Forms/Number", numberInputModel);
        }

        public MvcHtmlString Password<TProperty>(Expression<Func<TModel, TProperty>> expression, string placeholder = null,
            string label = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            PasswordInputModel passwordInputModel = new PasswordInputModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                label,
                placeholder);
            return Helper.Partial("EditorTemplates/Forms/Input", passwordInputModel);
        }

        public MvcHtmlString PasswordStrength(Expression<Func<TModel, string>> expression) {

            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            PasswordStrengthModel passwordStrengthModel = new PasswordStrengthModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression));
            return Helper.Partial("EditorTemplates/Forms/PasswordStrength", passwordStrengthModel);
        }


        public MvcHtmlString RadioButton(Expression<Func<TModel, bool>> expression, string label = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            RadioInputModel radioInputModel = new RadioInputModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                label);
            return Helper.Partial("EditorTemplates/Forms/ChoiceInput", radioInputModel);
        }

        /// <summary>
        ///     Zeigt statischen Inhalt (der Wert im Model) mit Label an.
        ///     Der angezeigte Wert wird nicht gebunden!
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="placeholder">Der Wert der angezeigt wird, wenn der Wert null ist.</param>
        /// <param name="label"></param>
        /// <param name="formatString">Optionale Angabe zur Formatierung des Inhaltes.</param>
        /// <param name="staticTemplate">Optionaler Name für das Template zur Darstellung des Werts.</param>
        /// <returns></returns>
        public MvcHtmlString Static<TProperty>(Expression<Func<TModel, TProperty>> expression, string label = null,
            string placeholder = null, string formatString = null, string staticTemplate = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);

            if (modelMetadata.PropertyName != "DisplayName") {
                label = FormControlModel.GetLabel(modelMetadata, label);
            } else {
                string reducedExpression = expression.ToString().Replace(".DisplayName", "").Split(new[] { '.'}, 2, StringSplitOptions.RemoveEmptyEntries)[1];
                ModelMetadata parentModelMetadata = ModelMetadata.FromStringExpression(reducedExpression, Helper.ViewData);
                label = FormControlModel.GetLabel(parentModelMetadata, label);
            }
            

            object value;
            if (modelMetadata.Model != null) {
                /*Wenn die Expression auf einen Wert ungleich NULL zeigt, zeige diesen als ToString an*/
                value = modelMetadata.Model;

                if (modelMetadata.ModelType.IsEnum) {
                    string localizedEnumValue = LabelHelper.GetLabelFromResourceByPropertyName<Resources_Domain>(modelMetadata.ModelType, value.ToString());
                    if (!string.IsNullOrWhiteSpace(localizedEnumValue)) {
                        value = localizedEnumValue;
                    }
                }
                if (!string.IsNullOrWhiteSpace(formatString)) {
                    /*Es ist ein Format für die Ausgabe angegeben => Formatieren!*/
                    value = string.Format(formatString, value);
                }
            } else {
                /*Wenn die Expression auf einen Wert zeigt der NULL ist, wird der Platzhalter-Text verwendet.*/
                value = placeholder;
            }

            StaticControlModel model = new StaticControlModel(Helper, label, value, staticTemplate);
            return Helper.Partial("EditorTemplates/Forms/Static", model);
        }


        /// <summary>
        ///     Zeigt statischen Inhalt mit Label an.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MvcHtmlString Static(string label, object value) {
            StaticControlModel model = new StaticControlModel(Helper, label, value);
            return Helper.Partial("EditorTemplates/Forms/Static", model);
        }

        /// <summary>
        ///     Rendert eine Textbox im Bootstrap-Stil für das übergebene Property.
        /// </summary>
        /// <returns></returns>
        public MvcHtmlString TextArea<TProperty>(Expression<Func<TModel, TProperty>> expression, bool resizeable = false,
            string placeholder = null, string label = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            TextAreaModel textAreaModel = new TextAreaModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                resizeable,
                label,
                placeholder);
            return Helper.Partial("EditorTemplates/Forms/TextArea", textAreaModel);
        }

        /// <summary>
        ///     Rendert eine Textbox im Bootstrap-Stil für das übergebene Property.
        /// </summary>
        /// <returns></returns>
        public MvcHtmlString TextBox<TProperty>(Expression<Func<TModel, TProperty>> expression, string placeholder = null, string label = null, string formatString = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            TextInputModel textInputModel = new TextInputModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                label,
                placeholder);
            return Helper.Partial("EditorTemplates/Forms/Input", textInputModel);
        }

        public MvcHtmlString Time<TProperty>(Expression<Func<TModel, TProperty>> expression, string placeholder = null, string label = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            TimeInputModel timeInputModel = new TimeInputModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                label,
                placeholder);
            return Helper.Partial("EditorTemplates/Forms/DateTime", timeInputModel);
        }

        public MvcHtmlString Url<TProperty>(Expression<Func<TModel, TProperty>> expression, string placeholder = null, string label = null) {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, Helper.ViewData);
            UrlInputModel urlInputModel = new UrlInputModel(Helper,
                modelMetadata,
                ExpressionHelper.GetExpressionText(expression),
                label,
                placeholder);
            return Helper.Partial("EditorTemplates/Forms/Input", urlInputModel);
        }

        private static string GetEnumMemberText(Type baseEnumType, object value) {
            if (value == null) {
                return null;
            }

            if (baseEnumType == typeof(Country)) {
                /*Sonderbehandlung für Country-enum*/
                return Resources_Countries.ResourceManager.GetString(value.ToString());
            }

            string labelFromResourceByPropertyName = LabelHelper.GetLabelFromResourceByPropertyName<Resources_Domain>(baseEnumType, value.ToString());
            if (!string.IsNullOrEmpty(labelFromResourceByPropertyName)) {
                return labelFromResourceByPropertyName;
            }

            return TypeHelper.GetEnumMemberDescription((Enum)value);
        }
    }
}