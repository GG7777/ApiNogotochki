import './SearchUsers.css';
import { useState } from 'react';
import { NavLink } from 'react-router-dom';
import SearchClient from './client/SearchClient';
import NogotochkiName from './NogotochkiName';
import NogotochkiSearchInput from './NogotochkiSearchInput';

const searchClient = new SearchClient();

function SearchUsers(props) {
    const [users, setUsers] = useState([]);

    function onSearch(text) {
        searchClient.searchUsers(text).then(users => {
            setUsers(users);
        }, response => {
            response.text().then(z => alert(z));
        });
    }

    return (
        <div className="search-users-container">

            <NogotochkiSearchInput onSearch={onSearch} />

            <br />

            <div className="search-result-container">
                {users.map(user => (
                    <div>
                        <NogotochkiName className="item-element">
                            <NavLink to={`/users/${user.id}`}>{`${user.nickname} (${user.name})`}</NavLink>
                        </NogotochkiName>
                    </div>
                ))}
            </div>

        </div>
    );
}

export default SearchUsers;