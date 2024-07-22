import React, {useState} from 'react';

import {Link, useNavigate} from "react-router-dom";
import {useAuth} from "../../../Authentication/AuthProvider.jsx";
import '../../../Styles/forms.css'


export default function Login() {
    const navigate = useNavigate();
    const { login } = useAuth();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    async function HandleLogin(e, email, password){
        e.preventDefault();
        
        try {
            const incoming = await login(email, password);

            if(incoming.success){
                console.log("Successfully logged in!");
                navigate("/homepage");
            }

        } catch(err){
            console.error("Error while logging in:", err);
        }
    }
    
    return (
        <>
            <h2>Welcome back!</h2>
            <div className="formContainerDiv">
                <form className="form" onSubmit={(e) => HandleLogin(e, email, password)}>
                    <label className="formLabel" htmlFor="email">
                        E-mail: </label>
                        <input className="formInput" type="email" name="email" required onChange={(e) => setEmail(e.target.value)}></input>
                   
                    <label className="formLabel" htmlFor="pswd">Password:</label>
                        <input className="formInput" type="password" name="password" required
                                   onChange={(e) => setPassword(e.target.value)}></input> 
                    <button className="formSubmitButton" key="logIn" type="submit">Log in</button>
                </form></div>
            <div className="transferDiv"><p className="transferText">Need an account?</p>
                <Link to="/register">
                    <button className="transferLink">Register</button>
                </Link></div></>
    );
}


