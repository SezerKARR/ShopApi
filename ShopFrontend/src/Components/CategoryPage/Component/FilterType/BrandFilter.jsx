import React from 'react';
import './BrandFilter.css';

const BrandFilter = ({ brands, selectedBrandIds, onBrandChange }) => {
    return (
        <div>
            <h4>Brands</h4>
            {brands.map((brand) => (
                <label key={brand.id}>
                    <input
                        type="checkbox"
                        checked={selectedBrandIds.includes(brand.id)}
                        onChange={() => onBrandChange(brand.id)}
                    />
                    {brand.name}
                </label>
            ))}
        </div>
    );
};


export default BrandFilter;