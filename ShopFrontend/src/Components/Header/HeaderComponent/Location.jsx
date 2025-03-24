import React from 'react';
import './Location.css';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faLocationDot} from "@fortawesome/free-solid-svg-icons";

const Location = () => {
    return (
        <div className={'Location'}>
            <FontAwesomeIcon icon={faLocationDot} size="2x"/>
            <div className={'Location__Name__Container'}>
                <p className={'Location__Label'}>
                    Location </p>
                <p className={'LocationSelect__Label'}>
                    Select a location </p>
            </div>
        </div>
    );
};
export default Location;