# Advanced EF Core Web API

An ASP.NET Core Web API project demonstrating advanced Entity Framework Core techniques, including efficient querying, pagination, soft deletion, change tracking, and optimistic concurrency.

## Technologies Used

- C# / .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server LocalDB
- LINQ
- Git & GitHub

## Features

### 1. Optimized Product Retrieval
- Uses `AsNoTracking()` for read-only queries.
- Uses `Select()` to retrieve only the required fields.
- Supports pagination using `Skip()` and `Take()`.
- Retrieves category names in a single query, avoiding the N+1 query problem.

### 2. Database Indexing
- Creates an index on the product `Name` column to support efficient searching.

### 3. Soft Delete
- Uses an `IsDeleted` property instead of permanently deleting products.
- Applies a global query filter using `HasQueryFilter()` to exclude deleted products from normal queries.

### 4. Change Tracking
- Updates existing products using EF Core change tracking.
- Restricts updates to the product name and price.

### 5. Optimistic Concurrency
- Uses SQL Server `RowVersion` as a concurrency token.
- Detects conflicting updates using `DbUpdateConcurrencyException`.
- Returns HTTP `409 Conflict` when a product has been modified by another user.

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/products` | Retrieve products |
| GET | `/api/products?pageNumber=1&pageSize=10` | Retrieve paginated products |
| PUT | `/api/products/{id}` | Update product name and price |
| DELETE | `/api/products/{id}` | Soft-delete a product |

## Example GET Response

```json
[
  {
    "id": 1,
    "name": "Laptop",
    "price": 27000,
    "categoryName": "Electronics",
    "rowVersion": "AAAAAAAAJxE="
  }
]
```

The `rowVersion` value is an example and changes whenever the product is updated.

## Example PUT Request

**Endpoint:**

`PUT /api/products/1`

**Request body:**

```json
{
  "name": "Laptop",
  "price": 27000,
  "rowVersion": "AAAAAAAAJxE="
}
```

The request must include the current `rowVersion` returned by the GET endpoint.

## Running the Project

### 1. Clone the Repository

```bash
git clone https://github.com/mogamal1111/AdvancedEFCoreApi.git
cd AdvancedEFCoreApi
```

### 2. Requirements

- .NET 10 SDK
- SQL Server LocalDB
- Entity Framework Core CLI tools

### 3. Configure the Database

Update the connection string in `appsettings.json` to match your SQL Server instance.

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\TestEFCore;Database=AdvancedEFCoreDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 4. Apply Database Migrations

```bash
dotnet ef database update
```

### 5. Run the Application

```bash
dotnet run
```

If Windows Application Control blocks the generated executable, the application can also be started using the .NET host:

```powershell
dotnet .\bin\Debug\net10.0\AdvancedEFCoreApi.dll
```

Use the HTTP address displayed in the terminal to access the API.

## Database Structure

**Category**
- Id
- Name

**Product**
- Id
- Name
- Price
- CategoryId
- IsDeleted
- RowVersion

## Testing

The following scenarios were tested successfully:

- Retrieving products with category names.
- Paginating product results.
- Updating product prices.
- Detecting outdated RowVersion values and returning HTTP 409.
- Soft-deleting products and excluding them from subsequent GET responses.

## Author

GitHub: [mogamal1111](https://github.com/mogamal1111)