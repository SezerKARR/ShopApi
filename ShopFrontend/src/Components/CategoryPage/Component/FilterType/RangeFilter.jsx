import React, {memo, useEffect, useState} from 'react';
import './RangeFilter.css';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faSearch} from "@fortawesome/free-solid-svg-icons";

const RangeFilter = memo(({label, maxRange, minRange, onRangeChange, currentMinMax = {min: -1, max: -1},selectedRangeId}) => {
    const {min, max} = currentMinMax;
    const [minValue, setMinValue] = useState(min);
    const [maxValue, setMaxValue] = useState(max);
    const [values, setValues] = useState([]);

    useEffect(() => {
        let maxValueForValues = maxRange;
        let tempValues = [];
        let Increase = Math.round(maxValueForValues / 5 / 10) * 10;
        for (let i = 5; i > 0; i--) {
            let minvalueForValues = maxValueForValues - Increase;
            tempValues.push({
                id: i,
                minValue: minvalueForValues,
                maxValue: maxValueForValues,
            });
            maxValueForValues = minvalueForValues;
        }
        setValues([...tempValues].reverse());
    }, [label]);

    const handleRangeChange = ({key, e}) => {
        const value = parseInt(e.target.value, 10);
        if (key === "min") {
            setMinValue(value);
            return;
        } else if (key === "max") {
            setMaxValue(value);
            return;
        }
        console.log("error");
    };

    const SearchForValues = () => {
        if (minValue > maxValue) {
            onRangeChange(maxValue, minValue);
            return;
        }
        onRangeChange(minValue, maxValue);
    };

    const submitVariables = ({minSubmit, maxSubmit, valueId = -1}) => {
        localStorage.setItem('selectedValue', valueId);

        if (minSubmit > maxSubmit) {
            onRangeChange(maxSubmit, minSubmit,valueId);
            return;
        }
        onRangeChange(minSubmit, maxSubmit,valueId);
    };


    if (values.length <= 0) return null;

    return (
        <div className="PriceFilter-container">
            <label>{label}</label>
            <div className={"row-range-field-container"}>
                <div className={"row-range-field-container-ranges-container"}>
                    <input className="input-range" type="number" value={minValue === -1 ? "" : minValue}
                           onChange={(e) => handleRangeChange({key: "min", e: e})}/>
                    <input className="input-range" type="number" value={maxValue === -1 ? "" : maxValue}
                           onChange={(e) => handleRangeChange({key: 'max', e: e})}/>
                    <FontAwesomeIcon icon={faSearch} onClick={SearchForValues}/>
                </div>
                {selectedRangeId == 0 &&
                    <div key={0} className={"priceFilter-Container-range"}>
                        <input
                            type="checkbox"
                            checked={true}  // selectedValue ile kontrol ediyoruz
                            readOnly={true}
                        />
                        <label className="priceFilter-Container-range-label" htmlFor={``}>
                            {`  ${min} - ${max} TL`}
                        </label>
                    </div>
                }
                {values && values.length > 0 && values.map(value => (
                    <div key={value.id} className={"priceFilter-Container-range"}>
                        <input
                            type="checkbox"
                            id={`option-${value.id}`}
                            checked={selectedRangeId === value.id}  // selectedValue ile kontrol ediyoruz
                            onChange={() => submitVariables({
                                minSubmit: value.minValue,
                                maxSubmit: value.maxValue,
                                valueId: value.id
                            })}
                        />
                        <label className="priceFilter-Container-range-label" htmlFor={`option-${value.id}`}>
                            {`  ${value.minValue} - ${value.maxValue}TL`}
                        </label>
                    </div>
                ))}
            </div>
        </div>
    );
});

export default RangeFilter;
