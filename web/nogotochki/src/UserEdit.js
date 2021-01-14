import { useEffect, useState } from "react";
import getNogotochkiMap from "./NogotochkiMap";
import UserEditInternal from "./UserEditInternal";

function UserEdit(props) {
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
            <UserEditInternal id={props.match.params.id} setMapData={setMapData} />
    );
}

export default UserEdit;