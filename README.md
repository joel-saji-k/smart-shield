# SmartShield: Insurance Management System

**SmartShield** is a comprehensive web-based insurance management platform that streamlines the operations of insurance companies, agents, and clients. Designed for clarity and efficiency, the system handles policy creation, agent onboarding, client management, claim processing, and more.

---

## Features

### Admin Panel

* Manage insurance companies, agents, and clients
* Monitor policy issuance and claims
* Handle maturity and death claim approvals

### Company Panel

* Register and manage agents
* Create and manage policy types
* View client portfolios

### Agent Panel

* Register clients under one or multiple companies
* Assign and issue policies
* Request claim approvals on behalf of clients

### Client Panel

* View and manage active policies
* Submit premium payments
* Request death or maturity claims

---

## Technologies Used

* **Frontend**: Angular
* **Backend**: ASP.NET Core Web API
* **Database**: Microsoft SQL Server
* **ORM**: Entity Framework Core

---

## System Architecture

* Modular architecture separating concerns for each actor (Admin, Company, Agent, Client)
* Uses Entity Framework for database operations and business logic
* Secure authentication and role-based access management

---

## How to Run

1. Clone the repository
2. Configure the database connection string in `appsettings.json`
3. Run the backend using Visual Studio or CLI:

   ```
   dotnet run
   ```
4. Navigate to the Angular project directory and start the frontend:

   ```
   npm install
   ng serve
   ```
5. Access the app at `http://localhost:4200`

---

## Credentials (Sample Data)

**Admin Login**

* Username: `admin`
* Password: `admin`



## Screenshots

![image](https://github.com/user-attachments/assets/bb7ca995-dfd3-4b5c-baef-3d63b2a958b6)
![image](https://github.com/user-attachments/assets/650c9b70-7b7e-4650-9908-230b1a47864a)
![image](https://github.com/user-attachments/assets/fd6af858-013e-4025-adcb-05134edd13aa)
![image](https://github.com/user-attachments/assets/38faa676-dc92-462d-9a67-6afd21de24f5)
