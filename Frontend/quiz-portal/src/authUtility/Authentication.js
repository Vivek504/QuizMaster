import React from "react";
import { Navigate, useLocation } from "react-router-dom";
import LocalStorageKey from "../constants/LocalStorageKey";

const Authentication = ({ children }) => {
    const token  = localStorage.getItem(LocalStorageKey.JWTTOKEN);
    const location = useLocation();
    
    if (!token) {
        return <Navigate to="/" state={{ path: location.pathname }} />;
    }

    return children;
};

export default Authentication;