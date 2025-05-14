import React, {useEffect, useState} from 'react';
import './Product.css';
import {useParams, useSearchParams} from "react-router-dom";
import {useGlobalContext} from "../../Providers/GlobalProvider.jsx";
import axios from "axios";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faGreaterThan, faHouse} from "@fortawesome/free-solid-svg-icons";
import {useBasketContext} from "../../Providers/BasketProvider.jsx";
import OtherSellers from "../../Components/product/OtherSellers.jsx";
import ProductTabs from "../../Components/product/ProductTabs.jsx";
import ProductInfo from "../../Components/product/ProductInfo.jsx";
import ProductImage from "../../Components/product/ProductImage.jsx";



const Product = () => {
    const {productId} = useParams();
    const [searchParams] = useSearchParams();
    const productSellerParam = searchParams.get("productSeller");

    const productSellerId = productSellerParam ? productSellerParam.split("-").pop() : "";
    const productSellerName = productSellerParam ? productSellerParam.split("-").slice(0, -1).join("-") : "";
    const {API_URL} = useGlobalContext();
    const [productData, setProductData] = useState({
        product: null, mainCategoriesNames: [], currentProductSeller: null
    });

    useEffect(() => {
        const fetchProduct = async () => {
            try {
                const res = await axios.get(`${API_URL}/api/product/${productId}?includes=255`);
                
                const tempProduct = res.data;
                console.log(tempProduct);
                
                const mainCategoriesName = await findAllMainCategories(tempProduct);
                setProductData({
                    product: tempProduct,
                    mainCategoriesNames: mainCategoriesName,
                    currentProductSeller: tempProduct.productSellers[0]
                });

            } catch (err) {
                console.log("Error while fetching product or categories:", err);
            }
        };

        fetchProduct();
    }, [productId]);

    useEffect(() => {
        if (productData.product == null || productSellerId == "") return; // Eğer productSellers verisi yoksa çalışmasın
        console.log(productData.product);
        const tempCurrentProductSeller = productSellerId === "" ? productData.product.productSellers[0] : productData.product.productSellers.find(ps => ps.id == productSellerId);
        console.log(tempCurrentProductSeller);
        setProductData(prevData => ({
            ...prevData, currentProductSeller: tempCurrentProductSeller
        }));
    }, [productSellerId]);

    const findAllMainCategories = async (product) => {
        const result = [];
        let currentId = product.categoryId;

        while (currentId > 0) {
            try {
                const res = await axios.get(`${API_URL}/api/category/${currentId}`);
                result.unshift(res.data.name);
                currentId = res.data.parentId;
            } catch (err) {
                console.log("Error fetching category:", err);
                break;
            }
        }
        return result;
    };
  
    if (!productData.product && !productData.currentProductSeller) {
        return;
    }
    return (<div className={"product-container"}>
        <div className="Product">
            {productData.mainCategoriesNames && (<div className="main-categories-container">
                <FontAwesomeIcon icon={faHouse}/>
                {productData.mainCategoriesNames.map((categoryName, index) => (<React.Fragment key={index}>
                    <FontAwesomeIcon icon={faGreaterThan} size="xs"/>
                    <span className="main-categories-container__item">{categoryName}</span>
                </React.Fragment>))}
            </div>)}

            <div className={"product__main"}>
                
               <ProductImage productImages={productData.product?.productImages} API_URL={API_URL} />
                <ProductInfo productData={productData} />
                
                <OtherSellers productSellers={productData.product.productSellers}
                              currentSellerId={productData.currentProductSeller.id}/>
            </div>
        </div>
        <ProductTabs product={productData.product}/>
    </div>);
};

export default Product;