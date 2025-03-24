import React, {useState} from 'react'
import {BrowserRouter, BrowserRouter as Router, Route, Routes} from 'react-router-dom';
import './App.css'
import Header from "./Components/Header/Header.jsx";
import Home from "./Pages/Home.jsx";
import Login from "./Pages/Login.jsx";
import {GlobalProvider} from "../GlobalProvider.jsx";

function App() {
    return (
        <BrowserRouter future={{v7_startTransition: true, v7_relativeSplatPath: true}}>
            <GlobalProvider>
            <div className="page">
                <header>
                    <Header/>
                </header>
                <main className="content">
                    <Routes> <Route path="/" element={<Home/>}/> <Route path="/login" element={<Login/>}/> </Routes>
                </main>
            </div>
            </GlobalProvider>
        </BrowserRouter> 
    );
}

export default App
