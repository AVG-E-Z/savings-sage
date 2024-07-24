import React, { useEffect, useState } from 'react';
import { useAuth } from "../../../Authentication/AuthProvider.jsx";
import '../../Pages/Transactions/transactions.css';

function AddNewTransaction() {
    const { user } = useAuth();
    const [allCategories, setAllCategories] = useState(null);
    const [isLoading, setLoading] = useState(true);
    const [chosenCategoryId, setChosenCategoryId] = useState(null);
    const [direction, setDirection] = useState(null);
    const [isRecurring, setIsRecurring] = useState(false);
    const [type, setType] = useState(0); //which means "payment" on the backend
    const [siblingTransaction, setSiblingTransaction] = useState(null);

    useEffect(() => {
        async function fetchCategories() {
            try {
                const response = await fetch(`api/Category/GetAll/${user.username}`);
                const data = await response.json();
                setAllCategories(data); 
            } catch (err) {
                console.error("Error fetching categories: " + err);
            } finally {
                setLoading(false); 
            }
        }
        fetchCategories();
    }, [user.username]);

    if (isLoading) {
        return <div>Loading...</div>;
    }
    
    const categories = allCategories?.$values || [];

    return (
        <><h2>Please choose one from the available categories:</h2>
    <div className='iconsDiv'>
    {categories.map(cat => (
                <div className="categoryDiv" onClick={()=> setChosenCategoryId(cat.id)}>
                    <img key={cat.id} src={cat.iconURL} alt={cat.name} />
               <p className="categoryName">{cat.name}</p>
                </div>
            ))}
        </div></>
    );
}

export default AddNewTransaction;
