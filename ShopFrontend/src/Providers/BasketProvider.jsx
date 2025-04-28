import React, { createContext, useContext, useState } from "react";
import BasketAdded from "../Components/Common/BasketAdded.jsx";
import {useGlobalContext} from "./GlobalProvider.jsx";
import axios from "axios";

const BasketContext = createContext();

export const BasketProvider = ({ children }) => {
    const {API_URL} = useGlobalContext();
    const [basketItems, setBasketItems] = useState([]);
    const [basketCount, setBasketCount] = useState(0);
    const addToBasket = (product) => {
        setBasketItems(prev => [...prev, product]);
        setBasketCount(prev => prev + 1);
    };
    const addToBasketById = (productId) => {
        const product = findProductById(productId); 
        if (product) {
           addToBasket(product);
           
        }
        else{
            console.log("product not found");
        }
        
    };
    const  findProductById = async (productId) => {
        console.log(productId);
        const productRes = await axios.get(`${API_URL}/api/product/${productId}`);
        console.log(productRes.data);
        return productRes.data;


    }
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
            clearBasket,
            addToBasketById
        }}>
            {children}
            <BasketAdded   />
            
        </BasketContext.Provider>
    );
};

export const useBasketContext = () => useContext(BasketContext);
