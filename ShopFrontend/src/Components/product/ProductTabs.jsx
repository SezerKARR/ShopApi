import React, {useState} from 'react';
import './ProductTabs.css';
import ProductQA from "./ProductQA.jsx";
import ProductReviews from "./Component/ProductReviews/ProductReviews.jsx";
import ProductDescription from "./ProductDescription.jsx";

const ProductTabs = ({ product }) => {
    const [activeTab, setActiveTab] = useState("description");
    const TabPanel = ({ children, value, index }) => {
        return value === index ? <div className="tab-panel">{children}</div> : null;
    };
    const tabs = [
        { id: "description", label: "Product Description", component: <ProductDescription product={product} /> },
        { id: "reviews", label: "Reviews", component: <ProductReviews comments={product.comments} /> },
        { id: "qa", label: "Question Answer", component: <ProductQA /> }
    ];
    const handleComments=()=>{
       

    }
    
    return (
        <div className="product-tabs">
            
            <div className="product-tabs__header">
                {tabs.map(tab => (
                    <button
                        key={tab.id}
                        onClick={() => setActiveTab(tab.id)}
                        className={`product-tabs__tab ${activeTab === tab.id ? "product-tabs__tab--active" : ""}`}
                    >
                        {tab.label}
                    </button>
                ))}
            </div>

            <div className="product-tabs__content">
                {tabs.map(tab => (
                    <TabPanel key={tab.id} value={activeTab} index={tab.id}>
                        {tab.component}
                    </TabPanel>
                ))}
            </div>
        </div>
    );
};

export default ProductTabs;