import React from 'react';
import './Products.css';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faStar} from "@fortawesome/free-solid-svg-icons";
import {useGlobalContext} from "../../GlobalProvider.jsx";

const Products = ({products, OnClickProduct}) => {
    const {API_URL}=useGlobalContext()
    function Product({product}) {
        return (
            <div  className={"products-Container__product"} onClick={OnClickProduct}>
                <img src={`${API_URL}/${product.imageUrl}`} className={"products-Container__product-photo"}
                     alt="Product image"/>
                <div className={"products-Container__product-name"}>{product.name}</div>
                <div>
                    <FontAwesomeIcon icon={faStar}/>
                </div>
                <div className={"products-Container__product-price"}>  {product.price}</div>
            </div>

        )

    }
    const repeatedProducts = Array.from({length: 4}).flatMap(() => products || []);

    return (
        <div className={"products-container"}>
            {repeatedProducts.map((product ,index) => (
                <Product key={`${product.id}-${index}`}  product={product}/>
            ))}
        </div>
    );
};

export default Products;