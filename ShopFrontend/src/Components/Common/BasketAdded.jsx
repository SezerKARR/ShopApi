import React, { memo, useEffect, useRef, useState } from 'react';
import './BasketAdded.css';
import { useBasketContext } from "../../Providers/BasketProvider.jsx";
import BasketAddedPanel from "./BasketAddedPanel.jsx";

const generateId = () => Date.now() + Math.random();

const BasketAdded = memo(() => {
    const { basketItems } = useBasketContext();
    const [addedProducts, setAddedProducts] = useState([]);
    const isFirstRender = useRef(true);
    
    const closeAddedPanel=(productId) => {
        console.log(productId);
        setAddedProducts(prev => prev.filter(p => p.uid !== productId));
    }
    useEffect(() => {
        if (isFirstRender.current) {
            isFirstRender.current = false;
            return;
        }

        const newProduct = {
            ...basketItems[basketItems.length - 1],
            uid: generateId()
        };

        setAddedProducts(prev => [newProduct, ...prev]);

        setTimeout(() => {
            setAddedProducts(prev => prev.filter(p => p.uid !== newProduct.uid));
        }, 5000000);
    }, [basketItems]);

    return (
        <div className="BasketAdded-wrapper">
            {addedProducts.map((product) => (
                <BasketAddedPanel key={product.uid} uid={product.uid} product={product} addedProducts={addedProducts} closeAddedPanel={closeAddedPanel} />
            ))}
        </div>
    );
    
});




export default BasketAdded;
