import React from "react";
import { Navigate, useLocation } from "react-router-dom";
import LocalStorageKey from "../constants/LocalStorageKey";

const Authorization = ({ children, allowedRoles }) => {
    const token = localStorage.getItem(LocalStorageKey.JWTTOKEN);
    const location = useLocation();

    if (token) {
        const userRole = localStorage.getItem(LocalStorageKey.ROLE);
        
        if(allowedRoles.includes(userRole)){
            return children;
        }

        localStorage.removeItem(LocalStorageKey.JWTTOKEN);
        localStorage.removeItem(LocalStorageKey.ROLE);
        return <Navigate to="/" state={{ path: location.pathname }} />;
    }

    localStorage.removeItem(LocalStorageKey.JWTTOKEN);
    localStorage.removeItem(LocalStorageKey.ROLE);
    return <Navigate to="/" state={{ path: location.pathname }} />;
};

export default Authorization;