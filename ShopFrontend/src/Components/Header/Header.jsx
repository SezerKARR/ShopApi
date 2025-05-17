import React from 'react';
import './Header.css';
import Logo from "./HeaderComponent/Logo.jsx";
import Search from "./HeaderComponent/Search.jsx";
import Location from "./HeaderComponent/Location.jsx";
import Authorize from "./HeaderComponent/Authorize.jsx";
import HeaderBasket from "./HeaderComponent/HeaderBasket.jsx";
import MainCategory from "./HeaderComponent/MainCategory.jsx";

const Header = () => {
    return (<div className={"Header"}>
        <div className={'Header__Components'}>
            <Logo/> <Search/> <Location/> <Authorize/> <HeaderBasket/>
        </div>
        <MainCategory/>
    </div>);
};
export default Header;