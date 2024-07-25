import React, {useEffect, useState} from 'react';
import AddNewTransaction from "../../Components/Transactions/AddNewTransaction.jsx";
import EditTransaction from "../../Components/Transactions/EditTransaction.jsx";
import TransactionOverview from "../../Components/Transactions/TransactionOverview.jsx";
import {useAuth} from "../../../Authentication/AuthProvider.jsx";

export default function Transactions() {
    const { user } = useAuth();
    const [isNewBeingAdded, setIsNewBeingAdded] = useState(false);
    const [isEditModeOn, setIsEditModeOn] = useState(false);
    const [allAccountIdsForUser, setAllAccountIdsForUser] = useState([]);
    const [allTransactions, setAllTransactions] = useState([]);
    const [filterRules, setFilterRules] = useState(null);

    // useEffect(() => {
    //     async function fetchAccountIds(){
    //         try {
    //             const response = await fetch(`api/u/${user}/ha/IdList`);
    //             const data = await response.json();
    //             setAllAccountIdsForUser(() => data);
    //
    //         } catch(err){
    //             console.error("Error fetching bankaccounts: " + err)
    //         } 
    //     }
    //    fetchAccountIds();
    // }, []);
    //
    // useEffect(() => {
    //     async function fetchAllTransactions(){
    //         try {
    //             const bodyToSend = {
    //                 accountIds: allAccountIdsForUser
    //             }
    //             const response = await fetch(`api/transaction/GetAll/ForAllUserAccounts`, {method: "POST", headers: {'Content-Type': 'application/json'}, body: JSON.stringify(bodyToSend)});
    //             const data = await response.json();
    //             setAllTransactions(() => data);
    //
    //         } catch(err){
    //             console.error("Error fetching bankaccounts: " + err)
    //         }
    //     }
    //     fetchAllTransactions();
    // }, [allAccountIdsForUser]);
    
    //valami ilyesmi struktúrában lesz a filterrules, és ezt fogja a function használni
    // {
    //     owner: "Someone", 
    //     startDate: "SomeDate",
    //     endDate: "SomeDate",
    //     category: "SomeCategory",
    //     bankAccount: "BankAccountId"
    // }
    //
    function FilterRules(transactions){
        return transactions;
    }
    
    return (
       <> {
           isNewBeingAdded ?  <AddNewTransaction allAccountIdsForUser={allAccountIdsForUser}/> : isEditModeOn ? <EditTransaction/> : <TransactionOverview allTransactions={FilterRules(allTransactions)} setIsNewBeingAdded={setIsNewBeingAdded} setIsEditModeOn={setIsEditModeOn} setFilterRules={setFilterRules}/>
       }
       </>
    );
}

