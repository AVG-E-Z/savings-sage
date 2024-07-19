import React, { createContext, useContext, useState, useEffect } from "react";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [auth, setAuth] = useState({ token: null, isAuthenticated: false });

    useEffect(() => {
        const token = localStorage.getItem("token");

        if (token) {
            setAuth({ token, isAuthenticated: true });
        }
    }, []);

    const login = (token) => {
        localStorage.setItem("token", token);
        setAuth({ token, isAuthenticated: true });
    };

    const logout = () => {
        localStorage.removeItem("token");
        setAuth({ token: null, isAuthenticated: false });
    };

    return (
        <AuthContext.Provider value={{ auth, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);
