using System;
using System.Web.Mvc;
using System.Web.Routing;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Forms;

namespace Com.QueoFlow.Peanuts.Net.Web.Helper {
    public class MvcDynamicListItem<TListItem> : IDisposable {

        private readonly HtmlHelper _listHtmlHelper;
        private bool _disposed;
        private readonly DynamicListItemModel _dynamicListItemModel;
        private readonly TListItem _listItem;

        private readonly HtmlHelper<TListItem> _itemHtmlHelper;
        private TemplateInfo _originalTemplateInfo;

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="T:System.Object"/>-Klasse.
        /// </summary>
        public MvcDynamicListItem(HtmlHelper htmlHelper, string key, DynamicListItemModel dynamicListItemModel, TListItem listItem)
            : this(key, dynamicListItemModel, htmlHelper) {

            _listItem = listItem;
            _itemHtmlHelper.ViewData.Model = listItem;
        }

        public MvcDynamicListItem(HtmlHelper htmlHelper, string key, DynamicListItemModel dynamicListItemModel)
            : this(key, dynamicListItemModel, htmlHelper) {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="T:System.Object"/>-Klasse.
        /// </summary>
        protected MvcDynamicListItem(string key, DynamicListItemModel dynamicListItemModel, HtmlHelper listHtmlHelper) {
            Require.NotNull(listHtmlHelper, "listHtmlHelper");
            Require.NotNull(dynamicListItemModel, "dynamicListItemModel");
            Require.NotNullOrWhiteSpace(key, "key");

            Key = key;

            _listHtmlHelper = listHtmlHelper;
            _dynamicListItemModel = dynamicListItemModel;
            _originalTemplateInfo = listHtmlHelper.ViewData.TemplateInfo;

            ViewDataDictionary viewDataDictionary = _listHtmlHelper.ViewData;
            ViewContext viewContext = new ViewContext(listHtmlHelper.ViewContext.Controller.ControllerContext, listHtmlHelper.ViewContext.View, viewDataDictionary, listHtmlHelper.ViewContext.TempData, listHtmlHelper.ViewContext.Writer);

            TemplateInfo templateInfo = new TemplateInfo();
            _itemHtmlHelper = new HtmlHelper<TListItem>(viewContext, new ViewPage(), new RouteCollection());
            _itemHtmlHelper.ViewData.TemplateInfo = templateInfo;
            _itemHtmlHelper.ViewContext.ViewData.TemplateInfo = templateInfo;
            ((HtmlHelper)_itemHtmlHelper).ViewData.TemplateInfo = templateInfo;
            ((HtmlHelper)_itemHtmlHelper).ViewContext.ViewData.TemplateInfo = templateInfo;

            foreach (string modelStateKey in _listHtmlHelper.ViewData.ModelState.Keys) {
                _itemHtmlHelper.ViewData.ModelState.Add(modelStateKey, _listHtmlHelper.ViewData.ModelState[modelStateKey]);
                ((HtmlHelper)_itemHtmlHelper).ViewData.ModelState.Add(modelStateKey, _listHtmlHelper.ViewData.ModelState[modelStateKey]);
            }
            
            templateInfo.HtmlFieldPrefix = dynamicListItemModel.ListExpressionText + "[" + Key + "]";

            /*Das ist der Key für den Dictionary-Eintrag*/
            _itemHtmlHelper.ViewContext.Writer.Write("<div " + _dynamicListItemModel.GetHtmlAttributes() + ">");
            //_itemHtmlHelper.ViewContext.Writer.Write("<input name=\"" + templateInfo.HtmlFieldPrefix + "\" type=\"hidden\" value=\"" + Key + "\">");

            /*Damit die Werte für die Dictionary-Values gebunden werden können, muss value davor.*/
            templateInfo.HtmlFieldPrefix = dynamicListItemModel.ListExpressionText + "[" + Key + "]";
            
        }

        /// <summary>
        /// Ruft die eindeutige Id dieses Eintrags ab.
        /// </summary>
        public string Key { get; private set; }

        public HtmlHelper<TListItem> Html {
            get { return _itemHtmlHelper; }
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

        /// <summary>
        ///     Führt anwendungsspezifische Aufgaben aus, die mit dem Freigeben, Zurückgeben oder Zurücksetzen von nicht
        ///     verwalteten Ressourcen zusammenhängen.
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void EndDynamicListItem() {
            _itemHtmlHelper.ViewContext.Writer.Write("</div>");

            _itemHtmlHelper.ViewData.TemplateInfo = _originalTemplateInfo;
            _itemHtmlHelper.ViewContext.ViewData.TemplateInfo = _originalTemplateInfo;
            ((HtmlHelper)_itemHtmlHelper).ViewData.TemplateInfo = _originalTemplateInfo;
            ((HtmlHelper)_itemHtmlHelper).ViewContext.ViewData.TemplateInfo = _originalTemplateInfo;
        }

        /// <summary>
        /// Ruft das Objekt ab, welches durch diesen Listen-Eintrag abgebildet wird.
        /// </summary>
        public TListItem ListItem {
            get { return _listItem; }
        }
    }
}