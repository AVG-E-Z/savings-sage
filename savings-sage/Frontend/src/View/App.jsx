
import {BrowserRouter, Route, Routes} from "react-router-dom";
import Layout from "./Layout/Layout.jsx";
import ProtectedRoute from "../Authentication/ProtectedRoute.jsx";
import Login from "./Pages/Login/Login.jsx";
import Registration from "./Pages/Registration/Registration.jsx";
import Settings from "./Pages/Settings/Settings.jsx";
import Transactions from "./Pages/Transactions/Transactions.jsx";
import HomePage from "./Pages/HomePage/HomePage.jsx";
import AboutUs from "./Pages/AboutUs/AboutUs.jsx";
import Statistics from "./Pages/Statistics/Statistics.jsx";
import Savings from "./Pages/Savings/Savings.jsx";
import BudgetManagement from "./Pages/BudgetManagement/BudgetManagement.jsx";
import AccountBalances from "./Pages/Accounts/AccountBalances.jsx";
import AddNewAccount from "./Components/Accounts/AddNewAccount.jsx";
import EditAccount from "./Components/Accounts/EditAccount.jsx";

export default function App() {
  
    return (
    <BrowserRouter>
        <Routes>
            <Route path='/' element={<Layout/>}>
                <Route path='/about-us' element={<AboutUs />}></Route>
                <Route path='/login' element={<Login />}></Route>
                <Route path='/register' element={<Registration />}></Route>
                <Route path='/account-balances' element={<ProtectedRoute><AccountBalances /></ProtectedRoute>}></Route>
                <Route path='/add-new-account' element={<ProtectedRoute><AddNewAccount /></ProtectedRoute>}></Route>
                <Route path='/edit-account' element={<ProtectedRoute><EditAccount /></ProtectedRoute>}></Route>
                <Route path='/transactions' element={<ProtectedRoute><Transactions /></ProtectedRoute>}></Route>
                <Route path='/settings' element={<ProtectedRoute><Settings /></ProtectedRoute>}></Route>
                <Route path='/homepage' element={<ProtectedRoute><HomePage /></ProtectedRoute>}></Route>
                <Route path='/statistics' element={<ProtectedRoute><Statistics /></ProtectedRoute>}></Route>
                <Route path='/savings' element={<ProtectedRoute><Savings /></ProtectedRoute>}></Route>
                <Route path='/budgets' element={<ProtectedRoute><BudgetManagement /></ProtectedRoute>}></Route>
            </Route>
        </Routes>
    </BrowserRouter>
  )
}

