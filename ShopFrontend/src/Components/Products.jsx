import React from 'react';
import './Products.css';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {useGlobalContext} from "../../GlobalProvider.jsx";
import {faStar} from "@fortawesome/free-regular-svg-icons";

const Products = ({products, OnClickProduct}) => {
    const {API_URL}=useGlobalContext()
    function Product({product}) {
        return (
            <div  className={"products-Container__product"} onClick={()=>OnClickProduct(product)}>
                <img src={`${API_URL}/${product.imageUrl}`} className={"products-Container__product-photo"}
                     alt="Product image"/>
                <div className={"products-Container__product-name"}>{product.name}</div>
                <div>
                    <FontAwesomeIcon icon={faStar} />
                </div>
                <div className={"products-Container__product-price"}>  {product.price}<span className={"products-Container__product-price-Tl"}>TL</span> </div>
            </div>

        )

    }

    return (
        <div className={"products-container"}>
            {products.map((product ) => (
                <Product key={product.id}  product={product}/>
            ))}
        </div>
    );
};

export default Products;