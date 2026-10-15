# Simple User Management API

A learning project for the peer-graded assignment **Building a Simple API with Copilot**. It demonstrates CRUD endpoints, request validation, duplicate-email checks, request logging, and centralized exception handling.

## Requirements

- .NET 8 SDK
- Optional: Visual Studio, Visual Studio Code, or another C# editor

## Run the project

From this directory, run:

```bash
dotnet restore
dotnet run
```

Open the Swagger UI URL printed in the console. In Development, it is normally available at `/swagger`.

## Endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/users` | List all users |
| GET | `/api/users/{id}` | Retrieve one user |
| POST | `/api/users` | Create a user |
| PUT | `/api/users/{id}` | Update a user |
| DELETE | `/api/users/{id}` | Delete a user |

### Example POST body

```json
{
  "name": "Aiman Iskandar",
  "email": "aiman@example.com",
  "age": 28
}
```

Validation rules include a required name (2–100 characters), a valid email address, and an age from 1 to 120. The API rejects duplicate email addresses.

## Implementation notes

- `UsersController.cs` implements the CRUD endpoints.
- `CreateUserRequest.cs` and `UpdateUserRequest.cs` define validation rules.
- `InMemoryUserService.cs` stores data in memory for a simple demonstration. Data is lost when the application restarts; this is not production persistence.
- `RequestLoggingMiddleware.cs` records the HTTP method, path, status code, and elapsed time.
- `ExceptionHandlingMiddleware.cs` logs unhandled exceptions and returns a generic HTTP 500 response without exposing exception details.

