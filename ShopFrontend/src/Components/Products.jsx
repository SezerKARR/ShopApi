import React, {memo, useState} from 'react';
import './Products.css';
import ProductComponent from "./CategoryPage/Component/ProductComponent.jsx";




const Products = memo(({products, OnClickProduct}) => {
    return (
        <div className={"products-container"}>
            {products.map((product) => (
                <ProductComponent key={product.id} product={product} OnClickProduct={OnClickProduct} />
            ))}
        </div>
    );
});

export default Products;