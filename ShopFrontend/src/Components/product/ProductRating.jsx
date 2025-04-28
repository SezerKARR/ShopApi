import React from 'react';

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCamera } from "@fortawesome/free-solid-svg-icons";
import StarRating from "./StarRating.jsx";

const ProductRating = ({ rating, commentCount }) => {
    return (
        <div className="product__information-container__comment">
            <StarRating rating={rating} count={5} />
            <div className="product__information-container__comment-count">
                {commentCount} comment
            </div>
            <FontAwesomeIcon icon={faCamera} size="xs" style={{ marginTop: '1px' }} />
        </div>
    );
};
export default ProductRating;