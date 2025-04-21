import React, { createContext, useContext, useState } from "react";
import BasketAdded from "../Components/Common/BasketAdded.jsx";

const BasketContext = createContext();

export const BasketProvider = ({ children }) => {
    const [basketItems, setBasketItems] = useState([]);
    const [basketCount, setBasketCount] = useState(0);
    const addToBasket = (product) => {
        setBasketItems(prev => [...prev, product]);
        setBasketCount(prev => prev + 1);
    };

    const removeFromBasket = (productId) => {
        setBasketItems(prev => prev.filter(item => item.id !== productId));
        setBasketCount(prev => Math.max(prev - 1, 0));
    };

    const clearBasket = () => {
        setBasketItems([]);
        setBasketCount(0);
    };

    return (
        <BasketContext.Provider value={{
            basketItems,
            basketCount,
            addToBasket,
            removeFromBasket,
            clearBasket
        }}>
            {children}
            <BasketAdded   />
            
        </BasketContext.Provider>
    );
};

export const useBasketContext = () => useContext(BasketContext);
