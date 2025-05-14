import React, {memo} from 'react';
import './Basket.css';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faCartShopping} from "@fortawesome/free-solid-svg-icons";
import { useGlobalContext} from "../../../Providers/GlobalProvider.jsx";
import { useBasketContext} from "../../../Providers/BasketProvider.jsx";
import BasketAdded from "../../Common/BasketAdded.jsx";
import {useNavigate} from "react-router-dom";

const Basket = memo(() => {
    const {basketCount}=useBasketContext();
    const navigate = useNavigate();
    const handleBasketClick=()=>{
        navigate("/basket");
    }
    return (
        <div className={'Basket__Container'} onClick={handleBasketClick}>
            <div className={'Basket__Cart'}>
                <FontAwesomeIcon icon={faCartShopping} size="2x" className={'Basket__Cart'}/>
                <p className={'BasketCircle'}>{basketCount}</p>
            </div>
            <p className={'Basket__MyBasket'}>My Basket </p>
        </div>
    );
});
export default Basket;