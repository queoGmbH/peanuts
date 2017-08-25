
/* Methode die ein Fehlerverhalten bei Jquery.validate und Datums korrigiert. */

jQuery.validator.addMethod(
            'date',
            function (value, element, params) {

                console.log("Validation of date");

                if (this.optional(element)) {
                    return true;
                };
                var result = false;
                try {
                    var dpg = $.fn.datepicker.DPGlobal;
                    var dateValue = dpg.parseDate(element.value, dpg.parseFormat("dd.mm.yyyy"));
                    console.log("Parsed value: " + dateValue);
                    result = true;
                } catch (err) {
                    console.log("Catch Validation error: " + err);
                    result = false;
                }
                return result;
            },
            ''
        );