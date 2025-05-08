import React, {useEffect, useState} from 'react';
import './ProductReviews.css';
import StarRating from "./StarRating.jsx";

const ProductReviews = ({comments, currentSellerId}) => {
    // useEffect(() => {
    //     const allComments = productSellers.flatMap(seller => {
    //        
    //         return seller.comments.map(comment => {
    //             console.log(comment);
    //             return {
    //                 ...comment,
    //                 sellerName: seller.sellerName,
    //                 sellerInfo: seller.seller,
    //                 productName: seller.productName,
    //                 userInitial:getInitials(comment.user.name)
    //             }
    //            
    //         });
    //     });
    //
    //     // const sortedComments = allComments.sort((a, b) => {
    //     //     if (a.id === 21 && b.id !== 21) return -1; 
    //     //     if (b.id === 21 && a.id !== 21) return 1;
    //     //
    //     //     // İkisi de 75 değilse veya ikisi de 75 ise tarihe göre sırala
    //     //     return new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();
    //     // }); todo:buraya ilk başta şuanki seller için gösterilecek şekilde yapılabilir.
    //    
    //     setComments(allComments);
    // }, [])
    const getInitials = (fullName) => {
        return fullName
            .split(" ")
            .map(name => name.charAt(0).toUpperCase())
            .join("");
    };

    console.log(comments);
    return (
        <div className="product-rewiews-container">
            {comments.map((comment, index) => (
                <div className="product-reviews__review-card" key={index}>
                    
                    <div className= "product-reviews__user-profile">{comment.userInitial} </div>
                    <StarRating rating={comment.rating} count={5}/>
                </div>
            ))}
        </div>
    );
};

export default ProductReviews;