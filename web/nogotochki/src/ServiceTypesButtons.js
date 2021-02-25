import "./ServiceTypesButtons.css";
import NogotochkiButton from "./NogotochkiButton";

function ServiceTypesButtons(props) {
    return (
        <div className="service-types-buttons-container">
            <NogotochkiButton className="item"><a href={`/${props.searchType}/haircut`}>Парикмахерские услуги</a></NogotochkiButton>
            <NogotochkiButton className="item"><a href={`/${props.searchType}/eyelashes`}>Ресницы</a></NogotochkiButton>
            <NogotochkiButton className="item"><a href={`/${props.searchType}/eyebrows`}>Брови</a></NogotochkiButton>
            <NogotochkiButton className="item"><a href={`/${props.searchType}/nails`}>Ногтевой сервис</a></NogotochkiButton>
        </div>
    );
}

export default ServiceTypesButtons;