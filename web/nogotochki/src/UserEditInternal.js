import "./UserEditInternal.css";
import { useEffect, useState } from "react";
import UsersClient from "./client/UsersClient";
import NogotochkiTextInput from "./NogotochkiTextInput";
import NogotochkiName from "./NogotochkiName";
import NogotochkiButton from "./NogotochkiButton";
import ConfirmationClient from "./client/ConfirmationClient";
import PhotosClient from "./client/PhotosClient";
import { NavLink } from "react-router-dom";

const usersClient = new UsersClient();
const confirmationClient = new ConfirmationClient();
const photosClient = new PhotosClient();

function UserEditInternal(props) {
    const [user, setUser] = useState(null);
    const [avatarPath, setAvatarPath] = useState(null);
    const [phoneNumber, setPhoneNumber] = useState(null);
    const [nickname, setNickname] = useState(null);
    const [confirmationCode, setConfirmationCode] = useState(null);

    const [socialNetworkEntry, setSocialNetworkEntry] = useState({});

    const [currentMarker, setCurrentMarker] = useState({ latitude: 0, longitude: 0, index: null });
    const [newMarker, setNewMarker] = useState({ latitude: 0, longitude: 0 });
    const [geolocations, setGeolocations] = useState([]);

    const [services, setServices] = useState([]);

    useEffect(() => {
        usersClient.getUserAsync(props.id).then(user => {
            setUser(user);
            setPhoneNumber(user.phoneNumber);
            setNickname(user.nickname);
            setGeolocations(user.geolocations || []);

            photosClient.getPhotoAsync(user.avatarId).then(path => {
                setAvatarPath(path);
            });

            usersClient.getServicesAsync(props.id).then(services => {
                setServices(services);
            }, response => {
                response.text().then(z => alert(z));
            });
        }, response => {
            response.text().then(z => alert(z));
        });
    }, [props.id]);

    useEffect(() => {
        // нужен компонент с id='map', но реакт рендерит асинхронно, поэтому тупо ждем
        setTimeout(() => {
            props.setMapData({
                center: {
                    latitude: '56.83',
                    longitude: '60.60'
                },
                markers: geolocations
                    ? geolocations.map((geolocation, index) => {
                        geolocation.onChoose = () => setCurrentMarker({
                            latitude: geolocation.latitude,
                            longitude: geolocation.longitude,
                            popup: geolocation.popup,
                            index: index
                        });
                        return geolocation;
                    })
                    : [],
                onDragMarker: (latitude, longitude) => {
                    setNewMarker({
                        latitude: latitude,
                        longitude: longitude
                    });
                }
            });
        }, 100);
    }, [geolocations]);

    function updatePhoneNumber() {
        confirmationClient.sendSmsCodeAsync(phoneNumber, 'change-phone-number', confirmationCode).then(response => {
            if (response.ok) {
                response.json().then(x => {
                    usersClient.updatePhoneNumberAsync(user.id, phoneNumber, x.confirmationToken).then(updatedUser => {
                        setUser(updatedUser);
                    }, resp => {
                        resp.text().then(z => alert(z));
                    });
                });
            } else {
                response.text().then(x => alert(x));
            }
        });
    }

    function updateNickname() {
        usersClient.updateNicknameAsync(user.id, nickname).then(updatedUser => {
            setUser(updatedUser);
        }, response => {
            response.text().then(x => alert(x));
        });
    }

    function requestSmsCode() {
        confirmationClient.requestSmsCodeAsync(phoneNumber, 'change-phone-number').then(response => {
            if (!response.ok) {
                response.text().then(x => alert(x));
            }
        });
    }

    function updateAvatar() {
        if (typeof avatarPath === "string" && avatarPath.length > 0) {
            photosClient.savePhotoAsync(avatarPath).then(avatarId => {
                usersClient.updateAvatarIdAsync(user.id, avatarId).then(newUser => {
                    setUser({ ...user, avatarId: avatarId });
                }, response => {
                    response.text().then(x => alert(x));
                });
            });
        }
    }

    function addSocialNetwork() {
        user.socialNetworks.splice(0, 0, {
            type: socialNetworkEntry.type,
            value: socialNetworkEntry.value
        });
        setUser({ ...user });
    }

    function removeSocialNetwork(index) {
        user.socialNetworks.splice(index, 1);
        setUser({ ...user });
    }

    function addGeolocation() {
        geolocations.splice(0, 0, newMarker);
        setGeolocations(geolocations.map(x => x));
    }

    function removeGeolocation() {
        if (currentMarker.index === null) {
            return;
        }
        geolocations.splice(currentMarker.index, 1);
        setCurrentMarker({ latitude: 0, longitude: 0, index: null });
        setGeolocations(geolocations.map(x => x));
    }

    function removeService(index) {
        services.splice(index, 1);
        setServices(services.map(x => x));
    }

    function updateServices() {
        usersClient.updateServicesAsync(props.id, services.map(x => x.id)).then(newServices => {
            setServices(newServices);
        }, response => {
            response.text().then(z => alert(z));
        });
    }

    function updateAll() {
        user.geolocations = geolocations.map(x => {
            return {
                latitude: x.latitude.toString(),
                longitude: x.longitude.toString(),
                popup: x.popup
            };
        });
        usersClient.updateUserAsync(props.id, user).then(user => {
            setUser(user);
        }, response => {
            response.text().then(z => alert(z));
        });
    }

    if (user === null) {
        return (
            <div></div>
        );
    }

    if (!user.socialNetworks) {
        user.socialNetworks = []
        setUser({ ...user });
    }

    return (
        <div className="user-container">

            <div className="show-user-container">
                <NogotochkiButton className="item-element">
                    <NavLink to={`/users/${user.id}`}>Пользователь</NavLink>
                </NogotochkiButton>
            </div>

            <br />

            <div className="avatar-container">

                <div className="photo-container">
                    <img width="500px" src={avatarPath} alt="avatar" />
                </div>

                <br />

                <div className="photo-path-container">
                    <NogotochkiName className="item-name item-element">Ссылка на фото</NogotochkiName>
                    <NogotochkiTextInput className="item-value item-element" onChange={e => setAvatarPath(e.target.value)}>{avatarPath}</NogotochkiTextInput>
                    <NogotochkiButton className="item-update item-element" onClick={updateAvatar}>Обновить</NogotochkiButton>
                </div>

            </div>

            <br />

            <div className="phone-number-container">
                <NogotochkiName className="item-name item-element">Номер телефона</NogotochkiName>
                <NogotochkiTextInput className="item-value item-element" onChange={e => setPhoneNumber(e.target.value)}>{phoneNumber}</NogotochkiTextInput>
                <NogotochkiTextInput className="item-value item-element" placeholder="Смс-код" onChange={e => setConfirmationCode(e.target.value)}>{confirmationCode}</NogotochkiTextInput>
                <NogotochkiButton className="item-update item-element" onClick={requestSmsCode}>Получить смс-код</NogotochkiButton>
                <NogotochkiButton className="item-update item-element" onClick={updatePhoneNumber}>Обновить</NogotochkiButton>
            </div>

            <br />

            <div className="nickname-container">
                <NogotochkiName className="item-name item-element">Никнейм</NogotochkiName>
                <NogotochkiTextInput className="item-value item-element" onChange={e => setNickname(e.target.value)}>{nickname}</NogotochkiTextInput>
                <NogotochkiButton className="item-update item-element" onClick={updateNickname}>Обновить</NogotochkiButton>
            </div>

            <br />

            <div className="services-container">

                <div className="item-container-name">
                    <NogotochkiName className="item-element">Услуги</NogotochkiName>
                </div>

                <div className="item-container-value">
                    {services.map((service, index) => (
                        <div>
                            <NogotochkiName
                                className="item-value item-element"
                            >
                                <NavLink to={`/services/${service.id}`}>{service.title || "none"}</NavLink>
                            </NogotochkiName>
                            <NogotochkiButton className="item-update item-element" onClick={e => removeService(index)}>Удалить</NogotochkiButton>
                        </div>
                    ))}
                </div>

                <div className="item-container-update">
                    <NogotochkiButton className="item-element" onClick={updateServices}>Обновить</NogotochkiButton>
                </div>

            </div>

            <br />
            <br />
            <br />

            <div>

                <div className="name-container">
                    <NogotochkiName className="item-name item-element">Имя</NogotochkiName>
                    <NogotochkiTextInput
                        className="item-value item-element"
                        onChange={e => setUser({ ...user, name: e.target.value })}
                    >
                        {user.name}
                    </NogotochkiTextInput>
                </div>

                <br />

                <div className="description-container">
                    <NogotochkiName className="item-name item-element">Описание</NogotochkiName>
                    <NogotochkiTextInput
                        className="item-value item-element"
                        onChange={e => setUser({ ...user, description: e.target.value })}
                    >
                        {user.description}
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
                                onChange={e => setSocialNetworkEntry({ ...socialNetworkEntry, type: e.target.value })}
                            >
                                {socialNetworkEntry.type}
                            </NogotochkiTextInput>
                            <NogotochkiTextInput
                                className="item-value item-element"
                                placeholder="Логин"
                                onChange={e => setSocialNetworkEntry({ ...socialNetworkEntry, value: e.target.value })}
                            >
                                {socialNetworkEntry.value}
                            </NogotochkiTextInput>
                            <NogotochkiButton className="item-update item-element" onClick={addSocialNetwork}>Добавить</NogotochkiButton>
                        </div>

                        {(user.socialNetworks || []).map((x, i) => (
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
                                <NogotochkiName className="item-value item-element">{`${parseFloat(newMarker.latitude).toFixed(2)}, ${parseFloat(newMarker.longitude).toFixed(2)}`}</NogotochkiName>
                                <NogotochkiTextInput className="item-value item-element" placeholder="Офис" onChange={e => setNewMarker({ ...newMarker, popup: e.target.value })}>{newMarker.popup}</NogotochkiTextInput>
                                <NogotochkiButton className="item-update item-element" onClick={addGeolocation}>Добавить</NogotochkiButton>
                            </div>

                            <div>
                                <NogotochkiName className="item-value item-element">{`${parseFloat(currentMarker.latitude).toFixed(2)}, ${parseFloat(currentMarker.longitude).toFixed(2)}`}</NogotochkiName>
                                <NogotochkiName className="item-value item-element">{currentMarker.popup || "none"}</NogotochkiName>
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

        </div>
    );
}

export default UserEditInternal;