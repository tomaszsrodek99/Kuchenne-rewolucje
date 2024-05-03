# Recipes blog

This is a website for sharing recipes with friends. 

## Introduction

The GreenGrass web application was built using the ASP.NET framework. The project aims to allow users to manage plants and receive notifications regarding watering and fertilizing plants.
Each user has access to their plant database. They specify basic conditions for growing each plant, and the application helps them keep all data organized while reminding them through notifications of specific actions that need to be taken.

## How to run?
1. Download the project from GitHub
2. Make sure you have a database named KuchenneRewolucje
3. Change the connection string in the configuration file and DBContext to your own if it is need
4. If there is a conflict with other projects on the same ports, change the ports in the launchSettings.json files
5. Execute the database migration by entering the following commands in the Package Manager Console: Add-Migration Init (if is not exist) and Update-Database
6. Launch application

## Features

- Exchange recipes with friends.

- Ability to add comments and rate recipes.

- The application features a login and registration system based on JWT tokens. It allows users to create an account, log in to the system, and manage their account data.

## Technologies 
- Framework ASP.NET 8.0
- MSSQL Server
- HTML, JS, CSS
- Entity Framework Core
- LINQ
- AutoMapper
- SkiaSharp



