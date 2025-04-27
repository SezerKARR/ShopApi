import React, {useEffect, useState} from 'react';
import './Product.css';
import {useParams, useSearchParams} from "react-router-dom";
import {useGlobalContext} from "../../Providers/GlobalProvider.jsx";
import axios from "axios";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faGreaterThan, faHouse} from "@fortawesome/free-solid-svg-icons";
import {useBasketContext} from "../../Providers/BasketProvider.jsx";
import ProductRating from "../../Components/CategoryPage/Component/ProductRating.jsx";
import SellerInformation from "../../Components/CategoryPage/Component/SellerInformation.jsx";
import ActionButtons from "../../Components/CategoryPage/Component/ActionButtons.jsx";
import DeliveryOptions from "../../Components/CategoryPage/Component/DeliveryOptions.jsx";
import Coupon from "../../Components/CategoryPage/Component/Coupon.jsx";
import OtherSellers from "../../Components/CategoryPage/Component/OtherSellers.jsx";


const Product = () => {
    const {productId} = useParams();
    const [searchParams] = useSearchParams();
    const sellerParam = searchParams.get("seller");
    const [sellerName, setSellerName] = useState("");
    const [sellerId, setSellerId] = useState("");
    const {API_URL} = useGlobalContext();
    const [productData, setProductData] = useState({
        product: null, mainCategoriesNames: [], currentSeller: null
    });
    const {addToBasket} = useBasketContext();

    useEffect(() => {
        const fetchProduct = async (sellerId) => {
            try {

                const res = await axios.get(`${API_URL}/api/product/${productId}?includes=58`);
                const tempProduct = res.data;
                const tempCurrentSeller=tempProduct.productSellers.filter(s=>s.sellerId==sellerId);
                console.log(tempCurrentSeller);
                console.log(tempProduct);
                const mainCategoriesName = await findAllMainCategories(tempProduct);

                // const brandRes = await axios.get(`${API_URL}/api/brand/${tempProduct.brandId}`);
                setProductData({
                    product: tempProduct, mainCategoriesNames: mainCategoriesName
                });
            } catch (err) {
                console.log("Error while fetching product or categories:", err);
            }
        };
        if (sellerParam) {
            const parts = sellerParam.split("-");
            const name = parts.slice(0, -1).join("-"); // isimde "-" olabilir diye
            const id = parts[parts.length - 1];
            console.log(id);
            console.log(name);
            setSellerName(decodeURIComponent(name));
            setSellerId(id);
            fetchProduct(id);
        }
        setSellerName(sellerName);

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
    const handleAddToCartClick = () => {
        addToBasket(productData.product)
    }
    if (!productData.product ) {
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
                <ProductRating
                    rating={productData.product.averageRating}
                    commentCount={productData.product.commentCount}
                />

                <SellerInformation
                    seller={productData.product.productSellers[0]?.seller}
                    onFollowClick={handleFollowClick}
                    onAskSellerClick={handleAskSellerClick}
                />

                <div className={"product__price-container"}>{productData.product.minPrice} Tl</div>

                <ActionButtons onAddToCartClick={handleAddToCartClick}/>

                <DeliveryOptions/>
                <Coupon productSeller={productData.product.productSellers[0]}/>
            </div>
            <OtherSellers productSellers={productData.product.productSellers}/>

            {/*{productData.product?.averageRating.map((averageRating, index) => {*/}

            {/*    const rating = (averageRating * (5 - index)) * 100;*/}
            {/*    return (*/}
            {/*        <StarRating rating={rating}/>*/}

            {/*    )*/}

            {/*})}*/}

        </div>


        {/*<CategoryLabel/>*/}
    </div>);
};

export default Product;