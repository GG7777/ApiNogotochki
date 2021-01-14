const uri = 'http://localhost:5000'

class UsersClient {
    async getCurrentUserAsync() {
        return await this.getUserAsync('me')
    }

    async getUserAsync(id) {
        let response = await fetch(uri + `/api/v1/users/${id}`, {
            headers: {
                'Authorization': document.cookie
            }
        });
        if (response.ok) {
            return await response.json();
        } else {
            return Promise.reject(response);
        }
    }

    async updatePhoneNumberAsync(id, phoneNumber, confirmationToken) {
        let response = await fetch(uri + `/api/v1/users/${id}/phone-number`, {
            method: 'PATCH',
            headers: {
                'Authorization': document.cookie,
                'Confirmation': confirmationToken,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                phoneNumber: phoneNumber
            })
        });
        if (response.ok) {
            return await response.json();
        } else {
            return Promise.reject(response);
        }
    }

    async updateNicknameAsync(id, nickname) {
        let response = await fetch(uri + `/api/v1/users/${id}/nickname`, {
            method: 'PATCH',
            headers: {
                'Authorization': document.cookie,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                nickname: nickname
            })
        });
        if (response.ok) {
            return await response.json();
        } else {
            return Promise.reject(response);
        }
    }

    async updateUserAsync(id, user) {
        let response = await fetch(uri + `/api/v1/users/${id}`, {
            method: 'PUT',
            headers: {
                'Authorization': document.cookie,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)
        });
        if (response.ok) {
            return await response.json();
        } else {
            return Promise.reject(response);
        }
    }

    async getServicesAsync(id) {
        let response = await fetch(uri + `/api/v1/users/${id}/services`, {
            headers: {
                'Authorization': document.cookie
            }
        });
        if (response.ok) {
            return await response.json();
        } else {
            return Promise.reject(response);
        }
    }

    async updateServicesAsync(id, serviceIds) {
        let response = await fetch(uri + `/api/v1/users/${id}/service-ids`, {
            method: 'PATCH',
            headers: {
                'Authorization': document.cookie,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({serviceIds: serviceIds})
        });
        if (response.ok) {
            return await response.json();
        } else {
            return Promise.reject(response);
        }
    }

    async updateAvatarIdAsync(id, avatarId) {
        let response = await fetch(uri + `/api/v1/users/${id}/avatar-id`, {
            method: 'PATCH',
            headers: {
                'Authorization': document.cookie,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({avatarId: avatarId})
        });
        if (response.ok) {
            return await response.json();
        } else {
            return Promise.reject(response);
        }
    }
}

export default UsersClient;