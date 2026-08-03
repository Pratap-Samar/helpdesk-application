# Help Desk Ticket Management System

A complete Help Desk Ticket Management System built with ASP.NET Core, featuring a Web API, MVC frontend, and unit tests.

## Solution Structure

- **HelpDesk.Api** - ASP.NET Core Web API with Entity Framework Core and Repository Pattern
- **HelpDesk.Mvc** - ASP.NET Core MVC application consuming the Web API through a Service Layer
- **HelpDesk.Tests** - xUnit test project with Moq-based unit tests (coming in Phase 3)

## Features Implemented

### Phase 1 - Web API (Completed)
- ✅ ASP.NET Core Web API with Entity Framework Core
- ✅ SQL Server Database with Migrations (LocalDB)
- ✅ Repository Pattern implementation
- ✅ Ticket entity with full CRUD operations
- ✅ RESTful endpoints for ticket management
- ✅ Filter tickets by status

### Phase 2 - MVC Application (Completed)
- ✅ ASP.NET Core MVC consuming Web API via HttpClient Service Layer
- ✅ Dashboard with Total/Open/Closed ticket counts
- ✅ All Tickets listing with Priority/Status badges
- ✅ Ticket Details view
- ✅ Create Ticket form (Priority dropdown, Status=Open hardcoded)
- ✅ Edit Ticket form (Title, Description, Priority dropdown, Status dropdown)
- ✅ Delete Ticket confirmation
- ✅ Filter Tickets by Status (Open/In Progress/Closed)
- ✅ Navigation links in layout

### Phase 3 - Unit Testing (Pending)
- [ ] xUnit test project with Moq
- [ ] Repository layer mocking
- [ ] Controller unit tests

## Technology Stack

- **Framework**: ASP.NET Core (.NET 10)
- **Database**: SQL Server (LocalDB) with Entity Framework Core
- **Architecture**: Repository Pattern, Service Layer
- **Testing**: xUnit, Moq (planned)
- **Frontend**: Bootstrap 5, Razor Views

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/ticket` | Get all tickets |
| GET | `/api/ticket/{id}` | Get ticket by ID |
| POST | `/api/ticket` | Create new ticket |
| PUT | `/api/ticket/{id}` | Update ticket |
| DELETE | `/api/ticket/{id}` | Delete ticket |
| GET | `/api/ticket/status/{status}` | Filter tickets by status |

## MVC Routes

| Route | Description |
|-------|-------------|
| `/` | Dashboard |
| `/Tickets` | All Tickets |
| `/Tickets/Create` | Raise New Ticket |
| `/Tickets/Details/{id}` | Ticket Details |
| `/Tickets/Edit/{id}` | Edit Ticket |
| `/Tickets/Delete/{id}` | Delete Ticket |
| `/Tickets/Filter` | Filter by Status |

## Getting Started

### Prerequisites
- .NET 10 SDK
- SQL Server LocalDB (included with Visual Studio)

### Running the Application

1. **Start the API** (Terminal 1):
   ```bash
   dotnet run --project HelpDesk.Api --urls http://localhost:5210
   ```

2. **Start the MVC App** (Terminal 2):
   ```bash
   dotnet run --project HelpDesk.Mvc --urls http://localhost:5257
   ```

3. **Access the application**:
   - Web UI: http://localhost:5257
   - API: http://localhost:5210/api/ticket

## Database

The application uses SQL Server LocalDB with automatic migrations. Database name: `HelpDeskDb`

## Status

**Phase 1 & 2 Complete** - Ready for Phase 3 (Unit Testing)

---

*Generated as part of Help Desk Ticket Management System assignment*