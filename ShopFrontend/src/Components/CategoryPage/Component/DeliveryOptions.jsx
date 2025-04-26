import React from "react";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTruck } from "@fortawesome/free-solid-svg-icons";
import "./DeliveryOptions.css"
const DeliveryOptions = () => {
    return (
        <div className="delivery-option-container">
            <h4 className="delivery-option-title">Delivery Options</h4>
            <div className="delivery-options-description">
                <FontAwesomeIcon icon={faTruck} />
            </div>
        </div>
    );
};

export default DeliveryOptions;
