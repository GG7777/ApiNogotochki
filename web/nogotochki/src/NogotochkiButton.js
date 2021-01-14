import './NogotochkiButton.css';

function NogotochkiButton(props) {
    return (
        <div className={`nogotochki-button ${props.className}`} onClick={props.onClick}>{props.children}</div>
    );
}

export default NogotochkiButton;