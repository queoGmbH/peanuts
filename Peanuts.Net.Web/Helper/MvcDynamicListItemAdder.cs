using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms;

namespace Com.QueoFlow.Peanuts.Net.Web.Helper {
    public class MvcDynamicListItemAdder<TListItem> : IDisposable {
        private const string ITEM_ID = "xxx_index_for_binding_xxx";
        private readonly DynamicListItemModel _dynamicListItemModel;
        private readonly TemplateInfo _indexTemplateInfo;

        private readonly TextWriter _itemAdderTextWriter;
        private readonly HtmlHelper<TListItem> _itemHtmlHelper;
        private readonly HtmlHelper _listHtmlHelper;
        private readonly TListItem _listItem;
        private readonly TextWriter _originalTextWriter;
        private readonly Stack<TextWriter> _viewTextWriterStack;
        private readonly TextWriter _viewTextWriter;
        private readonly Type _viewType;
        private bool _disposed;
        private TemplateInfo _originalTemplateInfo;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public MvcDynamicListItemAdder(HtmlHelper htmlHelper, DynamicListItemModel dynamicListItemModel, TListItem listItem)
                : this(dynamicListItemModel, htmlHelper) {

            _listItem = listItem;
            _itemHtmlHelper.ViewData.Model = listItem;
            _itemHtmlHelper.ViewData.ModelMetadata.Model = listItem;

            BeginDynamicListItem();
        }

        public MvcDynamicListItemAdder(HtmlHelper htmlHelper, DynamicListItemModel dynamicListItemModel) : this(dynamicListItemModel, htmlHelper) {
            

            BeginDynamicListItem();
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        protected MvcDynamicListItemAdder(DynamicListItemModel dynamicListItemModel, HtmlHelper listHtmlHelper) {
            Require.NotNull(listHtmlHelper, "listHtmlHelper");
            Require.NotNull(dynamicListItemModel, "dynamicListItemModel");

            _listHtmlHelper = listHtmlHelper;
            _originalTemplateInfo = listHtmlHelper.ViewData.TemplateInfo;
            _dynamicListItemModel = dynamicListItemModel;

            _viewType = _listHtmlHelper.ViewDataContainer.GetType();
            _viewTextWriterStack = GetPropertyValue<Stack<TextWriter>>(_listHtmlHelper.ViewDataContainer, _viewType, "OutputStack");
            _viewTextWriter = GetPropertyValue<TextWriter>(_listHtmlHelper.ViewDataContainer, _viewType, "Output");
            _itemAdderTextWriter = new TemporaryTextWriter(_viewTextWriter.Encoding);

            TemplateInfo templateInfo = new TemplateInfo();
            templateInfo.HtmlFieldPrefix = dynamicListItemModel.ListExpressionText + "[" + Key + "]";

            _indexTemplateInfo = new TemplateInfo();
            _indexTemplateInfo.HtmlFieldPrefix = dynamicListItemModel.ListExpressionText + "[" + Key + "]";

            ViewDataDictionary viewDataDictionary = new ViewDataDictionary();
            ViewContext viewContext = new ViewContext(listHtmlHelper.ViewContext.Controller.ControllerContext, listHtmlHelper.ViewContext.View, viewDataDictionary, listHtmlHelper.ViewContext.TempData, new TemporaryTextWriter(listHtmlHelper.ViewContext.Writer.Encoding));

            _itemHtmlHelper = new HtmlHelper<TListItem>(viewContext, new ViewPage(), new RouteCollection());
            _itemHtmlHelper.ViewData.TemplateInfo = templateInfo;
            _itemHtmlHelper.ViewContext.ViewData.TemplateInfo = templateInfo;
            ((HtmlHelper)_itemHtmlHelper).ViewData.TemplateInfo = templateInfo;

            _originalTextWriter = listHtmlHelper.ViewContext.Writer;
        }

        public HtmlHelper<TListItem> Html {
            get { return _itemHtmlHelper; }
        }

        public string Key {
            get { return ITEM_ID; }
        }

        /// <summary>
        ///     Ruft das Objekt ab, welches durch diesen Listen-Eintrag abgebildet wird.
        /// </summary>
        public TListItem ListItem {
            get { return _listItem; }
        }

        /// <summary>
        ///     Führt anwendungsspezifische Aufgaben aus, die mit dem Freigeben, Zurückgeben oder Zurücksetzen von nicht
        ///     verwalteten Ressourcen zusammenhängen.
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Gibt die von der aktuellen Instanz der <see cref="T:System.Web.Mvc.Html.MvcForm" />-Klasse verwendeten nicht
        ///     verwalteten Ressourcen und optional auch die verwalteten Ressourcen frei.
        /// </summary>
        /// <param name="disposing">
        ///     true, um sowohl verwaltete als auch nicht verwaltete Ressourcen freizugeben. false, um
        ///     ausschließlich nicht verwaltete Ressourcen freizugeben.
        /// </param>
        protected virtual void Dispose(bool disposing) {
            if (_disposed) {
                return;
            }
            _disposed = true;

            EndDynamicListItem();
        }

        private void BeginDynamicListItem() {
            /*Tag erstellen und alles was darin gerendert wird, als Empty-Item-Template rendern.*/
            if (_dynamicListItemModel.HtmlAttributes.ContainsKey("class")) {
                _dynamicListItemModel.HtmlAttributes["class"] += " " + "dynamic-item-adder";
            } else {
                _dynamicListItemModel.HtmlAttributes.Add("class", "dynamic-item-adder");
            }


            _viewTextWriter.Write("<div " + _dynamicListItemModel.GetHtmlAttributes() + " data-empty-item-template=\"");

            // SetFieldValue(_listHtmlHelper.ViewDataContainer, _viewType, "_currentWriter", _itemAdderTextWriter);
            _viewTextWriterStack.Push(_itemAdderTextWriter);

        }

        private void EndDynamicListItem() {
            
            _listHtmlHelper.ViewContext.Writer = _originalTextWriter;
            _viewTextWriterStack.Pop();
            string html = _itemAdderTextWriter.ToString();
            string replace = Html.Encode(html);
            _listHtmlHelper.ViewContext.Writer.Write(replace);
            
            /*Empty-Item-Template schließen*/
            _listHtmlHelper.ViewContext.Writer.Write("\">");
            _listHtmlHelper.ViewContext.Writer.Write("<button class=\"{0}\"></button>", "add-list-item");
            _listHtmlHelper.ViewContext.Writer.Write("</div>");

            _listHtmlHelper.ViewData.TemplateInfo = _originalTemplateInfo;
            _itemHtmlHelper.ViewContext.ViewData.TemplateInfo = _originalTemplateInfo;
        }

        private TField GetFieldValue<TField>(IViewDataContainer viewDataContainer, Type viewDataContainerType, string fieldName) where TField : class {
            if (viewDataContainerType == null) {
                return null;
            }

            FieldInfo writerPropertyInfo = viewDataContainerType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (writerPropertyInfo == null) {
                return GetFieldValue<TField>(viewDataContainer, viewDataContainerType.BaseType, fieldName);
            }

            return writerPropertyInfo.GetValue(viewDataContainer) as TField;
        }

        private TProperty GetPropertyValue<TProperty>(IViewDataContainer viewDataContainer, Type viewDataContainerType, string propertyName) where TProperty : class {
            if (viewDataContainerType == null) {
                return null;
            }

            PropertyInfo propertyInfo = viewDataContainerType.GetProperty(propertyName);
            if (propertyInfo == null) {
                return GetPropertyValue<TProperty>(viewDataContainer, viewDataContainerType.BaseType, propertyName);
            }

            return propertyInfo.GetValue(viewDataContainer) as TProperty;
        }

        private void SetFieldValue(IViewDataContainer viewDataContainer, Type viewDataContainerType, string fieldName, object value) {
            if (viewDataContainerType == null) {
                return;
            }

            FieldInfo fieldInfo = viewDataContainerType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo == null) {
                SetFieldValue(viewDataContainer, viewDataContainerType.BaseType, fieldName, value);
                return;
            }

            fieldInfo.SetValue(viewDataContainer, value);
        }

        private void SetPropertyValue(IViewDataContainer viewDataContainer, Type viewDataContainerType, string propertyName, object value) {
            if (viewDataContainerType == null) {
                return;
            }

            PropertyInfo propertyInfo = viewDataContainerType.GetProperty(propertyName);
            if (propertyInfo == null) {
                SetPropertyValue(viewDataContainer, viewDataContainerType.BaseType, propertyName, value);
                return;
            }

            viewDataContainerType.InvokeMember(propertyName, BindingFlags.SetProperty, null, viewDataContainer, new[] { value });
        }
    }
}