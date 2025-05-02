import React, {useEffect, useState} from 'react';
import './ProductReviews.css';
import StarRating from "./StarRating.jsx";

const ProductReviews = ({productSellers, currentSellerId}) => {
    const [comments, setComments] = useState([]);
    
    useEffect(() => {
        const allComments = productSellers.flatMap(seller => {

            return seller.comments.map(comment => {
                console.log(comment);
                return {
                    ...comment,
                    sellerName: seller.sellerName,
                    sellerInfo: seller.seller,
                    productName: seller.productName,
                    userInitial: getInitials(comment.user.name)
                }

            });
        });
        const imageComments = allComments.filter(ps=>ps?.imageUrls.length>0)
        console.log(imageComments);

        // const sortedComments = allComments.sort((a, b) => {
        //     if (a.id === 21 && b.id !== 21) return -1;
        //     if (b.id === 21 && a.id !== 21) return 1;
        //
        //     // İkisi de 75 değilse veya ikisi de 75 ise tarihe göre sırala
        //     return new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();
        // }); todo:buraya ilk başta şuanki seller için gösterilecek şekilde yapılabilir.

        setComments(allComments);
    }, [])
    const getInitials = (fullName) => {
        return fullName
            .split(" ")
            .map(name => name.charAt(0).toUpperCase())
            .join("");
    };

    console.log(comments);
    return (<div className="product-reviews-container">
        {comments.map((comment, index) => (<div className="product-reviews__review-card" key={index}>

            <div className="product-reviews__user-profile">{comment.userInitial} </div>
            <div className="product-reviews__comment-info-container">
                <div className="product-reviews__comment-info__header">
                    <StarRating rating={comment.rating} count={5}/>
                    {/*//todo:tariheklenecek*/}

                </div>
                <div className="product-reviews__comment-info__content">{comment.content}
                    <p className="product-reviews__comment-info__seller">User purchased this product by <span
                        style={{color: "blue"}}>{comment.sellerName}</span></p>
                </div>
            </div>
        </div>))}
    </div>);
};

export default ProductReviews;