import React from "react";
import "./TextInput.css"
const TextInput = ({ label, name, value, onChange, onBlur, error, type = "text" }) => {
    return (
        <div className="form-group">
            <label className={"formLabel"} htmlFor={name}>{label}</label>
            <input
                type={type}
                id={name}
                name={name}
                value={value}
                onChange={onChange}
                onBlur={onBlur}
                className={`form-input ${error ? "input-error" : ""}`}
            />
            {error && <div className="error">{error}</div>}
        </div>
    );
};

export default TextInput;
