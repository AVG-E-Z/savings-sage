import React, {useEffect, useState} from 'react';
import {Link, useNavigate} from "react-router-dom";
import {useAuth} from "../../../Authentication/AuthProvider.jsx";

export default function Registration() {
    const [success, setSuccess] = useState(false);
    let seconds = 2;
    const navigate = useNavigate();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [username, setUsername] = useState('');

    async function HandleSubmit(e, username, email, password){
        
        e.preventDefault();

        const data = {
            username: username,
            email: email,
            password: password,
        };

        try {
            const response = await fetch("api/auth/Register", {method: "POST", headers: {'Content-Type': 'application/json'}, body: JSON.stringify(data)});

            const incoming = await response.json();

            console.log(incoming);

            if(incoming.success){
                console.log("Successfully registered an account!");
                setSuccess(true);

            }

        } catch(err){
            console.error("Error while logging in:", err);
        }
    }

    useEffect(() => {
        if (success === true) {
            setTimeout(() => {
                navigate('/login');
            }, 1000 * seconds);
        }
    }, [success]);
    
    return (
        <>
            <h2>Welcome to our community!</h2>
            <div className="formContainerDiv">
                <form className="form" onSubmit={(e) => HandleSubmit(e, username, email, password)}>
                    <label className="formLabel" htmlFor="username">Username:</label>
                    <input className="formInput" type="text" name="username" required
                           onChange={(e) => setUsername(e.target.value)}></input>
                   
                    <label className="formLabel" htmlFor="email">
                        E-mail:</label>
                    <input className="formInput" type="email" name="email" required
                           onChange={(e) => setEmail(e.target.value)}></input>

                    <label className="formLabel" htmlFor="pswd">Password:</label>
                    <input className="formInput" type="password" name="password" required
                           onChange={(e) => setPassword(e.target.value)}></input>
                    <button className="formSubmitButton" id="signUpButton" type="submit">Sign up!</button>
                </form>
            </div>
            <div className="transferDiv"><p className="transferText">Already have an account?</p>
                <Link to="/login">
                    <button className="transferLink">Login</button>
                </Link></div>
        </>
    );
}

