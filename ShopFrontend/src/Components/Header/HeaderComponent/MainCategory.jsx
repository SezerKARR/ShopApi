import React, {useEffect, useState} from 'react';
import './MainCategory.css';
import axios from "axios";
import foto from "../../../../public/Foto.png"
const MainCategory = () => {
    const [mainCategories, setMainCategories] = useState(null)
    const [hoveredMainCategory, setHoveredMainCategory] = useState(null)
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
                    setHoveredMainCategory(filteredCategories[2]);
                    console.log(filteredCategories[2]);
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
                                    <p key={category.id} className="mainCategory__container-category">{category.name}</p>
                                ))
                            }
                        </div>
                    ))}
                </div>
                <div className="main-category-ad-container"></div>
            </div>
        );
    };
    const SelectedMainCategorya= () => {
     
       
        return (
          
            <div className="selected-main-category">
                <div className="selected-main-category__categories">
                    {mainCategories[2]?.subCategories?.map((subCategory) => (
                        <div key={subCategory.id} className="selected-main-category__subcategory">
                            <h3 className="selected-main-category__subcategory-name">{subCategory.name}</h3>
                            {subCategory.subCategories?.map((category) => (
                                <p key={category.id} className="selected-main-category__category">{category.name}</p>
                            ))}
                        </div>
                    ))}
                </div>
                <div className="selected-main-category__ad-container">
                    <img alt="ad" src={foto} className="selected-main-category__ad-image" />
                </div>
            </div>
        );
    };
    return (
        <div>
            <div className={"MainCategoryElements"}>
                {mainCategories?.map(mainCategory => (
                    <p key={mainCategory.id} onMouseEnter={() => setHoveredMainCategory(mainCategory)}
                        // onMouseLeave={() => setHoveredMainCategory(null)}
                       className="MainCategoryElement">
                        {mainCategory.name}</p>
                ))}

            </div>

            {mainCategories !== null && <SelectedMainCategorya />}
        </div>
    );
};
export default MainCategory;