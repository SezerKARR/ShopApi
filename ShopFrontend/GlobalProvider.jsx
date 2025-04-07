import React, {createContext, useContext, useEffect, useState} from 'react';
import axios from "axios";


const GlobalContext = createContext();

export const GlobalProvider = ({children,setLoading}) => {
    const [user, setUser] = useState(null);
    const API_URL = import.meta.env.VITE_API_URL;
    const [basketCount, setBasketCount] = useState(0);
    const [categories, setCategories] = useState(null);
    useEffect(() => {
        if (categories == null) {
            axios.get(`${API_URL}/api/category`)
                .then(response => {
                    try {
                        if (!response.data || response.data.length === 0) {
                            console.log("main category not found");
                        }
                        setCategories(response.data);
                        setLoading(false);
                        // setHoveredMainCategory(filteredCategories[2]);
                    } catch (error) {
                        console.log(error);
                    }
                })
                .catch(error => {
                    console.error("Veri yüklenirken hata oluştu:", error);
                });
        }

    }, []);
    return (
        <GlobalContext.Provider
            value={{user, setUser, basketCount, setBasketCount, API_URL, setCategories, categories}}>
            {children}
        </GlobalContext.Provider>
    );
};

export const useGlobalContext = () => useContext(GlobalContext);