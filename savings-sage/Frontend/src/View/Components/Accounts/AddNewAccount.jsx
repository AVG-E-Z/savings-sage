import React, {useEffect, useState} from "react";
import {useNavigate, useLocation} from "react-router-dom";
import {useAuth} from "../../../Authentication/AuthProvider.jsx";
import CurrencyArray from "../../../Utility/CurrencyArray.js";

export default function AddNewAccount(){
    const { user } = useAuth();
    const [values, setValues] = useState({
        name: '',
        currency: 'AED',
        amount: 0,
        amountInterest: null,
        amountCapital: null,
        parentAccountId: null,
        expirationDate: null,
        type: 'Debit'
    });
    let seconds = 2;
    const navigate = useNavigate();
    const location = useLocation();
    
    const { parentAccount, accounts } = location.state || { parentAccount: null, accounts: [] };


    useEffect(() => {
        //console.log(parentAccount)
        if (location.state.parentAccount) {
            console.log('location: '+location.state.parentAccount.id)
            setValues((prevValues) => ({
                ...prevValues,
                parentAccountId: location.state.parentAccount.id
            }));
        }
    }, [parentAccount, location.state]);

    function handleInput(event){
        const newObj = {...values, [event.target.name]: event.target.value}
        setValues(newObj);
    }

    useEffect(() => {
        console.log(parentAccount)
        console.log(values)
        //console.log(user.username)
    }, [values]);
    
    async function HandleSubmit(e){
        e.preventDefault();
        try {
            const response = await fetch(`api/Account/u/Add`, {method: "POST", headers: {'Content-Type': 'application/json'}, body: JSON.stringify(values)});

            const incoming = await response.json();

            //console.log('incoming: '+JSON.stringify(incoming));

            if(incoming.ok){
                console.log("Successfully added an account!");
                setTimeout(() => {
                                navigate('/account-balances');
                            }, 1000 * seconds)
            }

        } catch(err){
            console.error("Error while adding account:", err);
        }
    }

    return (<>
        <h2>Add a new account to keep track of:</h2>
        <div className="formContainerDiv">
            <form className="form" onSubmit={(e) => HandleSubmit(e)}>
                <label className="formLabel" htmlFor="name">AccountName:</label>
                <input className="formInput"
                       type="text"
                       name="name"
                       required
                       onChange={handleInput}></input>

                <label className="formLabel" htmlFor="type">Account type:</label>
                <select id="typeSelection" name="type" onChange={handleInput}>
                    <option value="Debit">Debit</option>
                    <option value="Credit">Credit</option>
                    <option value="Loan">Loan</option>
                    <option value="Cash">Cash</option>
                    <option value="Savings">Savings</option>
                </select>
                
                <label className="formLabel" htmlFor="currency">
                    Currency:</label>
                <select id="currencySelection" name="currency" onChange={handleInput}>
                    {CurrencyArray.map(x =>
                        <option key={x} value={x}>{x}</option>)}
                </select>
                
                <label className="formLabel" htmlFor="amount">Current balance:</label>
                <input className="formInput" type="number" name="amount" required onChange={handleInput}></input>
                {values.type === "Loan" && (<>
                    <label className="formLabel" htmlFor="amountCapital">Loan capital:</label>
                    <input className="formInput" type="number" name="amountCapital" required onChange={handleInput}></input>
                    
                    <label className="formLabel" htmlFor="amountInterest">Loan interest:</label>
                    <input className="formInput" type="number" name="amountInterest" required onChange={handleInput}></input>
                </>)}
                {(values.type === "Loan" || values.type === "Savings") && (<>
                    <label className="formLabel" htmlFor="expirationDate">Expiration date:</label>
                    <input className="formInput" type="date" name="expirationDate" required onChange={handleInput}></input>
                </>)}
                <button className="formSubmitButton" id="addNewAccountButton" type="submit">Add account</button>
            </form>

            {accounts && (<button className="formSubmitButton" id="backButton" type="submit" onClick={() => navigate('/account-balances')}>Back</button>)}
        </div>
    </>)
}