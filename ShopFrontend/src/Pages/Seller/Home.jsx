import React from 'react';
import './Home.css';
import ProductFilterAdd from "./ProductFilterAdd.jsx";
import Header from "../../Components/Header/Header.jsx";
import Product from "./Product.jsx";

const Home = () => {
    return (
        <div className="Home">
            <div className={"products-container"}>
                <Product/>
                
            </div>
            {/*<ProductFilterAdd/>*/}
        </div>
    );
};

export default Home;