import React from "react";
import { useAuth } from "./AuthProvider.jsx";
import {Navigate, Route} from "react-router-dom";

export default function ProtectedRoute ({ component: Component, ...rest }){
    const { user } = useAuth();

    return (
        <Route
            {...rest}
            render={props =>
                user ? (
                    <Component {...props} />
                ) : (
                    <Navigate to="/login" />
                )
            }
        />
    );
};
