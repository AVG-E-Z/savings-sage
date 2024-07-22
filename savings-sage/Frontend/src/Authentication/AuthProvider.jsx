import React, {createContext, useState, useEffect, useContext} from 'react';
import axios from 'axios';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);

    useEffect(() => {
        axios.get('api/auth/me', { withCredentials: true })
            .then(response => setUser(response.data))
            .catch(() => setUser(null));
    }, []);

    const login = async (username, password) => {
        await axios.post('api/auth/login', { username, password }, { withCredentials: true });
        
        const response = await axios.get('/api/auth/me', { withCredentials: true });
        setUser(response.data);
    };

    const logout = async () => {
        await axios.post('api/auth/logout', {}, { withCredentials: true });
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{ user, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};
export const useAuth = () => useContext(AuthContext);