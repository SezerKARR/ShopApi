import React, {memo, useMemo} from 'react';
import './FiltersColumn.css';
import BrandFilter from "./Component/FilterType/BrandFilter.jsx";
import RangeFilter from "./Component/FilterType/RangeFilter.jsx";
import FilterItem from "./Component/FilterItem.jsx";

const FiltersColumn = memo(({
                                productCount,
                                brands,
                                selectedBrandIds,
                                onBrandChange,
                                filters,
                                onFilterChange,
                                selectedFilters,
                                filterRequest,
                                onRangeChange
                            }) => {
    const productCountMessage = useMemo(() => {
        if (productCount <= 0) {
            return "doesn't have product";
        } else if (productCount > 3) {
            return "3+ ürün listeleniyor";
        } else {
            return `${productCount} product listing`;
        }
    }, [productCount]);

    return (
        <div className="category-filter-container">
            <div>
                <a className="products-filter-count">{productCountMessage}</a>
            </div>
            <BrandFilter
                brands={brands}
                selectedBrandIds={selectedBrandIds}
                onBrandChange={onBrandChange}
            />
            <RangeFilter
                label={"Price"}
                minRange={filterRequest.minPrice}
                maxRange={400}
                selectedRangeId={filterRequest.priceRangeId}
                onRangeChange={onRangeChange}
                currentMinMax={{ min: filterRequest.minPrice, max: filterRequest.maxPrice }}
            />
            {filters.map((filter) => (
                <FilterItem
                    key={filter.id}
                    filter={filter}
                    onFilterChange={onFilterChange}
                    selectedValue={selectedFilters[filter.id] || []}
                />
            ))}
        </div>
    );
});

export default FiltersColumn;