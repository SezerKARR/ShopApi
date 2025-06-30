import React from 'react';
import './TotalBasket.css';
import {formatAsTL} from "../../utils/Formatter.jsx";
import {useNavigate} from "react-router-dom";

const TotalBasket = ({basket}) => {
    const navigate = useNavigate();
    console.log('TotalBasket', basket.totalProductAmount);
    let selected=0;
    const HandleCategoryClick = () => {
        navigate(`/completeShop`)
    }
    return (
        <div className="TotalBasket-container">
            <span className={"total-basket__selected-products-label"}>SELECTED PRODUCTS({basket.totalProductAmount})</span>
            <span className={"total-basket__total-price"}>{formatAsTL(basket.grandTotal)} Tl</span>
               {/*    todo: total inidirim flaan filana eklenecek*/}
            <div className={"total-basket__complete-shop-container"}>
                <button onClick={HandleCategoryClick} className="button-primary">Complete shop</button>

            </div>
        </div>

    );
};

export default TotalBasket;