import React, {useEffect, useState} from 'react';
import './Product.css';
import {useParams, useSearchParams} from "react-router-dom";
import {useGlobalContext} from "../../Providers/GlobalProvider.jsx";
import axios from "axios";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faGreaterThan, faHouse} from "@fortawesome/free-solid-svg-icons";
import {useBasketContext} from "../../Providers/BasketProvider.jsx";
import ProductRating from "../../Components/product/ProductRating.jsx";
import SellerInformation from "../../Components/product/SellerInformation.jsx";
import ActionButtons from "../../Components/product/ActionButtons.jsx";
import DeliveryOptions from "../../Components/product/DeliveryOptions.jsx";
import Coupon from "../../Components/product/Coupon.jsx";
import OtherSellers from "../../Components/product/OtherSellers.jsx";
import ProductQA from "../../Components/product/ProductQA.jsx";
import ProductReviews from "../../Components/product/ProductReviews.jsx";
import ProductDescription from "../../Components/product/ProductDescription.jsx";



const Product = () => {
    const {productId} = useParams();
    const [searchParams] = useSearchParams();
    const productSellerParam = searchParams.get("productSeller");

    const productSellerId = productSellerParam ? productSellerParam.split("-").pop() : "";
    const productSellerName = productSellerParam ? productSellerParam.split("-").slice(0, -1).join("-") : "";
    console.log(productSellerParam, "sellerId:", productSellerId, "sellerName:", productSellerName);
    const {API_URL} = useGlobalContext();
    const [productData, setProductData] = useState({
        product: null, mainCategoriesNames: [], currentProductSeller: null
    });
    const {addToBasket} = useBasketContext();
    const [activeTab, setActiveTab] = useState("description");

    useEffect(() => {
        const fetchProduct = async () => {
            try {
                const res = await axios.get(`${API_URL}/api/product/${productId}?includes=58`);
                const tempProduct = res.data;
                tempProduct.productSellers = tempProduct.productSellers.sort((a, b) => a.price - b.price);
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
    const handleFollowClick = () => {
        console.log("follow");
    }
    const handleAskSellerClick = () => {
        console.log("askSellerClick");
    }
    const handleAddToCartClick = () => {
        addToBasket(productData.product)
    }
    if (!productData.product && !productData.currentProductSeller) {
        return;
    }
    return (<div className={"product-container"}>
        <div className="Product">
            {console.log(productData.currentProductSeller)}
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
                    <ProductRating
                        rating={productData.product.averageRating}
                        commentCount={productData.product.commentCount}
                    />

                    <SellerInformation
                        seller={productData.currentProductSeller?.seller}
                        onFollowClick={handleFollowClick}
                        onAskSellerClick={handleAskSellerClick}
                    />

                    <div className={"product__price-container"}>{productData.product.minPrice} Tl</div>

                    <ActionButtons onAddToCartClick={handleAddToCartClick}/>

                    <DeliveryOptions/>
                    <Coupon productSeller={productData.currentProductSeller}/>
                </div>
                <OtherSellers productSellers={productData.product.productSellers}
                              currentSellerId={productData.currentProductSeller.id}/>

                {/*{productData.product?.averageRating.map((averageRating, index) => {*/}

                {/*    const rating = (averageRating * (5 - index)) * 100;*/}
                {/*    return (*/}
                {/*        <StarRating rating={rating}/>*/}

                {/*    )*/}

                {/*})}*/}

            </div>
        </div>
        <div className={"product-container__product-info-container"}>
            <div className={"product-container__product-info-container__title-label"}>
                <div onClick={() => setActiveTab("description")}>
                    Product Description
                </div>
                <div onClick={() => setActiveTab("reviews")}>
                    Reviews
                </div>
                <div onClick={() => setActiveTab("qa")}>
                    Question Answer
                </div>

            </div>
            {console.log(activeTab === "description")}
            <div>
                {activeTab === "description" && <ProductDescription product={productData.product} />}
                {activeTab === "reviews" && <ProductReviews/>}
                {activeTab === "qa" && <ProductQA/>}
            </div>
        </div>
    </div>);
};

export default Product;