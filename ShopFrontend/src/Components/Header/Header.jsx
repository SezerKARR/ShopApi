import React from 'react';
import './Header.css';
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {
    faSearch,
    faLocationDot,
    faSignInAlt,
    faUser,
    faChevronDown,
    faCartShopping, faCircle
} from '@fortawesome/free-solid-svg-icons';

const Header = () => {
    return (
        <div className={"Header"}>
            <div className={"Header__Logo"}>
                <p className={"Header__logo_name"}
                    
                >
                    Sezer Shop</p>
                <div className="Header__Logo__DiscoverPremiumContainer">
                    <p className={"header__logo_Discover"}
                    >Discover</p>
                    <p className={"header__logo_Prem"}
                    >Premium</p>

                </div>

            </div>
            <div>
                <div className={"SearchOutline"}>

                    <div className={"SearchContainer"}>

                        <div className={"Search"}>
                            <FontAwesomeIcon icon={faSearch}/>

                        </div>
                        <input className={"SearchBar"}>

                        </input>
                    </div>

                </div>
                <div className={"RelativeSearch"}></div>
    
            </div>
            <div className={"Location"}> 
                    <FontAwesomeIcon icon={faLocationDot}size="2x"/>
                    
                <div className={"Location__Name__Container"}>
                    <p className={"Location__Label"}>
                        Location
                    </p>
                    <p className={"LocationSelect__Label"}>
                        Select a location
                    </p>
                </div>
            </div>
            <div className={"Authorize"}>
                <FontAwesomeIcon icon={faUser} size="2x"/>
                <div className={"Authorize__LoginLabel"}>
                    <p className={"Authorize__Login"}>
                        Login
                    </p>
                    <p className={"Authorize__SingUp"}>
                        or SingUp
                    </p>
                </div>
                <FontAwesomeIcon icon={faChevronDown}/>
                
            </div>
            <div className={"Basket__Container"}>
                <div className={"Basket__Cart"}>
                    <FontAwesomeIcon icon={faCartShopping} size="2x" className={"Basket__Cart"}/>
                    <p className={"BasketCircle"}>1</p>

                </div>
                <p className={"Basket__MyBasket"}>My Basket</p>

            </div>

        </div>
    );
};

export default Header;