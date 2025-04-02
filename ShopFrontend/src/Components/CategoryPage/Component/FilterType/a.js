import React, { memo } from "react";
import "./ToggleFilter.css";
const ToggleFilter = memo(({ filter, onFilterChange, isActive = false }) => {
    const handleToggle = () => {
        // Filtrenin değerini tersine çeviriyoruz
        onFilterChange(filter.id, !isActive);
    };
    return (
        <div className="category-filter-container__Toggle">

            <div className="toggle-container">
                <span>{filter.name}</span>
                <label className="toggle">
                    <input
                        type="checkbox"
                        onChange={handleToggle}
                    />
                    <span className="slider round"></span>
                </label>

            </div>
        </div>
    );
});
export default ToggleFilter;