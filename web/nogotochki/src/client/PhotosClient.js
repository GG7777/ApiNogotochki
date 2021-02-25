import {url} from "./ClientConfiguration";

class PhotosClient {
    async savePhotoAsync(path) {
        let response = await fetch(url + '/api/v1/photos', {
            method: 'POST',
            headers: {
                'Content-Type': 'text/json'
            },
        });
        if (response.ok) {
            return await response.text();
        } else {
            return Promise.reject(response);
        }
    }

    async getPhotoAsync(id) {
        if (typeof id === "string" && id.length > 0) {
            let response = await fetch(url + `/api/v1/photos/${id}`);
            if (response.ok) {
                return await response.text();
            } else {
                return Promise.reject(response);
            }
        }
        return Promise.reject();
    }
}

export default PhotosClient;