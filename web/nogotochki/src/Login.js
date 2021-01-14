import "./Login.css";
import { useState } from "react";
import NogotochkiButton from "./NogotochkiButton";
import NogotochkiTextInput from "./NogotochkiTextInput";
import ConfirmationClient from "./client/ConfirmationClient";
import AuthenticationClient from "./client/AuthenticationClient";
import { Redirect } from "react-router-dom";

const confirmationClient = new ConfirmationClient();
const authenticationClient = new AuthenticationClient();

function Login(props) {
    console.log(props);
    const [phoneNumber, setPhoneNumber] = useState("");
    const [confirmationCode, setConfirmationCode] = useState("");
    const [phonePage, setPhonePage] = useState(true);
    const [userId, setUserId] = useState(null);

    function requestSmsCode() {
        confirmationClient.requestSmsCodeAsync(phoneNumber, 'authentication').then(response => {
            if (response.ok) {
                setPhonePage(false);
            } else {
                response.text().then(z => alert(z));    
            }
        });
    }

    function sendSmsCode() {
        confirmationClient.sendSmsCodeAsync(phoneNumber, 'authentication', confirmationCode).then(response => {
            if (response.ok) {
                response.json().then(z => {
                    authenticate(z.confirmationToken);
                })
            } else {
                response.text().then(z => alert(z));
            }
        });
    }

    function authenticate(confirmationToken) {
        authenticationClient.authenticateAsync(confirmationToken).then(response => {
            if (response.ok) {
                response.json().then(x => {
                    document.cookie = x.authorizationToken;
                    setUserId(x.userId);
                });
            }
        });
    }

    if (userId !== null) {
        return (
            <Redirect to={props.location.pathname.substring(6).length > 0 ? `${props.location.pathname.substring(6)}` : `/users/${userId}`} />
        );
    }

    if (phonePage) {
        return (
            <div className="login-container">
                <NogotochkiTextInput className="login-item" placeholder="Номер телефона" onChange={e => setPhoneNumber(e.target.value)}>{phoneNumber}</NogotochkiTextInput>
                <NogotochkiButton className="login-item" onClick={requestSmsCode}>Получить смс-код</NogotochkiButton>
            </div>
        );
    }
    else {
        return (
            <div className="login-container">
                <NogotochkiTextInput className="login-item" placeholder="Номер телефона" onChange={e => setPhoneNumber(e.target.value)}>{phoneNumber}</NogotochkiTextInput>
                <NogotochkiTextInput className="login-item" placeholder="Смс-код" onChange={e => setConfirmationCode(e.target.value)}>{confirmationCode}</NogotochkiTextInput>
                <NogotochkiButton className="login-item" onClick={sendSmsCode}>Подтвердить смс-код</NogotochkiButton>
            </div>
        );
    }
}

export default Login;