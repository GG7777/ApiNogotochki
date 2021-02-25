import "./Service.css";
import { useEffect, useState } from 'react';
import ServicesClient from './client/SerivicesClient';  
import NogotochkiName from './NogotochkiName';
import UsersClient from "./client/UsersClient";
import NogotochkiButton from "./NogotochkiButton";

const servicesClient = new ServicesClient();
const usersClient = new UsersClient();

function Service(props) {
    const [service, setService] = useState(normalizeService({}));
    const [canEdit, setCanEdit] = useState(false);

    useEffect(() => {
        servicesClient.getServiceAsync(props.match.params.id).then(service => {
            setService(normalizeService(service));
            usersClient.getCurrentUserAsync().then(user => {
                console.log(user.id);
                console.log(service.userId);
                if (user.id === service.userId) {
                    setCanEdit(true);
                }
            });
        }, response => {
            response.text().then(z => alert(z));
        });
    }, [props.match.params.id]);

    function normalizeService(service) {

        return service;
    }

    function getServiceType(serviceType) {
        if (serviceType === "haircut") return "Парикмахерские услуги";
        if (serviceType === "eyelashes") return "Ресницы";
        if (serviceType === "eyebrows") return "Брови";
        if (serviceType === "nails") return "Ногтевой сервис";
        return serviceType;
    }

    function getSearchType(searchType) {
        if (searchType === "master") return "Мастер";
        if (searchType === "model") return "Модель";
        return searchType;
    }

    return (
        <div className="single-service-container">

            <div className="right-container items-container">
                {canEdit ? <NogotochkiButton className="item"><a href={`/services/${service.id}/edit`}>Редактировать</a></NogotochkiButton> : <div></div>}
            </div>

            <div className="right-container items-container">
                <NogotochkiButton className="item"><a href={`/users/${service.userId}`}>Владелец услуги</a></NogotochkiButton>
            </div>

            <div className="photo-container items-container">
                <img width="450px" src={`{http://176.119.157.211:5000/photos/${service.photoId}}`} alt="photo" />
            </div>

            <div className="items-container">
                <NogotochkiName className="name item">Тип услуги</NogotochkiName>
                <NogotochkiName className="value item">{getServiceType(service.type)}</NogotochkiName>
            </div>

            <div className="items-container">
                <NogotochkiName className="name item">Тип представителя услуги</NogotochkiName>
                <NogotochkiName className="value item">{getSearchType(service.searchType)}</NogotochkiName>
            </div>

            <div className="items-container">
                <NogotochkiName className="name item">Номер телефона</NogotochkiName>
                <NogotochkiName className="value item">{service.phoneNumber ? `+${service.phoneNumber}` : `Не указан`}</NogotochkiName>
            </div>

            <div className="items-container">
                <NogotochkiName className="name item">Описание услуги</NogotochkiName>
                <NogotochkiName className="value item">{service.description || `Нет описания`}</NogotochkiName>
            </div>

        </div>
    );
}

export default Service;