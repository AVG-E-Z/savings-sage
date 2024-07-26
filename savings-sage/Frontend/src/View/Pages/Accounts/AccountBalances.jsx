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
                const response = await fetch(`api/Account/All/u/${user.username}`);
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
        navigate('/add-new-account', { state: { parentAccount: account, accounts } });
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
                                     onAddSubAccountClick={() => handleAddSubAccountClick(account)}/>)}
                </div>
            </>)
    }</>
    </div>)
}