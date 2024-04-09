import React, { createContext, useContext } from "react";
import { useNavigate } from "react-router-dom";
import LocalStorageKey from "../constants/LocalStorageKey";
import { login as loginService, register as registerService } from "../services/authService";
import ROLES from "../constants/roles";

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
    const navigate = useNavigate();
    // const redirectPath = "/courses";

    const login = async (email, password, role) => {
        const response_data = await loginService(email, password);
        const token = response_data.payload?.token;

        if (token) {
            localStorage.setItem(LocalStorageKey.JWTTOKEN, token);
            localStorage.setItem(LocalStorageKey.ROLE, role);
            const redirectPath = role === ROLES.PROFESSOR ? '/courses': '/student-courses';
            navigate(redirectPath, { replace: true });
        }

        return response_data;
    };

    const logout = () => {
        localStorage.removeItem(LocalStorageKey.JWTTOKEN);
        localStorage.removeItem(LocalStorageKey.ROLE);
        navigate("/", { replace: true });
    };

    const register = async (email, password, role) => {
        const response_data = await registerService(email, password, role);
        const token = response_data.payload?.token;

        if (token) {
            localStorage.setItem(LocalStorageKey.JWTTOKEN, token);
            localStorage.setItem(LocalStorageKey.ROLE, role);
            const redirectPath = role === ROLES.PROFESSOR ? '/courses': '/student-courses';
            navigate(redirectPath, { replace: true });
        }

        return response_data;
    }

    return <AuthContext.Provider value={{ login, logout, register }}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
    return useContext(AuthContext);
};