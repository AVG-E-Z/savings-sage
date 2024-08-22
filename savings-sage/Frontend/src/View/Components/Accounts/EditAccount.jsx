import {useAuth} from "../../../Authentication/AuthProvider.jsx";
import React, {useEffect, useState} from "react";
import {useLocation, useNavigate} from "react-router-dom";

export default function EditAccount(){
    const { user } = useAuth();
    const navigate = useNavigate();
    const location = useLocation();
    const [category, setCategory] = useState(null);

    const { account } = location.state || { account: {} };
    
    const [values, setValues] = useState({
        name: '',
        amount: 0,
        amountInterest: null,
        amountCapital: null,
        parentAccountId: null,
        expirationDate: null,
        type: 'Debit'
    });

    useEffect(() => {
        //console.log(account)
        if (location.state.account) {
            setValues((prevValues) => ({
                ...prevValues,
                name: account.name,
                amount: account.amount,
                amountInterest: account.amountInterest !== undefined ? account.amountInterest : null,
                amountCapital: account.amountCapital !== undefined ? account.amountCapital : null,
                parentAccountId: account.parentAccountId,
                expirationDate: account.expirationDate,
                type: account.type,
            }));
            console.log(values)
        }
    }, [location.state]);

    useEffect(() => {
        async function fetchCategories() {
            try {
                const response = await fetch(`api/Category/GetAll/${user.username}`);
                const data = await response.json();
                if (response) {
                    var filtered = data.filter(x => x.name === "Correction")
                    console.log(filtered);
                    setCategory(filtered);
                }
            } catch (err) {
                console.error("Error fetching categories: " + err);
            }
        }
        fetchCategories();
    }, [user.username]);

    function handleInput(event){
        const newObj = {...values, [event.target.name]: event.target.value}
        setValues(newObj);
    }

    async function HandleSubmit(e){
        e.preventDefault();
        
        try {
            const response = await fetch(`api/Account/u/a/${account.id}/edit`, {method: "PUT", headers: {'Content-Type': 'application/json'}, body: JSON.stringify(values)})

            const incoming = await response.json();

            if(incoming.ok){
                console.log("Successfully edited account!");
            }

        } catch(err){
            console.error("Error editing account:", err);
        }
    }

    async function HandleSubmitAmount(e){
        e.preventDefault();

        const dataForBody = {
            Name: "adjustment",
            Currency: account.currency,
            AccountId: account.id,
            Date: Date.now,
            CategoryName: category[0].id,
            Amount: values.amount,
            Direction: account.amount-values.amount >= 0 ? "Out": "In",
            IsRecurring: false,
            RefreshDays: null,
            Type: "Correction",
            SiblingTransactionId: null,
        }
        
        if (account.amount !== values.amount) {
            try {
                const response = await fetch(`api/Transaction/Add/${user.username}`, {
                    method: "POST",
                    headers: {'Content-Type': 'application/json'},
                    body: JSON.stringify(dataForBody)
                });

                if (response.ok) {
                    console.log("Successfully edited amount!");
                }

            } catch (err) {
                console.error("Error while editing amount:", err);
            }
        }
    }

    
    return (<>
            <h2>Edit '{account.name}':</h2>

            <div className="formContainerDiv">
                <form className="form" onSubmit={(e) => HandleSubmit(e)}>
                    <label className="formLabel" htmlFor="name">AccountName:</label>
                    <input className="formInput"
                           type="text"
                           name="name"
                           placeholder={account.name}
                           onChange={handleInput}></input>

                    <label className="formLabel" htmlFor="type">Account type:</label>
                    <select id="typeSelection" name="type" value={account.type} onChange={handleInput}>
                        <option value="Debit">Debit</option>
                        <option value="Credit">Credit</option>
                        <option value="Loan">Loan</option>
                        <option value="Cash">Cash</option>
                        <option value="Savings">Savings</option>
                    </select>

                    {values.type === "Loan" && (<>
                        <label className="formLabel" htmlFor="amountCapital">Loan capital:</label>
                        <input className="formInput"
                               type="number"
                               name="amountCapital"
                               required
                               placeholder={account.amountCapital}
                               onChange={handleInput}></input>

                        <label className="formLabel" htmlFor="amountInterest">Loan interest:</label>
                        <input className="formInput"
                               type="number"
                               name="amountInterest"
                               required
                               placeholder={account.amountInterest}
                               onChange={handleInput}></input>
                    </>)}
                    {(values.type === "Loan" || values.type === "Savings") && (<>
                        <label className="formLabel" htmlFor="expirationDate">Expiration date:</label>
                        <input className="formInput"
                               type="date"
                               name="expirationDate"
                               required
                               placeholder={account.expirationDate}
                               onChange={handleInput}></input>
                    </>)}
                    <button className="formSubmitButton" id="editAccountButton" type="submit">Edit account</button>
                </form>
                <form className="form" onSubmit={(e) => HandleSubmitAmount(e)}>
                    <label className="formLabel" htmlFor="amount">Current balance:</label>
                    <input className="formInput"
                           type="number"
                           name="amount"
                           placeholder={account.amount}
                           onChange={handleInput}></input>
                    <button className="formSubmitButton" id="editAccountButton" type="submit">Edit amount</button>
                </form>
                <button className="formSubmitButton" id="doneEditingButton" type="button"
                        onClick={() => navigate('/account-balances')}>Done editing
                </button>
            </div>
        </>
    )
}