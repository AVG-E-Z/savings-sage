import React, {useEffect, useState} from 'react';
import {useAuth} from "../../../Authentication/AuthProvider.jsx";
//import 'src/View/Pages/Transactions/transactions.css';
function AddNewTransaction() {
    const { user } = useAuth();
    const [allCategories, setAllCategories] = useState([]);
    const [chosenCategoryId, setChosenCategoryId] = useState(null);
    const [direction, setDirection] = useState(null);
    const [isRecurring, setIsRecurring] = useState(false);
    const [type, setType] = useState(0); //which means "payment" on the backend
    const [siblingTransaction, setSiblingTransaction] = useState(null);

    useEffect(() => {
        async function fetchCategories(){
            try {
                const response = await fetch(`api/Category/GetAll/${user.username}`);
                const data = await response.json();
                setAllCategories(() => data);
                

            } catch(err){
                console.error("Error fetching categoriess: " + err)
            }
        }
        fetchCategories();
    }, []);

    useEffect(() => {
       console.log(allCategories);
    }, [allCategories]);
    
    
    
    
    return (
        // <div className='iconsDiv'>
        //     {allCategories.map(cat => <img src={cat.iconURL}>)}
        // </div>
  <>
  </>  );
}

export default AddNewTransaction;