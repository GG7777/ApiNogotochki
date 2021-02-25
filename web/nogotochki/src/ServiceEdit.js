import "./Service.css";
import { useEffect, useState, createRef } from 'react';
import ServicesClient from './client/SerivicesClient';  
import NogotochkiName from './NogotochkiName';
import NogotochkiButton from './NogotochkiButton';
import UsersClient from "./client/UsersClient";
import NogotochkiSelect from "./NogotochkiSelect";
import NogotochkiTextInput from "./NogotochkiTextInput";

const servicesClient = new ServicesClient();
const usersClient = new UsersClient();

const serviceTypeOptions = [
    {
        value: "haircut",
        description: "Парикмахерские услуги"
    },
    {
        value: "eyelashes",
        description: "Ресницы"
    },
    {
        value: "eyebrows",
        description: "Брови"
    },
    {
        value: "nails",
        description: "Ногтевой сервис"
    },
];

const searchTypeOptions = [
    {
        value: "master",
        description: "Мастер"
    },
    {
        value: "model",
        description: "Модель"
    }
];

function ServiceEdit(props) {
    const [service, setService] = useState(normalizeService({}));
    const [photoId, setPhotoId] = useState("");
    const [serviceType, setServiceType] = useState("");
    const [searchType, setSearchType] = useState("");
    const [phoneNumber, setPhoneNumber] = useState("");
    const [description, setDescription] = useState("");
    
    let photoFile = createRef();

    useEffect(() => {
        servicesClient.getServiceAsync(props.match.params.id).then(service => {
            setServiceAndProperties(service);
        }, response => {
            response.text().then(z => alert(z));
        });
    }, [props.match.params.id]);

    function normalizeService(service) {

        return service;
    }

    function setServiceAndProperties(service) {
        setService(normalizeService(service));
        setPhotoId(service.photoId);
        setServiceType(service.type);
        setSearchType(service.searchType);
        setPhoneNumber(service.phoneNumber);
        setDescription(service.description);
    }

    function updateService() {
        service.photoId = photoId;
        service.type = serviceType;
        service.searchType = searchType;
        service.phoneNumber = phoneNumber;
        service.description = description;

        servicesClient.updateServiceAsync(service.id, service).then(updatedService => {
            setServiceAndProperties(updatedService);
        }, response => {
            response.text().then(z => alert(z));
        })
    }

    return (
        <div className="single-service-container">

            <div className="right-container items-container">
                <NogotochkiButton className="item"><a href={`/services/${service.id}`}>Просмотр услуги</a></NogotochkiButton>
            </div>

            <div className="right-container items-container">
                <NogotochkiButton className="item"><a href={`/users/${service.userId}`}>Владелец услуги</a></NogotochkiButton>
            </div>

            <div className="right-container items-container">
                <NogotochkiButton className="item" onClick={updateService}>Обновить</NogotochkiButton>
            </div>

            <div className="photo-container items-container">
                <img width="450px" src={`{http://176.119.157.211:5000/photos/${service.photoId}}`} alt="photo" />
            </div>

            <div className="items-container">
                <NogotochkiName className="name item">Тип услуги</NogotochkiName>
                <NogotochkiSelect className="value item" defaultValue={service.type} options={serviceTypeOptions} />
            </div>

            <div className="items-container">
                <NogotochkiName className="name item">Тип представителя услуги</NogotochkiName>
                <NogotochkiSelect className="value item" defaultValue={service.searchType} options={searchTypeOptions} />
            </div>

            <div className="items-container">
                <NogotochkiName className="name item">Номер телефона</NogotochkiName>
                <NogotochkiTextInput className="value item" onChange={e => setPhoneNumber(e.target.value)}>{phoneNumber}</NogotochkiTextInput>
            </div>

            <div className="items-container">
                <NogotochkiName className="name item">Описание услуги</NogotochkiName>
                <NogotochkiTextInput className="value item" onChange={e => setDescription(e.target.value)}>{description}</NogotochkiTextInput>
            </div>

        </div>
    );
}

export default ServiceEdit;