import React, {useEffect, useState} from 'react'
import {BrowserRouter, BrowserRouter as Router, Route, Routes, useLocation} from 'react-router-dom';
import './App.css'
import Header from "./Components/Header/Header.jsx";
import Home from "./Pages/User/Home.jsx";
import SellerHome from "./Pages//Seller/Home.jsx"
import Login from "./Pages/User/Login.jsx";
import CategoryPage from "./Pages/User/CategoryPage.jsx";
import ProductAdd from "./Pages/Seller/ProductAdd.jsx";
import Product from "./Pages/User/Product.jsx";
import {GlobalProvider} from "./Providers/GlobalProvider.jsx";


const Layout = ({children}) => {
    const location = useLocation();
    const hideHeaderRoutes = ["/admin"]; 
  

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
    const [loading, setLoading] = useState(true);
    return (
        <GlobalProvider setLoading={setLoading}>
            {!loading &&(
                <BrowserRouter future={{v7_startTransition: true, v7_relativeSplatPath: true}}>
                    <Layout>
                        <Routes>
                            <Route path="/" element={<Home/>}/>
                            <Route path="/login" element={<Login/>}/>
                            <Route path="/category/:slugAndId" element={<CategoryPage/>}/>
                            <Route path="/product/:productId" element={<Product/>}/>
                            
                            
                            {/* Seller Panel */}
                            <Route path="/admin" element={<SellerHome/>}/>
                            <Route path="/productAdd" element={<ProductAdd/>}/>
                        </Routes>
                    </Layout>
                </BrowserRouter>)}
        </GlobalProvider>
    );
};

export default App
