
import {BrowserRouter, Route, Routes} from "react-router-dom";
import Layout from "./Layout/Layout.jsx";
import ProtectedRoute from "../Authentication/ProtectedRoute.jsx";
import DummyRoute from "./Pages/DummyRoute.jsx";
import Login from "./Pages/Login/Login.jsx";
import Registration from "./Pages/Registration/Registration.jsx";
import Settings from "./Pages/Settings/Settings.jsx";
import Transactions from "./Pages/Transactions/Transactions.jsx";
import HomePage from "./Pages/HomePage/HomePage.jsx";
import AboutUs from "./Pages/AboutUs/AboutUs.jsx";

export default function App() {
  
    return (
    <BrowserRouter>
        <Routes>
            <Route path='/' element={<Layout/>}>
                <Route path='/dummy-route' element={<ProtectedRoute><DummyRoute /></ProtectedRoute>}></Route>
                <Route path='/about-us' element={<AboutUs />}></Route>
                <Route path='/login' element={<Login />}></Route>
                <Route path='/register' element={<Registration />}></Route>
                <Route path='/transactions' element={<Transactions />}></Route>
                <Route path='/settings' element={<Settings />}></Route>
                <Route path='/homepage' element={<HomePage />}></Route>
            
            </Route>
        </Routes>
    </BrowserRouter>
  )
}

