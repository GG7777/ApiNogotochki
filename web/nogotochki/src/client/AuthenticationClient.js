import {url} from "./ClientConfiguration";

class AuthenticationClient {
    async authenticateAsync(confirmationToken) {
        return await fetch(url + '/api/v1/authentication', {
            method: 'POST',
            headers: {
                'Confirmation': confirmationToken
            }
        });
    }
}

export default AuthenticationClient;