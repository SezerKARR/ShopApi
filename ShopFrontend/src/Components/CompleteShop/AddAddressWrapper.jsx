import React from 'react';
import './AddAddressWrapper.css';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faXmark} from "@fortawesome/free-solid-svg-icons";

const AddAddressWrapper = (setAddAddressWrapperOpened) => {
    return (
        <div className="add-address-wrapper">

            <button className={"add-address-wrapper__close-button"} onClick={() => setAddAddressWrapperOpened(false)}>
                <FontAwesomeIcon icon={faXmark} size={"xl"}/>
            </button>
            <form className={"add-address-wrapper__address-form"}>
                <h1 className={"add-address-wrapper__add-address-header"}>
                    Adres Ekle
                </h1>
                <div className="address-form__receiver-info">
                    <div className="address-form__section-title">Kişisel Bilgileriniz</div>
                    <div className="address-form__receiver-label">
                        Teslim alacak kişinin bilgileri
                    </div>
                    <div className="address-form__name-fields">
                        <input className="address-form__name-input"/>
                        <input className="address-form__surname-input"/>
                    </div>
                </div>


            </form>

            {/* Adres formu veya içerik */}
        </div>
    );
};

export default AddAddressWrapper;