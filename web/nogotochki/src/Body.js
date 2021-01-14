import './Body.css'
import { Route, Switch } from 'react-router-dom';
import User from './User';
import UserEdit from './UserEdit';
import Login from './Login';
import ServiceCreation from './ServiceCreation';
import ServiceFactory from './ServiceFactory';
import SearchServices from './SearchServices.js';
import SearchUsers from './SearchUsers.js';
import SearchMap from './SearchMap.js';
import SearchTop from './SearchTop.js';

function Body() {
    return (
        <div className="body-container">
            <Switch>
                <Route path="/services/new" component={ServiceCreation} />
                <Route path="/services/:id/edit" component={ServiceFactory} />
                <Route path="/services/:id" component={ServiceFactory} />
                <Route path="/users/:id/edit" component={UserEdit} />
                <Route path="/users/:id" component={User} />
                <Route path="/search/services" component={SearchServices} />
                <Route path="/search/users" component={SearchUsers} />
                <Route path="/search/map" component={SearchMap} />
                <Route path="/login/*" component={Login} />
                <Route path="/login" component={Login} />
                <Route path="/*" component={SearchTop} />
            </Switch>
        </div>
    );
}

export default Body;