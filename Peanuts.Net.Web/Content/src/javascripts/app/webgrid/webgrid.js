/* Java-Script-Code f�r das queo Webgrid, welches f�r Pagination und Sortierung verantwortlich ist. */

/*Name des Events, das ausgel�st wird, wenn in einer Spalte gefiltert wird.*/
var WEBGRID_COLUMN_FILTERED_EVENT = "webgrid.column:filtered";

/*Name des Events, das gefeuert wird, wenn die Sortierung ge�ndert wurde.*/
var WEBGRID_COLUMN_SORTED_EVENT = "webgrid.column:sorted";

/*Name des Events, das gefeuert wird, wenn eine Spalte ausgeblendet wurde.*/
var WEBGRID_COLUMN_HIDDEN_EVENT = "webgrid.column:hidden";

/*Name des Events, das gefeuert wird, wenn eine Spalte eingeblendet wurde.*/
var WEBGRID_COLUMN_SHOWN_EVENT = "webgrid.column:shown";

/*Name des Events, das gefeuert wird, wenn eine Spalte ausgeblendet werden soll.*/
var WEBGRID_COLUMN_HIDE_EVENT = "webgrid.column:hide";

/*Name des Events, das gefeuert wird, wenn eine Spalte ausgeblendet werden soll.*/
var WEBGRID_COLUMN_REQUEST_HIDE_EVENT = "webgrid.column:request_hide";

/*Name des Events, das gefeuert wird, wenn eine Spalte eingeblendet werden soll.*/
var WEBGRID_COLUMN_SHOW_EVENT = "webgrid.column:show";

/*Name des Events, das gefeuert wird, wenn eine Spalte eingeblendet werden soll.*/
var WEBGRID_COLUMN_REQUEST_SHOW_EVENT = "webgrid.column:request_show";

/*Name des Events, das gefeuert wird, wenn das gesamte Grid selektiert werden soll.*/
var WEBGRID_COLUMN_REQUEST_GRID_SELECTOR_EVENT = "webgrid.column:request_gridselector";

/*Name des Events, das gefeuert wird, wenn eine Seite der Tabelle ge�ffnet wurde / angezeigt wird.*/
var WEBGRID_PAGE_GONE_TO_EVENT = "webgrid.pagination:gone-to";

/*Name des Events, das gefeuert wird, wenn eine Seite der Tabelle ge�ffnet/angezeigt werden soll.*/
var WEBGRID_PAGE_GOTO_EVENT = "webgrid.pagination:goto";


var WebGrid = (function () {

    function getVisibilityCookieName(gridId, columnId) {
        return "Visibility#" + gridId + "#" + columnId;
    }

    function getCurrentSortingColumnCookieName(gridId) {
        return gridId + "_Sort";
    }

    function getCurrentSortingDirectionCookieName(gridId) {
        return gridId + "_SortDirection";
    }

    function getGridSelector($grid) {
        var $gridSelector = $grid.triggerHandler(WEBGRID_COLUMN_REQUEST_GRID_SELECTOR_EVENT, $grid);
        return $gridSelector || $grid;
    }


    /* Klasse zum Abbilden einer Spalte */
    function webGridColumn(grid, columnName) {

        var gridId = $(grid).attr("id");
        var $headerCell = privateGetHeaderCell();

        var canFilter = $headerCell.attr("webgrid-column-can-filter") == "True";
        var canHide = $headerCell.attr("webgrid-column-can-hide") == "True";
        var canSort = $headerCell.attr("webgrid-column-can-sort") == "True";

        var saveFilter = $headerCell.attr("webgrid-column-save-filter") == "True";
        var filterKey = $headerCell.attr("webgrid-column-save-filter-key") || gridId + "_" + columnName;

        var toggleFilterButton = null;
        if (canFilter) {
            toggleFilterButton = $("a.webgrid-filter-by-column-link", $headerCell);
        }

        function privateGetHeaderCell() {
            return $(".th[webgrid-column-name='" + columnName + "'], th[webgrid-column-name='" + columnName + "']", getGridSelector($(grid))) || $();
        }

        function privateGetAllHeaderCells() {
            return $(".th, th, .td, td", getGridSelector($(grid)));
        }

        function privateToggleColumn() {

            if (canHide != true) {
                return;
            }

            if (privateIsVisible()) {
                privateHideColumn();
            } else {
                privateShowColumn();
            }
        }

        function privateIsVisible() {

            var columnCells = $("[webgrid-column-name='" + columnName + "']", grid);
            return !($(columnCells).hasClass('hidden'));
        }

        function privateHideColumn() {
            $(grid).trigger(WEBGRID_COLUMN_HIDE_EVENT, columnName);
            var columnCells = $("[webgrid-column-name='" + columnName + "']", grid);
            /* Spalte wird ausgeblendet */
            $(columnCells).addClass('hidden');
            $.cookie(getVisibilityCookieName(gridId, columnName), false, { expires: 10000 });
            $(grid).trigger(WEBGRID_COLUMN_HIDDEN_EVENT, columnName);
        }

        function privateShowColumn() {
            $(grid).trigger(WEBGRID_COLUMN_SHOW_EVENT, columnName);
            var columnCells = $("[webgrid-column-name='" + columnName + "']", grid);
            /* Spalte wird wieder angezeigt */
            $(columnCells).removeClass('hidden');
            $.cookie(getVisibilityCookieName(gridId, columnName), true, { expires: 10000 });
            $(grid).trigger(WEBGRID_COLUMN_SHOWN_EVENT, columnName);
        }

        function privateSaveColumnFilterToCookie(filterValue, isFilter) {
            var arr = privateGetColumnFilterValuesFromCookie();
            if (isFilter != true) {
                var indexInArray = $.inArray(filterValue, arr);
                if (indexInArray >= 0) {
                    /*Remove from array when in*/
                    arr.splice(indexInArray, 1);
                }
            } else {
                if ($.inArray(filterValue, arr) < 0) {
                    /*Add to array when not already in*/
                    arr.push(filterValue);
                }
            }

            if (arr.length) {
                var jsonStr = JSON.stringify(arr);
                $.cookie(filterKey, jsonStr);
            } else {
                $.removeCookie(filterKey);
            }

        }

        function privateGetColumnFilterValuesFromCookie() {
            if ($.cookie(filterKey)) {
                var jsonStr = $.cookie(filterKey);
                if (jsonStr) {
                    return JSON.parse(jsonStr);
                }
            }
            return new Array();
        }

        function privateFilterByColumn(filterValue, isValueFiltered) {
            if (canFilter != true) {
                return;
            }

            var webGridBody = $("tbody, .tbody", $(grid));
            var tableRows = $("tr.webGridRow, .tr.webGridRow", webGridBody);
            tableRows.each(function () {
                var row = $(this);
                var cell = $("[webgrid-column-name='" + columnName + "']", row);
                var valueAsText = $(cell).attr("webgrid-cell-value-as-text");

                if (isValueFiltered && valueAsText == filterValue) {
                    // cell is now filtered
                    $(cell).addClass('filtered');
                }
                if (!isValueFiltered && valueAsText == filterValue) {
                    // cell is no more filtered
                    $(cell).removeClass('filtered');
                }
            });

            /*Trigger Event to reload table*/
            $(grid).trigger(WEBGRID_COLUMN_FILTERED_EVENT, this);

            /* Change Icon depending on is filtered */
            setFilterByColumnIcon();

            /* Save Filtered Values to Cookie */
            privateSaveColumnFilterToCookie(filterValue, isValueFiltered);

            /*Status der alle-Checkbox aktualisieren*/
            refreshSelectAllCheckBox();

        }

        /*Aktualisiert das Filter-Icon im Spalten-Header*/
        function setFilterByColumnIcon() {
            if (canFilter != true) {
                return;
            }

            var $headerCell = privateGetHeaderCell();
            var contentCanFilterByColumn = $headerCell.attr("can-filter-by-column-content");
            var contentIsFilterByColumn = $headerCell.attr("is-filter-by-column-content");
            var filteredCells = $("td[webgrid-column-name='" + columnName + "'].filtered, .td[webgrid-column-name='" + columnName + "'].filtered");
            var filterLink = $("a.webgrid-filter-by-column-link", $headerCell);
            if (filterLink != "") {
                if (filteredCells.length > 0) {
                    $(filterLink).html(contentIsFilterByColumn);
                    $(filterLink).addClass("filtered");
                } else {
                    $(filterLink).html(contentCanFilterByColumn);
                    $(filterLink).removeClass("filtered");
                }
            }
        }

        function refreshSelectAllCheckBox() {
            if (canFilter != true) {
                return;
            }

            /* control the check all checkbox */
            var filterByColumnDiv = $("#filter_by_column_" + gridId + "_" + columnName);
            var selectAllCheckBox = $('input[type="checkbox"].filter-all-checkbox', filterByColumnDiv);
            var uncheckedSingleSelectionCheckBoxes = $('input:checkbox:not(:checked).filter-single-checkbox', filterByColumnDiv);
            var isChecked = uncheckedSingleSelectionCheckBoxes.length > 0;
            $(selectAllCheckBox).prop('checked', !isChecked);
        }

        /*Sortiert nach dieser Spalte.*/
        function privateSortByColumn(sortDirection) {

            if (canSort != true) {
                return;
            }
            if (!sortDirection) {
                sortDirection = $(grid).attr("sort-column", columnName);
            }

            if (sortDirection == "desc") {
                /* Wird beim Sortieren rumgedreht, deswegen das Gegenteil dranschreiben. */
                $(grid).attr("sort-column", columnName);
                $(grid).attr("sort-direction", "asc");
            }

            var webGridBody = $("tbody, .tbody", grid);
            var currentSortColumn = $(grid).attr("sort-column");
            var currentSortDirection = $(grid).attr("sort-direction");
            var newSortDirection = "asc";
            if (currentSortColumn == columnName && currentSortDirection == "asc") {
                newSortDirection = "desc";
            }

            var tableRows = $("tr.webGridRow, .tr.webGridRow", webGridBody);
            var rowsToSort = $.makeArray($(tableRows).remove());
            var sortedRows = rowsToSort.sort(function (a, b) {
                var arg1 = $("[webgrid-column-name='" + columnName + "']", a).attr("webgrid-row-index");
                var arg2 = $("[webgrid-column-name='" + columnName + "']", b).attr("webgrid-row-index");
                if (newSortDirection == "asc") {
                    return arg1 - arg2;
                } else {
                    return arg2 - arg1;
                }

            });
            $(webGridBody).html(sortedRows);

            privateGetAllHeaderCells().each(function () {
                var $cell = $(this);
                var contentSortedNone = $(this).attr("sort-content-none");
                var contentSortedAsc = $(this).attr("sort-content-asc");
                var contentSortedDesc = $(this).attr("sort-content-desc");

                // Spaltenheader Sortiertext anpassen.
                var $sortLink = $(".webgrid-sort-by-column-link", this);
                if ($sortLink != "") {

                    var sortContent = contentSortedNone;

                    if ($cell.attr("webgrid-column-name") == columnName) {
                        if (newSortDirection == "desc") {
                            sortContent = contentSortedDesc;
                            $sortLink.attr("data-sort", "desc");
                            $cell.attr("data-sort", "desc");
                        } else {
                            sortContent = contentSortedAsc;
                            $sortLink.attr("data-sort", "asc");
                            $cell.attr("data-sort", "asc");
                        }
                    } else {
                        $sortLink.attr("data-sort", "none");
                        $cell.attr("data-sort", "none");
                    }

                    $sortLink.html(sortContent);

                }
            });

            $.cookie(getCurrentSortingColumnCookieName(gridId), columnName);
            $.cookie(getCurrentSortingDirectionCookieName(gridId), newSortDirection);
            $(grid).attr("sort-column", columnName);
            $(grid).attr("sort-direction", newSortDirection);

            $(grid).trigger(WEBGRID_COLUMN_SORTED_EVENT, [columnName, newSortDirection == "desc"]);

        }

        /*Registriert die Event-Handler f�r das Sortieren nach dieser Spalte*/
        function registerSortByColumn() {
            if (canSort == true) {
                $("a.webgrid-sort-by-column-link", privateGetHeaderCell()).on("click", function (e) {
                    e.preventDefault();
                    privateSortByColumn();
                });
            }
        }

        /* Blendet den Filter ein. */
        function privateShowFilter() {

            if (canFilter == true) {
                // Auslesen des zugeh�rigen Elements
                var filterControlSelector = "#" + $(toggleFilterButton).attr("data-toggle-target");
                var filterControl = $(filterControlSelector, privateGetHeaderCell());
                filterControl.on('click', function (event) {
                    event.stopPropagation();
                });

                // Wenn irgendwohin geklickt wird, dann wieder zu machen
                $(document).on('click', function () {
                    filterControl.slideUp('fast');
                });

                /*Wenn auf anderen Filter geklickt wird, dann dieses Filter-Menu schlie�en*/
                $('.webgrid-filter-by-column-link').click(function (e) {
                    if (toggleFilterButton[0] != e.currentTarget) {
                        filterControl.slideUp('fast');
                    }
                });

                // Menu anzeigen
                filterControl.slideToggle('fast');
            }
        }

        /*Registriert das Einblenden des Filters f�r diese Spalte ein.*/
        function registerFilter() {
            if (canFilter == true) {
                $(toggleFilterButton).on('click', function (clickEvent) {
                    clickEvent.preventDefault();
                    clickEvent.stopPropagation();
                    privateShowFilter();
                });

                /*Checkbox f�r alle*/
                var filterByColumnDiv = $("#filter_by_column_" + gridId + "_" + columnName);
                var checkBoxes = $('input[type="checkbox"].filter-single-checkbox', filterByColumnDiv);
                var filterAllCheckbox = $(".filter-all-checkbox", filterByColumnDiv);

                /*Filter f�r nach allen Filtern registrieren*/
                $(filterAllCheckbox).on("change", function () {

                    var isFilterAllCheckBoxChecked = $(filterAllCheckbox).prop('checked');
                    checkBoxes.each(function (index, singleFilterCheckbox) {
                        /*Either check or uncheck the singleselection check*/
                        $(singleFilterCheckbox).prop('checked', isFilterAllCheckBoxChecked);
                        $(singleFilterCheckbox).change();
                    });
                });

                /*Einzelne Checkboxen f�r separate Werte*/
                $(checkBoxes).each(function (index, checkBox) {
                    $(checkBox).on("change", function (e) {
                        privateFilterByColumn($(checkBox).data("filter-value"), !($(checkBox).is(":checked")));
                    });
                });

                if (saveFilter) {
                    var filteredValues = privateGetColumnFilterValuesFromCookie();
                    $(filteredValues).each(function (index, filteredValue) {
                        privateFilterByColumn(filteredValue, true);
                        var accordingCheckbox = $('input[type="checkbox"][data-filter-value="' + filteredValue + '"].filter-single-checkbox', filterByColumnDiv);
                        if (accordingCheckbox.length > 0) {
                            accordingCheckbox.prop('checked', false);
                            $('input[type="checkbox"].filter-all-checkbox', filterByColumnDiv).prop('checked', false);
                        }
                    });

                }
            }


        }

        /* Init visibility from cookie */
        var cookieVisible = $.cookie(getVisibilityCookieName(gridId, columnName));
        if (cookieVisible == "false") {
            $("[webgrid-column-name='" + columnName + "']", grid).addClass('hidden');
        } else if (cookieVisible == "true") {
            $("[webgrid-column-name='" + columnName + "']", grid).removeClass('hidden');
        }

        registerSortByColumn();
        registerFilter();

        return {
            toggle: privateToggleColumn,
            show: privateShowColumn,
            hide: privateHideColumn,
            filterByColumn: privateFilterByColumn,
            sortByColumn: privateSortByColumn,
            name: columnName,
            webGrid: grid,
            canHide: canHide,
            canFilter: canFilter,
            showFilter: privateShowFilter

        };
    };

    /*Klasse zum Abbilden der Pagination.*/
    function webGridPaginationControl(paginationControl) {

        function privateGoneToPage(pageToShow, maxPageNumber) {

            if (pageToShow < 1 || pageToShow == undefined) {
                pageToShow = 1;
            }

            if (pageToShow > maxPageNumber) {
                pageToShow = maxPageNumber;
            }

            var directPageLinkControls = $("li.directpagelink", paginationControl);
            var directPageLinksCount = directPageLinkControls.length;
            var lastPageInRange = directPageLinksCount;
            for (var firstPageInRange = 1; firstPageInRange < maxPageNumber && lastPageInRange < maxPageNumber && firstPageInRange < (pageToShow - directPageLinksCount / 2) ; firstPageInRange++) {
                lastPageInRange++;
            }

            var pageNumber = firstPageInRange;
            $(directPageLinkControls).removeClass("active");
            $(directPageLinkControls).each(function () {
                $("a", this).html(pageNumber);
                $("a", this).data("goto-page", pageNumber);
                if (pageToShow == pageNumber) {
                    $(this).addClass("active");
                }
                if (pageNumber > maxPageNumber) {
                    $(this).hide();
                } else {
                    $(this).show();
                }
                pageNumber++;
            });
        }

        function privateGoToPage(pageToShow) {
            $(that).trigger(WEBGRID_PAGE_GOTO_EVENT, { page: pageToShow });
        }

        if (paginationControl.length <= 0) {
            /* No Pagination control => skip */
            return undefined;
        } else {
            /*Trigger Event On Button Click*/
            $("li a", paginationControl).click(function (e) {
                privateGoToPage($(this).data('goto-page'));
                e.preventDefault();
            });

            var that = {
                goToPage: privateGoToPage,
                goneToPage: privateGoneToPage,
            };

            return that;
        }
    };

    /*Klasse zum Abbilden des Controls, mit dem die Sichtbarkeit von Spalten kontrolliert werden kann.*/
    function webGridVisibilityControl(grid) {

        var visibilityControl = $("#column_visibility_" + $(grid).attr("id"));
        if ($(visibilityControl).length == 0) {
            return null;
        }

        function privateToggleColumnGroup(columnGroupName) {

            var isChecked = $(".toggle-column-group[column-header-group='" + columnGroupName + "']").prop('checked');

            /* Spaltenk�pfe die zur Gruppe geh�ren ermitteln */
            var columnHeaderCells = $(".toggle-column[column-header-group='" + columnGroupName + "']", visibilityControl);

            $(columnHeaderCells).each(function () {
                /* Namen der Spalte suchen, die ein/ausgeblendet werden soll */
                var columnName = $(this).attr("webgrid-column-name");

                if (isChecked) {
                    privateShowColumn(columnName);
                } else {
                    privateHideColumn(columnName);
                }

            });

            $(".toggle-column-group[column-header-group='" + columnGroupName + "']").prop('checked', isChecked);
        }

        /*Triggered das Event, zum Anfordern des Einblendens der Spalte*/
        function privateShowColumn(columnName) {
            $(grid).trigger(WEBGRID_COLUMN_REQUEST_SHOW_EVENT, columnName);
        }

        /*Triggered das Event, zum Anfordern des Ausblendens der Spalte*/
        function privateHideColumn(columnName) {
            $(grid).trigger(WEBGRID_COLUMN_REQUEST_HIDE_EVENT, columnName);
        }

        /*Aktualisiert den checked-Status des Parent-Togglers f�r eine Gruppe*/
        function privateRefreshColumnGroupVisibility(columnGroupName) {

            var groupInput = $(".toggle-column-group[column-header-group='" + columnGroupName + "']", visibilityControl);

            /*Wenn mindestens 1 Child nicht gecheckt ist, dann parent auch inaktiv*/
            var areAnyChildsUnchecked = ($(".toggle-column[column-header-group='" + columnGroupName + "']:not(:checked)", visibilityControl).length > 0);
            groupInput.prop('checked', !areAnyChildsUnchecked);

        }

        /*Initialisierung Toggle-Column-Group-Checkboxen*/
        $("input[type=checkbox].toggle-column-group", $(visibilityControl)).each(function (index, groupCheckBox) {

            var columnGroupName = $(groupCheckBox).attr("column-header-group");

            $(groupCheckBox).click(function () {
                privateToggleColumnGroup(columnGroupName);
            });
        });

        /*Initialisierung Toggle-Column-Checkboxen*/
        $("input[type=checkbox].toggle-column", $(visibilityControl)).each(function (index, columnCheckBox) {

            var columnName = $(columnCheckBox).attr("webgrid-column-name");

            $(columnCheckBox).click(function () {
                var show = $(columnCheckBox).prop('checked');
                if (show) {
                    privateShowColumn(columnName);
                } else {
                    privateHideColumn(columnName);
                }
            });
        });

        /*Initialisierung der Buttons zum Expandieren von Header-Groups*/
        $("button.toggle-column-group", $(visibilityControl)).each(function (index, toggleButton) {
            var itemsDiv = $(toggleButton).parent("div.column-header-group").find(".column-header-group-items");

            $(toggleButton).click(function () {
                $(toggleButton).toggleClass("expanded");
                $(itemsDiv).toggleClass('visible');
            });
        });

        /*Checkbox aktivieren, wenn eine Spalte eingblendet wurde.*/
        $(grid).on(WEBGRID_COLUMN_SHOWN_EVENT, function (e, columnName) {
            var accordingInput = $(".toggle-column[webgrid-column-name='" + columnName + "']");
            accordingInput.prop('checked', true);
            if (accordingInput.attr("column-header-group")) {
                privateRefreshColumnGroupVisibility(accordingInput.attr("column-header-group"));
            }
        });

        /*Checkbox deaktivieren, wenn eine Spalte ausgeblendet wurde.*/
        $(grid).on(WEBGRID_COLUMN_HIDDEN_EVENT, function (e, columnName) {
            var accordingInput = $(".toggle-column[webgrid-column-name='" + columnName + "']");
            accordingInput.prop('checked', false);

            if (accordingInput.attr("column-header-group")) {
                privateRefreshColumnGroupVisibility(accordingInput.attr("column-header-group"));
            }
        });

        return {

        };
    }

    /*Klasse zur Abbildung eines WebGrids*/
    function webGridControl(tableSelector) {

        function privateGetColumnByName(name) {
            var columnFound = null;
            $(columns).each(function (index, column) {
                if (column.name == name) {
                    columnFound = column;
                }
            });

            return columnFound;
        }

        function privateGetPaginationControlOrNull() {

            var paginationControl = $("ul.pagination[pagination-table='" + $(grid).attr('id') + "']");
            if (paginationControl.length > 0) {
                var gridPaginationControl = new webGridPaginationControl(paginationControl);
                $(gridPaginationControl).on(WEBGRID_PAGE_GOTO_EVENT, function (e, page) {
                    privateGoToPage(page.page);
                });
                return gridPaginationControl;
            } else {
                return null;
            }
        }

        function privateGoToPage(pageToShow) {

            var currentPage = $(grid).attr("pagination-currentpage");
            if (currentPage == undefined) {
                currentPage = 1;
            }

            var invisibleRowClass = $(grid).attr("pagination-invisiblerow-class");
            var unfilteredTableRows = $("tr.webGridRow:not(.filtered), .tr.webGridRow:not(.filtered)", webGridBody);
            var filteredTableRows = $("tr.webGridRow.filtered, .tr.webGridRow.filtered", webGridBody);

            var pageSize = $(grid).attr("pagination-pagesize");
            if (pageSize == undefined) {
                pageSize = unfilteredTableRows.length;
            }

            var maxPageNumber = parseInt(unfilteredTableRows.length / pageSize);
            if ((unfilteredTableRows.length % pageSize > 0)) {
                maxPageNumber++;
            }

            if (pageToShow == "left") {
                pageToShow = --currentPage;
            } else if (pageToShow == "right") {
                pageToShow = ++currentPage;
            }
            else if (pageToShow == "first") {
                pageToShow = 1;
            }
            else if (pageToShow == "last") {
                pageToShow = maxPageNumber;
            }

            if (pageToShow < 1 || pageToShow == undefined) {
                pageToShow = 1;
            }
            if (pageToShow > maxPageNumber) {
                pageToShow = maxPageNumber;
            }

            var firstPageEntry = pageToShow * pageSize - pageSize + 1;
            var lastPageEntry = pageToShow * pageSize;

            filteredTableRows.each(function () {
                // hide filtered rows
                var row = $(this);
                if (invisibleRowClass == "" || invisibleRowClass == undefined) {
                    $(row).hide();
                } else {
                    $(row).addClass(invisibleRowClass);
                }
            });

            var rowIndex = 1;
            unfilteredTableRows.each(function () {
                var row = $(this);
                if (rowIndex >= firstPageEntry && rowIndex <= lastPageEntry) {
                    if (invisibleRowClass == "" || invisibleRowClass == undefined) {
                        $(row).show();
                    } else {
                        $(row).removeClass(invisibleRowClass);
                    }
                } else {
                    if (invisibleRowClass == "" || invisibleRowClass == undefined) {
                        $(row).hide();
                    } else {
                        $(row).addClass(invisibleRowClass);
                    }
                }
                rowIndex++;
            });

            $(grid).attr("pagination-currentpage", pageToShow);
            $(grid).trigger(WEBGRID_PAGE_GONE_TO_EVENT, { page: pageToShow, maxPage: maxPageNumber });
            if (pagination != null) {
                pagination.goneToPage(pageToShow, maxPageNumber);
            }
        }

        function privateRefreshCurrentPage() {
            var currentPage = $(grid).attr("pagination-currentpage");
            privateGoToPage(currentPage);
        }


        var grid = $(tableSelector);
        if (grid.length != 1) {
            return null;
        }

        var gridId = $(grid).attr("id");
        var pagination = privateGetPaginationControlOrNull();
        var visibilityControl = new webGridVisibilityControl(grid);
        var columns = [];
        var webGridBody = $("tbody, .tbody", $(grid));

        $(grid).on(WEBGRID_COLUMN_FILTERED_EVENT, function () {

            var tableRows = $("tr.webGridRow, .tr.webGridRow", webGridBody);
            tableRows.each(function () {
                var row = $(this);
                var filteredCells = $(row).children("td.filtered, .td.filtered");
                if (filteredCells.length > 0) {
                    $(row).addClass('filtered');
                } else {
                    $(row).removeClass('filtered');
                }
            });
            privateRefreshCurrentPage();

        });

        $(grid).on(WEBGRID_COLUMN_SORTED_EVENT, function () {
            privateRefreshCurrentPage();
        });

        $(grid).on(WEBGRID_COLUMN_REQUEST_SHOW_EVENT, function (e, columnName) {

            privateGetColumnByName(columnName).show();
        });

        $(grid).on(WEBGRID_COLUMN_REQUEST_HIDE_EVENT, function (e, columnName) {
            privateGetColumnByName(columnName).hide();
        });

        $(".th[webgrid-column-name], th[webgrid-column-name]", tableSelector).each(function (index, column) {
            /* Spalten-Objekte erstellen */
            var columnName = $(column).attr("webgrid-column-name");
            var gridColumn = new webGridColumn(grid, columnName);
            columns[index] = gridColumn;

            /*Initialisierung der Visibility anhand von Cookies.*/
            var cookieVisible = $.cookie(getVisibilityCookieName(gridId, columnName));
            if (cookieVisible == "false") {
                gridColumn.hide();
            } else if (cookieVisible == "true") {
                gridColumn.show();
            }
        });

        var sortedByColumn = $.cookie(getCurrentSortingColumnCookieName(gridId));
        var sortDirection = $.cookie(getCurrentSortingDirectionCookieName(gridId));
        if (sortedByColumn != undefined) {
            var columnToSortBy = privateGetColumnByName(sortedByColumn);
            columnToSortBy.sortByColumn(sortDirection);
        }

        return {
            pagination: pagination,
            columns: columns,
            visibilityControl: visibilityControl,
            getColumnByName: privateGetColumnByName,
            refresh: privateRefreshCurrentPage,
            goToPage: privateGoToPage
        };

    }

    return {
        /*Namespace gibt nur initialisierungs-Methode f�r WebGrid frei.*/
        webGridControl: webGridControl
    }
})();

$ = jQuery;
$.fn.extend({
    webgrid: function () {
        return this.each(function () {
            WebGrid.webGridControl(this);
        });
    }
});

$(function () {
    $(".webGrid.init").webgrid();
});
