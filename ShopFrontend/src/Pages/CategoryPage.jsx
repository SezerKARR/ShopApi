import React, {useEffect, useState} from 'react';
import './CategoryPage.css';
import {useParams} from "react-router-dom";
import {useGlobalContext} from "../../GlobalProvider.jsx";
import axios from "axios";

const CategoryPage = () => {
    const {slugAndId} = useParams();
    const {API_URL} = useGlobalContext();
    const parts = slugAndId.split("-");
    const id = parts.pop();
    const slug = parts.join("-");
    const [filters, setFilters] = useState({});
    const [category, setCategory] = useState({});
    const [products, setProducts] = useState([]);
    useEffect(() => {
        axios.get(`${API_URL}/api/category/${id}`)
            .then(response => {
                try {
                    if (!response.data || response.data.length === 0) {
                        console.log("main category not found");
                    }
                    console.log(response.data);
                    setCategory(response.data);
                    setProducts(response.data.products);
                    console.log(response.data.products);
                } catch (error) {
                    console.log(error);
                }
            })
            .catch(error => {
                console.error("Veri yüklenirken hata oluştu:", error);
            });
    }, [slugAndId])
    const ReturnValue = () => {
        if (category.products?.length > 3) {
            return "3+ ürün listeleniyor"
        } else if (category.products?.length > 0) {
            return `${category.products?.length}ürün listeleniyor`
        } else {
            return "ürün yok"
        }
    }
    function ToggleCheckbox() {
        const [isChecked, setIsChecked] = useState(false);

        const handleToggle = () => {
            setIsChecked(!isChecked);
        };

        return (
            <div className="toggle-container">
                <span>Status: {isChecked ? 'ON' : 'OFF'}</span>
                <label className="toggle">
                    <input
                        type="checkbox"
                        checked={isChecked}
                        onChange={handleToggle}
                    />
                    <span className="slider round"></span>
                </label>

            </div>
        );
    }

    return <div>
    <h1 className="category-name">{category.name} Prices And Models</h1>
        <div className="category-filter-container">
            <div>
                <a className="products-filter-count">{<ReturnValue />}</a>

            </div>
            <div className="category-filter-container__fast-delivery">

                <ToggleCheckbox/>
            </div>
        </div>
        Kategori: {slug} (ID: {id})</div>;
};
export default CategoryPage;