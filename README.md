# TakeHome.API

Overview

The Packaging API is a RESTful service built with C# (.NET) and Microsoft SQL Server.
It provides endpoints for user authentication and product management, while supporting API versioning, JWT authentication, structured logging, and Swagger documentation.

The API integrates with the Packaging Database Schema from the database design assignment and exposes product data with different levels of detail depending on the API version.

# Tech Stack

* C# / .NET Web API
* Database
  * Microsoft SQL Server
* Authentication
  * JWT (JSON Web Token)
* Logging
  * Structured logging (Serilog / Microsoft.Extensions.Logging)
* API Documentation
  * Swagger / OpenAPI

# Project Setup
1. Clone the Repository
2. Configure Database - Update the connection string in appsettings.json
3. Users Table - Use Update Database comand to update existing Takehome Databse/PackagingDB
4. Run the API
