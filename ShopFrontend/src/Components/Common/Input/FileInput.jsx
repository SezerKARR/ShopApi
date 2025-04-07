
import React, {useState} from 'react';
import './FileInput.css';

const FileInput = ({onChange,name}) => {
    const [file, setFile] = useState(null);
    const [title, setTitle] = useState("");
    const [error, setError] = useState("");
    const handleFileChange = (e) => {
        const file = e.target.files[0];
        if (file && !file.type.startsWith("image/")) {
            setError("you can only upload a image file");
            alert("you can only upload a image file.");
            e.target.value = null; // seçimi temizle
            return;
        }
        onChange(e)

        // dosya geçerliyse buraya gelinir
        console.log("Yüklenen dosya:", file);
    };

    const handleSubmit = async (e) => {
       console.log(e.target.files);
    };
    
    return (
        <div className="FileInput">
            <label>Dosya Seç:</label>
            {/*<button type={"button"} onClick={() => setError("asds")}>sadsad</button>*/}
            <label className="upload-button">
                Select Image
                <input
                    name={name}
                    type="file"
                    accept="image/*"
                    onChange={handleFileChange}
                    style={{display: "none"}}
                />
            </label>
        </div>
    );
};

export default FileInput;