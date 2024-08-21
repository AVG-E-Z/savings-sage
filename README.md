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
If you'd like to take the 'easy' way, we have a deployed version you can reach [here]( https://savings-sage-latest.onrender.com/). This is a free render.com page, so you should experience slow responses from the server.

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

You're gonna need to setup an appsettings.Development.json on the backend to set your variables for the db connection and the jwt token settings. It should have the following structure, so the code can interpret it the right way:
```json
 "frontend_url": "here comes localhost and the port where your frontend is running",
  "ConnectionStrings": {
    "Default": "here comes your local postgres server connection string"
  },
  "Jwt": {
    "ValidIssuer": "here comes the valid issuer you'd like to set for the jwt token generation",
    "ValidAudience": "here comes the valid audience you'd like to set for the jwt token generation",
    "IssuerSigningKey": "here comes the issuer signing key you'd like to set for the jwt token generation"
  },
  "Roles": {
    "Admin": "here comes the name you'd like to use for admin roles",
    "User": "here comes the name you'd like to use for user roles"
  }
```

After that, you can start the application two different ways:
1. With docker-compose:
   - From the root folder: ```cd savings-sage```
   - In the terminal: ```docker compose up```
2. Running separately the frontend and the backend on your local machine:
   - Backend:
       - From the root folder: ```cd savings-sage/savings-sage```
       - In the terminal: ```dotnet run```
    - Frontend:
       - From the root folder: ```cd savings-sage/Frontend```
       - In the terminal: ```npm run dev```
    - Make sure your referenced database server is also running
     

# Usage 

1. Register an account
 
 
 ![image](https://github.com/user-attachments/assets/3bb0c749-fd78-4fad-948d-5d59611eefe2)

  

2. Log in
  
 ![image](https://github.com/user-attachments/assets/ae004ddd-7616-41ed-937c-761d7b05acdd)


  
3. Create an account
  
 ![image](https://github.com/user-attachments/assets/aa0c7bd8-64f9-4df6-939f-29bccb45c8c5)

  
  
4. Create transactions
  
 ![image](https://github.com/user-attachments/assets/52e53b23-c5ef-4b76-8cdb-37d20747e4a5)






