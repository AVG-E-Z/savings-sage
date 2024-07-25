import React from 'react';

function TransactionOverview({setIsNewBeingAdded}) {
    return (
        <button className="mainButton" onClick={()=> setIsNewBeingAdded(true)}>Add new transaction</button>
    );
}

export default TransactionOverview;