import React, { createContext, useState, useEffect, useContext } from 'react';
import {useNavigate} from "react-router-dom";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const response = await fetch('api/auth/me', { credentials: 'include' });
                if (response.ok) {
                    const data = await response.json();
                    setUser(data);
                    setLoading(false);
                } else {
                    setUser(null);
                    setLoading(false);
                    console.error('Error fetching user:', response.statusText);
                }
            } catch (error) {
                setUser(null);
                setLoading(false);
                console.error('Error fetching user:', error);
            }
        };
        fetchUser();
    }, []);

    const login = async (email, password) => {
        try {
            const response = await fetch('api/auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ email, password }),
                //credentials: 'include' 
            });
           const data = await response.json();
           
            console.log(data.token)

            if (!response.ok) {
                throw new Error('Login failed');
            }
            
            const userResponse = await fetch('api/auth/me', { credentials: 'include' });
            console.log(userResponse);
            
            if (userResponse.ok) {
                const data = await userResponse.json();
                console.log(data);
                setUser(data);
                return response;
            } else {
                setUser(null);
                console.error('Error fetching user after login:', userResponse.statusText);
            }
        } catch (error) {
            console.error('Error during login:', error);
        }
    };

    const logout = async () => {
        try {
            const response = await fetch('api/auth/logout', {
                method: 'POST',
                credentials: 'include'
            });

            if (response.ok) {
                setUser(null);
                
            } else {
                console.error('Logout failed:', response.statusText);
            }
        } catch (error) {
            console.error('Error during logout:', error);
        }
    };

    return (
        <AuthContext.Provider value={{ user, login, logout, loading }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);