import { useEffect, useState } from "react";
import HaircutServiceEditInternal from "./HaircutServiceEditInternal";
import getNogotochkiMap from "./NogotochkiMap";

function HaircutServiceEdit(props) {
    const [mapData, setMapData] = useState(null);

    useEffect(() => {
        if (mapData === null) {
            return;
        }

        let nogotochkiMap = getNogotochkiMap(mapData);

        return () => {
            nogotochkiMap.map.remove();
        }
    });

    return (
            <HaircutServiceEditInternal service={props.service} setMapData={setMapData} />
    );
}

export default HaircutServiceEdit;