import "./NogotochkiSelect.css";

function NogotochkiSelect(props) {
    let index = props.options.indexOf(props.defaultValue);
    if (index < 0) index = 0;
    return (
        <select className={`nogotochki-select ${props.className}`} onChange={props.onChange}>
            {props.options.map((x, i) => {
                if (i === index) {
                    return <option selected value={x.value}>{x.description}</option>
                }
                return <option value={x.value}>{x.description}</option>
            })}
        </select>
    );
}

export default NogotochkiSelect;