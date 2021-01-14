import "./User.css";
import { useEffect, useState } from "react";
import NogotochkiName from "./NogotochkiName";
import UsersClient from "./client/UsersClient";
import PhotosClient from "./client/PhotosClient";
import getNogotochkiMap from "./NogotochkiMap";
import { NavLink } from "react-router-dom";
import NogotochkiSocialNetwork from "./NogotochkiSocialNetwork";
import NogotochkiButton from "./NogotochkiButton";

const usersClient = new UsersClient();
const photosClient = new PhotosClient();

function User(props) {
    const [services, setServices] = useState([]);
    const [avatarPath, setAvatarPath] = useState(null);
    const [user, setUser] = useState(null);
    const [canEdit, setCanEdit] = useState(false);

    useEffect(() => {
        usersClient.getUserAsync(props.match.params.id).then(user => {
            setUser(user);
            photosClient.getPhotoAsync(user.avatarId).then(avatarPath => {
                setAvatarPath(avatarPath);
            });
            usersClient.getServicesAsync(user.id).then(services => {
                setServices(services);
            });
            usersClient.getCurrentUserAsync().then(currentUser => {
                if (currentUser.id === user.id) {
                    setCanEdit(true);
                }
            });
        });
    }, [props.match.params.id]);

    useEffect(() => {
        if (user === null) {
            return;
        }

        if (!user.geolocations || user.geolocations.length === 0) {
            return;
        }

        let nogotochkiMap = getNogotochkiMap({
            center: {
                latitude: '56.83',
                longitude: '60.60'
            },
            markers: user.geolocations || []
        });

        return () => {
            nogotochkiMap.map.remove();
        }
    }, [user]);

    if (user === null) {
        return (
            <div></div>
        );
    }

    return (
        <div className="user-show-container">

            <div className="edit-container">
                {canEdit ? <NogotochkiButton className="item-element"><NavLink to={`/users/${user.id}/edit`}>Редактировать</NavLink></NogotochkiButton> : null}
            </div>

            {canEdit ? <br /> : null}

            <div className="avatar-container">
                <img width="500px" src={avatarPath} alt="avatar" />
            </div>

            <br />

            <div className="nickname-container">
                <NogotochkiName className="item-name item-element">Никнейм</NogotochkiName>
                <NogotochkiName className="item-value item-element">{user.nickname}</NogotochkiName>
            </div>

            <br />

            <div className="name-container">
                <NogotochkiName className="item-name item-element">Имя</NogotochkiName>
                <NogotochkiName className="item-value item-element">{user.name}</NogotochkiName>
            </div>

            <br />

            <div className="description-container">
                <NogotochkiName className="item-name item-element">Описание</NogotochkiName>
                <NogotochkiName className="item-value item-element">{user.description}</NogotochkiName>
            </div>

            <br />

            <div className="social-networks-container">

                <div className="item-container-name item-container">
                    <NogotochkiName className="item-name item-element">Социальные сети</NogotochkiName>
                </div>

                <div className="item-container-value item-container">
                    {(user.socialNetworks || []).map((x, i) => (
                        <div>
                            <NogotochkiSocialNetwork className="item-value item-element" type={x.type} value={x.value} />
                        </div>
                    ))}
                </div>

            </div>

            <br />

            <div className="services-container">

                <div className="item-container-name item-container">
                    <NogotochkiName className="item-element">Услуги</NogotochkiName>
                </div>

                <div className="item-container-value item-container">
                    {services.map((service, index) => (
                        <div>
                            <NogotochkiName
                                className="item-value item-element"
                            >
                                <NavLink to={`/services/${service.id}`}>{service.title || "none"}</NavLink>
                            </NogotochkiName>
                        </div>
                    ))}
                </div>

            </div>

            <br />

            <div className="map-container">
                <div id="map"></div>
            </div>

        </div>
    );
}

export default User;