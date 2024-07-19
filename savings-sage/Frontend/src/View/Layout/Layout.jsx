import Header from "../Features/Header.jsx";
import NavbarSide from "../Nav/NavbarSide.jsx";
import Footer from "../Features/Footer.jsx";
import {Outlet} from "react-router-dom";
import styled from "styled-components";

const ContainerOfEverything = styled.div`
  display: flex;
    margin-left: 10px;
    margin-top: 40px;
    margin-bottom: 40px;
`;

const ContainerOfContent= styled.div`
    background-color: rgba(255, 255, 255, 0.67);
    margin:20px;
    padding: 20px;
    width: 75vw;
    height: 90vh;
    border-radius: 25px;
    align-items: center;
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