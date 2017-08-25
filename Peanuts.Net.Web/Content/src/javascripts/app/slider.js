import 'slick-carousel/slick/slick';

const Slider = (function ($) {
    const selectors = '.js-slick-slider';
    const sliders = $(selectors);

    const options = {
        variableWidth: true,
        centerMode: true,
        adaptiveHeight: true,
        arrows: true,
        autoplay: false,
        speed: 500,
        slidesToShow: 1,
        slidesToScroll: 1,
        infinite: true
    };

    if( sliders.length ){
        sliders.slick(options);
    }
}(jQuery));

export default Slider;
