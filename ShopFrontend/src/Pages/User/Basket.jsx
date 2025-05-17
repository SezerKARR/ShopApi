import React, {useEffect} from 'react';
import './Basket.css';
import {faTrash} from "@fortawesome/free-solid-svg-icons";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {useBasketContext} from "../../Providers/BasketProvider.jsx";
import BasketItem from "../../Components/Basket/BasketItem.jsx";

const Basket = () => {
    const {basket} = useBasketContext();
    const [selectedItems, setSelectedItems] = React.useState([]);
    const [loading, setLoading] = React.useState(true);
    const basketChanges = {};
    useEffect(() => {
        let tempSelectedItems = basket.sellerGroups?.flatMap(group => group.items.map(item => item.id));
        console.log(basket);
        setSelectedItems(tempSelectedItems);
        setLoading(false);
    }, [basket])
    const handleSelectedItem = (item, isChecked) => {
        if (isChecked) {
            setSelectedItems([...selectedItems, item]);
        } else {
            setSelectedItems(selectedItems.filter(i => i !== item));
        }
    };


    function handleCheckboxChange(e) {
        console.log(e);
    }
    const debounceAsync=()=>{
        const changedItems = basketChanges.filter((change) => {
            const original = basket.find((item) => item.id === change.id);
            return !original || original.quantity !== change.quantity;
        });
    }
    const handleItemChange = (item) => {
        basketChanges[item.id] = item;
        console.log(basketChanges);
        
    }
    if(loading){
        return null
    }
    
    return (
        <div className="Basket-container__layout">
            <div className={"basket-header__container"}>
                <div className={"basket-header__basket-title"}> My Basket <span
                    className={"basket-header__basket-title__span"}></span></div>
                <div className={"basket-header__delete-all"}> Delete products <FontAwesomeIcon icon={faTrash}
                                                                                               size="xs"
                                                                                               style={{color: "#f27c1c",}}/>
                </div>
            </div>
            <div className={"Basket-container"}>

                <div className={"coupons-container"}>
                    <div className={"coupons-container__coupons-label"}>
                        <img src="../../../public/coupon-icon.svg" alt="Coupon icon"/>
                        <span>My coupons</span>
                    </div>
                    <div className={"coupons-container__add-coupons-code-label"}>
                        Add coupon code
                        <img src="../../../public/plus.svg" alt="Coupon code"/>
                    </div>
                </div>

                {basket?.sellerGroups?.map((sellerGroup) => (
                    <div className={"basket-product"} key={sellerGroup.sellerId}>
                        <div className={"basket-product__seller-container"}>
                            satıcı:
                            <span className={"basket-product__seller"}>{sellerGroup.sellerName}</span>
                        </div>
                        <div className={"basket-product__product-seller-container"}>
                            {sellerGroup?.items?.map((item) => (
                                <BasketItem
                                    onItemChance={()=>handleItemChange}
                                    key={item.id}
                                    item={item}
                                    isSelected={selectedItems.includes(item.id)}
                                    onSelectChange={(checked) => handleSelectedItem(item.id, checked)}
                                />
                            ))}
                        </div>
                    </div>
                ))

                }
            </div>
        </div>);
};

export default Basket;