import React from 'react';
import './Search.css';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faSearch} from "@fortawesome/free-solid-svg-icons";

const Search = () => {
    return (
        <div>
            <div className={'SearchOutline'}>
                <div className={'SearchContainer'}>
                    <div className={'Search'}>
                        <FontAwesomeIcon icon={faSearch}/>
                    </div>
                    <input className={'SearchBar'}/>
                </div>
            </div>
            <div className={'RelativeSearch'}></div>
        </div>
    );
};
export default Search;