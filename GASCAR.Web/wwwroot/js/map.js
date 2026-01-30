var gMap = null;

window.initMap = function (stations) {
    try {
        // Verifica che l'elemento esista
        var mapElement = document.getElementById('googleMap');
        if (!mapElement) {
            console.error('Elemento googleMap non trovato nel DOM');
            return;
        }

        // Rimuovi mappa esistente se presente
        if (gMap != null) {
            gMap.remove();
            gMap = null;
        }

        // Crea la mappa con Leaflet (gratuito, nessuna API key richiesta)
        gMap = L.map('googleMap').setView([45.4642, 9.19], 12);

        // Aggiungi il layer della mappa (OpenStreetMap)
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
            maxZoom: 19
        }).addTo(gMap);

        // Aggiungi i marker per le colonnine
        if (stations && Array.isArray(stations)) {
            console.log('Aggiungo ' + stations.length + ' marker alla mappa');
            stations.forEach(function (station) {
                if (station.lat && station.lng) {
                    var marker = L.marker([station.lat, station.lng]).addTo(gMap);
                    var statusIcon = station.status === 'Disponibile' ? '✅' : '⏳';
                    marker.bindPopup('<b>' + statusIcon + ' ' + station.name + '</b><br>' + station.status);
                }
            });
        } else {
            console.warn('Nessuna stazione da visualizzare sulla mappa');
        }
    } catch (error) {
        console.error('Errore inizializzazione mappa:', error);
    }
};
