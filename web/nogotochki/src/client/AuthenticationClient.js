const uri = 'http://api:5000';

class AuthenticationClient {
    async authenticateAsync(confirmationToken) {
        return await fetch(uri + '/api/v1/authentication', {
            method: 'POST',
            headers: {
                'Confirmation': confirmationToken
            }
        });
    }
}

export default AuthenticationClient;