# LoanApp

LoanApp is a simple loan application management system built with .NET 9, Entity Framework Core, MediatR, and Blazor Server.
It supports creating, viewing, editing, approving, rejecting, and deleting loan applications with pagination.

# Getting Started

Follow these steps to clone and run the project locally.

1. Clone the Repository: open git bash or terminal and run
   -     git clone https://github.com/nskalu/loanapp_.git


Make sure you have the following installed on your computer:

- .NET 9 SDK

- SQL Server or any SQL-compatible DB 
  - if you want to use a remote SQL database, then update the credentials in the
    -  \loan_app\loanapp.ui\loanapp.ui\appsettings.json
    -  and \loan_app\loanapp.data\LoanAppContextFactory.cs files

 - Visual Studio 2022 or 2019

# Restore NuGet Packages

After cloning the repository, restore all NuGet packages required for the solution.

Option 1: Using .NET CLI

From the solution root, run:

  -      dotnet restore

Option 2: Using Visual Studio

  - Open the solution in Visual Studio.

  - Right-click the solution in Solution Explorer → select Restore NuGet Packages.

# Apply Database Migrations

This is inportant to make sure the database and database table gets created, you can use any of these commands to apply the migrations

  Option 1: Using .NET CLI (PowerShell / Terminal)
  - Run the following command from the solution root; 
      -     dotnet ef database update --project loanapp.data --startup-project loanapp.ui
  Option 2: Using Visual Studio Package Manager Console (PMC)

   - Open Package Manager Console in Visual Studio (Tools > NuGet Package Manager > Package Manager Console).

  - Set the Default Project to loanapp.data. (You should revert back to loanapp.ui as your start-up project after running the following)
    -      Update-Database
   
  # Features

  1. Create new loan applications

  2. View loan applications with pagination (Next and Previous buttons are pretty functional)

  3. Edit and delete applications

  4. Approve or reject loans
     
  5. Validation for Loan Amount, Loan Term, and Interest Rate

