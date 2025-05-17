import React, {memo, useEffect, useState} from 'react';
import './BasketItemCount.css';
import {faMinus, faPlus, faTrash} from "@fortawesome/free-solid-svg-icons";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";

const BasketItemCount = memo(({quantity,onItemQuantityChange}) => {
    const [count, setCount] = useState(quantity);
    useEffect(() => {
        const timer = setTimeout(() => {
            onItemQuantityChange( count); // Store'a yaz
        }, 300);

        return () => clearTimeout(timer);
    }, [count]);
    const decreaseClickHandler = () => {
        if (count > 0) {
            setCount(count - 1);
        }
    };

    const increaseClickHandler = () => {
        setCount(count + 1);
    };
    const TrashIconHandler = () => {
        if (count <= 1) {
            return (<FontAwesomeIcon icon={faTrash} color="orange" style={{cursor: "pointer"}}/>)
        } else
            return (<FontAwesomeIcon icon={faMinus} color="orange" style={{cursor: "pointer"}}/>)
    }

    return (
        <div className="BasketItemCount-container">
            <div onMouseDown={decreaseClickHandler}>
                <TrashIconHandler/>

            </div>
            {count}
            <div onMouseDown={increaseClickHandler}>

                <FontAwesomeIcon icon={faPlus} color="orange" style={{cursor: "pointer"}}/>
                
            </div>

        </div>
    );
});

export default BasketItemCount;