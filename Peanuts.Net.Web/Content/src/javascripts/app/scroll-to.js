const ScrollTo = (function ($) {
    const selector = '.js-scroll-to';
    const defaults = {
        speed: 200,
        easing: 'swing'
    };

    $(selector).each(function (idx, element) {
        const options = $.extend({}, defaults, $(element).data());
        const $target = $( $(element).attr('href') || $(element).data('target') );

        if( !$target.length ) {
            return;
        }

        const onComplete = function () {
            if( options.focus ) {
                $target.find('[name='+options.focus+'], #'+options.focus+','+options.focus).get(0).focus();
            }

            if( options.activate ) {
                $target.find('[name='+options.activate+'], #'+options.activate+','+options.activate).trigger('click');
            }
        };

        const onClick = function (e) {
            $('html,body').animate(
                {scrollTop: $target.offset().top},
                {speed: options.speed, easing: options.easing, complete: onComplete}
            );

            e.preventDefault();
        };

        $(document).on('click', selector, onClick);
    });

}(jQuery));

export default ScrollTo;
