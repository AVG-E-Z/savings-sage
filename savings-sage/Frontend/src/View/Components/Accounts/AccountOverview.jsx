import styled from "styled-components";
import {useEffect, useState} from "react";
import {
    Card, CardButton, CardButtonCont, CardButtonDngr,
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
                setTransactions(data.sort((a,b)=> new Date(b.date) - new Date(a.date)).slice(0, 5));
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
                    {account.subAccounts.slice(0,3).map((sAcc, i) => <>                
                        <CardSubAccountKey key={`${sAcc.parentAccountId}k${i}`}>{sAcc.name.replace('Interest', 'Int.').replace('Capital', 'Cap.')}</CardSubAccountKey>
                        <CardSubAccountValue key={`${sAcc.parentAccountId}v${i}`}>{sAcc.amount.toFixed(2)} {sAcc.currency}</CardSubAccountValue></> )}                    
                </CardSubAccount>)}
            {transactions.length > 0 ? (
                account.subAccounts.length === 0 ? (
                    transactions.map((t, i) => <AccountTransactions key={t.name + i} transaction={t} />)
                ) : (
                    account.subAccounts.length <= 2 ? (
                        transactions.slice(0, 4).map((t, i) => <AccountTransactions key={t.name + i} transaction={t} />)
                    ) : (
                        transactions.slice(0, 3).map((t, i) => <AccountTransactions key={t.name + i} transaction={t} />)
                    )
                )
            ) : null}
            <CardButtonCont>
                <CardButton>Add sub</CardButton>
                <CardButton>Edit</CardButton>
                <CardButtonDngr>Delete</CardButtonDngr>
            </CardButtonCont>
        </Card>)
}