import React from 'react';
import './Authorize.css';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faChevronDown, faUser} from "@fortawesome/free-solid-svg-icons";
import {Link} from "react-router-dom";
import { useGlobalContext} from "../../../../GlobalProvider.jsx";

const Authorize = () => {
    const {user}=useGlobalContext();
    return (
        <div className={'Authorize'}>
            {user !== null ? <><FontAwesomeIcon icon={faUser} size="2x"/> <Link to={'/login'}
                                                                                className={'Authorize__Login'}> Login </Link>
                <Link to={'/singUp'} className={'Authorize__SingUp'}> or SingUp </Link> <FontAwesomeIcon
                    icon={faChevronDown}/>
            </> : <>
                <h1 className={'Account'}>My account</h1>
                <h1 className={'Account__Name'}> sezer kar</h1>{/*<h1 className={"Account__Name"}> {user.name}</h1>*/}
            </>}
        </div>
    );
};
export default Authorize;