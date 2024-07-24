import styled from "styled-components";
import {useEffect, useState} from "react";

const Card = styled.div`
        background-color: white;
        width: 350px;
        height: 400px;
        border: #BEEDD1 3px solid;
        border-radius: 25px;
        box-shadow: 0 4px 4px rgba(0, 0, 0, 0.25);
        margin-top: 20px;
    `
const CardTitle = styled.h3`
        color: white;
        background-color: #60B879;
        font-size: 1.6rem;
        padding: 15px 0;
        text-align: center;
        width: 100%;
        border-radius: 22px 22px 0 0;
    `
const CardSubTitle = styled.div`
        width: 96%;
        border-bottom: black 2px solid;
        margin-bottom: 5px;
        display: grid;
        grid-template-columns: 30% 69%;
        column-gap: 1%;
        font-size: 1.3rem;
        padding: 5px;
        margin: auto;
    `
const CardSubTitleKey = styled.div`
    text-align: left;
    width: 100%;
`
const CardSubTitleValue = styled.div`
    text-align: right;
    width: 100%;
`
const TransactionLine = styled.div`
    width: 96%;
    margin-top: 5px;
    display: flex;
    justify-content: space-between;
    align-items: flex-end;
    font-size: 1.3rem;
    padding: 5px;
    margin: auto;
`

const CategoryColorDiv = styled.div`
    width: 30px;
    height: 30px;
    align-self: flex-start;
    border: transparent;
    border-radius: 7px;
    display: flex;
    justify-content: center;
`
const CategorySVG = styled.img`
    width: 85%;
    margin: auto;
    filter: invert();
`
const TransactionText = styled.div`
    font-size: 1.2rem;
    text-align: left;
    width: 50%;
`
const TransactionNumber = styled.div`
    font-size: 1.2rem;
    text-align: right;
    width: 36%;
`

export default function AccountOverview({account}){
    const [pathData, setPathData] = useState("");
    
    const dummyTransaction = {
        categoryColor: "#D3265D",
        categoryName: "Transportation",
        categoryIcon: "/icons/paw.svg",
        transactionName: "Zserbó nasi",
        transactionAmount: 15000000
    }
    
    return (
        <Card>
            <CardTitle>{account.name}</CardTitle>
            <CardSubTitle>
                <CardSubTitleKey>Balance:</CardSubTitleKey>
                <CardSubTitleValue>{account.amount.toFixed(2)} {account.currency}</CardSubTitleValue></CardSubTitle>
            {/*map transactions for accounts*/}
            <TransactionLine> 
                <CategoryColorDiv style={{ backgroundColor: `${dummyTransaction.categoryColor}` }} >
                    <CategorySVG src={"/icons/paw.svg"}/>
                </CategoryColorDiv>
                <TransactionText>{dummyTransaction.transactionName}</TransactionText>
                <TransactionNumber>{dummyTransaction.transactionAmount.toFixed(2)}</TransactionNumber>
            </TransactionLine>
        </Card>)
}