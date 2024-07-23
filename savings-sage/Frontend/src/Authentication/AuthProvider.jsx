import React, { createContext, useState, useEffect, useContext } from 'react';
import {useNavigate} from "react-router-dom";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const response = await fetch('api/auth/me', { credentials: 'include' });
            //headers: {Authorization: `Bearer ${}`}
                if (response.ok) {
                    const data = await response.json();
                    setUser(data);
                } else {
                    setUser(null);
                    console.error('Error fetching user:', response.statusText);
                }
            } catch (error) {
                setUser(null);
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
                //credentials: 'include' //kérdés, h ez ide kell e
            });
           const data = await response.json();
           
            console.log(data.token)

            if (!response.ok) {
                throw new Error('Login failed');
            }
            
            const userResponse = await fetch('api/auth/me', { credentials: 'include' });
            //headers: {Authorization: `Bearer ${data.token}`}, 
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
        <AuthContext.Provider value={{ user, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);