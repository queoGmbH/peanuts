const Table = (function($) {
    $(document).on('click', '.webGridRow--link', function(event) {
         location.href = $(this).find('a[href]').attr('href');
    });
})(jQuery);

export default Table;
