import "./NogotochkiName.css";

function NogotochkiName(props) {
    return (
        <div className={`nogotochki-name ${props.className}`}>{props.children}</div>
    );
}

export default NogotochkiName;