using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Utils;
using Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate;
using Com.QueoFlow.Peanuts.Net.Core.Resources;
using Com.QueoFlow.Peanuts.Net.Web.Models.Shared.Display;
using Com.QueoFlow.Peanuts.Net.Web.Resources;

using Common.Logging;

namespace Com.QueoFlow.Peanuts.Net.Web.Helper {
    public class MvcDisplayExtension {
        private readonly ILog _logger = LogManager.GetLogger(typeof(MvcDisplayExtension));

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public MvcDisplayExtension(HtmlHelper helper) {
            Helper = helper;
        }

        public HtmlHelper Helper { get; private set; }

        public MvcHtmlString Alert(string alert, AlertType alertType, bool dismissable = false) {
            return Helper.Partial("DisplayTemplates/Display/Alert", new AlertModel(alert, alertType, dismissable));
        }

        public MvcHtmlString AlertFormat(string alert, AlertType alertType, params object[] formatParams) {
            return Alert(string.Format(alert, formatParams), alertType, false);
        }

        public MvcHtmlString AlertFormat(string alert, AlertType alertType, bool dismissable = false, params object[] formatParams) {
            return Alert(string.Format(alert, formatParams), alertType, dismissable);
        }

        /// <summary>
        ///     Erzeugt das HTML für einen Anker.
        /// </summary>
        /// <param name="anchor">Der Anker-Text</param>
        /// <returns></returns>
        public MvcHtmlString Anchor(string anchor) {
            TagBuilder tagBuilder = new TagBuilder("a");
            tagBuilder.Attributes.Add("href", "#" + anchor);

            return new MvcHtmlString(tagBuilder.ToString());
        }

        /// <summary>
        ///     Liefert das Html für ein (Bootstrap)Badge => Bubble mit Text (i.d.R. eine Zahl) drin
        /// </summary>
        /// <param name="badgeValue"></param>
        /// <returns></returns>
        public MvcHtmlString Badge(object badgeValue) {
            TagBuilder badgeTagBuilder = new TagBuilder("span");

            badgeTagBuilder.AddCssClass("badge");
            badgeTagBuilder.InnerHtml = string.Format("{0}", badgeValue);

            return new MvcHtmlString(badgeTagBuilder.ToString());
        }

        ///// <summary>
        /////     Liefert die BreadCrumbs zur aktuell geöffneten Seite/Url.
        /////     http://getbootstrap.com/components/#breadcrumbs
        /////     Wird kein registrierter BreadCrumb gefunden, wird nicht gerendert bzw. eine leere Zeichenfolge geliefert.
        ///// </summary>
        ///// <returns></returns>
        //public MvcHtmlString BreadCrumbs() {
        //    // TODO: Sinnvoll aus dem Kontext holen.
        //    BreadCrumbsByAttributeProvider breadCrumbsByAttributeProvider = new BreadCrumbsByAttributeProvider();
        //    UrlHelper urlHelper = new UrlHelper(Helper.ViewContext.RequestContext);

        //    TagBuilder breadCrumbsTagBuilder = new TagBuilder("ol");
        //    breadCrumbsTagBuilder.AddCssClass("breadcrumb");

        //    StringBuilder breadCrumbsStringBuilder = new StringBuilder();
        //    IBreadCrumb currentBreadCrumb = breadCrumbsByAttributeProvider.GetCurrent(Helper.ViewContext);
        //    IBreadCrumb breadCrumbToRender = currentBreadCrumb;
        //    while (breadCrumbToRender != null) {
        //        string breadCrumbUrl = null;
        //        if (!breadCrumbToRender.Equals(currentBreadCrumb)) {
        //            /*Link nur für nicht aktive BreadCrumbs*/
        //            breadCrumbUrl = breadCrumbToRender.GetUrl(urlHelper);
        //        }
        //        string breadCrumbText = breadCrumbToRender.GetText(Helper.ViewContext.RequestContext);
        //        breadCrumbsStringBuilder.Insert(0, GetBreadCrumb(breadCrumbText, breadCrumbUrl));
        //        breadCrumbToRender = breadCrumbsByAttributeProvider.GetParent(breadCrumbToRender);
        //    }

        //    if (breadCrumbsStringBuilder.Length > 0) {
        //        breadCrumbsTagBuilder.InnerHtml = breadCrumbsStringBuilder.ToString();
        //        return new MvcHtmlString(breadCrumbsTagBuilder.ToString());
        //    }
        //    else {
        //        return new MvcHtmlString(string.Empty);
        //    }
        //}

        /// <summary>
        ///     Zeigt eine Zeitspanne an.
        ///     Dabei wird bei gleichem Datum nur der Beginn angezeigt.
        ///     Ist die Uhrzeit 00:00:00 wird nur das Datum angezeigt.
        /// </summary>
        /// <param name="from">Beginn der Zeitspanne</param>
        /// <param name="to">Ende der Zeitspanne</param>
        /// <returns></returns>
        public MvcHtmlString DateOrDateTimeRange(DateTime? from, DateTime? to) {
            if (from == null && to == null) {
                return new MvcHtmlString(Resources_Web.common_txt_DateRangeNone);
            }

            if (from.HasValue && from.Equals(from.Value.Date) && to.HasValue && to.Equals(to.Value.Date)) {
                /*Beide Zeiten sind 00:00:00 => Uhrzeiten ignorieren*/
                return DateRange(from, to);
            }
            else {
                return DateTimeRange(from, to);
            }
        }

        /// <summary>
        ///     Zeigt eine Zeitspanne ohne Uhrzeiten an.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public MvcHtmlString DateRange(DateTime? from, DateTime? to) {
            if (from == null && to == null) {
                /*Kein Zeitraum*/
                return new MvcHtmlString(Resources_Web.common_txt_DateRangeNone);
            }

            if (!from.HasValue) {
                /*Nur Ende.*/
                return new MvcHtmlString(string.Format(Resources_Web.common_txt_DateRangeEnd, to));
            }
            if (!to.HasValue) {
                /*Nur Beginn.*/
                return new MvcHtmlString(string.Format(Resources_Web.common_txt_DateRangeStart, from));
            }

            if (from.Equals(to)) {
                /*Anfang und Ende ist gleich also als Zeitpunkt*/
                return new MvcHtmlString(string.Format(Resources_Web.common_txt_Date, from));
            }
            else {
                return new MvcHtmlString(string.Format(Resources_Web.common_txt_DateRange, from, to));
            }
        }

        /// <summary>
        ///     Zeigt eine Zeitspanne mit Uhrzeiten an.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public MvcHtmlString DateTimeRange(DateTime? from, DateTime? to) {
            if (from == null && to == null) {
                /*Kein Zeitraum*/
                return new MvcHtmlString(Resources_Web.common_txt_DateRangeNone);
            }

            if (!from.HasValue) {
                /*Nur Ende.*/
                return new MvcHtmlString(string.Format(Resources_Web.common_txt_DateTimeRangeEnd, to));
            }
            if (!to.HasValue) {
                /*Nur Beginn.*/
                return new MvcHtmlString(string.Format(Resources_Web.common_txt_DateTimeRangeStart, from));
            }

            if (from.Equals(to)) {
                /*Anfang und Ende ist gleich also als Zeitpunkt*/
                return new MvcHtmlString(string.Format(Resources_Web.common_txt_DateTime, from));
            }
            else {
                if (from.Value.Date.Equals(to.Value.Date)) {
                    /*innerhalb eines Tages*/
                    return new MvcHtmlString(string.Format(Resources_Web.common_txt_DateWithTimeRange, from, to));
                }
                else {
                    /*Zeitraum über mehrere Tage*/
                    return new MvcHtmlString(string.Format(Resources_Web.common_txt_DateTimeRange, from, to));
                }
            }
        }

        /// <summary>
        ///     Liefert eine Zeichenfolge, welche die Größe einer Datei angibt.
        /// </summary>
        /// <param name="filesize"></param>
        /// <returns></returns>
        public MvcHtmlString Filesize(long filesize) {
            /* Formate und Einheiten stammen von hier:
             * https://de.wikipedia.org/wiki/Byte */

            if (filesize > 1073741824) {
                // GB
                return new MvcHtmlString(string.Format("{0:D} GB", filesize / 1073741824));
            }
            else if (filesize > 1048576 * 5) {
                // MB
                return new MvcHtmlString(string.Format("{0:D} MB", filesize / 1048576));
            }
            else if (filesize > 1024 * 5) {
                // KB
                return new MvcHtmlString(string.Format("{0:D} kB", filesize / 1024));
            }
            else {
                // Byte
                return new MvcHtmlString(string.Format("{0:D} Byte", filesize));
            }
        }

        public MvcHtmlString GetDayString(DateTime day) {
            if (day.Date == DateTime.Today) {
                return new MvcHtmlString("heute");
            }
            if (day.Date == DateTime.Today.AddDays(1)) {
                return new MvcHtmlString("morgen");
            }
            if (day.Date == DateTime.Today.AddDays(2)) {
                return new MvcHtmlString("übermorgen");
            } else {
                return new MvcHtmlString(day.ToString("dddd"));
            }
        }

        public MvcHtmlString Pagination<T>(IPage<T> page) {
            RouteValueDictionary routeValueDictionary = Helper.ViewContext.RouteData.Values;
            NameValueCollection parameterCollection = Helper.ViewContext.HttpContext.Request.QueryString;

            MvcHtmlString paginationPartial = Helper.Partial("DisplayTemplates/Pagination",
                    new PaginationModel(routeValueDictionary,
                            parameterCollection,
                            page.HasNextPage,
                            page.HasPreviousPage,
                            page.PageNumber,
                            page.TotalPages,
                            page.Size));
            return paginationPartial;
        }

        /// <summary>
        ///     Hilfsmethode zur Erzeugung eines Bread-Crumbs.
        ///     http://getbootstrap.com/components/#breadcrumbs
        /// </summary>
        /// <param name="text">Der im BreadCrumb anzuzeigende Text.</param>
        /// <param name="url">Die Url auf welche der BreadCrumb verweisen soll.</param>
        /// <returns></returns>
        private static string GetBreadCrumb(string text, string url = null) {
            TagBuilder pathNodeTag = new TagBuilder("li");

            if (!string.IsNullOrWhiteSpace(url)) {
                /*URL vorhanden => Breadcrumb MIT Link*/
                TagBuilder pathLinkTag = new TagBuilder("a");
                pathLinkTag.Attributes.Add("href", url);
                pathLinkTag.InnerHtml = text;
                pathNodeTag.InnerHtml = pathLinkTag.ToString();
            }
            else {
                /*Keine URL vorhanden => Breadcrumb OHNE Link*/
                pathNodeTag.InnerHtml = text;
            }

            return pathNodeTag.ToString();
        }

        /// <summary>
        /// Rendert den Wert eines Objektes oder liefert einen Platzhalter, wenn der Wert NULL ist.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object ValueOrDefault(object value, string placeholder = "---") {
            if (value != null) {

                if (value.GetType().IsEnum) {
                    // TODO: Funktioniert das bei Nullable<enum>
                    string localizedEnumValue = LabelHelper.GetLabelFromResourceByPropertyName<Resources_Domain>(value.GetType(), value.ToString());
                    if (!string.IsNullOrWhiteSpace(localizedEnumValue)) {
                        value = localizedEnumValue;
                    }
                }


                return value;
            } else {
                return placeholder;
            }
        }
    }



    public class MvcDisplayExtension<TModel> {
        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="T:System.Object"/>-Klasse.
        /// </summary>
        public MvcDisplayExtension(HtmlHelper<TModel> helper) {
            Require.NotNull(helper, "helper");
            
            Helper = helper;
        }

        public HtmlHelper<TModel> Helper { get; private set; }


        public Grid<TModel, TProperty> GridFor<TProperty>(Expression<Func<TModel, IEnumerable<TProperty>>> expression, bool isAllDataAvailable)
        {

            return new Grid<TModel, TProperty>(Helper, expression, isAllDataAvailable);
        }

        public MvcHtmlString Pagination<T>(Expression<Func<TModel, IPage<T>>> pageExpression, string paginationPrefix = null, string pageNumberRouteParameter = "pageNumber", string pageSizeRouteParameter = "pageSize") {
            RouteValueDictionary routeValueDictionary = Helper.ViewContext.RouteData.Values;
            NameValueCollection parameterCollection = Helper.ViewContext.HttpContext.Request.QueryString;
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(pageExpression, Helper.ViewData);
            IPage<T> page = modelMetadata.Model as IPage<T>;

            if (paginationPrefix == null) {
                paginationPrefix = modelMetadata.PropertyName;
            }
            
            MvcHtmlString paginationPartial = Helper.Partial("DisplayTemplates/Pagination",
                    new PaginationModel(routeValueDictionary,
                            parameterCollection,
                            page.HasNextPage,
                            page.HasPreviousPage,
                            page.PageNumber,
                            page.TotalPages,
                            page.Size,
                            paginationPrefix,
                            pageNumberRouteParameter,
                            pageSizeRouteParameter));
            return paginationPartial;
        }

    }
}