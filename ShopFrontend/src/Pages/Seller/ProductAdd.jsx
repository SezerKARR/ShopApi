/*import React, {useEffect, useState} from "react";
import {useFormik} from "formik";
import * as Yup from "yup";
import {useDispatch} from "react-redux";
import TextInput from "../../components/common/Input/TextInput.jsx";
import {useNotification} from "../hooks/useNotification";
import {useGlobalContext} from "../../../GlobalProvider.jsx";
import "./ProductAdd.css"
import SelectInput from "../../Components/Common/Input/SelectInput.jsx";
import FileInput from "../../Components/Common/Input/FileInput.jsx";
import axios from "axios";

const validationSchema = Yup.object({
    id: Yup.number().positive("id must be positive").required("id is required"),
    productName: Yup.string().required("Product name is required"),
    price: Yup.number().positive("Price must be positive").required("Price is required"),
    category: Yup.string().required("Category is required"),
    ImageFile: Yup.mixed().required("File is required"),
});

const ProductAdd = () => {
    const {categories,API_URL} = useGlobalContext();

    const [mainCategories, setMainCategories] = useState(null);
    const [normalCategories, setNormalCategories] = useState(null);
    const dispatch = useDispatch();
    const {showSuccess, showError} = useNotification();
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [selectedMainCategory, setSelectedMainCategory] = useState(null);
    const handleSelectedMainCategory = (e) => {
        console.log(e.target.value);
        let newMainCategory = mainCategories.find((category) => category.id == e.target.value);
        setSelectedMainCategory(newMainCategory);
    }
 
    const formik = useFormik({
        initialValues: {id:"",productName: "", price: "", category: "",imageFile: null,}, validationSchema, onSubmit: async (values) => {
            try {
                setIsSubmitting(true);
                await dispatch(addProduct(values)).unwrap();
                showSuccess("Product added successfully!");
                formik.resetForm();
            } catch (error) {
                showError(error.message || "Failed to add product");
            } finally {
                setIsSubmitting(false);
            }
        },
    });
  
    useEffect(() => {
        if (categories != null) {
            setMainCategories(categories.filter(category => category.parentId === -1));
            setNormalCategories(categories.filter(category => category.parentId !== -1))
        }


    }, [categories])
    useEffect(() => {
        // Her değer değiştiğinde formik.values'ı logla
        console.log("Formik values updated:", formik.values);
    }, [formik.values]);


    if (!categories) {
        return null;
    }
    const onSubmit = () => {
        axios.create(`${API_URL}/api/products`, {})
        console.log(formik.values);
    }
    return (

        <div className="product-add-container">
            <h2>Add New Product</h2>
            <form onSubmit={formik.handleSubmit} className="product-form">
                <TextInput
                    label="Product Name"
                    type="number"
                    name="id"
                    value={formik.values.id}
                    onChange={formik.handleChange}
                    error={formik.touched.productName && formik.errors.productName}
                />
                <TextInput
                    label="Product Name"
                    name="productName"
                    value={formik.values.productName}
                    onChange={formik.handleChange}
                    error={formik.touched.productName && formik.errors.productName}
                />

                <TextInput
                    label="Price"
                    name="price"
                    type="number"
                    value={formik.values.price}
                    onChange={formik.handleChange}
                    error={formik.touched.price && formik.errors.price}
                />
                <SelectInput elements={mainCategories} label={"Main Category"} onChange={handleSelectedMainCategory}/>
                <SelectInput elements={selectedMainCategory?.subCategories} label={"Category"} name="category"
                             onChange={formik.handleChange } value={formik.values.category} main={selectedMainCategory}/>
                <FileInput onChange={formik.handleChange} name="imageFile"/>
                <button type="button" onClick={()=>onSubmit()}>Add New Product</button>
            </form>
        </div>);


};

export default ProductAdd;


// import React, { useState } from 'react';
// import axios from 'axios';
//
// const ProductAdd = () => {
//     // Form state'i
//     const [formData, setFormData] = useState({
//         title: '',
//         description: '',
//         category: '',
//         price: ''
//     });
//
//     // Loading ve error state'leri
//     const [isLoading, setIsLoading] = useState(false);
//     const [error, setError] = useState(null);
//     const [success, setSuccess] = useState(false);
//
//     // Input değişikliklerini handle eden fonksiyon
//     const handleChange = (e) => {
//         const { name, value } = e.target;
//         setFormData(prevState => ({
//             ...prevState,
//             [name]: value
//         }));
//     };
//
//     // Form submit fonksiyonu
//     const handleSubmit = async (e) => {
//         e.preventDefault();
//         setIsLoading(true);
//         setError(null);
//         setSuccess(false);
//
//         try {
//             // Backend API endpoint'ine POST isteği
//             const response = await axios.post('https://api.example.com/items', formData);
//
//             // Başarılı yanıt
//             console.log('Veri başarıyla oluşturuldu:', response.data);
//             setSuccess(true);
//
//             // Formu temizle
//             setFormData({
//                 title: '',
//                 description: '',
//                 category: '',
//                 price: ''
//             });
//         } catch (err) {
//             // Hata durumunda
//             setError('Veri oluşturulurken bir hata oluştu: ' + err.message);
//             console.error('Error:', err);
//         } finally {
//             setIsLoading(false);
//         }
//     };
//
//     return (
//         <div className="max-w-md mx-auto mt-10 p-6 bg-white rounded-lg shadow-md">
//             <h2 className="text-xl font-bold mb-4">Yeni Ürün Oluştur</h2>
//
//             {success && (
//                 <div className="mb-4 p-2 bg-green-100 text-green-700 rounded">
//                     Ürün başarıyla oluşturuldu!
//                 </div>
//             )}
//
//             {error && (
//                 <div className="mb-4 p-2 bg-red-100 text-red-700 rounded">
//                     {error}
//                 </div>
//             )}
//
//             <form onSubmit={handleSubmit}>
//                 <div className="mb-4">
//                     <label className="block text-gray-700 mb-2" htmlFor="title">
//                         Başlık
//                     </label>
//                     <input
//                         type="text"
//                         id="title"
//                         name="title"
//                         value={formData.title}
//                         onChange={handleChange}
//                         className="w-full px-3 py-2 border rounded-md"
//                         required
//                     />
//                 </div>
//
//                 <div className="mb-4">
//                     <label className="block text-gray-700 mb-2" htmlFor="description">
//                         Açıklama
//                     </label>
//                     <textarea
//                         id="description"
//                         name="description"
//                         value={formData.description}
//                         onChange={handleChange}
//                         className="w-full px-3 py-2 border rounded-md"
//                         rows="3"
//                     />
//                 </div>
//
//                 <div className="mb-4">
//                     <label className="block text-gray-700 mb-2" htmlFor="category">
//                         Kategori
//                     </label>
//                     <select
//                         id="category"
//                         name="category"
//                         value={formData.category}
//                         onChange={handleChange}
//                         className="w-full px-3 py-2 border rounded-md"
//                         required
//                     >
//                         <option value="">Kategori Seçin</option>
//                         <option value="electronics">Elektronik</option>
//                         <option value="clothing">Giyim</option>
//                         <option value="books">Kitaplar</option>
//                         <option value="furniture">Mobilya</option>
//                     </select>
//                 </div>
//
//                 <div className="mb-4">
//                     <label className="block text-gray-700 mb-2" htmlFor="price">
//                         Fiyat
//                     </label>
//                     <input
//                         type="number"
//                         id="price"
//                         name="price"
//                         value={formData.price}
//                         onChange={handleChange}
//                         className="w-full px-3 py-2 border rounded-md"
//                         required
//                     />
//                 </div>
//
//                 <button
//                     type="submit"
//                     disabled={isLoading}
//                     className="w-full bg-blue-500 text-white py-2 px-4 rounded-md hover:bg-blue-600 disabled:bg-blue-300"
//                 >
//                     {isLoading ? 'Yükleniyor...' : 'Oluştur'}
//                 </button>
//             </form>
//         </div>
//     );
// };
//
// export default ProductAdd;*/
import React, { useEffect, useState } from "react";
import { useFormik } from "formik";
import * as Yup from "yup";
import { useDispatch } from "react-redux";
import TextInput from "../../components/common/Input/TextInput.jsx";
import { useNotification } from "../hooks/useNotification";
import { useGlobalContext } from "../../../GlobalProvider.jsx";
import SelectInput from "../../Components/Common/Input/SelectInput.jsx";
import FileInput from "../../Components/Common/Input/FileInput.jsx";
import axios from "axios";
import "./ProductAdd.css";

const validationSchema = Yup.object({
    id: Yup.number().positive("id must be positive").required("id is required"),
    name: Yup.string().required("Product name is required"),
    price: Yup.number().positive("Price must be positive").required("Price is required"),
    categoryId: Yup.number().required("Category is required"),
    imageFile: Yup.mixed().required("File is required"),  // Dosya alanı doğrulaması
});

const ProductAdd = () => {
    const { categories, API_URL } = useGlobalContext();

    const [mainCategories, setMainCategories] = useState(null);
    const [normalCategories, setNormalCategories] = useState(null);
    const dispatch = useDispatch();
    const { showSuccess, showError } = useNotification();
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [selectedMainCategory, setSelectedMainCategory] = useState(null);

    const handleSelectedMainCategory = (e) => {
        let newMainCategory = mainCategories.find((category) => category.id == e.target.value);
        setSelectedMainCategory(newMainCategory);
    };

    const formik = useFormik({
        initialValues: { id: "12", name: "saas", price: "1", categoryId: "59", imageFile: null },
        validationSchema,
        onSubmit: async (values) => {
            try {
                setIsSubmitting(true);
                // Form verileri ile dosya yüklemesi yapıyoruz
                const formData = new FormData();
                formData.append("id", values.id);
                formData.append("name", values.name);
                formData.append("price", values.price);
                formData.append("categoryId", values.categoryId);
                formData.append("imageFile", values.imageFile);  // Dosya alanını ekliyoruz

                const response = await axios.post(`${API_URL}/api/product`, formData, {
                    headers: {
                        "Content-Type": "multipart/form-data",
                    },
                });
                console.log(response.data);
                showSuccess("Product added successfully!");
            } catch (error) {
                if (error.response) {
                    console.log(error.response);
                    // API'den gelen hata mesajına erişim
                    console.error("Error response data:", error.response.data);
                    showError(error.response.data.message || "Failed to add product");
                } else {
                    // Ağı bağlantı hatası gibi durumlar
                    console.error("Error:", error);
                    showError("An unexpected error occurred.");
                }
                showError(error.message || "Failed to add product");
            } finally {
                setIsSubmitting(false);
            }
        },
    });

    useEffect(() => {
        if (categories != null) {
            setMainCategories(categories.filter(category => category.parentId === -1));
            setNormalCategories(categories.filter(category => category.parentId !== -1));
        }
    }, [categories]);

    useEffect(() => {
        // Her değer değiştiğinde formik.values'ı logla
        console.log("Formik values updated:", formik.values);
    }, [formik.values]);

    if (!categories) {
        return null;
    }

    return (
        <div className="product-add-container">
            <h2>Add New Product</h2>
            <form onSubmit={formik.handleSubmit} className="product-form">
                <TextInput
                    label="id"
                    name="id"
                    type="number"
                    value={formik.values.id}
                    onChange={formik.handleChange}
                    error={formik.touched.id && formik.errors.id}
                />
                <TextInput
                    label="Product Name"
                    type="text"
                    name="name"
                    value={formik.values.name}
                    onChange={formik.handleChange}
                    error={formik.touched.productName && formik.errors.productName}
                />

                <TextInput
                    label="Price"
                    name="price"
                    type="number"
                    value={formik.values.price}
                    onChange={formik.handleChange}
                    error={formik.touched.price && formik.errors.price}
                />

                <SelectInput
                    elements={mainCategories}
                    label={"Main Category"}
                    onChange={handleSelectedMainCategory}
                />
                <SelectInput
                    elements={selectedMainCategory?.subCategories}
                    label={"Category"}
                    name="categoryId"
                    onChange={formik.handleChange}
                    value={formik.values.categoryId}
                    main={selectedMainCategory}
                />

                <FileInput
                    onChange={(e) => formik.setFieldValue("imageFile", e.target.files[0])}
                    name="imageFile"
                />
                <button type="submit" onClick={formik.handleSubmit}>
                    {isSubmitting ? "Adding..." : "Add New Product"}
                </button>
            </form>
        </div>
    );
};

export default ProductAdd;
