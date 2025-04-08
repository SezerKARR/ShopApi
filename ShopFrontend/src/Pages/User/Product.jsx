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
    const [product, setProduct] = useState({});
    const [category, setCategory] = useState({});
    const [mainCategories, setMainCategories] = useState([]);

    useEffect(() => {
        axios.get(`${API_URL}/api/product/${productId}`).then((res) => {
            const tempProduct = res.data;
            const tempCategory = categories.find((c) => c.id == tempProduct.categoryId);
            setCategory(tempCategory);
            setProduct(tempProduct);
            findAllMainCategories(tempCategory.parentId);
            console.log(res.data, categories.find((c) => c.id == tempProduct.categoryId));
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

            result.unshift(category); // en üste ekle
            currentId = category.parentId;
        }
        console.log(result);
        setMainCategories(result);
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
        {mainCategories && (<div className={"Product-container__main-categories-container"}>
            <FontAwesomeIcon icon={faHouse}/>
            {mainCategories.map((category) => (<>
                <FontAwesomeIcon icon={faGreaterThan}/>
                <span className="Product-container__main-categories-container__item" key={category.id}>{category.name}</span>
            </>))}


        </div>)}
        {/*<CategoryLabel/>*/}
    </div>);
};

export default Product;