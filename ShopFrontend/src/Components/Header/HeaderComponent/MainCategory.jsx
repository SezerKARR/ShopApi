import React, {useEffect, useRef, useState} from 'react';
import {useNavigate} from "react-router-dom";
import './MainCategory.css';
import axios from "axios";
import foto from "../../../../public/Foto.png"
import {Link} from "react-router-dom";
import {useGlobalContext} from "../../../../GlobalProvider.jsx";

const MainCategory = () => {
    const navigate = useNavigate();
    const [mainCategories, setMainCategories] = useState(null)
    const [hoveredMainCategory, setHoveredMainCategory] = useState(null)
    const [a, seta] = useState(0)
    const {API_URL,categories}=useGlobalContext();
   
    useEffect(() => {
        console.log(categories)
        const filteredCategories = categories.filter(category => category.parentId === -1);
        setMainCategories(filteredCategories);
       
    }, [categories]);
    const SelectedMainCategory = (mainCategory) => {

        return (
            <div className="selected-main-category-container">
                {console.log(mainCategory)}
                <div className="main-category-categories-container">
                    {mainCategory?.subCategories?.map((subCategory) => (
                        <div key={subCategory.id}>
                            <p className="mainCategory__container-subCategory">{subCategory.name}</p>
                            {
                                subCategory.subCategories?.map((category) => (
                                    <p key={category.id}
                                       className="mainCategory__container-category">{category.name}</p>
                                ))
                            }
                        </div>
                    ))}
                </div>
                <div className="main-category-ad-container"></div>
            </div>
        );
    };
    const menuRef = useRef(null);
    const HandleCategoryClick = (category) => {
        console.log(category);
        navigate(`/category/${category.slug}-${category.id}`)
        setHoveredMainCategory(null);
    }
    const SelectedMainCategorya = ({mainCategory}) => {
        return (
            <div className="selected-main-category">
                <div className="selected-main-category__categories">
                    {mainCategory?.subCategories?.map((subCategory) => (
                        <div key={subCategory.id} className="selected-main-category__subcategory">
                            <p className="selected-main-category__subcategory-name"
                               onClick={() =>HandleCategoryClick(subCategory) }>
                                {subCategory.name}
                            </p>
                            {subCategory.subCategories?.map((category) => (
                                <p key={category.id}
                                   onClick={() => HandleCategoryClick(category) }
                                   className="selected-main-category__category">
                                    {category.name}
                                </p>
                            ))}
                        </div>
                    ))}
                </div>
              
            </div>
        );

    };
    const handleMouseLeave = (e) => {
        if (!e.relatedTarget || !(e.relatedTarget instanceof Node)) {
            setHoveredMainCategory(null);
            return;
        }

        if (menuRef.current?.contains(e.relatedTarget)) {
            return;
        }

        setHoveredMainCategory(null);
    };

    return (
        <div ref={menuRef} onMouseLeave={handleMouseLeave}>
            <div className="MainCategoryElements">
                {mainCategories?.map(mainCategory => (
                    <p key={mainCategory.id}
                       onMouseEnter={() => setHoveredMainCategory(mainCategory)}
                       className="MainCategoryElement">
                        {mainCategory.name}
                    </p>
                ))}
            </div>

            {hoveredMainCategory && mainCategories && (
                <SelectedMainCategorya mainCategory={hoveredMainCategory} />
            )}
        </div>
    );
   
};


export default MainCategory;