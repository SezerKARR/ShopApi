import React, {useEffect, useRef, useState} from 'react';
import {useNavigate} from "react-router-dom";
import './MainCategory.css';
import axios from "axios";
import {useGlobalContext} from "../../../Providers/GlobalProvider.jsx";

const MainCategory = () => {
    const navigate = useNavigate();
    const {categories}=useGlobalContext()
    const [mainCategories, setMainCategories] = useState(null)
    const [hoveredMainCategory, setHoveredMainCategory] = useState(null)
   
    useEffect(() => {
        console.log(categories);
        const filteredCategories = categories.filter(category => category.parentId === -1);
        setMainCategories(filteredCategories);
       
    }, []);
    
    
    const menuRef = useRef(null);
    const HandleCategoryClick = (category) => {
        console.log(category);
        navigate(`/category/${category.slug}-${category.id}`)
        setHoveredMainCategory(null);
    }
    const SelectedMainCategory = ({mainCategoryId}) => {
        return (
            <div className="selected-main-category">
                <div className="selected-main-category__categories">
                    {GetChildCategories(mainCategoryId)?.map(childCategory => (
                        <div key={childCategory.id} className="selected-main-category__subcategory">
                            <p className="selected-main-category__subcategory-name"
                               onClick={() => HandleCategoryClick(childCategory)}>
                                {childCategory.name}
                            </p>
                            {GetChildCategories(childCategory.id)?.map(childChildCategory => (
                                <p key={childChildCategory.id}
                                   onClick={() => HandleCategoryClick(childChildCategory)}
                                   className="selected-main-category__category">
                                    {childChildCategory.name}
                                </p>
                            ))}
                        </div>
                    ))}
                   
                </div>

            </div>
        );

    };
    const GetChildCategories = (mainCategoryId) => {
        return categories.filter(category => category.parentId === mainCategoryId);
    }
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

            {hoveredMainCategory && mainCategories && (<>
                {console.log(hoveredMainCategory)}
                <SelectedMainCategory mainCategoryId={hoveredMainCategory.id} /></>
            )}
        </div>
    );
   
};


export default MainCategory;