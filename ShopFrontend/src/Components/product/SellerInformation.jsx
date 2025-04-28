import React from 'react';
import './SellerInformation.css';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck } from "@fortawesome/free-solid-svg-icons";

const SellerInformation = ({ seller, onFollowClick, onAskSellerClick }) => {
    if (!seller) return null;

    return (
        <div className="seller-container">
            <div className="seller-info">
                <p className="seller-info__seller-label">seller:</p>
                <p className="seller-info__seller-name">{seller.name}</p>
                <div className="seller-info__check-container">
                    <div className="seller-info__check">
                        <FontAwesomeIcon icon={faCheck} size="lg" style={{ color: "#d0e6fd" }} />
                    </div>
                    <div className="seller-info__check-label-container">official seller</div>
                </div>
            </div>
            <div
                className="seller-container__follow-label"
                onClick={onFollowClick}
            >
                Follow
            </div>
            <div
                className="seller-container__ask-seller-label"
                onClick={onAskSellerClick}
            >
                Ask Seller
            </div>
        </div>
    );
};
export default SellerInformation;