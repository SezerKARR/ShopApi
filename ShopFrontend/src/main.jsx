import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import { GoogleOAuthProvider } from "@react-oauth/google";
import { Provider } from "react-redux";
import store from "./store"; // Redux store'unuzu import edin

const root = ReactDOM.createRoot(document.getElementById('root'));
const CLIENT_ID = "1001402098614-1oba7p60eh1btj342b7sb0b38ct294bm.apps.googleusercontent.com";

root.render(
    <GoogleOAuthProvider clientId={CLIENT_ID}>
        <Provider store={store}>
            <App />
        </Provider>
    </GoogleOAuthProvider>
);
