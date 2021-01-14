import { useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import SearchClient from './client/SearchClient';
import getNogotochkiMap from './NogotochkiMap';
import './SearchMap.css';

const searchClient = new SearchClient();

function SearchMap(props) {
    let nogotochkiMap = null;
    let previousBounds = null;

    useEffect(() => {
        if (nogotochkiMap === null) {
            nogotochkiMap = getNogotochkiMap({
                center: {
                    latitude: 56.83,
                    longitude: 60.60
                },
                markers: [],
                notFitBounds: true,
                onChangeBounds: onChangeBounds
            });
        }

        return () => {
            nogotochkiMap.map.remove();
        }
    });

    function onChangeBounds(bounds) {
        if (previousBounds !== null) {
            let shouldSearch = Math.abs(previousBounds._northEast.lat - bounds._northEast.lat) > 0.01 ||
                Math.abs(previousBounds._northEast.lng - bounds._northEast.lng) > 0.01 ||
                Math.abs(previousBounds._southWest.lat - bounds._southWest.lat) > 0.01 ||
                Math.abs(previousBounds._southWest.lng - bounds._southWest.lng) > 0.01;

            if (!shouldSearch) {
                return;
            }
        }
        previousBounds = bounds;

        let deltaLatitude = (bounds._northEast.lat - bounds._southWest.lat) / 2;
        let deltaLongitude = (bounds._northEast.lng - bounds._southWest.lng) / 2;
        let latitude = bounds._southWest.lat + deltaLatitude;
        let longitude = bounds._southWest.lng + deltaLongitude;

        searchClient.searchGeolocations(latitude, longitude, deltaLatitude, deltaLongitude).then(records => {
            nogotochkiMap.removeMarkers();
            let markers = [];
            records.map(record => {
                record.geolocations.map(x => {
                    markers.splice(0, 0, {
                        latitude: x.latitude,
                        longitude: x.longitude,
                        title: record.targetType === "service" ? "услуга" : "пользователь",
                        onChoose: () => {
                            if (record.targetType === "service") {
                                window.open(`/services/${record.targetId}`);
                            } else {
                                window.open(`/users/${record.targetId}`);
                            }
                        }
                    })
                })
            });
            nogotochkiMap.addMarkers(markers);
        }, response => {
            response.text().then(z => alert(z));
        });
    }

    return (
        <div className="search-map-container">
            <div id="map"></div>
        </div>
    );
}

export default SearchMap;