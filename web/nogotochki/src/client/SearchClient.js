import {url} from "./ClientConfiguration";

class SearchClient {
    async searchServices(query) {
        let response = await fetch(url + `/api/v1/search/services?q=${query}`);
        if (response.ok) {
            return await response.json();
        } else {
            return Promise.reject(response);
        }
    }

    async searchUsers(query) {
        let response = await fetch(url + `/api/v1/search/users?q=${query}`);
        if (response.ok) {
            return await response.json();
        } else {
            return Promise.reject(response);
        }
    }

    async searchGeolocations(latitude, longitude, deltaLatitude, deltaLongitude) {
        let response = await fetch(url + `/api/v1/search/geolocations?lat=${latitude}&lng=${longitude}&d-lat=${deltaLatitude}&d-lng=${deltaLongitude}`);
        if (response.ok) {
            return await response.json();
        } else {
            return Promise.reject(response);
        }
    }
}

export default SearchClient;