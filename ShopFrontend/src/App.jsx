import React, { useState, useEffect } from 'react';
import { BrowserRouter, Route, Routes, useLocation } from 'react-router-dom';
import './App.css';
import Header from "./Components/Header/Header.jsx";
import Home from "./Pages/User/Home.jsx";
import SellerHome from "./Pages/Seller/Home.jsx";
import Login from "./Pages/User/Login.jsx";
import CategoryPage from "./Pages/User/CategoryPage.jsx";
import ProductAdd from "./Pages/Seller/ProductAdd.jsx";
import Product from "./Pages/User/Product.jsx";
import { GlobalProvider } from "./Providers/GlobalProvider.jsx";
import { BasketProvider } from "./Providers/BasketProvider.jsx";
import Basket from "./Pages/User/Basket.jsx";
import CompleteShop from "./Pages/User/CompleteShop.jsx";

const Layout = ({ children }) => {
    const location = useLocation();
    const hideHeaderRoutes = ["/admin"];

    return (
        <div className="page">
            {!hideHeaderRoutes.includes(location.pathname) && (
                <header>
                    <Header />
                </header>
            )}
            <main className="content">{children}</main>
        </div>
    );
};

const App = () => {
    const [initialLoadingCompleted, setInitialLoadingCompleted] = useState(false);
    const [loading, setLoading] = useState(true);
    const [basketLoading, setBasketLoading] = useState(true);
    const isAnyLoading = loading || basketLoading;
    useEffect(() => {
        if (!loading && !basketLoading && !initialLoadingCompleted) {
            setInitialLoadingCompleted(true);
        }
    }, [loading, basketLoading]);
    const LoadingScreen = () => (
        <div style={{
            position: 'fixed',
            top: 0,
            left: 0,
            width: '100%',
            height: '100vh',
            backgroundColor: 'rgba(151,71,71,0.6)',
            color: 'black',
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            fontSize: '2rem',
            fontWeight: 'bold',
            zIndex: 9999,
        }}>
            {console.log("bekleniyor")}
            Bekleniyor...
        </div>
    );
  
    return (
        <>
            {isAnyLoading && <LoadingScreen />}

            <GlobalProvider setLoading={setLoading}>
                {(!loading || initialLoadingCompleted)  && (
                    <BasketProvider setBasketLoading={setBasketLoading} basketLoading={basketLoading} >
                        {(!basketLoading || initialLoadingCompleted)  && (
                            <BrowserRouter>
                                <Layout>
                                    <Routes>
                                        <Route path="/" element={<Home />} />
                                        <Route path="/login" element={<Login />} />
                                        <Route path="/category/:slugAndId" element={<CategoryPage />} />
                                        <Route path="/product/:productId" element={<Product />} />
                                        <Route path="/basket" element={<Basket />} />
                                        <Route path="/admin" element={<SellerHome />} />
                                        <Route path="/productAdd" element={<ProductAdd />} />
                                        <Route path="/completeShop" element={<CompleteShop />} />
                                    </Routes>
                                </Layout>
                            </BrowserRouter>
                        )}
                    </BasketProvider>
                )}
            </GlobalProvider>
        </>)
 
};

export default App;