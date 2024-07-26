import styled from "styled-components";
import {useEffect, useState} from "react";
import {
    Card,
    CardSubTitle,
    CardSubTitleKey,
    CardSubTitleValue,
    CardTitle
} from "../../../Styles/StyledComponents/AccountCard.js";
import {
    CardSubAccount,
    CardSubAccountKey,
    CardSubAccountValue
} from "../../../Styles/StyledComponents/AccountCard-SubAccount.js";
import {useAuth} from "../../../Authentication/AuthProvider.jsx";
import AccountTransactions from "./AccountTransactions.jsx";

export default function AccountOverview({account}){
    const { user } = useAuth();
    const [ transactions, setTransactions ] = useState([]);
    
    useEffect(() => {
        async function fetchAccountTransactions(){
            try {
                const response = await fetch(`api/Transaction/GetAll/Account/${account.id}`);
                const data = await response.json();
                console.log(data)
                setTransactions(data);
            } catch(err){
                console.error("Error fetching accounts: " + err)
            }
        }
        fetchAccountTransactions();
    }, []);

    
    useEffect(() => {
        if(account){
            console.log(account)
        }
    }, [account]);
    
    return (
        <Card>
            <CardTitle><h3>{account.name}</h3><p>{account.type}</p></CardTitle>
            <CardSubTitle>
                <CardSubTitleKey>Balance:</CardSubTitleKey>
                <CardSubTitleValue>{account.amount.toFixed(2)} {account.currency}</CardSubTitleValue></CardSubTitle>
            {account.subAccounts.length > 0 && 
                (<CardSubAccount>
                    {account.subAccounts.map((sAcc, i) => <>                
                        <CardSubAccountKey key={`${sAcc.parentAccountId}k${i}`}>{sAcc.name.replace('Interest', 'Int.').replace('Capital', 'Cap.')}</CardSubAccountKey>
                        <CardSubAccountValue key={`${sAcc.parentAccountId}v${i}`}>{sAcc.amount.toFixed(2)} {sAcc.currency}</CardSubAccountValue></> )}
                </CardSubAccount>)}
            {transactions.length > 0 &&  transactions.map((t, i) => <AccountTransactions key={t.name+i} transaction={t}/>)  }
        </Card>)
}