import React from 'react';
import './ProductDescription.css';

const ProductDescription = ({ product }) => {
    console.log(product.description);
    if(product.description==null)return ;
    return (
        <div className="ProductDescription-container">
            {product.description }
        </div>
    );
};

export default ProductDescription;