import './ActionButtons.css';

import React from 'react';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBookmark, faHeart } from "@fortawesome/free-regular-svg-icons";
import {useBasketContext} from "../../Providers/BasketProvider.jsx";

const ActionButtons = ({productSeller}) => {
    const {addToBasketByProductSellerId} = useBasketContext();
    return (
        <div className="save-container">
            <div className="add-basket-container"
                 onClick={()=>addToBasketByProductSellerId(productSeller.id)}
            >
                <svg
                    width="24"
                    height="24"
                    fill="none"
                    viewBox="0 0 12 12"
                    xmlns="http://www.w3.org/2000/svg"
                    className="add-basket"
                >
                    <path
                        d="M0.5 0.5H0.866019C1 0.5 1.7543 0.8375 1.84699 1.30585L2.875 6.5M2.875 6.5L3.11134 7.69415C3.20404 8.1625 3.61488 8.5 4.09231 8.5H10M2.875 6.5H9.71922C10.1781 6.5 10.5781 6.1877 10.6894 5.74254L11.4701 3.74254C11.6279 3.11139 11.1506 2.5 10.5 2.5M6.5 0.75V2.5M6.5 2.5V4.25M6.5 2.5H8.25M6.5 2.5H4.75M5 11C5 10.7239 4.77614 10.5 4.5 10.5C4.22386 10.5 4 10.7239 4 11C4 11.2761 4.22386 11.5 4.5 11.5C4.77614 11.5 5 11.2761 5 11ZM10 11C10 10.7239 9.77614 10.5 9.5 10.5C9.22386 10.5 9 10.7239 9 11C9 11.2761 9.22386 11.5 9.5 11.5C9.77614 11.5 10 11.2761 10 11Z"
                        stroke="white"
                    />
                </svg>
                <p className="add-basket-label">Add To Basket</p>
            </div>
            <div className="heart">
                <FontAwesomeIcon icon={faHeart} size="xl" />
            </div>
            <div className="book-mark">
                <FontAwesomeIcon icon={faBookmark} className="bookmark-icon" size="xl" />
            </div>
        </div>
    );
};
export default ActionButtons;