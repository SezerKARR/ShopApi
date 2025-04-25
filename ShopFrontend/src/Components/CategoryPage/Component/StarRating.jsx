import React from 'react';
import './StarRating.css';

const StarRating = ({rating, count}) => {
    const StarReturn = () => {
        const getPercent=(index)=>{
            const percent=(rating-index)*(20*count)//todo:yanlış olabilir belki kntrol edilecek
            if(percent>0)return percent;
            return 0;
        }
        return (
            <div className="StarRating-container">
                {Array.from({length: count}, (_, index) => (
                    
                    <div key={index}>
                        
                        <div className="star-rating">

                            <div className="star-back">★</div>
                            <div className="star-front" style={{width: `${getPercent(index)}%`}}>★</div>
                        </div>
                    </div>
                ))}
            </div>
        );
    }
    return (

        <StarReturn/>

        
    );
};

export default StarRating;