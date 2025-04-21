import React, {memo} from 'react';
import './BrandFilter.css';

const BrandFilter = memo(({brands, selectedBrandIds, onBrandChange}) => {
    return (
        <div className={"BrandFilter-container"}>
            <h4 className="label">Brands</h4>
            {brands.map((brand) => (
                <div className={"brand-option"} key={brand.id}>
                    <input
                        type="checkbox"
                        checked={selectedBrandIds?.includes(brand.id) || false}
                        onChange={() => onBrandChange(brand.id)}
                    />
                    <label  className={"brand-option-label"}>
                        {brand.name}
                    </label>
                </div>
            ))}

        </div>
    );
});


export default BrandFilter;