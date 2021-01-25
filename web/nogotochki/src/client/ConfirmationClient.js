import {url} from "./ClientConfiguration";

class ConfirmationClient {
    async requestSmsCodeAsync(phoneNumber, confirmationType) {
        return await fetch(url + '/api/v1/confirmation/sms/sending', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                phoneNumber: phoneNumber,
                confirmationType: confirmationType,
            })
        });
    }

    async sendSmsCodeAsync(phoneNumber, confirmationType, confirmationCode) {
        return await fetch(url + '/api/v1/confirmation/sms', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                phoneNumber: phoneNumber,
                confirmationType: confirmationType,
                confirmationCode: confirmationCode
            })
        });
    }
}

export default ConfirmationClient;