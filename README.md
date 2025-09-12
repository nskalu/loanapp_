# LoanApp

LoanApp is a simple loan application management system built with .NET 9, Entity Framework Core, MediatR, and Blazor Server.
It supports creating, viewing, editing, approving, rejecting, and deleting loan applications with pagination.

# Getting Started

Follow these steps to clone and run the project locally.

1. Clone the Repository: git clone https://github.com/nskalu/loanapp_.git
2. Prerequisites

Make sure you have the following installed on your computer:

- .NET 9 SDK

- SQL Server or any SQL-compatible DB 
  - if you want to use a remote SQL database, then update the credentials in the
    -  \loan_app\loanapp.ui\loanapp.ui\appsettings.json
    -  and \loan_app\loanapp.data\LoanAppContextFactory.cs files

 - Visual Studio 2022 or 2019

#Apply Database Migrations

you can use any of these commands to apply the migrations

 1: Using .NET CLI (PowerShell / Terminal)

    Run the following command from the solution root; 
      dotnet ef database update --project loanapp.data --startup-project loanapp.ui
