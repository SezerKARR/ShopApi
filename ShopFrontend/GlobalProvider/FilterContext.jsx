import { createContext, useContext, useState } from "react";

const FilterContext = createContext();

export const FilterProvider = ({ children }) => {
    const [selectedFilters, setSelectedFilters] = useState({});

    const handleFilterChange = (filterType, value) => {
        setSelectedFilters(prev => ({
            ...prev,
            [filterType]: value
        }));
    };

    return (
        <FilterContext.Provider value={{ selectedFilters, handleFilterChange }}>
            {children}
        </FilterContext.Provider>
    );
};

export const useFilter = () => useContext(FilterContext);
