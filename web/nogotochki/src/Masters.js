import Services from "./Services";
import ServiceTypesButtons from "./ServiceTypesButtons";

function Masters(props) {
    return (
        <div>
            <ServiceTypesButtons searchType="masters" />
            <Services searchType="masters" serviceType={props.match.params.type} />
        </div>
    );
}

export default Masters;