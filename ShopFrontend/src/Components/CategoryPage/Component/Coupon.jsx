import React, {useEffect} from 'react';
import './Coupon.css';
import axios from "axios";
import {useGlobalContext} from "../../../Providers/GlobalProvider.jsx";
import {Await} from "react-router-dom";

const Coupon = ({SellerId}) => {
    const {API_URL}=useGlobalContext();
    useEffect(() => {
        const fetchCoupons = async () => {
            console.log(SellerId);
            const coupons = await axios.get(`${API_URL}/api/coupon/bySellerId/${SellerId}`);
            console.log(coupons.data);
        };

        if (SellerId) {  // SellerId varsa çalıştır
            fetchCoupons();
        }
    }, [SellerId]);



    return (
        <div style={{overflow: "hidden", padding: "1px"}}>
            <div className="coupon-card">
                <div className="coupon-content">
                    <div className="coupon-amount">
                        <span>150 TL</span>
                        <div className="coupon-status">Kazandın ✓</div>
                    </div>
                    <div className="coupon-limit">Alt limit: 250 TL</div>
                </div>

                <div className="coupon-divider"/>

                <div className="coupon-footer">
                    <div className="coupon-validity">Geçerlilik 29.04.2025 23:59</div>
                    <div className="coupon-details">Detaylar</div>
                </div>
            </div>
        </div>
    );
};

export default Coupon;