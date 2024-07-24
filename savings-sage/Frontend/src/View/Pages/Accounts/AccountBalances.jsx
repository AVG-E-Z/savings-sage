//list of accounts (from mapping AccountOverview.jsx) with the option to create new ones (AddNewAccount.jsx), edit or remove existing ones
// (overview gonna have icons to switch to EditAccount.jsx)
import React, {useEffect, useState} from 'react';
import {useAuth} from "../../../Authentication/AuthProvider.jsx";
import AddNewAccount from "../../Components/Accounts/AddNewAccount.jsx";
import AccountOverview from "../../Components/Accounts/AccountOverview.jsx";
import {useLocation, useNavigate} from "react-router-dom";

export default function AccountBalances(){
    const { user } = useAuth();
    const [ accounts, setAccounts ] = useState([]);
    const [ parentAccount, setParentAccount] = useState({});
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        async function fetchAccounts(){
            try {
                const response = await fetch(`api/Account/All/u/${user.username}`);
                const data = await response.json();
                setAccounts(data.$values);
            } catch(err){
                console.error("Error fetching accounts: " + err)
            } 
        }
       fetchAccounts();
    }, []);
    
    function handleAddAccountClick() {
        navigate('/account-balances/add-new', { state: { parentAccount: null, accounts } });
    }

    function handleAddSubAccountClick(account) {
        navigate('/account-balances/add-new', { state: { parentAccount: account, accounts } });
    }
    
    return(<div>
        <button className={"mainButton"} onClick={handleAddAccountClick}>Add new account</button>
        <>{accounts.length === 0 ? 
            (<div>
                
                <p><br/>Seems like you don't have any accounts yet. Click the button to create one.</p>
            </div>)
            : (<div>                
                {accounts.map((account, i) => 
                    <AccountOverview key={i} 
                                     id={`${account.name.replaceAll(' ', '')}-${account.type}-${i}`} 
                                     account={account}
                                     onAddSubAccountClick={() => handleAddSubAccountClick(account)}/>)}
            </div>)
    }</>
    </div>)
}