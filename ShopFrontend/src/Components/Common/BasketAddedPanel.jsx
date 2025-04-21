import React, {useEffect, useRef} from 'react';
import './BasketAddedPanel.css';

const BasketAddedPanel = ({ uid, product, addedProducts,closeAddedPanel }) => {
    console.log( product);
    const ref = useRef();

    useEffect(() => {
        const el = ref.current;

        const index = addedProducts.findIndex(p => p.uid === uid);

        el.style.top = `0px`;

        requestAnimationFrame(() => {
            el.style.top = `${index * 55 + 20}px`;
        });
    }, [addedProducts, uid]);

    return (
        <div className="BasketAdded-container" ref={ref}>
            <div className="BasketAdded-content">
                <svg width="24" height="24" viewBox="0 0 24 24" fill="none"
                     xmlns="http://www.w3.org/2000/svg">
                    <circle cx="12" cy="12" r="12" fill="#28a745"/>
                    <path d="M6 12.5L10 16.5L18 8.5"
                          stroke="white"
                          fill="none"
                    />
                </svg>
                <h4>Product added basket {product?.id}</h4>
                <svg className={"BasketAdded-container-close-button"} width="32" height="32" viewBox="0 0 24 24"
                     xmlns="http://www.w3.org/2000/svg"
                     onClick={() => closeAddedPanel(uid)}
                >
                    <path d="M8 8 L16 16 M16 8 L8 16"
                          stroke="gray"
                          strokeWidth="2"
                          strokeLinecap="round"/>
                </svg>
            </div>
        </div>
    );
};
export default BasketAddedPanel;