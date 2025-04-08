import React, {useState} from 'react';
import './SelectInput.css';

const SelectInput = ({elements,label, name, onChange, value}) => {
    
    const [selectedElement, setSelectedElement] = useState(null);
    let isSelectable=elements?.length > 0;
    // label="Product Name"
    // name="productName"
    // value={formik.values.productName}
    // onChange={formik.handleChange}
    // error={formik.touched.productName && formik.errors.productName}

    return (<div className={`Select ${isSelectable ? "" : "null-main"}`}>

            <label className={`Select-label`}>{label}</label>
            <select name={name} key={1} onChange={onChange}
                    disabled={!isSelectable} value={value} className="SelectInput-Select">
                <option value="-1">Select {label}</option>
                {elements?.map((element) => (<option key={element.id} value={element.id}>
                    {element.name}
                </option>))}
            </select>


        </div>

    );
};

export default SelectInput;