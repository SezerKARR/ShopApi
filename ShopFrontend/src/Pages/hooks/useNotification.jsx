import { useState } from "react";

export const useNotification = () => {
    const [message, setMessage] = useState("");

    const showSuccess = (msg) => {
        setMessage(msg);
        alert(msg);
    };

    const showError = (msg) => {
        setMessage(msg);
        alert(msg);
    };

    return { showSuccess, showError };
    
};
