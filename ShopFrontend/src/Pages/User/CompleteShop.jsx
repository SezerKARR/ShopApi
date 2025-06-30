import React from 'react';
import './CompleteShop.css';
import DeliveryAddress from "../../Components/CompleteShop/DeliveryAddress.jsx";

const CompleteShop = () => {
    return (
        <div className="CompleteShop-container">
           <DeliveryAddress/>
        </div>
    );
};

export default CompleteShop;