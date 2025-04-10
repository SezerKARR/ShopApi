import React, { createContext, useContext, useEffect, useState } from 'react';
import axios from "axios";

const GlobalContext = createContext();

export const GlobalProvider = ({ children, setLoading }) => {
    const [user, setUser] = useState(null);
    const API_URL = import.meta.env.VITE_API_URL;
    const [basketCount, setBasketCount] = useState(0);
    const [categories, setCategories] = useState(() => {
        // localStorage'dan veriyi al ve kontrol et
        const savedCategories = localStorage.getItem('categories');
        if (savedCategories) {
            const parsedData = JSON.parse(savedCategories);
            const currentTime = new Date().getTime();
            console.log(parsedData);
            if (currentTime - parsedData.timestamp < 180000) {
                return parsedData.categories;
            } else {
                // 30 dakikadan eski ise veriyi sil
                localStorage.removeItem('categories');
            }
        }
        return null; // Eğer veri yoksa null döndür
    });

    const fetchCategories = async () => {
        console.log("fetchCategories çalışıyor");
        try {
            const response = await axios.get(`${API_URL}/api/category`);
            if (!response.data || response.data.length === 0) {
                console.log("main category not found");
                return;
            }
            setCategories(response.data); // State'i güncelle
            localStorage.setItem('categories', JSON.stringify({
                categories: response.data,
                timestamp: new Date().getTime(),
            })); // localStorage'a kaydet
            setLoading(false); // Yükleme işlemini bitir
        } catch (error) {
            console.error("Veri yüklenirken hata oluştu:", error);
            setLoading(false);
        }
    };

    useEffect(() => {
        // Eğer categories null ise ve localStorage'da da veri yoksa, API'dan çek
        if (categories === null && !localStorage.getItem('categories')) {
            console.log("API çağrısı yapılıyor");
            setLoading(true); // Yüklemeyi başlat
            fetchCategories();
        } else {
            console.log("Veri zaten var, API çağrısı yapılmadı");
            setLoading(false); // Veri varsa, yükleme işlemine son ver
        }
    }, []); // Bağımlılık dizisi, yalnızca categories state'i değiştiğinde çalışacak

    return (
        <GlobalContext.Provider
            value={{ user, setUser, basketCount, setBasketCount, API_URL, setCategories, categories }}>
            {children}
        </GlobalContext.Provider>
    );
};

export const useGlobalContext = () => useContext(GlobalContext);
