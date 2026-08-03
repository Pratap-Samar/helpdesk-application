# Help Desk Ticket Management System

A complete Help Desk Ticket Management System built with ASP.NET Core, featuring a Web API, MVC frontend, and unit tests.

## Solution Structure

- **HelpDesk.Api** - ASP.NET Core Web API with Entity Framework Core and Repository Pattern
- **HelpDesk.Mvc** - ASP.NET Core MVC application consuming the Web API through a Service Layer
- **HelpDesk.Tests** - xUnit test project with Moq-based unit tests

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

### Phase 3 - Unit Testing (Completed)
- ✅ xUnit test project with Moq
- ✅ Repository layer mocking
- ✅ 13 unit tests covering all CRUD operations and filtering

### Phase 4 - Git & GitHub (Completed)
- ✅ Git repository with proper .gitignore
- ✅ Pushed to GitHub: https://github.com/Pratap-Samar/helpdesk-application

## Technology Stack

- **Framework**: ASP.NET Core (.NET 10)
- **Database**: SQL Server (LocalDB) with Entity Framework Core
- **Architecture**: Repository Pattern, Service Layer
- **Testing**: xUnit, Moq
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
- .NET 10 SDK (or .NET 8+)
- SQL Server LocalDB (included with Visual Studio / Visual Studio Build Tools)

### Running the Application

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Pratap-Samar/helpdesk-application.git
   cd helpdesk-application
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Apply database migrations** (creates LocalDB database):
   ```bash
   dotnet ef database update --project HelpDesk.Api
   ```

4. **Start the API** (Terminal 1):
   ```bash
   dotnet run --project HelpDesk.Api --urls http://localhost:5210
   ```
   API will be available at: http://localhost:5210/api/ticket

5. **Start the MVC App** (Terminal 2):
   ```bash
   dotnet run --project HelpDesk.Mvc --urls http://localhost:5257
   ```
   Web UI will be available at: http://localhost:5257

### Alternative: Run from Visual Studio
1. Open `HelpDeskManagement.slnx` in Visual Studio
2. Set **HelpDesk.Api** as startup project
3. Press F5 to run API
4. Right-click **HelpDesk.Mvc** → Debug → Start New Instance
5. Or configure multiple startup projects in Solution properties

## Testing the Application

### Manual Testing (via Browser)

1. **Open Dashboard**: http://localhost:5257/
   - Verify Total/Open/Closed ticket counts
   - Click navigation links to test all pages

2. **Create a Ticket**:
   - Navigate to "Raise Ticket" or http://localhost:5257/Tickets/Create
   - Fill in Title, Description, Category (Software/Hardware/Network), Priority (Low/Medium/High/Critical)
   - Status is auto-set to "Open"
   - Submit → Redirects to All Tickets list

3. **View All Tickets**: http://localhost:5257/Tickets
   - Table shows all tickets with colored badges for Priority/Status
   - Click Details/Edit/Delete actions

4. **Edit Ticket**: Click "Edit" on any ticket
   - Modify Title, Description, Category, Priority, Status (Open/In Progress/Closed)
   - Save → Redirects to All Tickets

5. **Filter by Status**: http://localhost:5257/Tickets/Filter
   - Select Status from dropdown (Open/In Progress/Closed)
   - Click Filter → Shows matching tickets

6. **Delete Ticket**: Click "Delete" → Confirm deletion

7. **Verify API Data**: http://localhost:5210/api/ticket
   - Returns JSON array of all tickets

### Running Unit Tests

```bash
# Run all tests
dotnet test HelpDesk.Tests/HelpDesk.Tests.csproj

# Run with verbose output
dotnet test HelpDesk.Tests/HelpDesk.Tests.csproj --verbosity normal

# Run specific test class
dotnet test HelpDesk.Tests/HelpDesk.Tests.csproj --filter "FullyQualifiedName~TicketControllerTests"

# Run with coverage (requires coverlet)
dotnet test HelpDesk.Tests/HelpDesk.Tests.csproj --collect:"XPlat Code Coverage"
```

**Test Results**: 13 tests passing covering:
- GetAllTickets (with data, empty)
- GetTicketById (found, not found)
- CreateTicket (success)
- UpdateTicket (success, not found, bad request)
- DeleteTicket (success, not found)
- GetTicketsByStatus (with matches, empty)

### API Testing with curl/Postman

```bash
# Get all tickets
curl http://localhost:5210/api/ticket

# Get ticket by ID
curl http://localhost:5210/api/ticket/1

# Create ticket
curl -X POST http://localhost:5210/api/ticket \
  -H "Content-Type: application/json" \
  -d '{"title":"Test","description":"Test desc","category":"Software","priority":"High","status":"Open"}'

# Update ticket
curl -X PUT http://localhost:5210/api/ticket/1 \
  -H "Content-Type: application/json" \
  -d '{"id":1,"title":"Updated","description":"Updated desc","category":"Hardware","priority":"Critical","status":"In Progress"}'

# Delete ticket
curl -X DELETE http://localhost:5210/api/ticket/1

# Filter by status
curl http://localhost:5210/api/ticket/status/Open
```

## Database

The application uses SQL Server LocalDB with automatic migrations. Database name: `HelpDeskDb`

To reset database:
```bash
dotnet ef database drop --project HelpDesk.Api --force
dotnet ef database update --project HelpDesk.Api
```

## Project Structure

```
HelpDeskManagement/
├── HelpDesk.Api/
│   ├── Controllers/TicketController.cs
│   ├── Data/HelpDeskDbContext.cs
│   ├── Models/Ticket.cs
│   ├── Repositories/ITicketRepository.cs
│   ├── Repositories/TicketRepository.cs
│   ├── Migrations/
│   └── Program.cs
├── HelpDesk.Mvc/
│   ├── Controllers/HomeController.cs
│   ├── Controllers/TicketsController.cs
│   ├── Models/Ticket.cs
│   ├── Services/TicketService.cs
│   ├── Views/
│   │   ├── Home/Index.cshtml (Dashboard)
│   │   ├── Tickets/Index.cshtml (All Tickets)
│   │   ├── Tickets/Create.cshtml
│   │   ├── Tickets/Edit.cshtml
│   │   ├── Tickets/Details.cshtml
│   │   ├── Tickets/Delete.cshtml
│   │   ├── Tickets/Filter.cshtml
│   │   └── Shared/_Layout.cshtml
│   └── Program.cs
├── HelpDesk.Tests/
│   └── TicketControllerTests.cs (13 tests)
├── HelpDeskManagement.slnx
├── .gitignore
└── README.md
```

## Status

**All Phases Complete** ✅

- Phase 1: Web API with EF Core, Repository Pattern, LocalDB
- Phase 2: MVC Application with Service Layer, Full UI
- Phase 3: Unit Tests with xUnit + Moq (13 tests passing)
- Phase 4: Git Repository pushed to GitHub

---

*Generated as part of Help Desk Ticket Management System assignment*