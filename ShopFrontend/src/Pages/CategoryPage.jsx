import React, {useCallback, useEffect, useMemo, useState} from 'react';
import './CategoryPage.css';
import {useParams} from "react-router-dom";
import {useGlobalContext} from "../../GlobalProvider.jsx";
import axios from "axios";
import FilterItem from "../Components/CategoryPage/Component/FilterItem.jsx";

const CategoryPage = () => {
    const {slugAndId} = useParams();
    const {API_URL} = useGlobalContext();
    const parts = slugAndId.split("-");
    const id = parts.pop();
    const slug = parts.join("-");
    const [filters, setFilters] = useState([]);
    const [category, setCategory] = useState({});
    const [products, setProducts] = useState([]);
    const [selectedFilters, setSelectedFilters] = useState({});

    useEffect(() => {
        axios.get(`${API_URL}/api/category/${id}`)
            .then(response => {
                try {
                    if (!response.data || response.data.length === 0) {
                        console.log("main category not found");
                    }
                    setCategory(response.data);
                    setProducts(response.data.products);
                    console.log(response.data);

                } catch (error) {
                    console.log(error);
                }
            })
            .catch(error => {
                console.error("Veri yüklenirken hata oluştu:", error);
            });
        axios.get(`${API_URL}/api/filter`).then(response => {
            console.log("filter:", response.data);
            setFilters(response.data);

        })
    }, [slugAndId])
    useEffect(() => {
    }, [slugAndId])
    const productCountMessage = useMemo(() => {
        if (category.products?.length > 3) {
            return "3+ ürün listeleniyor"
        } else if (category.products?.length > 0) {
            return `${category.products?.length} product listing`
        } else {
            return "doesn't have product";
        }
    }, [category]);

    const handleFilterChange = (filterType, value) => {
        console.log(selectedFilters);
        setSelectedFilters(prev => ({
            ...prev,
            [filterType]: value
        }));
        console.log(selectedFilters);
    }


    const FiltersColumn = useCallback(() => {
        return (
            <div className="category-filter-container">
                <div>
                    <a className="products-filter-count">{productCountMessage}</a>
                </div>
                {filters.map((filter) => (
                    <FilterItem
                        key={filter.id}
                        filter={filter}
                        onFilterChange={handleFilterChange}
                        selectedValue={selectedFilters[filter.id]}
                    />
                ))}
            </div>
        );
    }, [filters, handleFilterChange, productCountMessage, selectedFilters]);
    return <div>
        <h1 className="category-name">{category.name} Prices And Models</h1>
        <FiltersColumn/>

        Kategori: {slug} (ID: {id})</div>;
};
export default CategoryPage;