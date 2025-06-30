import React, {useEffect, useMemo, useRef, useState} from 'react';
import './CategoryPage.css';
import { useNavigate, useParams} from "react-router-dom";
import {useGlobalContext} from "../../Providers/GlobalProvider.jsx";
import axios from "axios";
import Products from "../../Components/Products.jsx";
import FiltersColumn from "../../Components/CategoryPage/FiltersColumn.jsx";


const CategoryPage = () => {

    const {slugAndId} = useParams();
    const {categories, API_URL} = useGlobalContext();
    const parts = slugAndId.split("-");
    const categoryId = parts.pop();
    const slug = parts.join("-");
    
    // const [selectedFilterValueIds, setSelectedFilterValueIds] = useState([]);
    const [categoryState, setCategoryState] = useState({
        category: null,
        brands: [],
        filters: [],
        loading: true,
        error: null,
    });
    const isInitialRender = useRef(true);
    const [selectedFilters, setSelectedFilters] = useState({});
    const [selectedBrandIds, setSelectedBrandIds] = useState([]);
    const productsRef = useRef([]);
    const lastChangedFilterRef = useRef({
        filterId: null,
        filterValueId: null,
        boolean: null,
    });
    const [filterRequest, SetFilterRequest] = useState({
        categoryId: 59,
        groupedFilterValues: null,
        brandIds: null,
        minPrice: -1,
        maxPrice: -1,
        priceRangeId:-1,
    });
    const fetchCategoryData = async () => {
        try {
            if (!categoryId || categories.length === 0) return;

            const tempCategory = categories.find((c) => c.id == categoryId);

            let products = await handleProducts();
            let filters = await fetchFilters();
            let brands = await fetchBrands();
            productsRef.current = products;
            return {
                category: tempCategory,
                filters: filters,
                brands: brands,
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
    const fetchBrands = async () => {
        const response = await axios.get(`${API_URL}/api/brandCategory/brandsByCategoryId/${categoryId}`)
        return response.data;
    }
    const fetchFilters = async () => {
        const response = await axios.get(`${API_URL}/api/filter/by-category/${categoryId}?includes=2`);
        return response.data;
    };

    const handleProducts = async () => {
        const response = await axios.get(`${API_URL}/api/product/by-category/${categoryId}?includes=258`);
        response.data=response.data
        let productsWihStocks=response.data.filter(p=>p.minPrice!=null)
        console.log(productsWihStocks);
        return productsWihStocks;
    };


    useEffect(() => {
        fetchCategoryData().then(setCategoryState);
    }, []);


    useEffect(() => {
        if (isInitialRender.current) {
            return;
        }
        const fetchProducts = async () => {
            setCategoryState(prev => ({
                ...prev,
                loading: true,
                error: null
            }));
            const delay = (ms) => new Promise(resolve => setTimeout(resolve, ms));
            try {
                await delay(200);
                console.log(JSON.stringify(filterRequest));
                const response = await axios.post(`${API_URL}/api/product/by-filters`, filterRequest);
                const products = response.data;

                productsRef.current = products;

                setCategoryState(prev => ({
                    ...prev,
                    loading: false
                }));

                if (products.length === 0) {
                    console.log('Son filtre:', lastChangedFilterRef.current);
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

        fetchProducts().then(() => "");
    }, [filterRequest]);


    const handleFilterChange = (filterId, filterValueId, boolean) => {
        console.log(filterId, filterValueId, boolean);

        setTimeout(() => {
            lastChangedFilterRef.current = {
                filterId: filterId,
                filterValueId: filterValueId,
                boolean: boolean

            }


            setSelectedFilters(prev => {
                const current = prev[filterId] ?? [];

                const updated =
                    boolean
                        ? [...current, filterValueId]
                        : current.filter(id => id !== filterValueId);

                const newSelected = {
                    ...prev,
                    [filterId]: updated
                };

                if (updated.length === 0) {
                    delete newSelected[filterId];
                }

                SetFilterRequest(prevFilter => ({
                    ...prevFilter,
                    groupIds: newSelected
                }));

                return newSelected;
            });


        }, 150);
    };


    const productCountMessage = useMemo(() => {

        if (productsRef.current.length <= 0) {
            return "doesn't have product"
        } else if (productsRef.current?.length > 3) {
            return "3+ ürün listeleniyor"
        } else if (productsRef.current?.length > 0) {
            return `${productsRef.current?.length} product listing`
        }
    }, [productsRef.current]);
    const handleBrandChange = (brandId) => {
        setSelectedBrandIds((prev) => {
            const updatedBrands =
                prev.includes(brandId)
                    ? prev.filter((id) => id !== brandId)
                    : [...prev, brandId];

            SetFilterRequest((prevFilter) => ({
                ...prevFilter,
                brandIds: updatedBrands
            }));

            return updatedBrands;
        });
    };
    const handleRangeChange = (minValue, maxValue,valueId=0) => {
        SetFilterRequest((prev) => ({
            ...prev,
            minPrice: minValue,
            maxPrice: maxValue,
            priceRangeId: valueId
            
        }));
    };
    console.log(filterRequest);


   
  
   
    console.log("asd");
    if (categoryState.loading) {
        return null;
    }
 

    isInitialRender.current = false;
    
    return (
        <div className={`CategoryPage ${categoryState.loading ? 'loading' : ''}`}>
            
            <div>

                <h1 className="category-name">{categoryState.category?.name} Prices And Models</h1>
                <div className={"category-column-container"}>
                    <div className="filters-sticky">
                        {
                            categoryState.filters.length > 0 &&
                            <FiltersColumn
                                productCount={productsRef.current.length}
                                brands={categoryState.brands}
                                selectedBrandIds={selectedBrandIds}
                                onBrandChange={handleBrandChange}
                                filters={categoryState.filters}
                                onFilterChange={handleFilterChange}
                                selectedFilters={selectedFilters}
                                filterRequest={filterRequest}
                                onRangeChange={handleRangeChange}
                            />

                        }
                    </div>
                    <div style={{flex: 1}}>
                        <Products products={productsRef.current} 
                                  // OnClickProduct={handleProductClick}  
                        />

                    </div>
                </div>


                Kategori: {slug} (ID: {categoryId})
            </div>
        </div>
    )
};
export default CategoryPage;