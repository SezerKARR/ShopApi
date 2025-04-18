import React, {useEffect, useState} from 'react';
import './Product.css';
import axios from "axios";
import {useGlobalContext} from "../../Providers/GlobalProvider.jsx";
import Products from "../../Components/Products.jsx";
import {Navigate, useNavigate} from "react-router-dom";

const Product = () => {
    const navigate = useNavigate();
    const {API_URL} = useGlobalContext();
    const SellerId=1;
    const [adminProducts, setAdminProducts] = useState(null);
    useEffect(() => {
        axios.get(`${API_URL}/api/auth/1`)
            .then((response) => {
                console.log(response.data);
            })
            .catch((error) => {
                console.error("Hata:", error);
            });

        axios.get(`${API_URL}/api/product`).then(response => {
            try {
                console.log(response.data);
                let Products = response.data;
                console.log(Products);
                const adminProductHandle=Products.filter(product => product.sellerId===SellerId );
                console.log(adminProductHandle);
                setAdminProducts(adminProductHandle);
            } catch (error) {
                console.log(error);
            }
        }).catch(error => {
            console.log(error);
        })
    }, [])
    return (
        <div className="Product">
            <div className={"product-container"}>
                {adminProducts && <Products products={adminProducts}/>
                }
            </div>
           <button className="product__addProduct" onClick={()=> navigate(`/productAdd`)}> Add Product</button>
                
          
        </div>
    );
};

export default Product;