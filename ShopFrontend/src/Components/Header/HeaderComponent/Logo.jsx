import React from 'react';
import './Logo.css';

const Logo = () => {
    return (
        <div className={'Header__Logo'}>
            <p className={'Header__logo_name'}>
                Sezer Shop</p>
            <div className="Header__Logo__DiscoverPremiumContainer">
                <p className={'header__logo_Discover'}>Discover</p>
                <p className={'header__logo_Prem'}>Premium</p>
            </div>
        </div>
    );
};
export default Logo;