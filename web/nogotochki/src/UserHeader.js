import { useEffect, useState } from 'react';
import { NavLink } from 'react-router-dom';
import UsersClient from './client/UsersClient';

const usersClient = new UsersClient();

function UserHeader() {
    const [currentUser, setCurrentUser] = useState(null);

    useEffect(() => {
        usersClient.getCurrentUserAsync().then(user => {
            setCurrentUser(user);
        });
    }, []);

    if (currentUser !== null) {
        return (
            <div>
                <NavLink to={`/users/${currentUser.id}`}>Мой профиль</NavLink>
            </div>
        );
    } else {
        return (
            <div>
                <NavLink to="/login">Войти</NavLink>
            </div>
        );
    }
}

export default UserHeader;