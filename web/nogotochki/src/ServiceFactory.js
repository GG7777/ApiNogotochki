import { useEffect, useState } from "react";
import ServicesClient from "./client/SerivicesClient";
import HaircutService from "./HaircutService";
import HaircutServiceEdit from "./HaircutServiceEdit";

const servicesClient = new ServicesClient();

function ServiceFactory(props) {
    const [service, setService] = useState({});

    let isEdit = props.match.path.endsWith('/edit');

    useEffect(() => {
        servicesClient.getServiceAsync(props.match.params.id).then(service => {
            setService(service);
        }, response => {
            response.text().then(z => alert(z));
        });
    }, [props.match.params.id]);

    if (service.type === "haircut") {
        return isEdit ? <HaircutServiceEdit service={service} /> : <HaircutService service={service} />
    }

    return (
        <div></div>
    );
}

export default ServiceFactory;