import React from 'react';
import './Search.css';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faSearch} from "@fortawesome/free-solid-svg-icons";

const Search = () => {
    return (
        <div className="Search-container">
            <div className={'SearchOutline'}>
                <div className={'SearchContainer'}>
                    <div className={'search-container__search-icon'}>
                        <FontAwesomeIcon icon={faSearch}/>
                    </div>
                    <input className={'SearchBar'}/>
                </div>
            </div>
        </div>
    );
};
export default Search;