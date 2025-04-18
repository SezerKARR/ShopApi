import React, {useEffect, useMemo, useRef, useState} from 'react';
import './CategoryPage.css';
import {useNavigate, useParams} from "react-router-dom";
import {useGlobalContext} from "../../Providers/GlobalProvider.jsx";
import axios from "axios";
import FilterItem from "../../Components/CategoryPage/Component/FilterItem.jsx";
import Products from "../../Components/Products.jsx";
import BrandFilter from "../../Components/CategoryPage/Component/FilterType/BrandFilter.jsx";


const CategoryPage = () => {

    const {slugAndId} = useParams();
    const {categories, API_URL} = useGlobalContext();
    const parts = slugAndId.split("-");
    const categoryId = parts.pop();
    const slug = parts.join("-");
    const navigate = useNavigate();
    // const [selectedFilterValueIds, setSelectedFilterValueIds] = useState([]);
    const [categoryState, setCategoryState] = useState({
        category: null,
        brands: [],
        filters: [],
        loading: true,
        error: null,
    });
    const [selectedFilters, setSelectedFilters] = useState({});
    const [selectedBrandIds, setSelectedBrandIds] = useState([]);
    const productsRef = useRef([]);
    const [overlayVisible, setOverlayVisible] = useState(false);
    const [lastChangedFilter, setLastChangedFilter] = useState({
        filterId: null,
        filterValueId: null,
        boolean: null,
    });

    const fetchCategoryData = async () => {
        try {
            if (!categoryId || categories.length === 0) return;

            const tempCategory = categories.find((c) => c.id == categoryId);

            let products = await handleProducts();
            let filters = await fetchFilters();
            let brands=await fetchBrands();
            productsRef.current = products;
            return {
                category: tempCategory,
                filters: filters,
                brands:brands,
                loading: false,
                error: null,
            };
        } catch (err) {
            return {
                ...categoryState,
                loading: false,
                error: err,
            };
        }
    };
    const fetchBrands=async ()=>{
        const response=await  axios.get(`${API_URL}/api/brand/${categoryId}`)
        console.log(response)
        return response.data;
    }
    const fetchFilters = async () => {
        const response = await axios.get(`${API_URL}/api/filter/by-category/${categoryId}`);
        console.log(response.data)
        return response.data;
    };

    const handleProducts = async () => {
        const response = await axios.get(`${API_URL}/api/product/by-category/${categoryId}`);
        return response.data;
    };


    useEffect(() => {
        fetchCategoryData().then(setCategoryState);
    }, [categoryId]);

   


    const getFilteredProducts = async (filterRequest) => {
        setCategoryState(prev => ({
            ...prev,
            loading: true,
            error: null
        }));
        const delay = (ms) => new Promise(resolve => setTimeout(resolve, ms));
        try {
            await delay(2000);
            const response = await axios.post(`${API_URL}/api/product/by-filters`, filterRequest);

            const products = response.data;
            setCategoryState(prev => ({
                ...prev,
                loading: false
            }));
            productsRef.current = products;

            if (products.length === 0) {
                console.log(lastChangedFilter); // bu log kalabilir UI için
            }
        } catch (error) {
            console.error('Error fetching filtered products:', error);

            setCategoryState(prev => ({
                ...prev,
                loading: false,
                error: 'Bir hata oluştu.'
            }));
        }
    };


    useEffect(() => {
      
    }, [selectedFilters]);
  
    const handleFilterChange = (filterId, filterValueId, boolean) => {
        console.log(filterId, filterValueId, boolean);

        setTimeout(() => {
            setLastChangedFilter({
                filterId,
                filterValueId,
                boolean,
            });

            console.log(lastChangedFilter);
            
            
            const newSelectedFilters = { ...selectedFilters };
            const subCurrent = newSelectedFilters[filterId] || [];

            if (subCurrent.includes(filterValueId)) {
                if (boolean === false) {
                    newSelectedFilters[filterId] = subCurrent.filter(id => id !== filterValueId);

                    if (newSelectedFilters[filterId].length === 0) {
                        delete newSelectedFilters[filterId];
                    }
                }
            } else {
                newSelectedFilters[filterId] = [...subCurrent, filterValueId];
            }

            setSelectedFilters(newSelectedFilters);
            
            
           
            const filterRequest = {
                categoryId: categoryId,
                groupedFilterValues: newSelectedFilters,
            };
            console.log(getFilteredProducts(filterRequest));
            

        


        }, 150);
    };
    

    
    const productCountMessage = useMemo(() => {
        console.log(productsRef);
        if (productsRef.current?.length > 3) {
            return "3+ ürün listeleniyor"
        } else if (productsRef.current?.length > 0) {
            return `${productsRef.current?.length} product listing`
        } else {
            return "doesn't have product";
        }
    }, [productsRef.current]);
    const handleBrandChange = (brandId) => {
        setSelectedBrandIds((prev) =>
            prev.includes(brandId)
                ? prev.filter((id) => id !== brandId)
                : [...prev, brandId]
        );
    };

    const FiltersColumn = () => {
        return (<div className="category-filter-container">
            <div>
                <a className="products-filter-count">{productCountMessage}</a>
            </div>
            <BrandFilter
                brands={categoryState.brands}
                selectedBrandIds={selectedBrandIds}
                onBrandChange={handleBrandChange}
            />
            {categoryState.filters.map((filter) => {
               
                return (
                    <FilterItem
                        key={filter.id}
                        filter={filter}
                        onFilterChange={handleFilterChange}
                        selectedValue={selectedFilters[filter.id] || []}

                    />
                );
            })}

        </div>);
    }


    const handleProductClick = (product) => {
        console.log(product);
        navigate(`/product/${product.id}`);
    }
    console.log(categoryState.loading)
    return (
        <div className={`CategoryPage ${categoryState.loading ? 'loading' : ''}`}>
            <div>

                <h1 className="category-name">{categoryState.category?.name} Prices And Models</h1>
                <div className={"category-column-container"}>
                    <div className="filters-sticky">
                        <FiltersColumn/>
                    </div>
                    <div style={{flex: 1}}>
                        {productsRef.current &&
                            <Products products={productsRef.current} OnClickProduct={handleProductClick}/>}

                    </div>
                </div>


                Kategori: {slug} (ID: {categoryId})
            </div>
        </div>
    )
};
export default CategoryPage;