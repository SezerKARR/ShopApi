import React, {useEffect, useState} from 'react';
import axios from "axios";
import './Home.css'
const Home = () => {
    const [mainCategory, setMainCategory] = useState(null)
    const API_URL = import.meta.env.VITE_API_URL;
    useEffect(() => {
        console.log(API_URL);
        axios.get(`${API_URL}/api/main-category`)
            .then(response => {
                try {
                    if (!response.data || response.data.length === 0) {
                       console.log("main category not found");
                    }
                    setMainCategory(response.data);
                    console.log(response.data);
                } catch (error) {
                    console.log(error);
                }


            })

            .catch(error => {
                console.error("Veri yüklenirken hata oluştu:", error);
            });
    }, []);
    return (
        <div className={"Home"}>
            <ul className="MainCategoryHolder">

                <li className="MainCategoryName">{mainCategory?.[0]?.name}</li>

            </ul>
        </div>
    );
};

export default Home;