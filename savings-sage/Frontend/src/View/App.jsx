import {useEffect} from 'react';
import Logo from "./Components/Logo.jsx";
//this is a test push
function App() {

    useEffect(() => {
        async function fetchData(){
            try{
                const response =  await fetch("api/Dummy");
                console.log(response);
                return await response.json();
            } catch(err){
                console.error(err);
            }  
        }
       fetchData().then(r => console.log(r));
    }, []);
  
    return (
    <div className={"main"}>
        <div className={"logoNavbarResize"}>
            <Logo/>
        </div>
        <div>
            <h1>My React app</h1>
        </div>
    </div>
  )
}

export default App
