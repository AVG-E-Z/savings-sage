import React, { useEffect, useState } from 'react';
import { useAuth } from "../../../Authentication/AuthProvider.jsx";
import '../../Pages/Transactions/transactions.css';
import {all} from "axios";

function AddNewTransaction({setIsNewBeingAdded}) {
    const { user } = useAuth();
    const [allCategories, setAllCategories] = useState(null);
    const [allAccounts, setAllAccounts] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [areCategoriesDone, setAreCategoriesDone] = useState(false);
    const [areAccountsDone, setAreAccountsDone] = useState(false);
    const [chosenCategoryId, setChosenCategoryId] = useState(null);
    const [direction, setDirection] = useState(null);
    const [isRecurring, setIsRecurring] = useState(false);
    const [refreshDays, setRefreshDays] = useState(null);
    const [subType, setSubType] = useState('');
    const [siblingTransaction, setSiblingTransaction] = useState(null);
    const [chosenAccountId, setChosenAccountId] = useState(null);
    const [date, setDate] = useState(Date.now);
    const [amount, setAmount] = useState();
    const [type, setType] = useState("Payment"); //todo: when transfer gets implemented, this needs to be adjustable
    const [currency, setCurrency] = useState("HUF");
    const [name, setName] = useState("New transaction");
    
    //todo: legyen defaultja a választott számlának, hogyha egy van, akkor az legyen

    useEffect(() => {
        async function fetchCategories() {
            try {
                const response = await fetch(`api/Category/GetAll/${user.username}`);
                const data = await response.json();
                setAllCategories(data); 
            } catch (err) {
                console.error("Error fetching categories: " + err);
            } 
        }
        async function fetchAccounts() {
            try {
                const response = await fetch(`api/Account/All/u/${user.username}`);
                const data = await response.json();
                setAllAccounts(data);
            } catch (err) {
                console.error("Error fetching accounts: " + err);
            } 
        }
        fetchCategories();
        fetchAccounts();
    }, [user.username]);

    useEffect(() => {
        if (allCategories && allAccounts){
            setIsLoading(false);
            console.log(allAccounts);
        }
    }, [allCategories, allAccounts]);

    useEffect(() => {
        console.log(isRecurring);
    }, [isRecurring]);

    async function SubmitData(){
        const dataForBody = {
            Name: name,
            Currency: currency,
            AccountId: chosenAccountId,
            Date: date,
            CategoryId: chosenCategoryId,
            Amount: amount,
            Direction: direction,
            IsRecurring: isRecurring,
            RefreshDays: refreshDays,
            Type: type,
            SiblingTransactionId: siblingTransaction,
        }

        try {
            const response = await fetch(`api/Transaction/Add/${user.username}`, {method: "POST", headers: {'Content-Type': 'application/json'}, body: JSON.stringify(dataForBody)});

            if(response.ok){
                console.log("Successfully added a new transaction!");
                setIsNewBeingAdded(false);
            }

        } catch(err){
            console.error("Error while adding transaction:", err);
        }
        // const chosenBankAccountsCurrency = allAccounts.find(acc => acc.id === chosenAccountId).Currency;
        // setCurrency(chosenBankAccountsCurrency);
        //todo: fix this
    }
    
    function setTypeAndDirection(subtype){
        setSubType(subtype);
        if(subtype === "expense"){
            setDirection("Out");
        }else if(subtype === "income"){
            setDirection("In");
        }
    }
    
    if (isLoading) {
        return <div>Loading...</div>;
    }
    
    const categories = allCategories || [];

    return (
        <>
            <div className="nameCont"><input className="formInput nameDiv" value={name} onChange={(e)=> setName(e.target.value)}></input></div>
            <hr/>
            <div className="jointContainer">
                <div className='iconsDiv'><h2 className="title">Categories:</h2>
                    {categories.map(cat => (
                        <div key={cat.id + cat.name[0]} id={cat.id} className={`categoryDiv ${chosenCategoryId === cat.id? 'chosenCategory' : ''}`}
                             onClick={() => setChosenCategoryId(cat.id)}>
                            <img key={cat.id} src={cat.iconURL} alt={cat.name}/>
                            <p className="categoryName">{cat.name}</p>
                        </div>
                    ))}
                </div>
                <div className="formContainer">
                    <h2>Details:</h2>
                    <form className="addNewTransactionForm">
                        <label htmlFor="amount"
                               className="formLabel">Amount:</label>
                        <input
                            name="amount"
                            type="number"
                            className="transFormInput"
                            onChange={(e) => setAmount(e.target.value)}>
                        </input>
                        <label htmlFor="date" className="formLabel">Date:</label>
                        <input
                            name="date"
                            type="date"
                            className="transFormInput"
                            onChange={(e) => setDate(e.target.value)}>
                        </input>
                        <label htmlFor="account" className="formLabel">Account:</label>
                        <select className="transFormInput"
                                onChange={(e) => setChosenAccountId(e.target.value)}>{allAccounts.map(acc => <option
                            key={acc.id + acc.name[0]} value={acc.id}>{acc.name}</option>)}
                        </select>
                        <label htmlFor="account"
                               className="formLabel">Recurring?</label>
                        <label className="switch">
                            <input type="checkbox"
                                   checked={isRecurring}
                                   onChange={(e) => setIsRecurring(e.target.checked)}/>
                            <span className="slider round"></span>
                        </label>
                        {isRecurring &&
                            <><label htmlFor="repeatDays" className="formLabel">Repeat in:</label>
                                <input name="repeatDays" type="number" className="transFormInput"
                                       onChange={(e) => setRefreshDays(e.target.value)}></input>
                                <p>days</p>
                            </>}
                    </form>
                </div>
                <div className="typeContainer">
                    <h2>Type:</h2>
                    <button
                        className={`type expenseButton ${subType === "" ? "" : subType === 'expense' ? 'chosen' : 'notChosen'}`}
                        onClick={() => setTypeAndDirection("expense")}>
                        <p className="typeText">Expense</p></button>
                    <button
                        className={`type incomeButton ${subType === "" ? "" : subType === 'income' ? 'chosen' : 'notChosen'}`}
                        onClick={() => setTypeAndDirection("income")}
                    >
                        <p className="typeText">Income</p></button>
                    <button
                        className={`type transferButton notChosen`}>
                        <p className="typeText">Transfer</p></button>
                </div>
            </div>
            <div className="buttonContainer" onClick={() => SubmitData()}>
                <button className="transFormSubmitButton">Submit</button>
            </div>
        </>
    );
}

//todo: need to give transfer button an onclick and css styling later on, when transfer is implemented

export default AddNewTransaction;
