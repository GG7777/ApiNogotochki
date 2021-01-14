import "./ServiceCreation.css";
import { useEffect, useState } from "react";
import { Redirect } from "react-router-dom";
import ServicesClient from "./client/SerivicesClient";
import NogotochkiButton from "./NogotochkiButton";
import NogotochkiTextInput from "./NogotochkiTextInput";
import UsersClient from "./client/UsersClient";

const usersClient = new UsersClient();
const servicesClient = new ServicesClient();

function ServiceCreation(props) {
    const [service, setService] = useState(null);
    const [serviceType, setServiceType] = useState("haircut");
    const [canCreate, setCanCreate] = useState(null);

    useEffect(() => {
        usersClient.getCurrentUserAsync().then(user => {
            setCanCreate(true);
        }, response => {
            setCanCreate(false);
        });
    }, [props]);

    function createService() {
        servicesClient.createServiceAsync(serviceType).then(service => {
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
                <NogotochkiTextInput className="item-element" placeholder="Тип услуги" onChange={e => setServiceType(e.target.value)}>{serviceType}</NogotochkiTextInput>
                <NogotochkiButton className="item-element" onClick={createService}>Создать</NogotochkiButton>
            </div>
        </div>
    );
}

export default ServiceCreation;