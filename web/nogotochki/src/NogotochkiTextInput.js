import "./NogotochkiTextInput.css";

function NogotochkiTextInput(props) {
    return (
        <input className={`nogotochki-text-input ${props.className}`} placeholder={props.placeholder} value={props.children} onChange={props.onChange} />
    );
}

export default NogotochkiTextInput;