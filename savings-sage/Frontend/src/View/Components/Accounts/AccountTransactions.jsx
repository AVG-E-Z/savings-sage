import React, {useEffect, useState} from "react";
import {
    CategoryColorDiv,
    CategorySVG, TransactionDate,
    TransactionLine, TransactionNumber, TransactionText
} from "../../../Styles/StyledComponents/AccountCard-TransactionLine.js";
import {useAuth} from "../../../Authentication/AuthProvider.jsx";

export default function AccountTransactions({transaction}){
    const { user } = useAuth();
    const [ category, setCategory ] = useState([]);
    
    useEffect(() => {
        async function fetchCategory(){
            try {
                const response = await fetch(`api/Category/${user.username}/${transaction.categoryId}`);
                const data = await response.json();
                console.log(data)
                setCategory(data);
            } catch(err){
                console.error("Error fetching accounts: " + err)
            }
        }
        fetchCategory();
    }, []);
    
    // const dummyTransaction = {
    //     categoryColor: "#D3265D",
    //     categoryName: "Transportation",
    //     categoryIcon: "/icons/paw.svg",
    //     transactionName: "Zserbó nasi",
    //     transactionAmount: 15000000
    // }
    
    return <><TransactionLine>
        {category.color && (<CategoryColorDiv style={{backgroundColor: `#${category.color.hexadecimalCode}`}}>
            <CategorySVG src={category.iconURL}/>
        </CategoryColorDiv>)}
        <TransactionText>{transaction.name}</TransactionText>
        <TransactionNumber>{transaction.amount.toFixed(2)}</TransactionNumber>
    </TransactionLine>
        <TransactionDate>{transaction.date.slice(0,-9).replaceAll('-', '.')}.</TransactionDate></>
}