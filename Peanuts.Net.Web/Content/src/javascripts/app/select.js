import 'select2/dist/js/select2.full';
import 'select2/dist/js/i18n/de';

const Select = (function ($) {
    const selectors = '.js-select-single,.js-select-multiple';
    const selects = $(selectors);
    const options = {
        language: "de",
        width: null
    };

    if( selects.length ) {
        selects.each(function () {
            let _options = $.extend({}, options);
            if( $(this).find('option').length < 10 ) {
                _options['minimumResultsForSearch'] = -1;
            }
            $(this)
                .on('change', function(){
                    $(this).valid()
                })
                .select2(_options);
        });
    }

}(jQuery));

const SelectDepending = (function ($) {
    
    var dependingSelectsSelector = 'select[js-depends-on]';

    function initDependingSelect($select) {

        function filterOptions(byValue) {
            /*aktuell ausgewählten Wert merken*/
            var selectedValue = $select.val();

            /*alle Optionen aus dem Select entfernen*/
            $select.find('option').remove().end();

            /*Nur die passenden Optionen wieder in das Select eintragen*/
            var accordingOptions = options.filter(function (index, item) {
                var $item = $(item);
                if ($.inArray(byValue, $item.data("js-depends-on-value"))>=0) {
                    return true;
                } else {
                    return false;
                }
            });
            $select.append(accordingOptions);

            /*TODO: Placeholder*/

            /*alten Wert wieder auswählen bzw. Platzhalter-Wert auswählen.*/
            $select.val(selectedValue);

            /*Select2 aktualisieren*/
            $select.select2();
        }

        var $dependsOn = $('#' + $select.attr('js-depends-on'));
        var options = $select.find("option");

        if ($dependsOn) {

            /*initiale Anzeige*/
            filterOptions($dependsOn.val());

            /*auf Änderungen reagieren*/
            $dependsOn.on('change', function(){
                /*Das Select, von welchem dieses Select abhängig ist, wurde geändert.*/
                filterOptions($dependsOn.val());
            });
        }
    }

    function findAndInitSelects($parent) {
        var selects = $(dependingSelectsSelector, $parent);
        if( selects.length ) {
            selects.each(function (index, select) {
                initDependingSelect($(select));
            });
        }
    }

    /* Alle initial vorhandenen Selects initialisieren */
    findAndInitSelects($(document));

    /*Dynamisch hinzugefügte Elemente initialisieren*/
    $(".dynamic-list").on("dynamic-list:itemAdded", function(e) {
        var $listItem = $(e.item);
        findAndInitSelects($listItem);
    });

}(jQuery));

export default Select;