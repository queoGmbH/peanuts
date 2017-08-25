import 'moment';
import 'eonasdan-bootstrap-datetimepicker/src/js/bootstrap-datetimepicker';

const DatePicker = (function ($) {
    const selectors = '.js-date-picker';
    const datepickers = $(selectors);
    const options = {
        locale: 'de',
        format: 'DD.MM.YYYY',
        icons: {
            previous: 'icon-angle-left',
            next: 'icon-angle-right'
        },
        allowInputToggle: true
    };

    if( datepickers.length ) {
        datepickers.datetimepicker(options);
    }

}(jQuery));

export default DatePicker;
