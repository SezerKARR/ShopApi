import React, {createContext, useContext, useState} from 'react';


const GlobalContext = createContext();

export const GlobalProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [basketCount, setBasketCount] = useState(0);

    return (
        <GlobalContext.Provider value={{ user, setUser, basketCount, setBasketCount }}>
            {children}
        </GlobalContext.Provider>
    );
};

export const useGlobalContext = () => useContext(GlobalContext);