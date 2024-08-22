//list of accounts (from mapping AccountOverview.jsx) with the option to create new ones (AddNewAccount.jsx), edit or remove existing ones
// (overview gonna have icons to switch to EditAccount.jsx)
import React, {useEffect, useState} from 'react';
import {useAuth} from "../../../Authentication/AuthProvider.jsx";
import AddNewAccount from "../../Components/Accounts/AddNewAccount.jsx";
import AccountOverview from "../../Components/Accounts/AccountOverview.jsx";
import {useLocation, useNavigate} from "react-router-dom";
import AssetsOverview from "../../Components/Accounts/AssetsOverview.jsx";

export default function AccountBalances(){
    const { user } = useAuth();
    const [ accounts, setAccounts ] = useState([]);
    const [ parentAccount, setParentAccount] = useState({});
    const navigate = useNavigate();
    const location = useLocation();
    const [ statesInit, setStatesInit ] = useState();

    useEffect(() => {
        async function fetchAccounts(){
            try {
                const response = await fetch(`api/Account/All/u/owner`);
                const data = await response.json();
                setAccounts(data);
            } catch(err){
                console.error("Error fetching accounts: " + err)
            } 
        }
       fetchAccounts();
    }, []);

    
    function handleAddAccountClick() {
        if (!accounts) {
            navigate('/add-new-account', {state: {parentAccount: null}});
        } else {
            navigate('/add-new-account', {state: {parentAccount: null, accounts}});
        }
    }

    function handleAddSubAccountClick(account) {
        console.log(account);
        navigate('/add-new-account', { state: { parentAccount: account, accounts } });
    }
    
    function handleUpdateClick(account) {
        navigate('/edit-account', { state: { account: account } });
    }
    function updateStateAfterDeletion(accountId) {
        setAccounts((prevAccounts) => prevAccounts.filter(account => account.id !== accountId));
        setAccounts((prevAccounts) => prevAccounts.filter(account => account.parentAccountId !== accountId));
        // Or navigate to another page
        navigate('/account-balances');
    }
    
    function handleDeleteClick(accountId) {
        const isConfirmed = window.confirm("Are you sure you want to delete this account?");
        if (isConfirmed) {
            // Perform the delete operation
            deleteAccount(accountId);
        }
    }
    async function deleteAccount(accountId) {
        try {
            const response = await fetch(`api/Account/u/a/${accountId}`, { method: 'DELETE', headers: {'Content-Type': 'application/json'} });
            
            if (response.status == 204) {
                console.log('Account deleted successfully');
                updateStateAfterDeletion(accountId);
            } else {
                alert('Failed to delete the account');
            }
        } catch (err) {
            console.error("Error deleting account: " + err);
            alert('Error deleting the account');
        }
    }

    return(<div className={"accountsBalances"}>
        <button className={"mainButton"} onClick={handleAddAccountClick}>Add new account</button>
        <>{!accounts ? 
            (<div>                
                <p><br/>Seems like you don't have any accounts yet. Click the button to create one.</p>
            </div>)
            : (<>

                {/*todo finish this and the edit - add - delete buttons ? subAccounts view is capped needs solution */}
                
                <AssetsOverview accounts={accounts} /> 
                <div className={"accountCards"}>                
                {accounts.map((account, i) => 
                    <AccountOverview key={i} 
                                     id={`${account.type}-${i}`} 
                                     account={account}
                                     onAddSubAccountClick={() => handleAddSubAccountClick(account)}
                                     onDeleteClick={() => handleDeleteClick(account.id)} onEditClick ={() => handleUpdateClick(account)}/>)}
                </div>
            </>)
    }</>
    </div>)
}