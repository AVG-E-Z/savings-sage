# Savings Sage 
Savings Sage is a budgeting webapp, that lets you track your finances via creating and customizing bank accounts, transactions and categories. This application was created as a part of Codecool's Fullstack Web Developer course and is still under development by the two team members. There are many more features planned for the near future (see more about these below).
# Created by
<a href="https://github.com/AVG-E-Z">
  <img src="https://img.shields.io/badge/github_organization-AVG(E%2CZ)-blue?logo=github" alt="Static Badge">
</a>
<a href="https://github.com/JeanetteMoKa">
  <img src="https://img.shields.io/badge/github-JeanetteMoKa-purple?logo=github" alt="Static Badge">
</a>
<a href="https://github.com/kveszti">
  <img src="https://img.shields.io/badge/github-kveszti-lightblue?logo=github" alt="Static Badge">
</a>  

# Table Of Contents
- [Used technologies](#used-technologies)  
- [Features](#features)  
- [Installation](#installation)   
- [Usage](#usage) 
# Used technologies  
![Static Badge](https://img.shields.io/badge/ASP.NET-red?logo=.net) ![Static Badge](https://img.shields.io/badge/C%23-red?logo=c%23) ![Static Badge](https://img.shields.io/badge/Entity%20Framework-red?logo=dotnet%20entity) ![Static Badge](https://img.shields.io/badge/Identity-red?logo=identity)



![Static Badge](https://img.shields.io/badge/React-blue?logo=react) ![Static Badge](https://img.shields.io/badge/Javascript-blue?logo=javascript)
 ![Static Badge](https://img.shields.io/badge/Vite-blue?logo=vite) ![Static Badge](https://img.shields.io/badge/NPM-blue?logo=npm)


 ![Static Badge](https://img.shields.io/badge/PostgreSQL-black?logo=postgresql) ![Static Badge](https://img.shields.io/badge/Docker-black?logo=docker)

 


# Features  
Deployed features: 
- Bank account creation and overview
- Transaction creation

Planned features: 
- Transaction overview
- Statistics page
- Cutomizable categories
- Customizable homepage with widgets
- Saving goals creation
- Budget threshold creation for categories
# Installation   
If you'd like to take the 'easy' way, we have a deployed version you can reach [here]( https://savings-sage-latest.onrender.com/).

If you'd like to install and run the development version, you're going to need to take the following steps:
1. Prerequisites:
   - Backend software and package versions:
      - .NET 8.0 SDK
      - Aspire.Npgsql.EntityFrameworkCore.PostgreSQL	^8.1.0
      - Currencyapi	^1.0.2
      - DotNetEnv	^3.0.0	
      - Microsoft.AspNetCore.Authentication.JwtBearer	^8.0.7	
      - Microsoft.AspNetCore.Identity.EntityFrameworkCore	^8.0.7	
      - Microsoft.AspNetCore.OpenApi ^8.0.2	
      - Microsoft.EntityFrameworkCore	^8.0.7
      - Microsoft.EntityFrameworkCore.Design	^8.0.7 
      - Microsoft.EntityFrameworkCore.InMemory	^8.0.7	
      - Npgsql	^8.0.3	
      - Npgsql.EntityFrameworkCore.PostgreSQL	^8.0.4	
      - Swashbuckle.AspNetCore	^6.4.0	
      - System.Text.Json	^9.0.0-preview.5.24306.7	
   - Frontend software and package versions:
      - Node.js ^22.2.0
      - Npm ^10.7.0
      - Axios	^1.7.2
      - React	^18.2.0
      - React-dom	^18.2.0
      - React-router-dom	^6.25.1
      - Styled-components	^6.1.12

Clone the repository and download the above packages. 

Then set up your .env file for the following variables: 
- here there are gonna be the variables, this should be done by friday

After that, you can start the application with docker-compose (docker compose up), or starting the backend, frontend and db separately. 
Backend: dotnet run (in the savings-sage folder)
Frontend: npm run dev (in the Frontend folder)
Db: start your local postgres server and use your connection string in the .env 
    
    

# Usage 
1. Register an account
2. Log in
3. Create an account
4. Create transactions




