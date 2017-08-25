(function ($) {
    /*
     * Localized default methods for the jQuery validation plugin.
     * Locale: DE
     */
    $.extend($.validator.methods, {
        date: function (value, element) {
            return this.optional(element) || /^\d\d?\.\d\d?\.\d\d\d?\d?$/.test(value);
        }
    });
    return $;
});