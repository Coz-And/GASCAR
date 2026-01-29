window.initMap = function (stations) {
    var map = new google.maps.Map(document.getElementById('googleMap'), {
        center: { lat: 45.4642, lng: 9.19 }, // Default center (Milan)
        zoom: 12
    });

    if (stations && Array.isArray(stations)) {
        stations.forEach(function (station) {
            if (station.lat && station.lng) {
                new google.maps.Marker({
                    position: { lat: station.lat, lng: station.lng },
                    map: map,
                    title: station.name + ' - ' + station.status
                });
            }
        });
    }
};
