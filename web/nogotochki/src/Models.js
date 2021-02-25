import Services from "./Services";
import ServiceTypesButtons from "./ServiceTypesButtons";

function Models(props) {
    return (
        <div>
            <ServiceTypesButtons searchType="models" />
            <Services searchType="models" serviceType={props.match.params.type} />
        </div>
    );
}

export default Models;