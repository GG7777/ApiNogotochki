import Services from "./Services";

function SearchTypeService(props) {
    console.log(props);
    let searchType = "2";
    let serviceType = "a";

    return (
        <div>
            <Services searchType={searchType} serviceType={serviceType} />
        </div>
    );
}

export default SearchTypeService;