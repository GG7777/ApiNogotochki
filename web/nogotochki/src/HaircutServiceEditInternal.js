import "./HaircutServiceEditInternal.css";
import { useEffect, useState } from "react";
import PhotosClient from "./client/PhotosClient";
import UsersClient from "./client/UsersClient";
import NogotochkiButton from "./NogotochkiButton";
import NogotochkiImageSlider from "./NogotochkiImageSlider";
import NogotochkiName from "./NogotochkiName";
import NogotochkiTextInput from "./NogotochkiTextInput";
import ServicesClient from "./client/SerivicesClient";
import { NavLink } from "react-router-dom";

const servicesClient = new ServicesClient();
const usersClient = new UsersClient();
const photosClient = new PhotosClient();

function HaircutServiceEditInternal(props) {
    const [service, setService] = useState(normalizeService(props.service));
    const [photos, setPhotos] = useState([]);
    const [newPhoto, setNewPhoto] = useState({});
    const [newSocialNetwork, setNewSocialNetwork] = useState({});
    const [newMarker, setNewMarker] = useState({ latitude: 0, longitude: 0 });
    const [selectedMarker, setSelectedMarker] = useState({ latitude: 0, longitude: 0, index: null });
    const [geolocations, setGeolocations] = useState(service.geolocations);
    const [user, setUser] = useState({});

    useEffect(() => {
        usersClient.getUserAsync(props.service.userId).then(user => {
            setUser(user);
        }, response => {
            response.text().then(z => alert(z));
        });

        service.photos.photosValue.map((photo, index) => {
            photosClient.getPhotoAsync(photo.photoId).then(path => {
                photos.splice(index, 0, { ...photo, path: path });
                setPhotos(photos);
            });
            return null;
        });
    }, [props.service.id]);

    useEffect(() => {
        // нужен компонент с id='map', но реакт рендерит асинхронно, поэтому тупо ждем
        setTimeout(() => {
            props.setMapData({
                center: {
                    latitude: '56.83',
                    longitude: '60.60'
                },
                markers: geolocations.map((geolocation, index) => {
                    geolocation.onChoose = () => setSelectedMarker({
                        latitude: geolocation.latitude,
                        longitude: geolocation.longitude,
                        popup: geolocation.popup,
                        index: index
                    });
                    return geolocation;
                }),
                onDragMarker: (latitude, longitude) => {
                    setNewMarker({
                        latitude: latitude,
                        longitude: longitude
                    });
                }
            });
        }, 100);
    }, [geolocations]);

    function normalizeService(service) {
        service.title = service.title || {};
        service.description = service.description || {};
        service.photos = service.photos || {};
        service.photos.photosValue = service.photos.photosValue || [];
        service.socialNetworks = service.socialNetworks || [];
        service.geolocations = service.geolocations || [];
        return service;
    }

    function addNewPhoto() {
        photosClient.savePhotoAsync(newPhoto.path).then(photoId => {
            newPhoto.photoId = photoId;
            photos.splice(0, 0, newPhoto);
            setPhotos(photos.map(x => x));
        });
    }

    function removePhoto(index) {
        photos.splice(index, 1);
        setPhotos(photos.map(x => x));
    }

    function addNewSocialNetwork() {
        service.socialNetworks.splice(0, 0, newSocialNetwork);
        setService({ ...service });
    }

    function removeSocialNetwork(index) {
        service.socialNetworks.splice(index, 1);
        setService({ ...service });
    }

    function addNewGeolocation() {
        geolocations.splice(0, 0, newMarker);
        setGeolocations(geolocations.map(x => x));
    }

    function removeGeolocation() {
        if (selectedMarker.index === null) {
            return;
        }
        geolocations.splice(selectedMarker.index, 1);
        setGeolocations(geolocations.map(x => x));
        setSelectedMarker({ latitude: 0, longitude: 0, index: null });
    }

    function updateAll() {
        service.geolocations = geolocations.map(x => {
            return {
                ...x,
                latitude: x.latitude.toString(),
                longitude: x.longitude.toString()
            };
        });
        service.photos.photosValue = photos;

        servicesClient.updateServiceAsync(service.id, service).then(updatedService => {
            setService(updatedService);
        }, response => {
            response.text().then(z => alert(z));
        });
    }

    return (
        <div className="haircut-service-edit-container">

            <div className="show-service-container">
                <NogotochkiButton className="item-element">
                    <NavLink to={`/services/${service.id}`}>Услуга</NavLink>
                </NogotochkiButton>
            </div>

            <br />

            <div className="user-owner-container">
                <NogotochkiName className="name-element item-element">Пользователь</NogotochkiName>
                <NogotochkiName className="value-element item-element">
                    <NavLink to={`/users/${user.id}`}>{user.nickname} ({user.name})</NavLink>
                </NogotochkiName>
            </div>

            <br />

            <div className="photos-container">

                <NogotochkiImageSlider images={photos} />

                {photos.length > 0 ? <div><br /> <br /></div> : null}

                <div className="edit-container">

                    <div className="values-container">

                        <div className="creation-container">

                            <div className="values-edit-container items-container">

                                <NogotochkiTextInput
                                    className="title-element item-element"
                                    placeholder="Название"
                                    onChange={e => setNewPhoto({ ...newPhoto, title: e.target.value })}
                                >
                                    {newPhoto.title}
                                </NogotochkiTextInput>

                                <NogotochkiTextInput
                                    className="description-element item-element"
                                    placeholder="Описание"
                                    onChange={e => setNewPhoto({ ...newPhoto, description: e.target.value })}
                                >
                                    {newPhoto.description}
                                </NogotochkiTextInput>

                                <NogotochkiTextInput
                                    className="path-element item-element"
                                    placeholder="Ссылка на фото"
                                    onChange={e => setNewPhoto({ ...newPhoto, path: e.target.value })}
                                >
                                    {newPhoto.path}
                                </NogotochkiTextInput>

                            </div>

                            <div className="add-values-container items-container">
                                <NogotochkiButton
                                    className="item-element"
                                    onClick={addNewPhoto}
                                >
                                    Добавить
                                    </NogotochkiButton>
                            </div>

                        </div>

                        <br />

                        {photos.map((photo, index) => (
                            <div className="removing-container">

                                <div>

                                    <div className="show-values-container items-container">
                                        <NogotochkiName className="title-element item-element">{photo.title}</NogotochkiName>
                                        <NogotochkiName className="description-element item-element">{photo.description}</NogotochkiName>
                                        <NogotochkiName className="path-element item-element">{photo.path}</NogotochkiName>
                                    </div>

                                    <div className="remove-values-container items-container">
                                        <NogotochkiButton
                                            className="item-element"
                                            onClick={e => removePhoto(index)}
                                        >
                                            Удалить
                                        </NogotochkiButton>
                                    </div>

                                </div>

                                <br />

                            </div>
                        ))}

                    </div>

                </div>

            </div>

            <div className="title-container">
                <NogotochkiName className="name-element item-element">Название</NogotochkiName>
                <NogotochkiTextInput
                    className="value-element item-element"
                    onChange={e => {
                        service.title.titleValue = e.target.value;
                        setService({ ...service });
                    }}
                >
                    {service.title.titleValue}
                </NogotochkiTextInput>
            </div>

            <br />

            <div className="description-container">
                <NogotochkiName className="name-element item-element">Описание</NogotochkiName>
                <NogotochkiTextInput
                    className="value-element item-element"
                    onChange={e => {
                        service.description.descriptionValue = e.target.value;
                        setService({ ...service });
                    }}
                >
                    {service.description.descriptionValue}
                </NogotochkiTextInput>
            </div>

            <br />

            <div className="social-networks-container">

                <div className="item-container-name">
                    <NogotochkiName className="item-name item-element">Социальные сети</NogotochkiName>
                </div>

                <div className="item-container-value">

                    <div>
                        <NogotochkiTextInput
                            className="item-value item-element"
                            placeholder="instagram"
                            onChange={e => setNewSocialNetwork({ ...newSocialNetwork, type: e.target.value })}
                        >
                            {newSocialNetwork.type}
                        </NogotochkiTextInput>
                        <NogotochkiTextInput
                            className="item-value item-element"
                            placeholder="Логин"
                            onChange={e => setNewSocialNetwork({ ...newSocialNetwork, value: e.target.value })}
                        >
                            {newSocialNetwork.value}
                        </NogotochkiTextInput>
                        <NogotochkiButton className="item-update item-element" onClick={addNewSocialNetwork}>Добавить</NogotochkiButton>
                    </div>

                    {service.socialNetworks.map((x, i) => (
                        <div>
                            <NogotochkiName className="item-value item-element">{x.type}</NogotochkiName>
                            <NogotochkiName className="item-value item-element">{x.value}</NogotochkiName>
                            <NogotochkiButton className="item-update item-element" onClick={() => removeSocialNetwork(i)}>Удалить</NogotochkiButton>
                        </div>
                    ))}

                </div>

            </div>

            <br />

            <div className="map-coordinates-container">

                <div className="coordinates-container">

                    <div className="item-container-name">
                        <NogotochkiName className="item-element">Местоположения</NogotochkiName>
                    </div>

                    <div className="item-container-value">

                        <div>
                            <NogotochkiName className="item-value item-element">
                                {`${parseFloat(newMarker.latitude).toFixed(2)}, ${parseFloat(newMarker.longitude).toFixed(2)}`}
                            </NogotochkiName>
                            <NogotochkiTextInput
                                className="item-value item-element"
                                placeholder="Офис"
                                onChange={e => setNewMarker({ ...newMarker, popup: e.target.value })}
                            >
                                {newMarker.popup}
                            </NogotochkiTextInput>
                            <NogotochkiButton className="item-update item-element" onClick={addNewGeolocation}>Добавить</NogotochkiButton>
                        </div>

                        <div>
                            <NogotochkiName className="item-value item-element">
                                {`${parseFloat(selectedMarker.latitude).toFixed(2)}, ${parseFloat(selectedMarker.longitude).toFixed(2)}`}
                            </NogotochkiName>
                            <NogotochkiName className="item-value item-element">{selectedMarker.popup || "none"}</NogotochkiName>
                            <NogotochkiButton className="item-update item-element" onClick={removeGeolocation}>Удалить</NogotochkiButton>
                        </div>

                    </div>

                </div>

                <br />

                <div className="map-container">
                    <div id="map"></div>
                </div>

            </div>

            <br />

            <div className="update-all-container">
                <NogotochkiButton className="item-element" onClick={updateAll}>Обновить</NogotochkiButton>
            </div>

        </div>
    );
}

export default HaircutServiceEditInternal;