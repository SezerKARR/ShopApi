import React from 'react';
import './BasketItem.css';
import {useGlobalContext} from "../../Providers/GlobalProvider.jsx";
import {useNavigate} from "react-router-dom";

const BasketItem = ({item, isSelected, onSelectChange}) => {
    const navigate = useNavigate();
    const {API_URL} = useGlobalContext();
    console.log(item);
    const getNavigateProp = () => {
        return `/product/${item.product.id}`;
    }
    const handleMouseDown = (event) => {
        const url = getNavigateProp();

        if (event.button === 0) { // Sol Tıklama
            event.preventDefault();
            navigate(url);

        }
        // Sağ tıklama (button === 2) için varsayılan davranış genellikle iyidir (context menu).
    };
    return (
        <div className="basket-item">
            <label className="custom-checkbox">
                <input
                    type="checkbox"
                    checked={isSelected}
                    onChange={(e) => onSelectChange(e.target.checked)}
                />
                <span className="checkmark"></span>
            </label>
            <img alt={item.id}  className={"basket-item__image"} src={`${API_URL}/${item.product.productImages[0]?.image?.url}`}/>
            <a href={getNavigateProp()}
               className={"basket-item__product-title"}
               onMouseDown={handleMouseDown}>{item.product.brandName}<span>{item.product.name}</span></a>
        </div>
    );
};
export default BasketItem;