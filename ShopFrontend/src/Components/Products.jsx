import React, {memo} from 'react';
import './Products.css';
import ProductComponent from "./product/ProductComponent.jsx";




const Products = memo(({products}) => {
    return (
        <div className={"products-container"}>
            {products.map((product) => (
                <ProductComponent key={product.id} product={product} 
                               
                />
            ))}
        </div>
    );
});

export default Products;