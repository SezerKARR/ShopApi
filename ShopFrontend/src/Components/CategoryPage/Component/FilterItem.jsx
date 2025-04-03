import React, {memo} from "react";
import CheckboxFilter from "./FilterType/CheckboxFilter.jsx";
import ToggleFilter from "./FilterType/ToggleFilter.jsx";

const FilterItem = memo(({filter, onFilterChange, selectedValue}) => {
    const filterComponents = {
        1: <ToggleFilter
            filter={filter}
            onFilterChange={onFilterChange}
            isActive={selectedValue}
        />,
        3: <CheckboxFilter
            filter={filter}
            onFilterChange={onFilterChange}
            selectedOptions={selectedValue}
        />,

    };
    return (
        <div className="filter-item">
            <h4>{filter.name}</h4>
            {filterComponents[filter.type] || null}
        </div>
    );
});

export default FilterItem;