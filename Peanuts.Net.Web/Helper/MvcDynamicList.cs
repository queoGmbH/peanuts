using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms;

namespace Com.QueoFlow.Peanuts.Net.Web.Helper {
    public class MvcDynamicList<TList> : IDisposable {
        private readonly HtmlHelper _htmlHelper;
        private bool _disposed;
        private readonly DynamicListModel _dynamicListModel;
        private readonly IDictionary<string, TList> _listItems;

        private string _originalTemplatePrefix;

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="T:System.Object"/>-Klasse.
        /// </summary>
        public MvcDynamicList(HtmlHelper htmlHelper, DynamicListModel dynamicListModel, IDictionary<string, TList> listItems) {
            Require.NotNull(htmlHelper, "htmlHelper");
            Require.NotNull(dynamicListModel, "dynamicListModel");

            _htmlHelper = htmlHelper;
            _dynamicListModel = dynamicListModel;

            if (listItems == null) {
                _listItems = new Dictionary<string, TList>();
            }
            else {
                _listItems = listItems;
            }

            BeginDynamicList();
            
            _originalTemplatePrefix = _htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix;

            string temporaryTemplatePrefix = dynamicListModel.ExpressionText;
            if (!string.IsNullOrEmpty(_originalTemplatePrefix)) {
                temporaryTemplatePrefix = _originalTemplatePrefix + "." + temporaryTemplatePrefix;
            }
            _htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = temporaryTemplatePrefix;
        }

        private void BeginDynamicList() {
            _htmlHelper.ViewContext.Writer.Write("<div " + _dynamicListModel.GetHtmlAttributes() + ">");
        }

        private void EndDynamicList() {
            _htmlHelper.ViewContext.Writer.Write("</div>");

            _htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = _originalTemplatePrefix;
        }

        /// <summary>
        ///     Führt anwendungsspezifische Aufgaben aus, die mit dem Freigeben, Zurückgeben oder Zurücksetzen von nicht
        ///     verwalteten Ressourcen zusammenhängen.
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IDictionary<string, TList> ListItems {
            get { return _listItems; }
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

            EndDynamicList();
        }

        public MvcDynamicListItem<TList> BeginListItem(KeyValuePair<string, TList> listItem, RouteValueDictionary htmlAttributes = null) {
            return new MvcDynamicListItem<TList>(_htmlHelper, listItem.Key, new DynamicListItemModel(_dynamicListModel.ExpressionText, htmlAttributes), listItem.Value);
        }

        public MvcDynamicListItem<TList> BeginListItem(string key, TList listItem, RouteValueDictionary htmlAttributes = null) {
            return new MvcDynamicListItem<TList>(_htmlHelper, key, new DynamicListItemModel(_dynamicListModel.ExpressionText, htmlAttributes), listItem);
        }

        public MvcDynamicListItemAdder<TList> BeginListItemAdder(TList defaultValue,  RouteValueDictionary htmlAttributes = null) {
            return new MvcDynamicListItemAdder<TList>(_htmlHelper, new DynamicListItemModel(_dynamicListModel.ExpressionText, htmlAttributes), defaultValue);
        }

        public MvcDynamicListItemAdder<TList> BeginListItemAdder(RouteValueDictionary htmlAttributes = null) {
            return new MvcDynamicListItemAdder<TList>(_htmlHelper, new DynamicListItemModel(_dynamicListModel.ExpressionText, htmlAttributes));
        }
    }
}