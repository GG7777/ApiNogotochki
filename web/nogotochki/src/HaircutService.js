import './HaircutService.css';
import NogotochkiImageSlider from './NogotochkiImageSlider';
import NogotochkiButton from './NogotochkiButton';
import { NavLink } from 'react-router-dom';
import { useEffect, useState } from 'react';
import UsersClient from './client/UsersClient';
import NogotochkiName from './NogotochkiName';
import NogotochkiSocialNetwork from './NogotochkiSocialNetwork';
import PhotosClient from './client/PhotosClient';
import getNogotochkiMap from './NogotochkiMap';

const usersClient = new UsersClient();
const photosClient = new PhotosClient();

function HaircutService(props) {
    const service = normalizeService(props.service);
    const [user, setUser] = useState({});
    const [canEdit, setCanEdit] = useState(false);
    const [photos, setPhotos] = useState([]);

    useEffect(() => {
        usersClient.getUserAsync(props.service.userId).then(user => {
            setUser(user);
            usersClient.getCurrentUserAsync().then(currentUser => {
                if (currentUser.id === user.id) {
                    setCanEdit(true);
                }
            });
        }, response => {
            response.text().then(z => alert(z));
        });

        service.photos.photosValue.map((photo, index) => {
            photosClient.getPhotoAsync(photo.photoId).then(path => {
                photos.splice(index, 0, { ...photo, path: path });
                setPhotos(photos);
                console.log(photos);
            });
            return null;
        });
    }, [service.id]);

    useEffect(() => {
        if (service.geolocations.length === 0) {
            return;
        }

        let nogotochkiMap = getNogotochkiMap({
            center: {
                latitude: '56.83',
                longitude: '60.60'
            },
            markers: service.geolocations
        });

        return () => {
            nogotochkiMap.map.remove();
        }
    }, [service]);

    function normalizeService(service) {
        service.title = service.title || {};
        service.description = service.description || {};
        service.photos = service.photos || {};
        service.photos.photosValue = service.photos.photosValue || [];
        service.socialNetworks = service.socialNetworks || [];
        service.geolocations = service.geolocations || [];
        return service;
    }

    return (
        <div className="haircut-service-show-container">

            <div className="edit-container">
                {canEdit ? <NogotochkiButton className="item-element"><NavLink to={`/services/${service.id}/edit`}>Редактировать</NavLink></NogotochkiButton> : null}
            </div>

            {canEdit ? <br /> : null}

            <div className="title-container">
                <NogotochkiName className="item-element">{service.title.titleValue}</NogotochkiName>
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
            </div>

            {photos.length > 0 ? <div><br /> <br /></div> : null}

            <div className="description-container">
                <NogotochkiName className="name-element item-element">Описание</NogotochkiName>
                <NogotochkiName className="value-element item-element">{service.description.descriptionValue}</NogotochkiName>
            </div>

            <br />

            <div className="social-networks-container">

                <div className="item-container-name item-container">
                    <NogotochkiName className="item-name item-element">Социальные сети</NogotochkiName>
                </div>

                <div className="item-container-value item-container">
                    {service.socialNetworks.map((x, i) => (
                        <div>
                            <NogotochkiSocialNetwork className="item-value item-element" type={x.type} value={x.value} />
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

export default HaircutService;