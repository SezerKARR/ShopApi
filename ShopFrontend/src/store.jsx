import { configureStore } from "@reduxjs/toolkit";

const initialState = {
    product: null,
};

const productReducer = (state = initialState, action) => {
    switch (action.type) {
        case "ADD_PRODUCT":
            return { ...state, product: action.payload };
        default:
            return state;
    }
};

// Redux store'unu oluşturuyoruz
const store = configureStore({
    reducer: {
        product: productReducer,
    },
});

export default store;
