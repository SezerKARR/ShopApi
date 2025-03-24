import React, {useContext, useEffect, useState} from "react";
import { GoogleLogin } from "@react-oauth/google";
import axios from "axios";
import {jwtDecode} from "jwt-decode"
import {GlobalProvider, useGlobalContext} from "../../GlobalProvider.jsx";
const Login = () => {
    const {setUser}=useGlobalContext();
    const API_URL = import.meta.env.VITE_API_URL;
  
    const handleLoginSuccess = (response) => {
        const token = response.credential; // Google'dan gelen token

        // Query string olarak token'ı backend'e gönderin
        axios.post(`${API_URL}/api/auth`, {}, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        })
            .then((res) => {
                console.log(res);
                setUser(res.data.user); 
                localStorage.setItem("user", JSON.stringify(res.data.user)); 
            })
            .catch((error) => {
                console.error("Login failed", error);
            });
      
    };

    return (
        <div>
            <h1>Login with Google</h1>
            <GoogleLogin
                onSuccess={(credentialResponse)=>{
                    console.log(credentialResponse);
                    console.log(jwtDecode(credentialResponse.credential))
                    handleLoginSuccess(credentialResponse);
                }}
                onError={() => console.error("Login Failed")}

            />
        </div>
    );
};

export default Login;