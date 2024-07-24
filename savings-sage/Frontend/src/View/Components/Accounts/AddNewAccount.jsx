import React, {useEffect, useState} from "react";
import {useNavigate, useLocation} from "react-router-dom";
import {useAuth} from "../../../Authentication/AuthProvider.jsx";

const currencyArray = ["AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARS", "AUD", "AWG", "AZN", "BAM", "BBD", "BDT", "BGN", "BHD", "BIF", "BMD", "BND", "BOB", "BRL", "BSD", "BTN", "BWP", "BYR", "BZD", "CAD", "CDF", "CHF", "CLF", "CLP", "CNY", "COP", "CRC", "CUC", "CUP", "CVE", "CZK", "DJF", "DKK", "DOP", "DZD", "EGP", "ERN", "ETB", "EUR", "FJD", "FKP", "GBP", "GEL", "GGP", "GHS", "GIP", "GMD", "GNF", "GTQ", "GYD", "HKD", "HNL", "HRK", "HTG", "HUF", "IDR", "ILS", "IMP", "INR", "IQD", "IRR", "ISK", "JEP", "JMD", "JOD", "JPY", "KES", "KGS", "KHR", "KMF", "KPW", "KRW", "KWD", "KYD", "KZT", "LAK", "LBP", "LKR", "LRD", "LSL", "LTL", "LVL", "LYD", "MAD", "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRO", "MUR", "MVR", "MWK", "MXN", "MYR", "MZN", "NAD", "NGN", "NIO", "NOK", "NPR", "NZD", "OMR", "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG", "QAR", "RON", "RSD", "RUB", "RWF", "SAR", "SBD", "SCR", "SDG", "SEK", "SGD", "SHP", "SLL", "SOS", "SRD", "SVC", "SYP", "SZL", "THB", "TJS", "TMT", "TND", "TOP", "TRY", "TTD", "TWD", "TZS", "UAH", "UGX", "USD", "UYU", "UZS", "VEF", "VND", "VUV", "WST", "XAF", "XCD", "XDR", "XOF", "XPF", "YER", "ZAR", "ZMW", "ZWL"]

export default function AddNewAccount(){
    const { user } = useAuth();
    const [values, setValues] = useState({
        name: '',
        currency: '',
        amount: 0,
        amountInterest: null,
        amountCapital: null,
        parentAccountId: null,
        expirationDate: null,
        accountType: ''
    });
    const [ success, setSuccess ] = useState(false);
    let seconds = 2;
    const navigate = useNavigate();
    const location = useLocation();
    const { parentAccount, accounts } = location.state || { parentAccount: null, accounts: [] };


    useEffect(() => {
        if (parentAccount) {
            setValues((prevValues) => ({
                ...prevValues,
                parentAccountId: parentAccount.Id
            }));
        }
    }, [parentAccount]);

    function handleInput(event){
        const newObj = {...values, [event.target.name]: event.target.value}
        setValues(newObj);
    }

    useEffect(() => {
        console.log(accounts)
        console.log(values)
        console.log(user.username)
    }, [values]);
    
    async function HandleSubmit(e){
        e.preventDefault();
        let data = values;
        
        if (values.accountType !== "Loan"){
            data.amountCapital = null;
            data.amountInterest = null;
        }
        
        try {
            const response = await fetch(`api/Account/u/${user.username}/Add`, {method: "POST", headers: {'Content-Type': 'application/json'}, body: JSON.stringify(data)});

            const incoming = await response.json();

            console.log(incoming);

            if(incoming.ok){
                console.log("Successfully added an account!");
                setSuccess(true);
            }

        } catch(err){
            console.error("Error while adding account:", err);
        }
    }
    
    useEffect(() => {
        if (success === true) {
            setTimeout(() => {
                navigate('/account-balances');
            }, 1000 * seconds);
        }
    }, [success]);

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

                <label className="formLabel" htmlFor="accountType">Account type:</label>
                <select id="typeSelection" name="accountType" onChange={handleInput}>
                    <option value="Debit">Debit</option>
                    <option value="Credit">Credit</option>
                    <option value="Loan">Loan</option>
                    <option value="Cash">Cash</option>
                    <option value="Savings">Savings</option>
                </select>
                
                <label className="formLabel" htmlFor="currency">
                    Currency:</label>
                <select id="currencySelection" name="currency" onChange={handleInput}>
                    {currencyArray.map(x =>
                        <option key={x} value={x}>{x}</option>)}
                </select>
                
                <label className="formLabel" htmlFor="amount">Current balance:</label>
                <input className="formInput" type="number" name="amount" required onChange={handleInput}></input>
                {values.accountType === "Loan" && (<>
                    <label className="formLabel" htmlFor="amountCapital">Loan capital:</label>
                    <input className="formInput" type="number" name="amountCapital" required onChange={handleInput}></input>
                    
                    <label className="formLabel" htmlFor="amountInterest">Loan interest:</label>
                    <input className="formInput" type="number" name="amountInterest" required onChange={handleInput}></input>
                </>)}
                {(values.accountType === "Loan" || values.accountType === "Savings") && (<>
                    <label className="formLabel" htmlFor="expirationDate">Expiration date:</label>
                    <input className="formInput" type="date" name="expirationDate" required onChange={handleInput}></input>
                </>)}
                <button className="formSubmitButton" id="addNewAccountButton" type="submit">Add account</button>
            </form>

            {accounts && (<button className="formSubmitButton" id="backButton" type="submit" onClick={() => navigate('/account-balances')}>Back</button>)}
        </div>
    </>)
}