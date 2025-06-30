import React, {useState} from 'react';
import './DeliveryAddress.css';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPlus} from "@fortawesome/free-solid-svg-icons";
import AddAddressWrapper from "./AddAddressWrapper.jsx";

const DeliveryAddress = () => {
    const [selectedAddress, setSelectedAddress] = useState("myAddress");
    const [addAddressWrapperOpened, setAddAddressWrapperOpened] = useState(true);
    return (
        <div className="DeliveryAddress-container">
            <h1>Teslimat adresim</h1>
            <div className="DeliveryAddress__my-address">
                <label htmlFor="myAdress">
                    <input
                        id="myAdress"
                        name="address"
                        type="radio"
                        value="myAddress"
                        checked={selectedAddress === "myAddress"}
                        onChange={(e) => setSelectedAddress(e.target.value)}
                    />


                    Kendi adresim</label>
                {selectedAddress === "myAdress" && (
                    
                    <div className={"myAddress__addresses-container"}>
                        <div className={"myAddress__addresses-container__address"}>
                            sas
                        </div>
                        <button className={"myAddress__addresses-container__addAddress"} onClick={(e) => setAddAddressWrapperOpened(!addAddressWrapperOpened)}>
                            <FontAwesomeIcon icon={faPlus} color={"#fd7102"} />
                            Add billing address
                        </button>
                    </div>
                )}

            </div>

            <div>
                <label htmlFor="boi">
                    <input
                        id="boi"
                        name="address"
                        type="radio"
                        value="boi"
                        checked={selectedAddress === "boi"}
                        onChange={(e) => setSelectedAddress(e.target.value)}
                    />
                    Başka biri</label>
                {/*todo:bunlarıda yaparım*/}
                {/*/!* 👇 Sadece myAdress seçiliyken gösterilecek div *!/*/}
                {/*{selectedAddress === "boi" && (*/}
                {/*    <div>*/}
                {/*        Bu sadece kendi adresin seçiliyken görünür.*/}
                {/*    </div>*/}
                {/*)}*/}
            </div>
            {addAddressWrapperOpened && <div className="overlay"></div>}

            {addAddressWrapperOpened && (
              <AddAddressWrapper setAddAddressWrapperOpened={setAddAddressWrapperOpened} />
            )}
            {/* Your JSX content here */}
        </div>
    );
};

export default DeliveryAddress;