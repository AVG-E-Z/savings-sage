import Header from "../Features/Header.jsx";
import NavbarSide from "../Nav/NavbarSide.jsx";
import Footer from "../Features/Footer.jsx";
import {Outlet} from "react-router-dom";
import styled from "styled-components";

const ContainerOfEverything = styled.div`
  display: flex;
    margin-left: 10px;
    margin-top: 1vh;
    margin-bottom: 2vh;
`;

const ContainerOfContent= styled.div`
    background-color: rgba(255, 255, 255, 0.67);
    margin-right:2vw;
    margin-left: 10px;
    margin-top: 2vh;
    margin-bottom: 1px;
    padding: 20px;
    border-radius: 25px;
    align-items: center;
    height: calc(100vh - 5vh);
`
export default function Layout(){
    return (
        <><ContainerOfEverything>
           <Header/> 
        <NavbarSide/>
            <ContainerOfContent><Outlet/></ContainerOfContent>
            <Footer/>
        </ContainerOfEverything>
        </>
    )
}