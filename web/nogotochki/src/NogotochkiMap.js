import './NogotochkiMap.css';
import DG from '2gis-maps';

function getNogotochkiMap(mapData) {
    let map = DG.map('map', {
        center: [mapData.center.latitude, mapData.center.longitude],
        zoom: 13
    });
    let markersGroup = null;

    function addMarkers(markers) {
        markersGroup = DG.featureGroup(markers.map(x => {
            let marker = DG.marker([x.latitude, x.longitude]);
            if (x.popup) {
                marker.bindPopup(x.popup);
            }
            if (x.title) {
                marker.bindLabel(x.title);
            }
            marker.on('click', e => {
                if (x.onChoose) {
                    x.onChoose();
                }
            });
            marker.on('mouseover', e => {
                marker.showLabel();
            });
            marker.on('mouseout', e => {
                marker.hideLabel();
            });
            return marker;
        }));

        if (markers.length > 0) {
            markersGroup.addTo(map);
            if (!mapData.notFitBounds) {
                map.fitBounds(markersGroup.getBounds());
            }
        }
    }

    function removeMarkers() {
        if (markersGroup !== null) {
            markersGroup.remove();
        }
    }

    map.on('zoom', e => {
        if (mapData.onChangeBounds) {
            mapData.onChangeBounds(map.getBounds());
        }
    })

    map.on('move', e => {
        if (mapData.onChangeBounds) {
            mapData.onChangeBounds(map.getBounds());
        }
    })

    if (mapData.onChangeBounds) {
        mapData.onChangeBounds(map.getBounds());
    }

    addMarkers(mapData.markers);

    if (mapData.onDragMarker) {
        let center = (mapData.markers.length > 0 ? markersGroup : map).getBounds().getCenter();
        let dragMarker = DG.marker([center.lat, center.lng], {
            draggable: true
        }).addTo(map);
        dragMarker.on('drag', e => {
            mapData.onDragMarker(e.target._latlng.lat, e.target._latlng.lng);
        });
        mapData.onDragMarker(center.lat, center.lng);
    }

    return {
        map: map,
        addMarkers: addMarkers,
        removeMarkers: removeMarkers
    };
}

export default getNogotochkiMap;