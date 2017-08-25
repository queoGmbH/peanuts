
const Charts = (function($, Chart) {

    $(".js-chart").each(function(index, item) {

        var $item = $(item);
        var chartData = $item.find("[type=\"application/json\"]");
        var chartDataJsonString = chartData.text();
        chartDataJsonString = chartDataJsonString.replace(/\\n/g, "\\n")  
               .replace(/\\'/g, "\\'")
               .replace(/\\"/g, '\\"')
               .replace(/\\&/g, "\\&")
               .replace(/\\r/g, "\\r")
               .replace(/\\t/g, "\\t")
               .replace(/\\b/g, "\\b")
               .replace(/\\f/g, "\\f");
        // remove non-printable and other non-valid JSON chars
        chartDataJsonString = chartDataJsonString.replace(/[\u0000-\u0019]+/g,""); 
        var chartDataJson = JSON.parse(chartDataJsonString);
        var canvas = $item.children("canvas")[0];
        var chart = new Chart(canvas, chartDataJson);

    });

}(jQuery, Chart));

export default Charts;