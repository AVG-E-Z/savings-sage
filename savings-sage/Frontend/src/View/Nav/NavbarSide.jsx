import Logo from "../Components/Design/Logo.jsx";
import {useAuth} from "../../Authentication/AuthProvider.jsx";
import {useNavigate} from "react-router-dom";
import "./navbar.css";

export default function NavbarSide() {
    const { logout, auth} = useAuth();
    const navigate = useNavigate();
    
    
    
    return <><div className="navBarContainer">
        <div className={"logoNavbarResize"}><Logo/></div>
        {auth.isAuthenticated
            ? <>
                <div id="homeDiv" className="navbarDiv" onClick={() => navigate("/homepage")}>
                    <img src={"/icons/home.svg"}/>
                    <p id="homeDivText" className="navbarDivText">Home</p>
                </div>
                <div id="transactionsDiv" className="navbarDiv" onClick={() => navigate("/transactions")}>
                    <img src={"/icons/history.svg"}/>
                    <p id="transactionsDivText" className="navbarDivText">Transactions</p>
                </div>
                <div id="savingsDiv" className="navbarDiv" onClick={() => navigate("/savings")}>
                    <img src={"/icons/savings.svg"}/>
                    <p id="savingsDivText" className="navbarDivText">Savings</p>
                </div>
                <div id="budgetsDiv" className="navbarDiv" onClick={() => navigate("/budgets")}>
                    <img src={"/icons/budget.svg"}/>
                    <p id="budgetsDivText" className="navbarDivText">My budgets</p>
                </div>
                <div id="statsDiv" className="navbarDiv" onClick={() => navigate("/statistics")}>
                    <img src={"/icons/statistics.svg"}/>
                    <p id="statsDivText" className="navbarDivText">Statistics</p>
                </div>
                <div id="settingsDiv" className="navbarDiv" onClick={() => navigate("/settings")}>
                    <img src={"/icons/settings.svg"}/>
                    <p id="settingsDivText" className="navbarDivText">Settings</p>
                </div>

                <div id="logoutDiv" className="navbarDiv" onClick={() => logout()}>
                    <img src={"/icons/logout.svg"}/>
                    <p id="logoutDivText" className="navbarDivText">Logout</p>
                </div>
            </>
            : <>
                <div id="aboutUsDiv" className="navbarDiv" onClick={() => navigate("/about-us")}>
                    <img src={"/icons/info.svg"}/>
                    <p id="aboutUsDivText" className="navbarDivText">About us</p>
                </div>
                <div id="loginDiv" className="navbarDiv" onClick={() => navigate("/login")}>
                    <img src={"/icons/login.svg"}/>
                    <p id="loginDivText" className="navbarDivText">Login</p>
                </div>
                <div id="registerDiv" className="navbarDiv" onClick={() => navigate("/register")}>
                    <img src={"/icons/newuser.svg"}/>
                    <p id="registerDivText" className="navbarDivText">Sign up</p>
                </div>
            </>
        }</div>
    </>
}