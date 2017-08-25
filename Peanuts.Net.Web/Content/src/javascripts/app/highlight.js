'use strict';

$('pre code').each(function () {
    var html = $(this).html();
    html = html.replace(/^\r?\n/g, '');
    var spaces = html.match(/^[ ]*/);
    html = html.replace(new RegExp('^' + spaces), '');
    html = html.replace(new RegExp('\n' + spaces, 'g'), '\n');
    $(this).text(html);
});
