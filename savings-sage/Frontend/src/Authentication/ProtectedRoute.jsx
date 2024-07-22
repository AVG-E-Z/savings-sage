import React from 'react';
import { useAuth } from './AuthProvider';
import { Navigate, Outlet } from 'react-router-dom';

const ProtectedRoute = ({ element, ...rest }) => {
    const { user } = useAuth();

    return user ? element : <Navigate to="/login" />;
};

export default ProtectedRoute;
