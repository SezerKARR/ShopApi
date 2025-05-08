import React, {useEffect, useState} from 'react';
import './ProductImage.css';

const ProductImage = ({productImages, API_URL}) => {
    const [selectedImage, setSelectedImage] = useState(null);
    useEffect(() => {
        changeSelectedImage(productImages[0])
    }, [productImages])
    const changeSelectedImage = (selectedImage) => {
        setSelectedImage(selectedImage);
    }
    if (!selectedImage) {
        return null;
    }
    console.log(selectedImage);

    return (
        <div className="ProductImage-container">
            <div className={"image-container"}>
                <img
                    className="product--image"
                    alt={selectedImage.image.name}
                    src={`${API_URL}/${selectedImage.image.url}`}
                />
            </div>
            <div className={"product-image__other-images"}>
                {productImages.map((image, index) => (
                    <div className={"product-image__other-image_container"} key={index}>
                        <img
                            className={`product-image__other-image ${selectedImage.id == image.id ? " selected" : ""}`}
                            alt={productImages[index].image.name}
                            src={`${API_URL}/${productImages[index].image.url}`}
                            onClick={() => changeSelectedImage(image)}
                        />
                    </div>

                ))}
            </div>

        </div>
    );
};

export default ProductImage;