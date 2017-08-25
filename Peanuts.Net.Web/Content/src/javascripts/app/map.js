(function ($) {

    // Hier drin werden alle Maps, Daten und Marker gespeichert
    const $maps = $('.js-map');

    // Der googleMapsApiKey wird in einem Scripttag vom Typo3 gesetzt. Hier ist er in bs18-anfahrt.jade hinterlegt.
    if ($maps.length) {
        loadScripts($maps.data('api-key'));
    }

    function loadScripts(googleMapsApiKey) {
        // Das Script wird nur geladen wenn es auch passende Elemente gibt.
        var mapsApiScript = document.createElement('script');
        mapsApiScript.onload = initGoogleMaps;
        mapsApiScript.src = "https://maps.googleapis.com/maps/api/js?key=" + googleMapsApiKey + "&region=DE";
        document.getElementsByTagName('head')[0].appendChild(mapsApiScript);
    }

    function initGoogleMaps() {
        const styleArray = [
            {
                featureType: 'all',
                stylers: [
                    {saturation: -80}
                ]
            }, {
                featureType: 'road.arterial',
                elementType: 'geometry',
                stylers: [
                    {hue: '#00ffee'},
                    {saturation: 50}
                ]
            }, {
                featureType: 'poi.business',
                elementType: 'labels',
                stylers: [
                    {visibility: 'off'}
                ]
            }
        ];

        var $mapLocations = $maps.children('.map-location');
        var map = new google.maps.Map($maps.get(0), {
            zoom: 12,
            center: {lat: 51.049972, lng: 13.735260},
            styles: styleArray
        });

        if ($mapLocations.length) {
            $mapLocations.each((index, item) => {
                var $item = $(item);

                let marker = {
                    street: $item.data('street'),
                    streetnumber: $item.data('streetnumber'),
                    city: $item.data('city')
                };

                let address = marker.street + ' ' + marker.streetnumber + ' ' + marker.city;

                var geocoder = new google.maps.Geocoder();
                geocodeAddress(address, geocoder, map);
            })
        }
    }

    function geocodeAddress(address, geocoder, resultsMap) {
        geocoder.geocode({'address': address}, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
                resultsMap.setCenter(results[0].geometry.location);

                var marker = new google.maps.Marker({
                    map: resultsMap,
                    position: results[0].geometry.location
                });
            } else {
                alert('Geocode was not successful for the following reason: ' + status);
            }
        });
    }

    function attachInfoWindow(marker, message, infowindow, itemId, $this) {
        marker.addListener('click', function () {
            infowindow.setContent(message);

            var maxWidth = Math.min($(window).width(), 400);
            infowindow.setOptions({maxWidth: maxWidth});
            infowindow.open(marker.get('map'), marker);
            $($this).trigger({type: "anfahrt-map-active", item: itemId});

        });
    }

    function getBounds(mapData) {
        var bounds = {
            north: -90,
            south: 90,
            east: -180,
            west: 180
        };

        for (var i in mapData) {
            bounds.north = Math.max(mapData[i].lat, bounds.north);
            bounds.south = Math.min(mapData[i].lat, bounds.south);
            bounds.east = Math.max(mapData[i].lng, bounds.east);
            bounds.west = Math.min(mapData[i].lng, bounds.west);
        }
        return bounds;
    }

    function parseData(dataset, obj) {

        var items = [];

        // Angaben für die gesamte Karte
        dataset.zoomLevel = $(obj).find("[data-anfahrt-map-data]").attr("data-anfahrt-zoom-level");
        dataset.mapType = $(obj).find("[data-anfahrt-map-data]").attr("data-anfahrt-map-type");

        // Angaben für die Marker und Infofenster
        $(obj).find("[data-anfahrt-map-data] > *").each(function () {
            var item = {};
            item.lat = $(this).attr("data-anfahrt-lat");
            item.lng = $(this).attr("data-anfahrt-lng");
            item.title = $(this).attr("data-anfahrt-title");
            item.info = $(this).find("template").html();
            items.push(item);
        });

        dataset.mapData = items;
    }
}(jQuery))
