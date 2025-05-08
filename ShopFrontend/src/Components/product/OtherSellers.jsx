import React, {useEffect, useState} from 'react';
import './OtherSellers.css';
import OtherSeller from "./OtherSeller.jsx";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faGreaterThan, faXmark} from "@fortawesome/free-solid-svg-icons";

const OtherSellers = ({productSellers, currentSellerId}) => {
    const [twoOtherSellers, setTwoOtherSellers] = useState([]);
    const [isWantSeeAll, setIsWantSeeAll] = useState(false);
    useEffect(() => {
        setTwoOtherSellers(productSellers.filter(productSeller => productSeller.id != currentSellerId).slice(0, 2));
    }, [productSellers]);
    useEffect(() => {
        if (isWantSeeAll) {
            document.body.style.overflow = "hidden";
        } else {
            document.body.style.overflow = "auto";
        }

    }, [isWantSeeAll])
    if (twoOtherSellers.length == 0) {
        return;
    }

    return (
        <>
            <div className="OtherSellers-container">
                <div className={"other-sellers__other-sellers-label"}>Other Sellers <span
                    onClick={() => setIsWantSeeAll(!isWantSeeAll)}
                    className={"other-sellers__other-sellers-label__see-all-label"}>See All
                    <FontAwesomeIcon icon={faGreaterThan} size={"m"}/></span></div>
                <div className={"other-sellers__other-sellers-container"}>
                    {twoOtherSellers.map(seller => (
                        <OtherSeller key={seller.id} productSeller={seller}/>
                    ))}
                </div>

            </div>
            {isWantSeeAll &&
                <>
                <div className={"other-seller__overlay"} onClick={()=>setIsWantSeeAll(false)}/>
                
                <div className={"other-seller__all-seller-container"}>
                    <div className={"other-seller__all-seller-header"}>
                        <div className={"other-seller__all-seller__close-button"} onClick={()=>setIsWantSeeAll(false)}>
                            <FontAwesomeIcon icon={faXmark} size="2xl"/>
                        </div>
                        <div className={"other-seller__all-seller-title"}>
                            Other Sellers 
                        </div>
                    </div>
                    <div className="other-seller__all-seller__sellers-container">
                        {productSellers.map(seller => (
                            <OtherSeller key={seller.id} productSeller={seller} isAllOtherSeller={true} />
                        ))}
                       
                    </div>
                </div>
                </>
            }
        </>)
};

export default OtherSellers;