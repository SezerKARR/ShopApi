import React, {createContext, useCallback, useContext, useEffect, useState} from "react";
import BasketAdded from "../Components/Common/BasketAdded.jsx";
import {useGlobalContext} from "./GlobalProvider.jsx";
import axios from "axios";
import * as Yup from "yup";
import {useFormik} from "formik";

const BasketContext = createContext();

const validationSchemaBasketItem = Yup.object({
    userId: Yup.string().required("userId name is required"),
    quantity: Yup.number().positive("quantity must be positive").required("quantity is required"),
    productSellerId: Yup.number().required("productSellerId is required"),
});
export const BasketProvider = ({children,setBasketLoading}) => {
    const {API_URL, user} = useGlobalContext();
    console.log(user);
    const [basket, setBasket] = useState({});
    const [basketCount, setBasketCount] = useState(0);
    const [error, setError] = useState(null);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [submitResponse, setSubmitResponse] = useState({success: false, message: ""});
    const addToBasketByProductSellerId = async (productSellerId, quantity = 1) => {
        const response = await axios.post(`${API_URL}/api/basket`, {
            userId: user.id,
            quantity: quantity,
            productSellerId: productSellerId
        });
        console.log(response.data);
        // setBasketItems(prev => [...prev, product]);
        // setBasketCount(prev => prev + 1);
    };
    const addToBasketById = (productId) => {
        const product = findProductById(productId);
        if (product) {
            addToBasketByProductSellerId(product);

        } else {
            console.log("product not found");
        }

    };
    const fetchBasket = useCallback(async () => {
        setBasketLoading(true);
        setError(null);
        // Eğer sepet kullanıcıya özelse ve giriş yapılmamışsa çekme
        // if (requiresAuth && !user) {
        //     setBasketItems([]);
        //     setIsLoading(false);
        //     return;
        // }
        try {
            const response = await axios.get(`${API_URL}/api/basket/byUserId/${user.id}?include=3`); // VEYA fetch('/api/basket').then(res => res.json());
            console.log(response.data);
            setBasket(response.data);
        } catch (err) {
            console.error("Failed to fetch basket:", err);
            setError("Sepet yüklenirken bir hata oluştu.");
            setBasket(null); 
        } finally {
            setBasketLoading(false);
        }
    }, [/* user */]); // Eğer kullanıcıya bağlıysa dependency'ye user ekleyin

    // Provider ilk yüklendiğinde sepeti çek
    useEffect(() => {
        fetchBasket();
    }, [fetchBasket]);
    const findProductById = async (productId) => {
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
            basket,
            basketCount,
            addToBasket: addToBasketByProductSellerId,
            removeFromBasket,
            clearBasket,
            addToBasketById
        }}>
            {children}
            <BasketAdded/>

        </BasketContext.Provider>
    );
};

export const useBasketContext = () => useContext(BasketContext);
