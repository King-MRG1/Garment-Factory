# Garment Factory Management System

A modern, enterprise-grade ASP.NET Core REST API for comprehensive garment production and inventory management. The system provides end-to-end solutions for order management, employee records, fabric inventory, financial tracking, and role-based access control.

**Built With:**
- .NET 10.0
- ASP.NET Core Identity & JWT Authentication
- Entity Framework Core 10.0.5
- SQL Server Database
- Scalar API Documentation
- Clean Layered Architecture

---

## 📚 Table of Contents

1. [Solution Overview](#solution-overview)
2. [Architecture & Design](#architecture--design)
3. [Project Structure](#project-structure)
4. [Database Layer](#database-layer)
5. [Repository Layer](#repository-layer)
6. [Services Layer](#services-layer)
7. [Shared Layer](#shared-layer)
8. [API Layer](#api-layer)
9. [Getting Started](#getting-started)
10. [Configuration](#configuration)
11. [Authentication & Security](#authentication--security)
12. [API Documentation](#api-documentation)
13. [Database Migrations](#database-migrations)
14. [Best Practices](#best-practices)
15. [Development Guidelines](#development-guidelines)

---

## Solution Overview

The **Garment Factory Management System** is an enterprise-class REST API designed to streamline garment production operations. Built on proven architectural patterns, it provides robust capabilities for:

- **Order Management**: Create, track, and manage production orders
- **Inventory Control**: Track fabric inventory and material consumption
- **Employee Management**: Maintain worker records, advances, and deductions
- **Financial Tracking**: Monitor expenses and revenue
- **Business Partnerships**: Manage trader and supplier relationships
- **Authentication & Authorization**: Secure, role-based access control

### Solution Components

| Project | Purpose | Technology |
|---------|---------|-----------|
| **Database** | Data persistence layer with EF Core models and migrations | .NET 10.0 Class Library |
| **Repository** | Data access abstraction with Unit of Work pattern | .NET 10.0 Class Library |
| **Services** | Business logic and domain operations | .NET 10.0 Class Library |
| **Shared** | Cross-cutting DTOs, mappings, and configuration | .NET 10.0 Class Library |
| **API** | REST endpoints and HTTP request handling | .NET 10.0 Web API |

---

## Architecture & Design

### Layered Architecture

The system implements a **Clean Layered Architecture** with strict separation of concerns:

```
┌─────────────────────────────────────────────────────┐
│                   API Layer                         │
│  Controllers, OpenAPI, JWT Auth, HTTP Handling      │
└────────────────────────┬────────────────────────────┘
                         │
┌────────────────────────┴────────────────────────────┐
│                  Services Layer                     │
│  Business Rules, Validations, Orchestration         │
│  - Authentication Service                           │
│  - Order Service                                    │
│  - Inventory Service                                │
│  - Worker Management Service                        │
│  - Financial Tracking Service                       │
└────────────────────────┬────────────────────────────┘
                         │
┌────────────────────────┴────────────────────────────┐
│              Repository Layer                       │
│  Unit of Work, Generic CRUD Pattern                 │
│  - Order Repository     - Expense Repository        │
│  - Worker Repository    - Revenue Repository        │
│  - Fabric Repository    - Trader Repository         │
└────────────────────────┬────────────────────────────┘
                         │
┌────────────────────────┴────────────────────────────┐
│               Database Layer                        │
│  EF Core Models, DbContext, Migrations              │
│  - 13 Domain Models                                 │
│  - ASP.NET Core Identity Integration                │
│  - 2 Migration Versions                             │
└────────────────────────┬────────────────────────────┘
                         │
                    ┌────┴────────┐
                    │ SQL Server  │
                    │ Database    │
                    └─────────────┘

Shared Layer (Cross-Cutting)
├── DTOs (Data Transfer Objects)
├── Mapping (Extension Methods)
├── Utilities (EnumHelper)
└── Settings (JwtSettings)
```

### Architectural Principles

1. **Separation of Concerns**: Each layer has a single responsibility
2. **Dependency Inversion**: High-level modules depend on abstractions, not concrete implementations
3. **Repository Pattern**: Data access logic is abstracted behind repository interfaces
4. **Unit of Work Pattern**: Coordinates multiple repositories for atomic operations
5. **Dependency Injection**: All services registered with Microsoft DI container
6. **Async/Await**: Fully asynchronous operations for scalability and responsiveness

---

## Project Structure

```
Garment_Factory/
├── Garment_Factory.slnx              # Solution file
├── README.md                         # Project documentation
│
├── API/                              # Web API Project
│   ├── Program.cs                    # Application startup and configuration
│   ├── appsettings.json              # Production configuration
│   ├── appsettings.Development.json  # Development configuration
│   ├── Controllers/                  # HTTP endpoint controllers (ready for expansion)
│   ├── Properties/
│   │   └── launchSettings.json       # Launch profile settings
│   └── API.csproj                    # Project file
│
├── Database/                         # Database Layer Project
│   ├── Data/
│   │   ├── AppDbContext.cs           # Entity Framework DbContext
│   │   ├── AppDbContextFactory.cs    # Design-time context factory
│   │   └── Configurations/           # Fluent API entity configurations
│   │       ├── ApplicationUserConfiguration.cs
│   │       ├── OrderConfiguration.cs
│   │       ├── WorkerConfiguration.cs
│   │       ├── FabricConfiguration.cs
│   │       ├── ExpenseConfiguration.cs
│   │       ├── RevenueConfiguration.cs
│   │       ├── TraderConfiguration.cs
│   │       ├── AdvanceAndDeductionConfiguration.cs
│   │       ├── PhoneConfiguration.cs
│   │       ├── ModelConfiguration.cs
│   │       ├── OrderModelConfiguration.cs
│   │       └── RefreshTokenStoreConfiguration.cs
│   ├── Models/                       # Domain Models
│   │   ├── Model.cs                  # Base model class
│   │   ├── IUserOwner.cs             # Interface for user-owned entities
│   │   ├── ApplicationUser.cs        # Identity user model
│   │   ├── Worker.cs                 # Employee records
│   │   ├── Order.cs                  # Production orders
│   │   ├── OrderModel.cs             # Order line items
│   │   ├── Fabric.cs                 # Fabric inventory
│   │   ├── Expense.cs                # Expense records
│   │   ├── Revenue.cs                # Revenue records
│   │   ├── Trader.cs                 # Business partners
│   │   ├── Phone.cs                  # Contact information
│   │   ├── AdvanceAndDeduction.cs    # Employee financial records
│   │   └── RefreshTokenStore.cs      # JWT refresh tokens
│   ├── Migrations/                   # Database schema versions
│   │   ├── 20260320232530_Initial_Migrate.cs
│   │   ├── 20260321132814_AddRefreshTokenStore.cs
│   │   └── AppDbContextModelSnapshot.cs
│   └── Database.csproj               # Project file
│
├── Repository/                       # Repository Layer Project
│   ├── Interfaces/                   # Repository contracts
│   │   ├── IGenericRepository.cs
│   │   ├── IUnitOfWork.cs
│   │   ├── IOrderRepository.cs
│   │   ├── IWorkerRepository.cs
│   │   ├── IFabricRepository.cs
│   │   ├── IExpenseRepository.cs
│   │   ├── IRevenueRepository.cs
│   │   ├── ITraderRepository.cs
│   │   ├── IAdvanceAndDeductionRepository.cs
│   │   ├── IAuthRepository.cs
│   │   ├── IPhoneRepository.cs
│   │   ├── IModelRepository.cs
│   │   └── IOrderModelRepository.cs
│   ├── Implementations/              # Repository implementations
│   │   ├── GenericRepository.cs      # Base CRUD operations
│   │   ├── UnitOfWork.cs             # Transaction coordination
│   │   ├── OrderRepository.cs
│   │   ├── WorkerRepository.cs
│   │   ├── FabricRepository.cs
│   │   ├── ExpenseRepository.cs
│   │   ├── RevenueRepository.cs
│   │   ├── TraderRepository.cs
│   │   ├── AdvanceAndDeductionRepository.cs
│   │   ├── AuthRepository.cs
│   │   ├── PhoneRepository.cs
│   │   ├── ModelRepository.cs
│   │   └── OrderModelRepository.cs
│   ├── Extensions/
│   │   └── RepositoryServiceExtensions.cs  # Dependency injection setup
│   └── Repository.csproj             # Project file
│
├── Services/                         # Services Layer Project
│   ├── Interfaces/                   # Service contracts
│   │   ├── IOrderService.cs
│   │   ├── IWorkerService.cs
│   │   ├── IFabricService.cs
│   │   ├── IExpenseService.cs
│   │   ├── IRevenueService.cs
│   │   ├── ITraderService.cs
│   │   ├── IAdvanceAndDeductionService.cs
│   │   ├── IAuthService.cs
│   │   ├── ITokenService.cs
│   │   ├── IModelService.cs
│   │   └── IUnitOfServices.cs
│   ├── Implementations/              # Service implementations
│   │   ├── OrderService.cs
│   │   ├── WorkerService.cs
│   │   ├── FabricService.cs
│   │   ├── ExpenseService.cs
│   │   ├── RevenueService.cs
│   │   ├── TraderService.cs
│   │   ├── AdvanceAndDeductionService.cs
│   │   ├── AuthService.cs            # Authentication & JWT
│   │   ├── TokenService.cs           # Token generation
│   │   ├── ModelService.cs
│   │   └── UnitOfServices.cs         # Service coordination
│   ├── Extensions/
│   │   └── ServicesExtensions.cs     # Dependency injection setup
│   └── Services.csproj               # Project file
│
├── Shared/                           # Shared Layer Project
│   ├── Dtos/                         # Data Transfer Objects
│   │   ├── UserDtos/
│   │   │   ├── LoginDto.cs
│   │   │   ├── RegisterDto.cs
│   │   │   ├── AuthResponseDto.cs
│   │   │   └── UserDataDto.cs
│   │   ├── OrderDtos/
│   │   │   ├── CreateOrderDto.cs
│   │   │   ├── ViewOrderDto.cs
│   │   │   ├── UpdateOrderDto.cs
│   │   │   ├── OrderModelDto.cs
│   │   │   └── ViewOrderModelDto.cs
│   │   ├── WorkerDtos/
│   │   ├── FabricDtos/
│   │   ├── ExpenseDtos/
│   │   ├── RevenueDtos/
│   │   ├── TraderDtos/
│   │   ├── PhoneDtos/
│   │   ├── AdvanceAndDeductionDtos/
│   │   ├── ModelDtos/
│   │   └── ReportsDtos/              # Analytics/Report DTOs
│   ├── Mapping/                      # DTO Mapping Extensions
│   │   ├── OrderMapping.cs
│   │   ├── UserMapping.cs
│   │   ├── WorkerMapping.cs
│   │   ├── FabricMapping.cs
│   │   ├── ExpenseMapping.cs
│   │   ├── RevenueMapping.cs
│   │   ├── TraderMapping.cs
│   │   ├── AdvanceAndDeductionMapping.cs
│   │   ├── PhoneMapping.cs
│   │   ├── ModelMapping.cs
│   │   ├── OrderModelMapping.cs
│   │   └── ReportMapping.cs
│   ├── Helper/
│   │   └── EnumHelper.cs             # Enumeration utilities
│   ├── Settings/
│   │   └── JwtSettings.cs            # JWT configuration
│   └── Shared.csproj                 # Project file
│
└── .gitignore                        # Git ignore rules
```

---

## Database Layer

### Overview

The **Database** project provides the foundational data persistence layer using Entity Framework Core 10.0.5 with SQL Server.

**Responsibilities:**
- Define domain models representing business entities
- Configure entity relationships and constraints
- Manage database migrations and schema versioning
- Provide DbContext for data access

### Core Domain Models

| Model | Purpose | Key Features |
|-------|---------|--------------|
| **ApplicationUser** | User identity | Inherits from IdentityUser; multi-tenant owner tracking |
| **Worker** | Employee records | Salary tracking, contact info, advance/deduction support |
| **Order** | Production orders | Links to trader, contains line items, cost tracking |
| **OrderModel** | Order line items | References order and garment model, quantity tracking |
| **Fabric** | Inventory management | Material tracking, stock quantities |
| **Model** | Garment designs | Design catalog, style definitions |
| **Expense** | Expense tracking | Categorized expenses with date and amount |
| **Revenue** | Revenue records | Linked to orders and users for reporting |
| **Trader** | Business partners | Vendor and customer information |
| **AdvanceAndDeduction** | Employee financial | Salary advances and deductions |
| **Phone** | Contact information | Multiple phone numbers per entity |
| **RefreshTokenStore** | JWT token management | Secure token storage with expiration and revocation |

### Database Context

```csharp
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    // DbSets for all domain models
    public DbSet<Order> Orders { get; set; }
    public DbSet<Worker> Workers { get; set; }
    public DbSet<Fabric> Fabrics { get; set; }
    // ... additional DbSets
}
```

### Migrations

Two migrations are currently applied:

1. **Initial_Migrate** (20260320232530) - Core schema with all entities and ASP.NET Core Identity tables
2. **AddRefreshTokenStore** (20260321132814) - JWT refresh token storage with expiration tracking

#### Creating New Migrations

```bash
# Add new migration
dotnet ef migrations add DescriptiveMigrationName --project Database

# Apply migrations to database
dotnet ef database update --project Database

# Revert to previous migration
dotnet ef database update PreviousMigrationName --project Database

# Remove last migration
dotnet ef migrations remove --project Database
```

### Fluent API Configuration

Entity configurations use Fluent API for:
- Primary key definitions
- Relationship configurations (one-to-many, many-to-many)
- Cascade delete rules
- Index creation
- Property constraints

Example configuration:
```csharp
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        
        builder.HasOne(o => o.Trader)
            .WithMany()
            .HasForeignKey(o => o.Trader_Id)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasMany(o => o.OrderModels)
            .WithOne(om => om.Order)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
```

### Best Practices

1. ✅ Always use migrations for schema changes
2. ✅ Test migrations in development before production
3. ✅ Use descriptive migration names reflecting changes
4. ✅ Implement `IUserOwner` for multi-tenant entity tracking
5. ✅ Configure relationships in configuration classes for clarity
6. ✅ Use nullable reference types for compile-time null safety

---

## Repository Layer

### Overview

The **Repository** project implements data access abstraction using the Repository Pattern and Unit of Work Pattern, providing:

- Generic CRUD operations for all entities
- Coordinated data access across multiple repositories
- Transaction support for atomic operations
- Easy unit testing through interface-based abstraction

### Key Patterns

#### Generic Repository Pattern

```csharp
public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<bool> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
    Task<int> SaveChangesAsync();
}
```

**Implementation Highlights:**
- Async/await for non-blocking operations
- IQueryable support for LINQ composition
- Change tracking optimization
- Configurable query behaviors

#### Unit of Work Pattern

```csharp
public interface IUnitOfWork : IDisposable
{
    // Repository properties for each entity type
    IModelRepository Models { get; }
    ITraderRepository Traders { get; }
    IWorkerRepository Workers { get; }
    IOrderRepository Orders { get; }
    IOrderModelRepository OrderModels { get; }
    IFabricRepository Fabrics { get; }
    IPhoneRepository Phones { get; }
    IRevenueRepository Revenues { get; }
    IExpenseRepository Expenses { get; }
    IAdvanceAndDeductionRepository AdvanceAndDeductions { get; }
    IAuthRepository Auth { get; }
    
    // Coordinated save across all repositories
    Task<int> SaveChangesAsync();
}
```

**Benefits:**
- Single entry point for data access
- Coordinated SaveChanges across repositories
- Atomic operations with transaction support
- Simplified dependency injection

### Repository Implementations

| Repository | Access Via | Features |
|------------|-----------|----------|
| **GenericRepository** | Base interface | Base CRUD operations for any entity |
| **OrderRepository** | `_unitOfWork.Orders` | Order queries, filtering, creation |
| **OrderModelRepository** | `_unitOfWork.OrderModels` | Order line item operations |
| **WorkerRepository** | `_unitOfWork.Workers` | Employee searches and salary tracking |
| **FabricRepository** | `_unitOfWork.Fabrics` | Inventory queries and stock management |
| **ExpenseRepository** | `_unitOfWork.Expenses` | Expense filtering and categorization |
| **RevenueRepository** | `_unitOfWork.Revenues` | Revenue calculation and reporting |
| **TraderRepository** | `_unitOfWork.Traders` | Partner searches and contact management |
| **ModelRepository** | `_unitOfWork.Models` | Garment design catalog access |
| **AuthRepository** | `_unitOfWork.Auth` | User lookup and authentication helpers |
| **AdvanceAndDeductionRepository** | `_unitOfWork.AdvanceAndDeductions` | Financial tracking for employees |
| **PhoneRepository** | `_unitOfWork.Phones` | Contact information management |

### Usage Example

```csharp
// In a service class
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ViewOrderDto?> CreateOrderAsync(CreateOrderDto createOrderDto)
    {
        // Validate trader exists
        var trader = await _unitOfWork.Traders.GetByIdAsync(createOrderDto.Trader_Id);
        if (trader == null)
            throw new Exception($"Trader with ID {createOrderDto.Trader_Id} not found.");

        // Convert DTO to domain model
        var order = createOrderDto.ToOrder();

        // Process order items with validation and calculation
        var ordermodels = await ProcessOrderModelsAsync(order.OrderModels);

        // Calculate totals
        var totalCost = ordermodels.Sum(om => om.Price);
        var totalQuantity = ordermodels.Sum(om => om.Quantity);

        // Update trader amount
        trader.Amount += totalCost;

        // Persist changes (all operations succeed or fail together)
        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        // Return as DTO
        var orderDto = order.ToOrderDto();
        orderDto.OrderModels = ordermodels;
        orderDto.Total_Cost = totalCost;
        orderDto.Total_Quantity = totalQuantity;

        return orderDto;
    }
    
    private async Task<List<ViewOrderModelDto>> ProcessOrderModelsAsync(IEnumerable<OrderModel> orderModels)
    {
        // Get all models referenced in order items
        var modelIds = orderModels.Select(om => om.Model_Id).ToList();
        var models = (modelIds.Count > 0)
            ? await _unitOfWork.Models.GetModelsByIdsAsync(modelIds)
            : new List<Model>();

        var result = new List<ViewOrderModelDto>();

        foreach (var orderModel in orderModels)
        {
            // Validate model exists
            var model = models.FirstOrDefault(m => m.Id == orderModel.Model_Id);
            if (model == null)
                throw new Exception($"Model with ID {orderModel.Model_Id} not found.");

            // Validate inventory availability
            if (orderModel.Quantity > model.Total_Units)
                throw new InvalidOperationException(
                    $"Not enough units for model '{model.Model_Name}'. " +
                    $"Available: {model.Total_Units}, Requested: {orderModel.Quantity}.");

            // Reduce inventory
            model.Total_Units -= orderModel.Quantity;

            // Calculate item total
            result.Add(new ViewOrderModelDto
            {
                Model_Id = orderModel.Model_Id,
                Model_Name = model.Model_Name,
                Quantity = orderModel.Quantity,
                Price = model.Price_Trader * orderModel.Quantity
            });
        }

        return result;
    }
}
```

### Dependency Injection Setup

```csharp
// In RepositoryServiceExtensions.cs
public static IServiceCollection AddRepositories(this IServiceCollection services)
{
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    return services;
}

// In Program.cs
builder.Services.AddRepositories();
```

### Best Practices

1. ✅ Use Unit of Work for multi-repository operations
2. ✅ Keep repositories focused on data access only
3. ✅ Use async/await consistently
4. ✅ Leverage transactions for critical operations
5. ✅ Mock repositories in unit tests
6. ✅ Implement specific repositories only when custom logic is needed

---

## Services Layer

### Overview

The **Services** project implements business logic, validation, and domain operations. Services orchestrate repositories, apply business rules, and manage application workflows.

**Responsibilities:**
- Business logic implementation
- Data validation and sanitization
- Cross-cutting operations (logging, caching)
- Transaction management
- External service integration

### Core Services

#### Authentication Service

```csharp
public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<bool> LogoutAsync(string userId);
    Task<AuthResponseDto> RefreshTokenAsync(string token);
}
```

**Features:**
- User registration and login
- JWT token generation
- Refresh token management
- Password validation and hashing

#### Order Service

```csharp
public interface IOrderService
{
    Task<ViewOrderDto> GetOrderAsync(int orderId);
    Task<IEnumerable<ViewOrderDto>> GetAllOrdersAsync();
    Task<bool> CreateOrderAsync(CreateOrderDto orderDto);
    Task<bool> UpdateOrderAsync(int orderId, UpdateOrderDto updateDto);
    Task<bool> DeleteOrderAsync(int orderId);
}
```

**Features:**
- Order creation and management
- Order status tracking
- Linked item management
- Cost calculation

#### Token Service

```csharp
public interface ITokenService
{
    string GenerateAccessToken(ApplicationUser user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
```

**Features:**
- JWT token generation
- Refresh token creation
- Token validation
- Claims-based authorization

#### Additional Services

- **WorkerService**: Employee management
- **FabricService**: Inventory management
- **ExpenseService**: Expense tracking
- **RevenueService**: Revenue monitoring
- **TraderService**: Business partner management
- **AdvanceAndDeductionService**: Employee financial operations

### Unit of Services Coordinator

```csharp
public interface IUnitOfServices
{
    IOrderService OrderService { get; }
    IWorkerService WorkerService { get; }
    IFabricService FabricService { get; }
    // ... additional services
}
```

Provides centralized access to all services in the application.

### Service Registration

```csharp
// In ServicesExtensions.cs
public static IServiceCollection AddServices(this IServiceCollection services)
{
    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IOrderService, OrderService>();
    // ... register additional services
    services.AddScoped<IUnitOfServices, UnitOfServices>();
    return services;
}
```

### Best Practices

1. ✅ Keep services focused on single responsibility
2. ✅ Use async/await for all I/O operations
3. ✅ Validate input at service entry points
4. ✅ Use repository abstraction, not DbContext directly
5. ✅ Implement comprehensive logging
6. ✅ Use DTOs for service boundaries

---

## Shared Layer

### Overview

The **Shared** project provides cross-cutting concerns used throughout the application:

- **DTOs**: Type-safe data transfer between layers
- **Mappings**: Extension methods for entity-to-DTO conversions
- **Utilities**: Helper functions and enumerations
- **Configuration**: Service settings and constants

### Data Transfer Objects (DTOs)

DTOs define the API contract, decoupling internal domain models from external representations.

#### User DTOs

```csharp
public class LoginDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class AuthResponseDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public UserDataDto User { get; set; }
}

public class UserDataDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}
```

#### Order DTOs

```csharp
public class CreateOrderDto
{
    public int TraderId { get; set; }
    public decimal TotalCost { get; set; }
    public List<OrderModelDto> OrderModels { get; set; }
}

public class ViewOrderDto
{
    public int Id { get; set; }
    public int TraderId { get; set; }
    public decimal TotalCost { get; set; }
    public DateOnly OrderDate { get; set; }
    public List<ViewOrderModelDto> OrderModels { get; set; }
}

public class UpdateOrderDto
{
    public decimal TotalCost { get; set; }
    public int TraderId { get; set; }
}
```

### DTO Mapping Extensions

Lightweight extension methods for entity-to-DTO and DTO-to-entity conversions:

```csharp
// Example: OrderMapping.cs
public static class OrderMapping
{
    public static ViewOrderDto ToOrderDto(this Order order)
    {
        return new ViewOrderDto
        {
            Id = order.Id,
            Total_Cost = order.Total_Cost,
            Order_Date = order.Order_Date,
            TraderId = order.Trader_Id,
            OrderModels = order.OrderModels?
                .Select(om => om.ToOrderModelDto())
                .ToList() ?? new List<ViewOrderModelDto>()
        };
    }

    public static Order ToOrder(this CreateOrderDto dto)
    {
        return new Order
        {
            Total_Cost = dto.TotalCost,
            Trader_Id = dto.TraderId,
            Order_Date = DateOnly.FromDateTime(DateTime.Now)
        };
    }
}
```

**Advantages:**
- No external dependencies (AutoMapper not required)
- Type-safe conversions
- Fluent API style
- Minimal performance overhead

### Helper Utilities

#### EnumHelper

```csharp
public static class EnumHelper
{
    public static Dictionary<int, string> GetEnumDictionary<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .ToDictionary(e => Convert.ToInt32(e), e => e.ToString());
    }
}
```

Use cases:
- Populating dropdown lists
- Validation against allowed values
- Client-side enum serialization

### Settings & Configuration

#### JwtSettings

```csharp
public class JwtSettings
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
}
```

Configuration binding in appsettings.json:
```json
{
  "JWT": {
    "Secret": "your-secret-key-min-32-characters",
    "Issuer": "yourapp",
    "Audience": "yourapp-users",
    "ExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  }
}
```

### Project Dependencies

```xml
<ItemGroup>
  <ProjectReference Include="..\Database\Database.csproj" />
</ItemGroup>
```

Shared only depends on Database (for model definitions), keeping it lightweight and reusable.

### Best Practices

1. ✅ Keep DTOs lean and focused
2. ✅ Use extension methods for simple conversions
3. ✅ Don't expose sensitive data in DTOs
4. ✅ Use operation-specific DTOs (Create, View, Update)
5. ✅ Centralize configuration in settings classes
6. ✅ Organize DTOs by entity type

---

## API Layer

### Overview

The **API** project exposes REST endpoints using ASP.NET Core, providing the public interface for client applications.

**Responsibilities:**
- HTTP request handling
- Route definition
- Request validation
- Response serialization
- API documentation

### Startup Configuration (Program.cs)

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Identity Configuration
        builder.Services
            .AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        // API & OpenAPI
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddHttpContextAccessor();

        // Data Access
        builder.Services.AddRepositories();
        builder.Services.AddServices();

        // Database
        builder.Services.AddDbContext<AppDbContext>(option =>
            option.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));

        // JWT Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "JwtBearer";
            options.DefaultChallengeScheme = "JwtBearer";
        })
        .AddJwtBearer("JwtBearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidAudience = builder.Configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
            };
        });

        var app = builder.Build();

        // Middleware
        app.MapOpenApi();
        app.MapScalarApiReference();
        app.UseHttpsRedirection();
        app.MapControllers();
        app.UseAuthorization();

        app.Run();
    }
}
```

### Configuration Structure

#### appsettings.json (Production)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=GarmentFactoryDb;Integrated Security=True;TrustServerCertificate=True;"
  },
  "JWT": {
    "Key": "your-secret-key-minimum-32-characters-long",
    "Issuer": "garment-factory-api",
    "Audience": "garment-factory-users",
    "ExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  }
}
```

#### appsettings.Development.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### API Documentation

The API includes **Scalar OpenAPI** documentation accessible at `/scalar/v1` when running in development mode.

Features:
- Interactive endpoint testing
- Request/response schema inspection
- Authentication testing
- API versioning support

---

## Getting Started

### Prerequisites

- **.NET SDK 10.0** or later ([Download](https://dotnet.microsoft.com/download))
- **SQL Server** (local, Express, or cloud)
  - Windows Authentication or SQL Server Authentication
  - TrustServerCertificate=True for self-signed certificates
- **Visual Studio 2022** or **VS Code** with C# extension
- **Git** for version control

### Installation Steps

#### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/garment-factory.git
cd Garment_Factory
```

#### 2. Restore NuGet Packages

```bash
dotnet restore
```

#### 3. Configure Database Connection

Edit `API/appsettings.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=GarmentFactoryDb;Integrated Security=True;TrustServerCertificate=True;"
  }
}
```

**Connection string options:**

| Scenario | Connection String |
|----------|-------------------|
| Local SQL Server Express (Windows Auth) | `Server=.\\SQLEXPRESS;Database=GarmentFactoryDb;Integrated Security=True;TrustServerCertificate=True;` |
| Local SQL Server (SQL Auth) | `Server=localhost;Database=GarmentFactoryDb;User Id=sa;Password=YourPassword;Encrypt=True;` |
| Azure SQL Database | `Server=servername.database.windows.net;Database=GarmentFactoryDb;User Id=admin;Password=YourPassword;Encrypt=True;TrustServerCertificate=False;` |

#### 4. Apply Database Migrations

```bash
# Create the database and apply all migrations
dotnet ef database update --project Database

# Verify the migration was successful
dotnet ef migrations list --project Database
```

#### 5. Run the Application

```bash
# From the solution directory
dotnet run --project API

# The API will start at https://localhost:5001 (Development)
```

#### 6. Access the API Documentation

Open your browser and navigate to:
```
https://localhost:5001/scalar/v1
```

### Project Build

To build the entire solution:

```bash
dotnet build
```

To build in Release mode:

```bash
dotnet build -c Release
```

---

## Configuration

### Environment Variables

The application supports the following environment variables:

| Variable | Description | Default |
|----------|-------------|---------|
| `ASPNETCORE_ENVIRONMENT` | Execution environment (Development/Production/Staging) | Development |
| `ConnectionStrings:DefaultConnection` | Database connection string | See appsettings.json |
| `JWT:Key` | JWT signing key (min 32 characters) | Value from appsettings |
| `JWT:Issuer` | JWT issuer claim | garment-factory-api |
| `JWT:Audience` | JWT audience claim | garment-factory-users |

### Configuration Precedence

ASP.NET Core loads configuration in this order (later values override earlier ones):

1. `appsettings.json`
2. `appsettings.{ENVIRONMENT}.json` (e.g., Development, Production)
3. Environment variables
4. Command-line arguments

Example for production override:
```bash
set ConnectionStrings:DefaultConnection="Server=prod-server;Database=ProductionDb;..."
set JWT:Key="production-secret-key"
dotnet run --project API
```

---

## Authentication & Security

### JWT Authentication Flow

```
1. Client → POST /api/auth/login with credentials
                  ↓
2. Server validates credentials and generates tokens
                  ↓
3. Client ← Response: JWT (15 min) + RefreshToken (7 days)
                  ↓
4. Client → Include JWT in Authorization header
   Authorization: Bearer <jwt_token>
                  ↓
5. Server validates JWT on each request
                  ↓
6. When JWT expires, client refreshes using RefreshToken
   POST /api/auth/refresh with RefreshToken
                  ↓
7. Server → New JWT and RefreshToken pair
```

### Token Configuration

In `appsettings.json`:

```json
{
  "JWT": {
    "Key": "your-super-secret-key-that-is-at-least-32-characters-long",
    "Issuer": "garment-factory-api",
    "Audience": "garment-factory-users",
    "ExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  }
}
```

### Security Best Practices

1. **Secret Key**: Use a strong, random key (minimum 32 characters)
   ```bash
   # Generate a secure key
   [System.Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes([System.Guid]::NewGuid().ToString() + [System.Guid]::NewGuid().ToString()))
   ```

2. **HTTPS Only**: Always use HTTPS in production
   - Configure in launchSettings.json
   - Enable HSTS headers

3. **Token Expiration**: Use short-lived access tokens with refresh tokens
   - Access Token: 15 minutes
   - Refresh Token: 7 days

4. **Password Requirements**: Enforce strong password policies
   - Minimum 8 characters
   - Mix of uppercase, lowercase, numbers, and symbols

5. **CORS Configuration**: Restrict to known origins
   ```csharp
   builder.Services.AddCors(options =>
   {
       options.AddPolicy("AllowSpecificOrigins", builder =>
       {
           builder.WithOrigins("https://yourdomain.com")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
       });
   });
   ```

6. **Rate Limiting**: Implement rate limiting for API endpoints
   ```csharp
   builder.Services.AddRateLimiter(options =>
   {
       // Configure rate limiting policies
   });
   ```

### Authorization Attributes

Protect endpoints with `[Authorize]` attribute:

```csharp
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<ViewOrderDto>> GetOrder(int id)
    {
        // Endpoint requires valid JWT
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult> CreateOrder(CreateOrderDto dto)
    {
        // Endpoint requires Admin or Manager role
    }
}
```

---

## API Documentation

### OpenAPI (Swagger) Integration

The API includes OpenAPI 3.0 support via **Scalar** for interactive documentation.

**Features:**
- ✅ Interactive endpoint testing
- ✅ Request/response schema visualization
- ✅ Authentication testing
- ✅ Real-time API exploration

**Access Points:**
- OpenAPI JSON: `/openapi/v1.json`
- Scalar UI: `/scalar/v1`
- Traditional Swagger UI: Available via additional package

### Example API Endpoints (To Be Implemented)

```http
# Authentication
POST /api/auth/login
POST /api/auth/register
POST /api/auth/refresh-token
POST /api/auth/logout

# Orders
GET /api/orders
GET /api/orders/{id}
POST /api/orders
PUT /api/orders/{id}
DELETE /api/orders/{id}

# Workers
GET /api/workers
GET /api/workers/{id}
POST /api/workers
PUT /api/workers/{id}

# Inventory
GET /api/fabrics
POST /api/fabrics
PUT /api/fabrics/{id}

# Financial
GET /api/expenses
POST /api/expenses
GET /api/revenue
POST /api/revenue
```

---

## Database Migrations

### Understanding Migrations

Migrations provide version control for your database schema. Each migration represents a set of changes.

### Creating Migrations

```bash
# Generate a new migration
dotnet ef migrations add AddUserStatusField --project Database

# Naming convention: DescriptiveActionName
# Examples: AddOrderTable, RenameWorkerColumn, AddIndexToOrders
```

### Applying Migrations

```bash
# Apply all pending migrations
dotnet ef database update --project Database

# Apply to specific migration
dotnet ef database update AddOrderTable --project Database

# Revert all migrations
dotnet ef database update 0 --project Database
```

### Viewing Migration Dynamics

```bash
# List all migrations
dotnet ef migrations list --project Database

# Show SQL for migration
dotnet ef migrations script --project Database

# Show SQL between migrations
dotnet ef migrations script FromMigration ToMigration --project Database
```

### Current Migrations

| Migration | Date | Purpose |
|-----------|------|---------|
| Initial_Migrate | 2026-03-20 | Core schema with all models and Identity tables |
| AddRefreshTokenStore | 2026-03-21 | JWT refresh token storage with expiration |

---

## Best Practices

### General Development

1. **Version Control**
   ```bash
   git commit -m "feat: implement order creation service"
   git push origin feature/order-service
   ```

2. **Code Organization**
   - Keep classes and methods focused
   - Follow SOLID principles
   - Use meaningful names

3. **Async/Await**
   ```csharp
   // ✅ Good
   public async Task<Order> GetOrderAsync(int id)
   {
       return await _repository.GetByIdAsync(id);
   }
   
   // ❌ Avoid
   public Order GetOrder(int id)
   {
       return _repository.GetById(id).Result; // Blocks thread
   }
   ```

4. **Error Handling**
   ```csharp
   try
   {
       await _service.ProcessOrderAsync(order);
   }
   catch (ArgumentException ex)
   {
       _logger.LogWarning($"Invalid order: {ex.Message}");
       return BadRequest(ex.Message);
   }
   catch (Exception ex)
   {
       _logger.LogError($"Unexpected error: {ex}");
       return StatusCode(500, "Internal server error");
   }
   ```

5. **Validation**
   ```csharp
   if (string.IsNullOrWhiteSpace(order.Trader_Name))
       throw new ArgumentException("Trader name is required");
   
   if (order.Total_Cost <= 0)
       throw new ArgumentException("Total cost must be greater than zero");
   ```

### Architecture Guidelines

1. **Don't Skip Layers**: Always use the layered architecture
   - ❌ Controllers calling DbContext directly
   - ✅ Controllers → Services → Repositories → Database

2. **Repository Layer**: Keep it data-access focused
   ```csharp
   // ✅ Good - Data access only
   public async Task<List<Order>> GetOrdersByTraderAsync(int traderId)
   {
       return await _context.Orders
           .Where(o => o.Trader_Id == traderId)
           .ToListAsync();
   }
   
   // ❌ Bad - Business logic in repository
   public async Task<decimal> GetTraderTotalRevenueAsync(int traderId)
   {
       var orders = await GetOrdersByTraderAsync(traderId);
       return orders.Sum(o => o.Total_Cost * 1.1); // Tax calculation shouldn't be here
   }
   ```

3. **Service Layer**: Implement business logic here
   ```csharp
   // ✅ Good - Business logic in service
   public async Task<decimal> CalculateTraderRevenueWithTaxAsync(int traderId)
   {
       var orders = await _unitOfWork.OrderRepository
           .GetOrdersByTraderAsync(traderId);
       
       var baseRevenue = orders.Sum(o => o.Total_Cost);
       return baseRevenue * TAX_MULTIPLIER; // Business rule
   }
   ```

4. **DTO Usage**: Use DTOs at layer boundaries
   - Services expose DTOs, not domain models
   - API controllers work with DTOs exclusively
   - Mapping happens in Shared layer

5. **Dependency Injection**: Register dependencies in extension methods
   ```csharp
   // In ServicesExtensions.cs
   public static IServiceCollection AddServices(this IServiceCollection services)
   {
       services.AddScoped<IOrderService, OrderService>();
       services.AddScoped<IWorkerService, WorkerService>();
       // ... all services
       return services;
   }
   
   // In Program.cs
   builder.Services.AddServices();
   ```

---

## Development Guidelines

### Setting Up Your Development Environment

#### Visual Studio 2022

1. Create solution-level folder
2. Create projects under solution
3. Set API project as startup project
4. Configure launch settings for debugging

#### VS Code

```bash
# Install required extensions
code --install-extension ms-dotnettools.csharp
code --install-extension ms-dotnettools.vscode-dotnet-runtime

# Open folder
code .

# Run and debug
Ctrl+F5 (Debug)
Ctrl+Shift+D (Debug panel)
```

### Code Style & Formatting

```csharp
// Namespace organization
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Database.Models;
using Repository.Interfaces;
using Services.Interfaces;

// Class declaration
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    // Method
    public async Task<ViewOrderDto> GetOrderAsync(int orderId)
    {
        if (orderId <= 0)
            throw new ArgumentException("Order ID must be positive", nameof(orderId));
        
        var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
        return order?.ToOrderDto();
    }
}
```

### Unit Testing Strategy

```csharp
// xUnit test example
public class OrderServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly OrderService _service;
    
    public OrderServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _service = new OrderService(_mockUnitOfWork.Object);
    }
    
    [Fact]
    public async Task GetOrderAsync_WithValidId_ReturnsOrder()
    {
        // Arrange
        var orderId = 1;
        var order = new Order { Id = orderId, Total_Cost = 1000 };
        _mockUnitOfWork.Setup(x => x.OrderRepository.GetByIdAsync(orderId))
            .ReturnsAsync(order);
        
        // Act
        var result = await _service.GetOrderAsync(orderId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderId, result.Id);
    }
}
```

### Deployment Considerations

1. **Database Backups**: Always backup before migrations
   ```sql
   BACKUP DATABASE GarmentFactoryDb TO DISK = 'C:\Backups\GarmentFactoryDb.bak'
   ```

2. **Environment Secrets**: Use User Secrets in development, Key Vault in production
   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "JWT:Key" "your-production-key"
   ```

3. **Logging**: Implement comprehensive logging
   ```csharp
   builder.Services.AddLogging(logging =>
   {
       logging.AddConsole();
       logging.AddDebug();
       logging.AddApplicationInsights();
   });
   ```

4. **Health Checks**: Implement health check endpoints
   ```csharp
   builder.Services.AddHealthChecks()
       .AddDbContextCheck<AppDbContext>();
   ```

---

## Troubleshooting

### Common Issues

**Issue: "Unable to connect to database"**

Solution:
```bash
# Verify connection string
# Check server is running
# Check credentials
# For Windows Auth, ensure user has database access
# For SQL Auth, verify username/password
```

**Issue: "Migration failed"**

Solution:
```bash
# Revert last migration
dotnet ef migrations remove --project Database

# Check for syntax errors in code
# Verify entity configurations are correct
```

**Issue: "Controller endpoints not working"**

Solution:
```csharp
// Ensure controllers are in Controllers folder
// Verify [ApiController] and [Route] attributes
// Check middleware order in Program.cs
// Verify CORS is configured if calling from different origin
```

---

## Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/your-feature`)
3. Commit changes (`git commit -m 'Add feature'`)
4. Push to branch (`git push origin feature/your-feature`)
5. Create Pull Request

### Code Review Checklist

- [ ] Code follows project style guidelines
- [ ] Comments added for complex logic
- [ ] Tests included for new features
- [ ] Documentation updated
- [ ] No breaking changes to public APIs
- [ ] Database migrations are tested

---

## License

This project is licensed under the MIT License - see LICENSE file for details.

---

## Support & Documentation

For additional help:

- 📖 **[.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)**
- 📖 **[Entity Framework Core Docs](https://docs.microsoft.com/en-us/ef/core/)**
- 📖 **[ASP.NET Core Docs](https://docs.microsoft.com/en-us/aspnet/core/)**
- 🔐 **[JWT.io](https://jwt.io/)** - JWT Token Debugger
- 🐛 **[GitHub Issues](https://github.com/King-MRG1/garment-factory/issues)** - Report bugs

---

**Last Updated**: April 2, 2026
**Version**: 1.0.0
**Status**: Active Development
