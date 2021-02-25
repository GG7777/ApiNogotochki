import { NavLink } from 'react-router-dom';
import './Header.css';
import NogotochkiButton from './NogotochkiButton';
import UserHeader from './UserHeader';

function Header() {
    return (
        <div className="header-container">
            <div className="creation-container item-container">
                <NogotochkiButton className="header-item"><NavLink to="/services/new">Создание услуги</NavLink></NogotochkiButton>
            </div>
            <div className="search-container item-container">
                <NogotochkiButton className="header-item"><NavLink to="/models/haircut">Я мастер</NavLink></NogotochkiButton>
                <NogotochkiButton className="header-item"><NavLink to="/masters/haircut">Я модель</NavLink></NogotochkiButton>
            </div>
            <div className="user-header-container item-container">
                <NogotochkiButton className="header-item"><UserHeader /></NogotochkiButton>
            </div>
        </div>
    );
}

export default Header;