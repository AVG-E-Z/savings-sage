import styled from "styled-components";

export const Card = styled.div`
    background-color: white;
    min-width: 350px;
    max-width: 350px;
    height: 400px;
    border: #BEEDD1 3px solid;
    border-radius: 25px;
    box-shadow: 0 4px 4px rgba(0, 0, 0, 0.25);
    margin: 20px;
    position: relative;
    `

export const CardTitle = styled.div`
    color: white;
    background-color: #60B879;
    text-align: center;
    width: 100%;
    border-radius: 22px 22px 0 0;
    h3 {
        font-size: 1.6rem;
        padding: 6px 0 0 0;
    }
    p{
        font-size: 0.8rem; 
        padding-bottom: 5px;
    }    
    `

export const CardSubTitle = styled.div`
    width: 96%;
    border-bottom: black 2px solid;
    margin-bottom: 5px;
    display: grid;
    grid-template-columns: 30% 69%;
    column-gap: 1%;
    font-size: 1.3rem;
    padding: 2px 4px;
    margin: auto;
    `
export const CardSubTitleKey = styled.div`
    text-align: left;
    width: 100%;
`
export const CardSubTitleValue = styled.div`
    text-align: right;
    width: 100%;
`
export const CardButtonCont = styled.div`
    position: absolute;
    display: grid;
    grid-template-columns: 30% 30% 30%;
    column-gap: 3%;
    bottom: 5px;
    width: 94%;
    margin: 0 4%;
    font-size: 1.1rem;
`
export const CardButton = styled.div`
    background-color: #60B879 ;
    color:white;
    border-radius: 25px;
    width: 100%;
    height: 30px;
    border: none;
    text-align: center;
    padding-top: 0.15rem;
    
    &:hover {
        background-color: #66d385;
    }    
`
export const CardButtonDngr = styled.div`
    background-color: #60B879;
    color: white;
    border-radius: 25px;
    width: 100%;
    height: 30px;
    border: none;
    text-align: center;
    padding-top: 0.15rem;

    &:hover {
        background-color: #cc7e77;
    }
`