### CountriesApi Documentation

#### Overview
This project is built using Clean Architecture with CQRS and the Mediator pattern, along with Entity Framework for database management and FluentValidation. It also includes an exception middleware for handling exceptions, Serilog for logging, and a Unit of Work pattern to commit changes to the database. Additionally, caching is used within a pipeline behavior with cache invalidation implemented, and health checks are included.

#### Getting Started
To initialize the database using Entity Framework, run the following command from the NuGet Package Manager Console:
```
update-database
```

#### Connection String
The following ConnectionString is used in appsettings.json and it was used to connect to local MSSQL Database (appsettings.json or similar):
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=CountriesDb;Trusted_Connection=True;Encrypt=false;"
}
```
You can use this connection string to connect to the database via Microsoft SQL Server Management Studio.

#### Cloud Redis
This project uses cloud Redis for caching. Ensure that your Redis server is properly configured and the connection string is set in your configuration file.

#### Advanced Patterns
- **Clean Architecture**: The project follows Clean Architecture principles to ensure separation of concerns and maintainability.
- **CQRS and Mediator Pattern**: The project utilizes cQRS (Command Query Responsibility Segregation) and the Mediator pattern for handling commands and queries.
- **Exception Middleware**: Custom middleware is implemented to handle exceptions globally.
- **Serilog**: Serilog is used for logging throughout the application.
- **Unit of Work**: The Unit of Work pattern is used to manage database transactions and ensure data integrity.
- **Caching**: Caching is implemented within a pipeline behavior, and cache invalidation is also handled.

#### Health Checks
Health checks are implemented to monitor the application's health and ensure it is running as expected.
