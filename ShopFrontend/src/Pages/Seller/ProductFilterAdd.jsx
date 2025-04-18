import React, {useEffect, useState} from 'react';
import './ProductFilterAdd.css';
import axios from "axios";
import {useGlobalContext} from "../../../GlobalProvider.jsx";

const ProductFilterAdd = () => {
    const {API_URL,categories} = useGlobalContext();
    const [products, setProducts] = useState([]);
    const [selectedCategory, setSelectedCategory] = useState(null);
    const [filters, setFilters] = useState([])
    const [selectedCategoryFilter, setSelectedCategoryFilter] = useState(null)
    useEffect(() => {
        axios.get(`${API_URL}/api/product`).then(response => {
            try {
                console.log(response.data);
                setProducts(response.data);

            } catch (e) {
                console.log(e);
            }

        }).catch(err => {
            console.log(err);
        })
        // axios.get(`${API_URL}/api/category`).then(response => {
        //     try {
        //         console.log(response.data);
        //         setCategories(response.data);
        //     } catch (e) {
        //         console.log(e);
        //     }
        // }).catch(err => {
        //     console.log(err);
        // })
        axios.get(`${API_URL}/api/filter`).then(response => {
            try {
                console.log(response.data);

                setFilters(response.data);
            } catch (err) {
                console.log(err);
                setFilters(null);
            }
        }).catch(err => {
            console.log(err);
        })
    }, [])
    
    const CategorySelect = ({ category }) => {
        console.log(category)
        return (<>


            {category && category.name && (
                <button key={category?.id} onMouseEnter={() => console.log(category.name)} onClick={() => {

                    SelectCategory(category);
                }}>
                    {category?.name}
                </button>
            )}
        </>)

    }
    const SelectCategory = (category) => {
        // setSelectedCategory(category);
        // setSelectedCategoryFilter(filters.filter(filter => filter.categoryId === category.id))
        return(
            <div className="ProductCategory"></div>
        )

    }
    const SelectFilterValue=({Product,Filters})=>{
        
    }
    return (
        <div className="ProductFilterAdd">
            {selectedCategory === null ? (categories.map((category) => (
                <CategorySelect category={category}/>
            ))) : (
                <div>{console.log(filters)}</div>
            )}

        </div>
    );
};

export default ProductFilterAdd;