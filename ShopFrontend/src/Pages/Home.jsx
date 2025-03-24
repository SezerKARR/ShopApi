import React, {useEffect, useState} from 'react';
import axios from "axios";
import './Home.css'
import Login from "./Login.jsx";
import { useGlobalContext} from "../../GlobalProvider.jsx";

const Home = () => {
    const{setBasketCount} =useGlobalContext();
    const [mainCategory, setMainCategory] = useState(null)
    const API_URL = import.meta.env.VITE_API_URL;
    useEffect(() => {
        axios.get(`${API_URL}/api/main-category`)
            .then(response => {
                try {
                    if (!response.data || response.data.length === 0) {
                        console.log("main category not found");
                    }
                    setMainCategory(response.data);
                } catch (error) {
                    console.log(error);
                }
            })
            .catch(error => {
                console.error("Veri yüklenirken hata oluştu:", error);
            });
    }, []);
    const ChangeBasketCount = (number) => {
        setBasketCount(prev => prev + number);
    };

    return (
        <div className={"Home"}>
            <div className={"math"}>
                <div className={"Increase"} onClick={() => ChangeBasketCount(1)}>+</div>
                <div className={"Decrease"} onClick={() => ChangeBasketCount(-1)}>-</div>
            </div>
        </div>
    );
};
export default Home;