import React from 'react';
import './Basket.css';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faCartShopping} from "@fortawesome/free-solid-svg-icons";
import { useGlobalContext} from "../../../Providers/GlobalProvider.jsx";

const Basket = () => {
    const {basketCount}=useGlobalContext();
    
    return (
        <div className={'Basket__Container'}>
            <div className={'Basket__Cart'}>
                <FontAwesomeIcon icon={faCartShopping} size="2x" className={'Basket__Cart'}/>
                <p className={'BasketCircle'}>{basketCount}</p>
            </div>
            <p className={'Basket__MyBasket'}>My Basket </p>
        </div>
    );
};
export default Basket;