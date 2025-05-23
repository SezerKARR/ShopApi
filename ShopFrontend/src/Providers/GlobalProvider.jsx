import React, {createContext, useContext, useEffect, useState} from 'react';
import axios from "axios";

const GlobalContext = createContext();

export const GlobalProvider = ({children, setLoading}) => {
    const [user, setUser] = useState(null);
    const API_URL = import.meta.env.VITE_API_URL;
    const [categories, setCategories] = useState([]);
    const [products, setProducts] = useState([]);


    useEffect(() => {
        const fetchAll = async () => {
            try {

                const categoryRes = await axios.get(`${API_URL}/api/category`);
                const productRes = await axios.get(`${API_URL}/api/product`);
                const userRes = await axios.get(`${API_URL}/api/auth/1`)
                setUser(userRes.data);
                setCategories(categoryRes.data);
                setProducts(productRes.data);
                setLoading(false);
            } catch (err) {
                console.error("Init error", err);
            }
        };

        fetchAll();
    }, []);


    return (
        <GlobalContext.Provider
            value={{user, setUser,  API_URL, categories, products}}>
            {children}
        </GlobalContext.Provider>
    );
};

export const useGlobalContext = () => useContext(GlobalContext);
