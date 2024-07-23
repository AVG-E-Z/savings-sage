import React, { createContext, useContext, useState, useEffect } from "react";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [auth, setAuth] = useState({ token: null, isAuthenticated: false });

    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const token = localStorage.getItem("token");

        if (token) {
            setAuth({ token, isAuthenticated: true });
        }
        setLoading(false);
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
        <AuthContext.Provider value={{ auth, login, logout, loading}}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);
