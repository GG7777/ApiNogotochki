import "./Services.css";
import { useEffect, useState } from "react";
import SearchClient from "./client/SearchClient";
import UsersClient from "./client/UsersClient";

const searchClient = new SearchClient();
const usersClient = new UsersClient();

function Services(props) {
    const [serviceUserPairs, setServiceUserPairs] = useState([]);

    useEffect(() => {
        if (props.searchType === "masters") {
            searchClient.searchMasters(50, props.serviceType)
                        .then(x => {
                            x.map(z => {
                                usersClient.getUserAsync(z.userId)
                                           .then(user => {
                                               serviceUserPairs.push({user: user, service: z});
                                               let copy = serviceUserPairs.map(m => m);
                                               setServiceUserPairs(copy);
                                           }, r => {
                                               r.text().then(y => alert(y));
                                           })
                            });
                        }, r => {
                            r.text().then(z => alert(z))
                        });
        } else if (props.searchType === "models") {
            searchClient.searchModels(50, props.serviceType)
            .then(x => {
                x.map(z => {
                    usersClient.getUserAsync(z.userId)
                               .then(user => {
                                   serviceUserPairs.push({user: user, service: z});
                                   setServiceUserPairs(serviceUserPairs);
                               }, r => {
                                   r.text().then(y => alert(y));
                               })
                });
            }, r => {
                r.text().then(z => alert(z))
            });
        }
    }, [props.searchType, props.serviceType]);

    function renderItem(service, user) {
        return (
            <div className="item">
                <div className="left-container">
                    <div className="top-container">
                        <a href={`/services/${service.id}`}>
                            <img src={`http://176.119.157.211:5000/photos/${service.photoId}`} alt="photo" />
                        </a>
                    </div>
                    <div>
                        <p><a href={`/users/${user.id}`}>{user.nickname}</a></p>
                        <p>Номер телефона: {service.phoneNumber ? `+${service.phoneNumber}` : `не указан`}</p>
                    </div>
                </div>
                <div className="right-container">
                    <p>{service.description || `Нет описания`}</p>
                </div>
            </div>
        );
    }

    return (
        <div className="show-services-container">
            {serviceUserPairs.map(x => renderItem(x.service, x.user))}
        </div>
    );
}

export default Services;