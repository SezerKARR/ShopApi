import React from 'react';
import './Home.css';
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