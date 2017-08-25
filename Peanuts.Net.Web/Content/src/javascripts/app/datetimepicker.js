import 'moment';
import 'eonasdan-bootstrap-datetimepicker/src/js/bootstrap-datetimepicker';

const DateTimePicker = (function ($) {
    const selectors = '.js-datetime-picker';
    const datetimepickers = $(selectors);
    const options = {
        locale: 'de',
        icons: {
            time: "icon-clock-o",
            date: "icon-calendar",
            up: "icon-angle-up",
            down: "icon-angle-down",
            previous: 'icon-angle-left',
            next: 'icon-angle-right'
        },
        allowInputToggle: true
    };

    if( datetimepickers.length ) {
        datetimepickers.datetimepicker(options);
    }

}(jQuery));

export default DateTimePicker;
