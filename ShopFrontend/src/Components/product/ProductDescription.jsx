import React from 'react';
import './ProductDescription.css';

const ProductDescription = ({product}) => {
    if (product.description == null) return;
    return (
        <div className="ProductDescription-container">
            <p className="ProductDescription__product-name">  {product.name}</p>

            <br/>

            <p className="ProductDescription__product-description">{product.description}</p>
            <div className={"product-description__product-features-container"}>
                <p className={"product-description__product-features__title"}> Product Features</p>
                <div className={"product-description__product-features-container"}></div>
                {product.filterValues?.length > 0 && product.filterValues.map((productFilterValue) => (
                    <div className={"product-description__product-feature"} key={productFilterValue.id}>
                        <p className={"product-description__product-features__features-name"}> {productFilterValue.filter.name}</p>
                        <p className={"product-description__product-features__features-value"}> {productFilterValue.filterValue.name}</p>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default ProductDescription;