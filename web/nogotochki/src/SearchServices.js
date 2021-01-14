import { useState } from 'react';
import { NavLink } from 'react-router-dom';
import SearchClient from './client/SearchClient';
import NogotochkiName from './NogotochkiName';
import NogotochkiSearchInput from './NogotochkiSearchInput';
import './SearchServices.css';

const searchClient = new SearchClient();

function SearchServices(props) {
    const [services, setServices] = useState([]);

    function onSearch(text) {
        searchClient.searchServices(text).then(services => {
            setServices(services);
        }, response => {
            response.text().then(z => alert(z));
        });
    }

    return (
        <div className="search-services-container">

            <NogotochkiSearchInput onSearch={onSearch} />

            <br />

            <div className="search-result-container">
                {services.map(service => (
                    <div>
                        <NogotochkiName className="item-element">
                            <NavLink to={`/services/${service.id}`}>{service.title || 'none'}</NavLink>
                        </NogotochkiName>
                    </div>
                ))}
            </div>

        </div>
    );
}

export default SearchServices;