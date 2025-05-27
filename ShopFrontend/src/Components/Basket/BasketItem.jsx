import React, {memo, useRef} from 'react';
import './BasketItem.css';
import {useGlobalContext} from "../../Providers/GlobalProvider.jsx";
import {useNavigate} from "react-router-dom";
import BasketItemCount from "./BasketItemCount.jsx";
import {formatAsTL} from "../../utils/Formatter.jsx";

const BasketItem = memo(({basketItem, isSelected, onSelectChange,onItemChance}) => {
    const navigate = useNavigate();
    const isFirstRender = useRef(true);
    const {API_URL} = useGlobalContext();
    console.log(basketItem);
    const getNavigateProp = () => {
        return `/product/${basketItem.product.id}`;
    }
    const handleMouseDown = (event) => {
        const url = getNavigateProp();

        if (event.button === 0) { // Sol Tıklama
            event.preventDefault();
            navigate(url);

        }
    };
    const handleQuantityChange = (quantity) => {
        basketItem.quantity = quantity;
        console.log(quantity);
        onItemChance(basketItem)
    }
    isFirstRender.current = false;
    return (
        <div className="basket-item">
            <label className="custom-checkbox">
                <input
                    type="checkbox"
                    checked={isSelected}
                    onChange={(e) => onSelectChange(e.target.checked)}
                />
                <span className="checkmark"></span>
            </label>
            <img alt={basketItem.id} className={"basket-item__image"}
                 src={`${API_URL}/${basketItem.product.productImages[0]?.image?.url}`}
            />
            <div className={"basket-item__info"}>
                {console.log(basketItem.product)}
                <a href={getNavigateProp()}
                   className={"basket-item__product-title"}
                   style={{textTransform:"uppercase"}}
                   onMouseDown={handleMouseDown}>{basketItem.product.brandName}<span>{" "+basketItem.product.name}</span></a>
                
                <div className={"basket-item__info__count-money"}>
                    <BasketItemCount quantity={basketItem.quantity} onItemQuantityChange={handleQuantityChange}/>
                    <p className={"basket-item__info__basket-count-money__money"}>{formatAsTL(basketItem.totalPrice)}TL</p>
                </div>

            </div>
        </div>
    );
});
export default BasketItem;