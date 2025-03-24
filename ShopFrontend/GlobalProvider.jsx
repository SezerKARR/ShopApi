import React, {createContext, useContext, useState} from 'react';


const GlobalContext = createContext();

export const GlobalProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const API_URL = import.meta.env.VITE_API_URL;
    const [basketCount, setBasketCount] = useState(0);

    return (
        <GlobalContext.Provider value={{ user, setUser, basketCount, setBasketCount,API_URL }}>
            {children}
        </GlobalContext.Provider>
    );
};

export const useGlobalContext = () => useContext(GlobalContext);