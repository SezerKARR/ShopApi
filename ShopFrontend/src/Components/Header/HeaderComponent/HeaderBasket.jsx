import React, {memo} from 'react';
import './HeaderBasket.css';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faCartShopping} from "@fortawesome/free-solid-svg-icons";
import { useBasketContext} from "../../../Providers/BasketProvider.jsx";
import {useNavigate} from "react-router-dom";

const HeaderBasket = memo(() => {
    const {basketCount}=useBasketContext();
    const navigate = useNavigate();
    const handleBasketClick=()=>{
        navigate("/basket");
    }
    return (
        <a href={"/basket"} className={'Basket__Container'} onClick={handleBasketClick}>
            <div className={'Basket__Cart'}>
                <FontAwesomeIcon icon={faCartShopping} size="2x" className={'Basket__Cart'}/>
                <p className={'BasketCircle'}>{basketCount}</p>
            </div>
            <p className={'Basket__MyBasket'}>My Basket </p>
        </a>
    );
});
export default memo(HeaderBasket);