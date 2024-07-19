import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import '../Styles/index.css'
import {AuthProvider} from "../Authentication/AuthProvider.jsx";

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
      <AuthProvider>
    <App />
      </AuthProvider>
  </React.StrictMode>,
)
