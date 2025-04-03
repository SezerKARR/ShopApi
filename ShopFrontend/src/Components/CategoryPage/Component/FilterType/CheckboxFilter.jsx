import React, { memo } from "react";

const CheckboxFilter = memo(({ filter, onFilterChange, selectedOptions = {} }) => {
    const handleChange = (optionId, checked) => {
        
        const newSelectedOptions = {
            
            ...selectedOptions,
            [optionId]: checked
        };
       
        // Ana bileşene değişikliği bildiriyoruz
        onFilterChange(filter.id, newSelectedOptions);
    };
    return (
        <div className="checkbox-filter">
            {filter.values?.map(option => (
                <div key={option.id} className="checkbox-option">
                    <input
                        type="checkbox"
                        id={`option-${option.id}`}
                        checked={selectedOptions[option.id] || false}
                        onChange={(e) => handleChange(option.id, e.target.checked)}
                    />
                    <label htmlFor={`option-${option.id}`}>{option.name}</label>
                </div>
            ))}
        </div>
    );
});

export default CheckboxFilter;