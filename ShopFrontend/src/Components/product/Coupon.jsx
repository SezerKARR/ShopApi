import React, {useEffect, useState} from 'react';
import './Coupon.css';
// import axios from "axios";
// import {useGlobalContext} from "../../Providers/GlobalProvider.jsx";
// import {Await} from "react-router-dom";

const Coupon = ({productSeller}) => {
    const [sortedCoupons, setSortedCoupons] = useState([]);
    useEffect(() => {
        setSortedCoupons(
            productSeller.seller.coupons.sort((a, b) => b.reduction - a.reduction)
        )

    }, [productSeller])
    if (sortedCoupons.length === 0) {
        return null;
    }

    return (
        <div style={{overflow: "hidden", padding: "1px", cursor: "pointer"}}>
            coupon
            <div className="coupon-card">
                <div className="coupon-content">
                    <div className="coupon-amount">

                        <span>{sortedCoupons[0].reduction}tl</span>
                        <div className="coupon-status">Kazandın ✓</div>
                    </div>
                    <div className="coupon-limit">Alt limit:{sortedCoupons[0].minLimit}tl</div>
                </div>

                <div className="coupon-divider"/>

                <div className="coupon-footer">
                    <div
                        className="coupon-validity">validity {sortedCoupons[0].onlyDate}{"  "}{sortedCoupons[0].time}</div>
                    <div className="coupon-details">Detaylar</div>
                </div>
            </div>
        </div>
    );
};

export default Coupon;