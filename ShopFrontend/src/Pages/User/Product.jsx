import React, {use, useEffect, useState} from 'react';
import './Product.css';
import {useParams} from "react-router-dom";
import {useGlobalContext} from "../../Providers/GlobalProvider.jsx";
import axios from "axios";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faGreaterThan, faHouse} from "@fortawesome/free-solid-svg-icons";

const Product = () => {
    const {productId} = useParams();
    const {API_URL} = useGlobalContext();
    const [productData, setProductData] = useState({
        product: null,
        mainCategoriesNames: [],
    });

    useEffect(() => {
        axios.get(`${API_URL}/api/product/${productId}`).then((res) => {
            const tempProduct = res.data;
            findAllMainCategories(tempProduct).then(mainCategoriesName => {
                console.log(tempProduct);
                setProductData({
                    product: tempProduct,
                    mainCategoriesNames: mainCategoriesName
                });
            }).catch((err) => {
                console.log("Error while fetching main categories:", err);
            });
        }).catch((err) => {
            console.log("Error while fetching product:", err);
        });
    }, [productId]);

    const findAllMainCategories = async (product) => {
        const result = [];
        let currentId = product.categoryId;

        while (currentId > 0) {
            try {
                const res = await axios.get(`${API_URL}/api/category/${currentId}`);
                console.log(res.data);
                result.unshift(res.data.name); 
                currentId = res.data.parentId;
            } catch (err) {
                console.log("Error fetching category:", err);
                break;
            }
        }
        return result; 
    };




    // const CategoryLabel=()=>{
    //     useEffect(()=>{
    //         console.log("geldi")
    //         if(mainCategories[mainCategories.length-1]?.parentId>0){
    //            
    //             console.log("geldi2")
    //         }
    //       
    //     },[mainCategories])
    //     return (
    //        
    //         <div>
    //             sa
    //         </div>
    //     )
    // }
    return (<div className="Product">
        {productData.mainCategoriesNames && (
            <div className="main-categories-container">
                <FontAwesomeIcon icon={faHouse}/>
                {productData.mainCategoriesNames.map((categoryName,index) => (
                    <React.Fragment key={index}>
                        <FontAwesomeIcon icon={faGreaterThan} size="xs"/>
                        <span className="main-categories-container__item">{categoryName}</span>
                    </React.Fragment>
                ))}
            </div>
        )}


        <div className={"product__main"}>
            <div className={"image-container"}>
                {productData.product?.imageUrl && (
                    <img
                        className="product--image"
                        alt={productData.product.id}
                        src={`${API_URL}/${productData.product.imageUrl}`}
                    />
                )}
                <div className={"product-information-container"}>
                    <h1 className={"product-information-container-title"}></h1>
                </div>
            </div>
        </div>


        {/*<CategoryLabel/>*/
        }
    </div>)
        ;
};

export default Product;