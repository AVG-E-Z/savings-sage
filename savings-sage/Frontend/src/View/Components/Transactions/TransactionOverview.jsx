import React from 'react';

function TransactionOverview({setIsNewBeingAdded, allAccounts}) {
    return (<>
            {allAccounts.length === 0 ? (<p>You do not have an account. You need to create an account before you can add transactions.</p>):
                (<button className="mainButton" onClick={() => setIsNewBeingAdded(true)}>Add new transaction</button>)}
    </>);
}

export default TransactionOverview;