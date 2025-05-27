import React, {useEffect, useRef} from 'react';
import './Basket.css';
import {faTrash} from "@fortawesome/free-solid-svg-icons";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {useBasketContext} from "../../Providers/BasketProvider.jsx";
import BasketItem from "../../Components/Basket/BasketItem.jsx";
import TotalBasket from "../../Components/Basket/TotalBasket.jsx";

const Basket = () => {
    const {basket, updateBasketItems} = useBasketContext();
    const [selectedItems, setSelectedItems] = React.useState([]);
    const [loading, setLoading] = React.useState(true);
    const basketChanges = useRef({});
    const timer = useRef(null);
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

    const debounceAsync = () => {
        const changedItems = basketChanges.filter((change) => {
            const original = basket.find((item) => item.id === change.id);
            return !original || original.quantity !== change.quantity;
        });
    }
    const handleItemChange = (item) => {
        basketChanges.current[item.id] = item.quantity;
        if (timer.current) {
            clearTimeout(timer.current);
        }

        timer.current = setTimeout(async () => {
            await handleApiRequest();
        }, 1000); // 1 saniye sonra çalış
    }
    if (loading) {
        return null
    }
    const handleApiRequest = async () => {
        const response = await updateBasketItems(basketChanges.current);

        console.log(response.success)
        basketChanges.current = {};
        timer.current = null;
    }

    return (
        <div className="Basket-container__layout">
            <div className={"basket-header__container"}>
                <div className={"basket-header"}>
                    <div className={"basket-header__basket-title"}> My Basket <span
                        className={"basket-header__basket-title__span"}></span></div>
                    <div className={"basket-header__delete-all"}> Delete products <FontAwesomeIcon icon={faTrash}
                                                                                                   size="xs"
                                                                                                   style={{color: "#f27c1c",}}/>
                    </div>
                </div>
            </div>
            <div className={"basket-page-container"}>

                <div className={"Basket-container"}>
                    <div className={"coupons-container"}>
                        <div className={"coupons-container__coupons-label"}>
                            <img src="/coupon-icon.svg" alt="Coupon icon"/>
                            <span>My coupons</span>
                        </div>
                        <div className={"coupons-container__add-coupons-code-label"}>
                            Add coupon code
                            <img src="/plus.svg" alt="Coupon code"/>
                        </div>
                    </div>

                    {basket?.sellerGroups?.map((sellerGroup) => (
                        <div className={"basket-product"} key={sellerGroup.sellerId}>
                            {console.log(sellerGroup.isShippingFree)}
                            <div className={"basket-product__seller-container"}>
                                <div>
                                    satıcı:
                                    <span className={"basket-product__seller"}>{sellerGroup.sellerName}</span>
                                </div>
                                
                                {!sellerGroup.isShippingFree &&
                                    <div className={"basket-product__shipping-threshold-container"}>You're <span className={"basket-product__shipping-threshold__money"}>{sellerGroup.freeShippingMinimumOrderAmount-sellerGroup.subtotal} TL</span> away from free shipping – add more items!</div>}

                            </div>
                            <div className={"basket-product__product-seller-container"}>
                                {sellerGroup?.items?.map((item) => (
                                    <BasketItem
                                        onItemChance={handleItemChange}
                                        key={item.id}
                                        basketItem={item}
                                        isSelected={selectedItems.includes(item.id)}
                                        onSelectChange={(checked) => handleSelectedItem(item.id, checked)}
                                    />
                                ))}
                            </div>
                        </div>
                    ))}

                </div>

                <TotalBasket basket={basket}/>
            </div>
        </div>
    );
};

export default Basket;