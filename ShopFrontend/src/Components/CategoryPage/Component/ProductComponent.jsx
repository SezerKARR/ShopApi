import React, { memo, useState } from 'react';
import { useGlobalContext } from "../../../Providers/GlobalProvider.jsx";
import BasketAdded from "../../Common/BasketAdded.jsx";
import foto from "../../../../public/Foto.png";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faStar } from "@fortawesome/free-regular-svg-icons";
import "./ProductComponent.css";
import {useBasketContext} from "../../../Providers/BasketProvider.jsx";
const ProductComponent = memo(({ product, OnClickProduct }) => {
    const { API_URL } = useGlobalContext();
    const {addToBasket}=useBasketContext();
    // Sepete ekleme tıklama fonksiyonu
    const handleAddToCartClick = ( e) => {
        e.stopPropagation(); 
        e.preventDefault();
        addToBasket(product); 
       
    };

    const handleProductClick = () => {
        OnClickProduct(product);
    };

    return (
        <div className={"products-Container__product"} onClick={handleProductClick}>
          
            <img
                src={product.imageUrl ? `${API_URL}/${product.imageUrl}` : foto}
                className="products-Container__product-photo"
                alt={"product image"}
            />
            <div className={"products-Container__product-name"}>{product.name}</div>
            <div>
                <FontAwesomeIcon icon={faStar} />
            </div>
            <div className={"products-Container__product-price"}>
                {product.price}
                <span className={"products-Container__product-price-Tl"}>TL</span>

                <svg
                    width="12"
                    height="12"
                    fill="none"
                    xmlns="http://www.w3.org/2000/svg"
                    className="addToCart"
                    onClick={(e) => handleAddToCartClick( e)} // tıklama olayına parametre eklenmiş
                >
                    <path
                        d="M0.5 0.5H0.866019C1.34345 0.5 1.7543 0.8375 1.84699 1.30585L2.875 6.5M2.875 6.5L3.11134 7.69415C3.20404 8.1625 3.61488 8.5 4.09231 8.5H10M2.875 6.5H9.71922C10.1781 6.5 10.5781 6.1877 10.6894 5.74254L11.4701 3.74254C11.6279 3.11139 11.1506 2.5 10.5 2.5M6.5 0.75V2.5M6.5 2.5V4.25M6.5 2.5H8.25M6.5 2.5H4.75M5 11C5 10.7239 4.77614 10.5 4.5 10.5C4.22386 10.5 4 10.7239 4 11C4 11.2761 4.22386 11.5 4.5 11.5C4.77614 11.5 5 11.2761 5 11ZM10 11C10 10.7239 9.77614 10.5 9.5 10.5C9.22386 10.5 9 10.7239 9 11C9 11.2761 9.22386 11.5 9.5 11.5C9.77614 11.5 10 11.2761 10 11Z"
                        stroke="#1E1E1E"
                    ></path>
                </svg>
            </div>
        </div>
    );
});

export default ProductComponent;
