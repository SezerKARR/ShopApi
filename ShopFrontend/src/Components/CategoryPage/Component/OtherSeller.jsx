import React from 'react';
import './OtherSeller.css';
import {useNavigate} from "react-router-dom";

const OtherSeller = ({productSeller, isAllOtherSeller = false}) => {
    const navigate = useNavigate();
    if (!isAllOtherSeller) {
        const handleSeeAllClick = () => {

            navigate(`/product/${productSeller.productId}?productSeller=${productSeller.sellerName}-${productSeller.id}`);
        }
        return (
            <div className="OtherSeller-container">
                <div className="other-seller__seller-info">
                    <p className="other-seller__seller-info__seller-name">{productSeller.sellerName}</p>
                    {/*<div className="other-seller__seller-info__seller-rating">{seller.rating}</div>//todo*/}

                </div>
                <div className="other-seller__seller-delivery-time">
                    {/*todo*/}
                </div>
                <div className="other-seller__product-info">
                    <p className={"other-seller__product-info__product-price"}>{productSeller.price}tl</p>
                    <p className={"other-seller__product-info__go-to-product"} onClick={handleSeeAllClick}>See
                        product</p>
                </div>
            </div>
        );
    } else if (isAllOtherSeller) {
        return (
            <div className="other-seller__all-seller__seller-container">
                <div className="other-seller__all-seller__seller-info-container">
                    <div className={"other-seller__all-seller__seller-name"}>
                        {productSeller.sellerName}<span>
                    {/*    todo:sellerpoint*/}
                    </span>
                        
                    </div>
                    <div className="other-seller__all-seller__delivery-time">
                        {/*//todo:deliverytime*/}
                    </div>
                    <div className={"other-seller__all-seller__price"}>
                        {productSeller.price}tl
                    </div>
                </div>
                <div className="other-seller__all-seller__button-container">
                    <button className="other-seller__all-seller__add-basket">
                        Add Basket
                    </button>
                    <button className={"other-seller__all-seller__see-product"}>
                        See Product
                    </button>
                </div>
            </div>
        )
    }

};

export default OtherSeller;