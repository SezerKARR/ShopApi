import React, {use, useEffect, useState} from 'react';
import './Product.css';
import {useParams} from "react-router-dom";
import {useGlobalContext} from "../../../GlobalProvider.jsx";
import axios from "axios";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faGreaterThan, faHouse} from "@fortawesome/free-solid-svg-icons";

const Product = () => {
    const {productId} = useParams();
    const {API_URL, categories} = useGlobalContext();
    const [productData, setProductData] = useState({
        product: null,
        category: null,
        mainCategories: [],
    });

    useEffect(() => {
        axios.get(`${API_URL}/api/product/${productId}`).then((res) => {
            const tempProduct = res.data;
            const tempCategory = categories.find((c) => c.id == tempProduct.categoryId);
            const mainCategories = findAllMainCategories(tempCategory?.parentId);

            setProductData({
                product: tempProduct,
                category: tempCategory,
                mainCategories: mainCategories
            });
        }).catch((err) => {
            console.log(err);
        })
    }, [productId]);
    const findAllMainCategories = (startId) => {
        const result = [];
        let currentId = startId;

        while (currentId > 0) {
            const category = categories.find(c => c.id === currentId);
            if (!category) break;

            result.unshift(category);
            currentId = category.parentId;
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
        {productData.mainCategories && (
            <div className="main-categories-container">
                <FontAwesomeIcon icon={faHouse}/>
                {productData.mainCategories.map((category) => (
                    <React.Fragment key={category.id}>
                        <FontAwesomeIcon icon={faGreaterThan} size="xs"/>
                        <span className="main-categories-container__item">{category.name}</span>
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

            </div>
        </div>


        {/*<CategoryLabel/>*/
        }
    </div>)
        ;
};

export default Product;