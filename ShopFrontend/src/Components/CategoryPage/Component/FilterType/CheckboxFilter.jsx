import React, {memo} from "react";

const CheckboxFilter = memo(({filter, onFilterChange, selectedOptions = {}}) => {
    const handleChange = (optionId, checked) => {


    

        onFilterChange(filter.id, optionId,checked);
    };
    const handleValue = (optionId) => {

        return selectedOptions.includes(optionId);
    };
    return (
        <div className="checkbox-filter">
            <h4>{filter.name}</h4>
            {filter.values?.map(option => (
                <div key={option.id} className="checkbox-option">
                   
                    <input
                        type="checkbox"
                        id={`option-${option.id}`}
                        checked={ handleValue(option.id) }
                        onChange={(e) => handleChange(option.id, e.target.checked)}
                    />
                    <label htmlFor={`option-${option.id}`}>{option.name}</label>
                </div>
            ))}
        </div>
    );
});

export default CheckboxFilter;