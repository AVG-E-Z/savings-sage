import React, { useEffect, useState } from 'react';
import { useAuth } from "../../../Authentication/AuthProvider.jsx";
import '../../Pages/Transactions/transactions.css';

function AddNewTransaction() {
    const { user } = useAuth();
    const [allCategories, setAllCategories] = useState(null);
    const [allAccounts, setAllAccounts] = useState(null);
    const [isLoading, setLoading] = useState(true);
    const [areCategoriesDone, setAreCategoriesDone] = useState(false);
    const [areAccountsDone, setAreAccountsDone] = useState(false);
    const [chosenCategoryId, setChosenCategoryId] = useState(null);
    const [direction, setDirection] = useState(null);
    const [isRecurring, setIsRecurring] = useState(false);
    const [refreshDays, setRefreshDays] = useState(null);
    const [type, setType] = useState(0); //which means "payment" on the backend
    const [siblingTransaction, setSiblingTransaction] = useState(null);
    const [chosenAccountId, setChosenAccountId] = useState(null);
    const [date, setDate] = useState(Date.now);
    const [amount, setAmount] = useState();

    useEffect(() => {
        async function fetchCategories() {
            try {
                const response = await fetch(`api/Category/GetAll/${user.username}`);
                const data = await response.json();
                setAllCategories(data); 
            } catch (err) {
                console.error("Error fetching categories: " + err);
            } finally {
                setAreCategoriesDone(true); 
            }
        }
        async function fetchAccounts() {
            try {
                const response = await fetch(`api/Account/All/u/${user.username}`);
                const data = await response.json();
                setAllAccounts(data);
            } catch (err) {
                console.error("Error fetching accounts: " + err);
            } finally {
                setAreAccountsDone(true);
            }
        }
        fetchCategories();
        fetchAccounts();
    }, [user.username]);

    useEffect(() => {
        if (allCategories && allAccounts){
            setIsLoading(false);
        }
    }, [allCategories, allAccounts]);

    useEffect(() => {
        console.log(isRecurring);
    }, [isRecurring]);

    if (isLoading) {
        return <div>Loading...</div>;
    }
    
    const categories = allCategories || [];

    return (
        <>
            <div className="jointContainer">
                <div className='iconsDiv'><h2>Categories:</h2>
                    {categories.map(cat => (
                        <div key={cat.id + cat.name[0]} id={cat.id} className="categoryDiv" onClick={() => setChosenCategoryId(cat.id)}>
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
                            onChange={(e)=> setAmount(e.target.value)}>
                            
                        </input>
                        <label htmlFor="date" className="formLabel">Date:</label>
                        <input 
                            name="date" 
                            type="date" 
                            className="transFormInput" 
                            onChange={(e)=> setDate(e.target.value)}>
                            
                        </input>
                        <label htmlFor="account" className="formLabel">Account:</label>
                        <select className="transFormInput">
                            <option>DummyAccount1</option>
                            <option>DummyAccount2</option>
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
                                <input name="repeatDays" type="number" className="transFormInput" onChange={(e)=> setRefreshDays(e.target.value)}></input>
                                <p>days</p>
                            </>}
                    </form>
                </div>
                <div className="typeContainer">
                    <h2>Type:</h2>

                </div>
            </div>
        </>
    );
}

export default AddNewTransaction;
