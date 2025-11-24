// location.js
let leafletLoaded = false;

function loadLeaflet() {
    return new Promise((resolve, reject) => {
        if (typeof L !== 'undefined') {
            leafletLoaded = true;
            resolve();
            return;
        }

        // Check if Leaflet CSS is loaded
        if (!document.querySelector('link[href*="leaflet"]')) {
            const link = document.createElement('link');
            link.rel = 'stylesheet';
            link.href = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.css';
            link.integrity = 'sha256-p4NxAoJBhIIN+hmNHrzRCf9tD/miZyoHS5obTRR9BMY=';
            link.crossOrigin = '';
            document.head.appendChild(link);
        }

        // Check if Leaflet JS is loaded
        if (!document.querySelector('script[src*="leaflet"]')) {
            const script = document.createElement('script');
            script.src = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.js';
            script.integrity = 'sha256-20nQCchB9co0qIjJZRGuk2/Z9VM+kNiyxNV1lvTlZBo=';
            script.crossOrigin = '';

            script.onload = () => {
                leafletLoaded = true;
                resolve();
            };

            script.onerror = () => {
                reject(new Error('Failed to load Leaflet library'));
            };

            document.head.appendChild(script);
        } else {
            // Script tag exists but L might not be loaded yet
            const checkLeaflet = setInterval(() => {
                if (typeof L !== 'undefined') {
                    leafletLoaded = true;
                    clearInterval(checkLeaflet);
                    resolve();
                }
            }, 100);

            // Timeout after 10 seconds
            setTimeout(() => {
                clearInterval(checkLeaflet);
                reject(new Error('Leaflet library loading timeout'));
            }, 10000);
        }
    });
}

export function initializeMap(containerId, initialLat, initialLng, dotnetHelper) {
    return new Promise(async (resolve, reject) => {
        try {
            // Ensure Leaflet is loaded
            if (!leafletLoaded) {
                await loadLeaflet();
            }

            // Check if container exists
            const container = document.getElementById(containerId);
            if (!container) {
                reject(new Error(`Container with id '${containerId}' not found`));
                return;
            }

            // Clean up existing map if any
            if (container._leaflet_map) {
                destroyMap(containerId);
            }

            // Create map
            const map = L.map(containerId).setView([initialLat, initialLng], 13);

            // Store reference
            container._leaflet_map = map;

            // Add OpenStreetMap tiles
            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                attribution: '© OpenStreetMap contributors',
                maxZoom: 19
            }).addTo(map);

            // Create crosshair cursor
            const crosshairIcon = L.divIcon({
                className: 'crosshair-cursor',
                html: '<div style="position: relative; width: 30px; height: 30px; margin: -15px 0 0 -15px;">' +
                    '<div style="position: absolute; top: 50%; left: 0; right: 0; height: 2px; background: red; transform: translateY(-50%);"></div>' +
                    '<div style="position: absolute; left: 50%; top: 0; bottom: 0; width: 2px; background: red; transform: translateX(-50%);"></div>' +
                    '<div style="position: absolute; top: 50%; left: 50%; width: 8px; height: 8px; background: red; border-radius: 50%; transform: translate(-50%, -50%);"></div>' +
                    '</div>',
                iconSize: [30, 30]
            });

            const crosshairMarker = L.marker(map.getCenter(), {
                icon: crosshairIcon,
                interactive: false,
                zIndexOffset: 1000
            }).addTo(map);

            // Event handlers
            const moveHandler = function () {
                crosshairMarker.setLatLng(map.getCenter());
            };

            const clickHandler = function (e) {
                const center = map.getCenter();
                dotnetHelper.invokeMethodAsync('OnMapLocationSelected', center.lat, center.lng)
                    .catch(err => console.error('Error calling .NET method:', err));
            };

            map.on('move', moveHandler);
            map.on('click', clickHandler);

            // Store handlers for cleanup
            map._eventHandlers = { move: moveHandler, click: clickHandler };

            // Initial location selection
            const center = map.getCenter();
            dotnetHelper.invokeMethodAsync('OnMapLocationSelected', center.lat, center.lng)
                .catch(err => console.error('Error calling .NET method:', err));

            resolve('Map initialized');
        } catch (error) {
            reject(error);
        }
    });
}

export function updateMapView(containerId, lat, lng) {
    try {
        const mapElement = document.getElementById(containerId);
        if (mapElement && mapElement._leaflet_map) {
            const map = mapElement._leaflet_map;
            map.setView([lat, lng], map.getZoom());
            return true;
        }
        console.warn(`Map with containerId '${containerId}' not found`);
        return false;
    } catch (error) {
        console.error('Error updating map view:', error);
        return false;
    }
}

export function destroyMap(containerId) {
    try {
        const mapElement = document.getElementById(containerId);
        if (mapElement && mapElement._leaflet_map) {
            const map = mapElement._leaflet_map;

            // Remove event handlers
            if (map._eventHandlers) {
                Object.entries(map._eventHandlers).forEach(([event, handler]) => {
                    map.off(event, handler);
                });
            }

            map.remove();
            delete mapElement._leaflet_map;
            return true;
        }
        return false;
    } catch (error) {
        console.error('Error destroying map:', error);
        return false;
    }
}

export function getCurrentLocation() {
    return new Promise((resolve, reject) => {
        if (!navigator.geolocation) {
            reject(new Error('Geolocation is not supported by this browser.'));
            return;
        }

        navigator.geolocation.getCurrentPosition(
            (position) => {
                resolve({
                    latitude: position.coords.latitude,
                    longitude: position.coords.longitude,
                    accuracy: position.coords.accuracy
                });
            },
            (error) => {
                let errorMessage = 'Unable to retrieve your location.';
                switch (error.code) {
                    case error.PERMISSION_DENIED:
                        errorMessage = 'Location access denied by user.';
                        break;
                    case error.POSITION_UNAVAILABLE:
                        errorMessage = 'Location information unavailable.';
                        break;
                    case error.TIMEOUT:
                        errorMessage = 'Location request timed out.';
                        break;
                }
                reject(new Error(errorMessage));
            },
            {
                enableHighAccuracy: true,
                timeout: 10000,
                maximumAge: 60000
            }
        );
    });
}

export function searchAddress(address) {
    return new Promise((resolve, reject) => {
        if (!address || address.trim() === '') {
            reject(new Error('Address cannot be empty'));
            return;
        }

        // Use OpenStreetMap Nominatim API for geocoding
        const url = `https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(address)}&limit=1`;

        fetch(url)
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
                if (data && data.length > 0) {
                    const result = data[0];
                    resolve({
                        latitude: parseFloat(result.lat),
                        longitude: parseFloat(result.lon),
                        displayName: result.display_name
                    });
                } else {
                    resolve(null);
                }
            })
            .catch(error => {
                reject(new Error(`Geocoding failed: ${error.message}`));
            });
    });
}

// Make functions available globally
window.initializeMap = initializeMap;
window.updateMapView = updateMapView;
window.getCurrentLocation = getCurrentLocation;
window.destroyMap = destroyMap;
window.searchAddress = searchAddress;