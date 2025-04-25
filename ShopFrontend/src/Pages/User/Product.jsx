import React, {use, useEffect, useState} from 'react';
import './Product.css';
import {useParams} from "react-router-dom";
import {useGlobalContext} from "../../Providers/GlobalProvider.jsx";
import axios from "axios";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faCamera, faCheck, faGreaterThan, faHouse} from "@fortawesome/free-solid-svg-icons";
import StarRating from "../../Components/CategoryPage/Component/StarRating.jsx";

const Product = () => {
    const {productId} = useParams();
    const {API_URL} = useGlobalContext();
    const [productData, setProductData] = useState({
        product: null, mainCategoriesNames: [], minPriceSeller: null
    });

    useEffect(() => {
        const fetchProduct = async () => {
            try {

                const res = await axios.get(`${API_URL}/api/product/${productId}?includes=26`);
                const tempProduct = res.data;
                const mainCategoriesName = await findAllMainCategories(tempProduct);
                const resSeller = await axios.get(`${API_URL}/api/ProductSeller/${tempProduct.minPriceSellerId}?includes=1`);
                console.log(resSeller.data.seller);

                // const brandRes = await axios.get(`${API_URL}/api/brand/${tempProduct.brandId}`);
                setProductData({
                    product: tempProduct, mainCategoriesNames: mainCategoriesName, minPriceSeller: resSeller.data.seller
                });
            } catch (err) {
                console.log("Error while fetching product or categories:", err);
            }
        };

        fetchProduct();
    }, [productId]);
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
    const handleFollowClick = () => {
    console.log("follow");
    }
    const handleAskSellerClick = () => {
        console.log("askSellerClick");
    }
    if (!productData.product) {
        return;
    }
    return (<div className="Product">
            {console.log(productData.product)}
            {productData.mainCategoriesNames && (<div className="main-categories-container">
                <FontAwesomeIcon icon={faHouse}/>
                {productData.mainCategoriesNames.map((categoryName, index) => (<React.Fragment key={index}>
                    <FontAwesomeIcon icon={faGreaterThan} size="xs"/>
                    <span className="main-categories-container__item">{categoryName}</span>
                </React.Fragment>))}
            </div>)}


            <div className={"product__main"}>
                <div className={"image-container"}>
                    {productData.product?.imageUrl && (<img
                        className="product--image"
                        alt={productData.product.id}
                        src={`${API_URL}/${productData.product.imageUrl}`}
                    />)}

                </div>
                <div className={"product__information-container"}>
                    <div className={"product__information-container__name-container"}><h1
                        className={"Product__information-container__title"}>{productData?.product?.brandName}</h1>
                        <h1 className={"Product__information-container__title-product-name"}>{productData.product?.name}</h1>
                    </div>

                    <div className={"product__information-container__comment"}>
                        <StarRating rating={productData.product?.averageRating} count={5}/>
                        <div
                            className={"product__information-container__comment-count"}>{productData.product.commentCount}{" "} comment
                        </div>
                        <FontAwesomeIcon icon={faCamera} size="xs" style={{marginTop: '1px'}}/>
                    </div>
                    <div className={"product__seller-container"}>
                        <div className="Product__seller-info">
                            <p//todo: eğer official satıcı değilse kaldırılacak alttaki element ve puanlama propu gelince puan eklenecek
                                className={"product__seller-info__seller-label"}>seller:</p>
                            <p className={"Product__seller-info__seller-name"}>{productData.minPriceSeller.name}</p>
                            <div className={"Product__seller-info__check-container"}>
                                <div className={"product__seller-info__check"}>
                                    <FontAwesomeIcon icon={faCheck} size="lg" style={{color: "#d0e6fd"}}/>
                                </div>
                                <div className={"product__seller-info__check-label-container"}>official seller</div>
                            </div>

                        </div>
                        <div className={"product__seller-container__follow-label"} onClick={handleFollowClick}>Follow</div>
                        <div className={"product__seller-container__ask-seller-label"} onClick={handleAskSellerClick}>Ask Seller</div>
                    </div>
                </div>

                {/*{productData.product?.averageRating.map((averageRating, index) => {*/}

                {/*    const rating = (averageRating * (5 - index)) * 100;*/}
                {/*    return (*/}
                {/*        <StarRating rating={rating}/>*/}

                {/*    )*/}

                {/*})}*/}

            </div>


            {/*<CategoryLabel/>*/}
        </div>
    );
};

export default Product;