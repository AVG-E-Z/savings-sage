import styled from "styled-components";

export const TransactionLine = styled.div`
    width: 96%;
    margin: 5px 2% 0 2%;
    display: flex;
    justify-content: space-between;
    align-items: flex-end;
    font-size: 1.3rem;
    padding: 5px;
    `
export const CategoryColorDiv = styled.div`
    width: 30px;
    height: 30px;
    align-self: flex-start;
    border: transparent;
    border-radius: 7px;
    display: flex;
    justify-content: center;
    `
export const CategorySVG = styled.img`
    width: 22px;
    margin: auto;
    `
export const TransactionText = styled.div`
    font-size: 1.2rem;
    text-align: left;
    width: 50%;
    `
export const TransactionNumber = styled.div`
    font-size: 1.2rem;
    text-align: right;
    width: 36%;
    `
export const TransactionDate = styled.div`
    font-size: 0.8rem;
    text-align: right;
    width: 96%;
    line-height: 0;
    margin: 0 2%;
    padding-right: 5px;
`