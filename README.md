# Advanced EF Core Web API

A practical ASP.NET Core Web API project demonstrating how Entity Framework Core can be used to manage product data with reliable querying, updates, and deletion.

## Project Overview

This application provides a simple product management API backed by SQL Server. It focuses on several important EF Core capabilities, including efficient read operations, pagination, soft deletion, and optimistic concurrency.

## Technology Stack

- C# and .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- Microsoft SQL Server LocalDB
- EF Core Migrations

## Implemented Features

### Product Retrieval

The API retrieves products using `AsNoTracking()` to avoid unnecessary change tracking. Results are ordered and returned in pages using `Skip()` and `Take()`.

### Soft Deletion

Products are not physically removed from the database when deleted. Instead, the `IsDeleted` flag is updated, and a global query filter excludes deleted records from normal queries.

### Concurrency Management

The `RowVersion` property helps detect conflicting updates. If a product has changed since the client last retrieved it, the API returns HTTP `409 Conflict`.

### Database Configuration

The EF Core model includes:

- An index on the product name
- A global query filter for deleted products
- Decimal precision for product prices
- Row-version concurrency configuration
- A relationship between products and categories

## API Routes

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/products` | Retrieve paginated products |
| PUT | `/api/products/{id}` | Update a product |
| DELETE | `/api/products/{id}` | Soft-delete a product |

Pagination parameters: `pageNumber` and `pageSize`.

## Running the Application

**1. Restore dependencies**

```bash
dotnet restore
```

**2. Apply database migrations**

```bash
dotnet ef database update
```

**3. Build the project**

```bash
dotnet build
```

**4. Start the API**

```bash
dotnet run
```

The application requires a working SQL Server LocalDB instance and a matching connection string in `appsettings.json`.

## Testing Scenarios

The API can be tested using Postman or another HTTP client.

Suggested checks include:

- Retrieve products using different page numbers
- Update a product using its current row version
- Submit an outdated row version and verify the `409 Conflict` response
- Delete a product and verify it no longer appears in standard GET results

## Learning Outcomes

This project demonstrates practical techniques for working with EF Core in a REST API, particularly around query performance, data consistency, and logical deletion.

## License

Created for educational and practical learning purposes.