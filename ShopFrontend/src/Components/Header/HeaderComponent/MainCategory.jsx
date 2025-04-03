import React, {useEffect, useRef, useState} from 'react';
import {useNavigate} from "react-router-dom";
import './MainCategory.css';
import axios from "axios";
import foto from "../../../../public/Foto.png"
import {Link} from "react-router-dom";

const MainCategory = () => {
    const navigate = useNavigate();
    const [mainCategories, setMainCategories] = useState(null)
    const [hoveredMainCategory, setHoveredMainCategory] = useState(null)
    const [a, seta] = useState(0)

    const API_URL = import.meta.env.VITE_API_URL;
    useEffect(() => {
        axios.get(`${API_URL}/api/category`)
            .then(response => {
                try {
                    if (!response.data || response.data.length === 0) {
                        console.log("main category not found");
                    }
                    const filteredCategories = response.data.filter(category => category.parentId === -1);
                    setMainCategories(filteredCategories); // Filtrelenmiş kategorileri set et
                    // setHoveredMainCategory(filteredCategories[2]);
                    console.log(filteredCategories);
                } catch (error) {
                    console.log(error);
                }
            })
            .catch(error => {
                console.error("Veri yüklenirken hata oluştu:", error);
            });
    }, []);
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
    const SelectedMainCategorya = ({mainCategory}) => {
        return (
            <div className="selected-main-category">
                <div className="selected-main-category__categories">
                    {mainCategory?.subCategories?.map((subCategory) => (
                        <div key={subCategory.id} className="selected-main-category__subcategory">
                            <p className="selected-main-category__subcategory-name"
                               onClick={() => navigate(`/category/${subCategory.slug}-${subCategory.id}`)}>
                                {subCategory.name}
                            </p>
                            {subCategory.subCategories?.map((category) => (
                                <p key={category.id}
                                   onClick={() => navigate(`/category/${category.slug}-${category.id}`)}
                                   className="selected-main-category__category">
                                    {category.name}
                                </p>
                            ))}
                        </div>
                    ))}
                </div>
                <div className="selected-main-category__ad-container">
                    <img alt="ad" src={foto} className="selected-main-category__ad-image"/>
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