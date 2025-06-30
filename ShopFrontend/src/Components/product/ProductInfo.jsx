import React from 'react';
import './ProductInfo.css';
import ProductRating from "./ProductRating.jsx";
import SellerInformation from "./SellerInformation.jsx";
import ActionButtons from "./ActionButtons.jsx";
import DeliveryOptions from "./DeliveryOptions.jsx";
import Coupon from "./Coupon.jsx";

const ProductInfo = ({productData, onFollowClick, onAskSellerClick }) => {
    return (
        <div className="product-info">
            <div className="product-info__header">
                <h1 className="product-info__brand">{productData.product?.brandName}</h1>
                <h1 className="product-info__name">{productData.product?.name}</h1>
            </div>

            <ProductRating
                rating={productData.product.averageRating}
                commentCount={productData.product.commentCount}
            />

            <SellerInformation
                seller={productData.currentProductSeller?.seller}
                onFollowClick={onFollowClick}
                onAskSellerClick={onAskSellerClick}
            />

            <div className="product-info__price">{productData.product.minPrice} TL</div>
            {console.log(productData)}
            <ActionButtons   productSeller={productData.currentProductSeller}/>
            <DeliveryOptions />
            <Coupon productSeller={productData.currentProductSeller} />
        </div>
    );
};

export default ProductInfo;