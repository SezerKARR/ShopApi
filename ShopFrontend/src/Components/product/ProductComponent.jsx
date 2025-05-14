import React, {memo} from 'react';
import {useGlobalContext} from '../../Providers/GlobalProvider.jsx';
import {useBasketContext} from '../../Providers/BasketProvider.jsx';
import foto from '../../../public/Foto.png';
import './ProductComponent.css';
import {faCamera} from '@fortawesome/free-solid-svg-icons';
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {useNavigate} from "react-router-dom";

const ProductComponent = memo(({product}) => {
    const {API_URL} = useGlobalContext();
    const {addToBasket} = useBasketContext();
    const navigate = useNavigate();
    console.log(product);

    const handleAddToCartClick = (e) => {
        e.preventDefault();
        addToBasket(product.minPriceSellerId);
        console.log('Add to cart clicked for product:', product.id);
    };


    const handleMouseDown = (event) => {
        const url = getNavigateProp();

        if (event.button === 0) { // Sol Tıklama
            event.preventDefault();
            navigate(url);

        }
        // Sağ tıklama (button === 2) için varsayılan davranış genellikle iyidir (context menu).
    };

    const getNavigateProp = () => {
        return `/product/${product.id}`;
    }


    const handleDragStart = (e) => {
        // Set drag data
        const url = window.location.origin + getNavigateProp();
        e.dataTransfer.setData('text/uri-list', url);
        e.dataTransfer.setData('text/plain', url);
        e.dataTransfer.effectAllowed = 'copy';

        // Create custom drag image
        const dragDiv = document.createElement('div');
        dragDiv.setAttribute('style', 'width: 200px; height: 14px; background-color: #8f8f86;; border: 1px solid gray; ' + 'padding: 8px; font-size: 14px; font-family: Arial; color: black; border-radius:4px');
        dragDiv.innerText = ` ${url}`;

        // Append to body for rendering
        document.body.appendChild(dragDiv);

        // Set drag image, centered
        e.dataTransfer.setDragImage(dragDiv, dragDiv.clientWidth / 2, dragDiv.clientHeight / 2);

        // Clean up after drag starts
        setTimeout(() => {
            if (document.body.contains(dragDiv)) {
                document.body.removeChild(dragDiv);
            }
        }, 0);
    };

    return (<a
        href={getNavigateProp()}
        className={`products-Container__product `}
        onMouseDown={handleMouseDown}
        draggable={true}
        onDragStart={handleDragStart}
    >


        <img
            draggable={false}
            src={product.imageUrl ? `${API_URL}/${product.imageUrl}` : foto}
            className="products-Container__product-photo"
            alt="product image"
        />

        <div className="products-Container__product-name">
        <span className="products-Container__product-name-brand">
          {product.brandName + ' '}
        </span>
            {product.name}
        </div>

        <div className="product-component__details">
            {product.commentCount > 0 && (<div className="product-component__rating-wrapper">
                <div className="product-component__stars">
                    <div className="product-component__star-back">★</div>
                    <div
                        className="product-component__star-front"
                        style={{width: `${product.averageRating * 20}%`}}
                    >
                        ★
                    </div>
                </div>
                <div className="product-component__rating-int">{product.averageRating}</div>
                <div className="product-component__comment-count">({product.commentCount})</div>
                <FontAwesomeIcon icon={faCamera} size="xs" style={{marginTop: '1px'}}/>
                {product.minPriceSellerId}
            </div>)}

            <div className="products-Container__product-price">
                {product.minPrice}
                <span className="products-Container__product-price-Tl">TL</span>



                    <svg
                        onClick={handleAddToCartClick}
                        onMouseDown={(e)=>e.stopPropagation()}
                        width="12"
                        height="12"
                        fill="none"
                        xmlns="http://www.w3.org/2000/svg"
                        className="addToCart"
                    >
                        <path
                            d="M0.5 0.5H0.866019C1 0.5 1.7543 0.8375 1.84699 1.30585L2.875 6.5M2.875 6.5L3.11134 7.69415C3.20404 8.1625 3.61488 8.5 4.09231 8.5H10M2.875 6.5H9.71922C10.1781 6.5 10.5781 6.1877 10.6894 5.74254L11.4701 3.74254C11.6279 3.11139 11.1506 2.5 10.5 2.5M6.5 0.75V2.5M6.5 2.5V4.25M6.5 2.5H8.25M6.5 2.5H4.75M5 11C5 10.7239 4.77614 10.5 4.5 10.5C4.22386 10.5 4 10.7239 4 11C4 11.2761 4.22386 11.5 4.5 11.5C4.77614 11.5 5 11.2761 5 11ZM10 11C10 10.7239 9.77614 10.5 9.5 10.5C9.22386 10.5 9 10.7239 9 11C9 11.2761 9.22386 11.5 9.5 11.5C9.77614 11.5 10 11.2761 10 11Z"
                            stroke="#1E1E1E"
                        />

                    </svg>
            </div>
        </div>
    </a>);
});

export default ProductComponent;