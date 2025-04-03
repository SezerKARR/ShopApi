import React, {useState} from 'react'
import {BrowserRouter, BrowserRouter as Router, Route, Routes, useLocation} from 'react-router-dom';
import './App.css'
import Header from "./Components/Header/Header.jsx";
import Home from "./Pages/Home.jsx";
import AdminHome from "./Admin/Pages/Home.jsx"
import Login from "./Pages/Login.jsx";
import {GlobalProvider} from "../GlobalProvider.jsx";
import CategoryPage from "./Pages/CategoryPage.jsx";
import {FilterProvider} from "../GlobalProvider/FilterContext.jsx";


const Layout = ({children}) => {
    const location = useLocation();
    const hideHeaderRoutes = ["/admin"]; // Header'ın görünmemesi gereken sayfalar

    return (
        <div className="page">
            {!hideHeaderRoutes.includes(location.pathname) && (
                <header>
                    <Header/>
                </header>
            )}
            <main className="content">{children}</main>
        </div>
    );
};

const App = () => {
    return (
        <BrowserRouter future={{v7_startTransition: true, v7_relativeSplatPath: true}}>
            <GlobalProvider>
                <Layout>
                    <Routes>
                        <Route path="/" element={<Home/>}/>
                        <Route path="/login" element={<Login/>}/>
                        <Route path="/category/:slugAndId" element={<CategoryPage/>}/>


                        {/* Admin Panel */}
                        <Route path="/admin" element={<AdminHome/>}/>
                    </Routes>
                </Layout>
            </GlobalProvider>
        </BrowserRouter>
    );
};
export default App
