import React, {useState} from 'react';
import './FileInput.css';

const FileInput = ({onChange, name,error}) => {
    const [selectedFiles, setSelectedFiles] = useState([]);
    const [title, setTitle] = useState("");
    const [previewUrls, setPreviewUrls] = useState([]);
    const [selectedImage, setSelectedImage] = useState(null);
    const handleFileChange = (e) => {
        const files = Array.from(e.target.files);

        const validImages = [];
        const invalidImages = [];

        files.forEach((file) => {
            if (file && file.type.startsWith("image/")) {
                validImages.push(file);
                const reader = new FileReader();
                reader.onloadend = () => {
                    setPreviewUrls((prevUrls) => [...prevUrls, reader.result]);
                };
                reader.readAsDataURL(file);
            } else {
                invalidImages.push(file);
            }
        });
        console.log("files", files, "validImages", validImages, "invalidImages", invalidImages);
        if (validImages.length > 0) {
            onChange(e);
            setSelectedFiles(prev => [...prev, ...validImages]);
        }

        // invalidImages istersen loglayabilirsin, ama zorunlu değil
        // console.log("Geçersiz dosyalar:", invalidImages);
    };


    const handleSubmit = async (e) => {
        console.log(e.target.files);
    };
    const openModal = (imageUrl) => {
        setSelectedImage(imageUrl); // Modal için resmi ayarla
    };

    const closeModal = () => {
        setSelectedImage(null); // Modal'ı kapat
    };
    return (
        <div className="FileInput">
            <label>Dosya Seç:</label>
            {error && <div className="error">{error}</div>}
            
            {/*<button type={"button"} onClick={() => setError("asds")}>sadsad</button>*/}
            <div className="upload">
                <label className={`upload-button ${error ? 'upload-error' : ''}`}>
                    Select Image
                    <input
                        name={name}
                        type="file"
                        accept="image/*"
                        multiple
                        onChange={handleFileChange}
                        style={{display: "none"}}
                    />


                </label>
                <div className="selected-file-container">
                    {selectedFiles.length > 0 && (
                        selectedFiles.map((file, index) => (
                            <div key={index}>
                                {/* Fotoğrafın küçük boyutlu önizlemesi */}
                                {previewUrls[index] && (
                                    <img
                                        src={previewUrls[index]}
                                        alt={`preview-${index}`}
                                        
                                        className="selected-file-container__preview"
                                        onClick={() => openModal(previewUrls[index])}
                                    />
                                )}
                                <p>{file.name}</p> 
                            </div>
                        ))
                    )}
                </div>
                {selectedImage && (
                    <div
                        style={{
                            position: 'fixed',
                            top: '0',
                            left: '0',
                            right: '0',
                            bottom: '0',
                            backgroundColor: 'rgba(0, 0, 0, 0.7)',
                            display: 'flex',
                            justifyContent: 'center',
                            alignItems: 'center',
                            zIndex: '1000',
                        }}
                        onClick={closeModal}
                    >
                        <img
                            src={selectedImage}
                            alt="enlarged"
                            style={{ maxWidth: '90%', maxHeight: '90%' }}
                        />
                    </div>
                )}
            </div>
        </div>
    );
};

export default FileInput;