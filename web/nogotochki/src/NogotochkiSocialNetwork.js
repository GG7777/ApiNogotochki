import "./NogotochkiSocialNetwork.css";
import NogotochkiName from "./NogotochkiName";

function NogotochkiSocialNetwork(props) {
    let url = null;

    if (props.type === 'instagram') {
        url = `https://instagram.com/${props.value}`;
    }
    if (props.type === 'vk') {
        url = `https://vk.com/${props.value}`;
    }

    return (
        <NogotochkiName
            className={`nogotochki-social-network ${props.className}`}
        >
            {url ? <a href={url}>{`${props.type}@${props.value}`}</a> : `${props.type}@${props.value}`}
        </NogotochkiName>
    );
}

export default NogotochkiSocialNetwork;