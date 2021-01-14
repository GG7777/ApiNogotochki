import { Redirect } from 'react-router-dom';
import './SearchTop.css';

function SearchTop(props) {
    return (
        <div>
            <Redirect to="/search/map" />
        </div>
    );
}

export default SearchTop;