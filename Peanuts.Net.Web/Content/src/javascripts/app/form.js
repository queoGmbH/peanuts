const Form = (function ($) {
    const invalidClass = 'invalid';

    const selectors = {
        pane: '.tab-pane[id]',
        nav: '.nav-tabs'
    };

    const defaults = {
        validator: {
            ignore: '',
            focusInvalid: false
        },
        unobtrusive: {
            invalidHandler: invalidHandler
        }
    };

    function invalidHandler(event, validator) {
        let failedTabs = [];

        for( let error of validator.errorList ) {
            let tabId = $(error.element).closest(selectors.pane).attr('id');
            let tabItem = $('a[data-toggle=tab][aria-controls='+tabId+']').parent().get(0);
            failedTabs.push(tabItem);
        }

        failedTabs = $.unique(failedTabs);

        highlightTabs($(failedTabs));
    }

    function highlightTabs($tabs) {
        $tabs.closest(selectors.nav).children().removeClass(invalidClass);
        $tabs.addClass(invalidClass);
    }

    $.validator.setDefaults( defaults.validator );
    $.validator.unobtrusive.options = defaults.unobtrusive;

    // Special checkbox trick
    $.validator.unobtrusive.adapters.addBool("mandatory", "required");
}(jQuery));

export default Form;
