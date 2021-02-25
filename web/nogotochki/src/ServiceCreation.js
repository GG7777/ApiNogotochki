import "./ServiceCreation.css";
import { useEffect, useState } from "react";
import { Redirect } from "react-router-dom";
import ServicesClient from "./client/SerivicesClient";
import NogotochkiButton from "./NogotochkiButton";
import UsersClient from "./client/UsersClient";
import NogotochkiSelect from "./NogotochkiSelect";

const usersClient = new UsersClient();
const servicesClient = new ServicesClient();

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
        description: "Я мастер"
    },
    {
        value: "model",
        description: "Я модель"
    }
];

function ServiceCreation(props) {
    const [service, setService] = useState(null);
    const [serviceType, setServiceType] = useState(serviceTypeOptions[0]);
    const [searchType, setSearchType] = useState(searchTypeOptions[0]);
    const [canCreate, setCanCreate] = useState(null);

    useEffect(() => {
        usersClient.getCurrentUserAsync().then(user => {
            setCanCreate(true);
        }, response => {
            setCanCreate(false);
        });
    }, [props]);

    function createService() {
        servicesClient.createServiceAsync(serviceType, searchType).then(service => {
            setService(service);
        }, response => {
            response.text().then(z => alert(z));
        });
    }

    if (canCreate === null) {
        return (
            <div></div>
        );
    }

    if (canCreate === false) {
        return (
            <Redirect to="/login/services/new" />
        );
    }

    if (service !== null) {
        return (
            <Redirect to={`/services/${service.id}/edit`} />
        );
    }

    return (
        <div className="service-creation-container">
            <div className="values-container">
                <NogotochkiSelect className="item-element" onChange={e => setServiceType(e.target.value)} options={serviceTypeOptions} />
                <NogotochkiSelect className="item-element" onChange={e => setSearchType(e.target.value)} options={searchTypeOptions} />
                <NogotochkiButton className="item-element" onClick={createService}>Создать</NogotochkiButton>
            </div>
        </div>
    );
}

export default ServiceCreation;