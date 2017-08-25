/**
* Note: Das Skript initialisiert die Datepicker und DateRangePicker in dem Moment, wo sie den Fokus erhalten.
*/

$(function () {

    var metas = document.getElementsByTagName('meta');
    var currentLanguageCode = null;
    for (var i = 0; i < metas.length && currentLanguageCode == null; i++) {
        if (metas[i].getAttribute("lang") != "") {
            currentLanguageCode = metas[i].getAttribute("lang");
        }
    }

    if (currentLanguageCode == null) {
        currentLanguageCode = 'de';
    }

    $('body').on('focus',".daterange .from", function(){
        $(this).datepicker({
            language: currentLanguageCode,
            onClose: function (selectedDate) {
                var dateRangeDiv = $(this).parents(".daterange:first");
                var relatedTo = dateRangeDiv.find(".to");
                relatedTo.datepicker("option", "minDate", selectedDate - 1);
            }
        });
    });

    $('body').on('focus',".daterange .to", function() {
        $(this).datepicker({
            language: currentLanguageCode,
            onClose: function(selectedDate) {
                var dateRangeDiv = $(this).parents(".daterange:first");
                var relatedFrom = dateRangeDiv.find(".from");
                relatedFrom.datepicker("option", "maxDate", selectedDate + 1);
            }
        });
    });

    $('body').on('focus', ".date", function() {
        $(this).datepicker({
            language: currentLanguageCode
        });
    });

});