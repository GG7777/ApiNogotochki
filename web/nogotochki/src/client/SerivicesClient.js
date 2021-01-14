const uri = 'http://localhost:5000'

class ServicesClient {
    async createServiceAsync(serviceType) {
        let response = await fetch(uri + '/api/v1/services', {
            method: 'POST',
            headers: {
                'Authorization': document.cookie,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({type: serviceType})
        });
        if (response.ok) {
            return await response.json();
        } else {
            return Promise.reject(response);
        }
    }

    async updateServiceAsync(id, service) {
        let response = await fetch(uri + `/api/v1/services/${id}`, {
            method: 'PUT',
            headers: {
                'Authorization': document.cookie,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(service)
        });
        if (response.ok) {
            return await response.json();
        } else {
            return Promise.reject(response);
        }
    }

    async getServiceAsync(id) {
        let response = await fetch(uri + `/api/v1/services/${id}`);
        if (response.ok) {
            return await response.json();
        } else {
            return Promise.reject(response);
        }
    }
}

export default ServicesClient;