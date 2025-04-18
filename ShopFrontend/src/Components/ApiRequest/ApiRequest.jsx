import React from 'react';
import './ApiRequest.css';
import axios from "axios";
import {useGlobalContext} from "../../../GlobalProvider.jsx";
let cachedProducts = null;
const ApiRequest = () => {
    const{API_Url}=useGlobalContext();
    const GetProductsData = () => {
        axios.get(`${API_Url}/api/products`).then((res) => {
            console.log(res);
            return res;
        }).catch((err) => {
            console.log(err);
        })
    }
    
    return (
        <div className="ApiRequest-container">
            {/* Your JSX content here */}
        </div>
    );
};

export default ApiRequest;