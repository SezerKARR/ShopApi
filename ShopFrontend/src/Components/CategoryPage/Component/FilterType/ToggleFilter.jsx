import React, { memo, useState } from "react";
import "./ToggleFilter.css";

const ToggleFilter = memo(({ filter, onFilterChange, isActive = false }) => {
    const [checked, setChecked] = useState(isActive); // isActive başlangıç değeri olarak kullanılıyor

    const handleToggle = () => {
        const newChecked = !checked;
        setChecked(newChecked);

        setTimeout(() => {
            onFilterChange(filter.id, newChecked);
        }, 250);
    };

    return (
        <div className="category-filter-container__Toggle">
            <div className="toggle-container">
                <span>{filter.name}</span>
                <label className="toggle">
                    <input
                        type="checkbox"
                        checked={checked} // Checkbox'ın durumu buradan kontrol ediliyor
                        onChange={handleToggle}
                    />
                    <span className="slider round"></span>
                </label>
            </div>
        </div>
    );
});

export default ToggleFilter;
