# Database

A robust Entity Framework Core-based database layer for the Garment Factory Management System, providing comprehensive data models, migrations, and database context configuration for enterprise-level garment production and inventory management.

## 📋 Overview

The **Database** project establishes the foundational data access layer for the Garment Factory application. It leverages Entity Framework Core with SQL Server to manage all database operations, including:

- **Entity Models**: Comprehensive domain models for garment production, inventory, employees, and financial operations
- **Database Context**: ASP.NET Core Identity integration with custom DbContext implementation
- **Migrations**: Version-controlled database schema evolution with support for multi-user environments
- **Configuration**: Fluent API configurations for complex entity relationships and constraints

## 🏗️ Project Structure

```
Database/
├── Data/
│   ├── AppDbContext.cs              # Main database context with Identity integration
│   ├── AppDbContextFactory.cs       # Design-time context factory for migrations
│   └── Configurations/              # Entity configuration classes
│       ├── ApplicationUserConfiguration.cs
│       ├── AdvanceAndDeductionConfiguration.cs
│       ├── ExpenseConfiguration.cs
│       ├── FabricConfiguration.cs
│       ├── ModelConfiguration.cs
│       ├── OrderConfiguration.cs
│       ├── OrderModelConfiguration.cs
│       ├── PhoneConfiguration.cs
│       ├── RefreshTokenStoreConfiguration.cs
│       ├── RevenueConfiguration.cs
│       ├── TraderConfiguration.cs
│       └── WorkerConfiguration.cs
├── Models/
│   ├── ApplicationUser.cs           # Identity user model
│   ├── Worker.cs                    # Employee/worker entity
│   ├── Order.cs                     # Production orders
│   ├── OrderModel.cs                # Order line items
│   ├── Fabric.cs                    # Fabric inventory
│   ├── Expense.cs                   # Business expenses
│   ├── Revenue.cs                   # Revenue tracking
│   ├── Trader.cs                    # Business partners
│   ├── Phone.cs                     # Contact information
│   ├── AdvanceAndDeduction.cs       # Employee advances/deductions
│   ├── RefreshTokenStore.cs         # Refresh token storage for authentication
│   ├── Model.cs                     # Base model entity
│   └── IUserOwner.cs                # Interface for user-owned entities
└── Migrations/
    └── [Migration files]            # Database schema versions
```

## 🔧 Technology Stack

- **.NET Framework**: .NET 10.0
- **ORM**: Entity Framework Core 10.0.5
- **Database**: SQL Server
- **Authentication**: ASP.NET Core Identity
- **Language Features**: C# with nullable reference types enabled

## 📦 Dependencies

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="10.0.5" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.5" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.5" />
  <PackageReference Include="Microsoft.AspNetCore.Http.Abstractions" Version="2.3.9" />
</ItemGroup>
```

## 🚀 Getting Started

### Prerequisites

- .NET 10.0 SDK or later
- SQL Server (local, Express, or cloud instance)
- Visual Studio 2022 or VS Code (with C# extension)

### Installation

1. **Clone or navigate to the repository**

   ```bash
   cd Garment_Factory
   ```

2. **Restore Dependencies**

   ```bash
   dotnet restore
   ```

3. **Configure Connection String**
   Update the connection string in your API or Services project's appsettings.json:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=GarmentFactoryDb;User Id=sa;Password=YOUR_PASSWORD;Encrypt=false;"
     }
   }
   ```

4. **Apply Database Migrations**
   ```bash
   dotnet ef database update --project Database
   ```

## 📊 Database Models

### Core Entities

| Entity                  | Purpose                                                    |
| ----------------------- | ---------------------------------------------------------- |
| **ApplicationUser**     | System user accounts with authentication/authorization     |
| **Worker**              | Employee records with contact and advance information      |
| **Order**               | Production orders with trader and cost information         |
| **OrderModel**          | Order line items linking orders to specific garment models |
| **Fabric**              | Inventory management for fabrics and materials             |
| **Model**               | Garment designs/templates                                  |
| **Expense**             | Business expense tracking and reporting                    |
| **Revenue**             | Revenue records linked to orders and users                 |
| **Trader**              | External business partners and vendors                     |
| **AdvanceAndDeduction** | Employee salary advances and deductions                    |
| **RefreshTokenStore**   | Refresh token storage with expiry and revocation tracking  |
| **Phone**               | Contact phone numbers for users and traders                |

## 🔄 Migrations

The project uses Entity Framework Core migrations for version-controlled schema management. Current migrations include:

- `Initial_Migrate` (20260320232530): Initial database schema setup with all core entities and ASP.NET Core Identity tables
- `AddRefreshTokenStore` (20260321132814): Added RefreshTokenStore table with token expiry tracking, revocation support, and foreign key relationship to ApplicationUser

### Migration Details

The **RefreshTokenStore** migration creates a `RefreshTokens` table with:

- Token validation and management for JWT refresh token flow
- Automatic timestamp tracking (CreatedAt via `GETUTCDATE()`)
- Unique constraint on Token column for integrity
- Indexes on UserId, ExpiryDate, and Token for efficient queries
- Cascade delete when associated user is removed

### Creating New Migrations

```bash
# Generate a new migration
dotnet ef migrations add MigrationName --project Database

# Update database with new migration
dotnet ef database update --project Database

# Revert to previous migration (if needed)
dotnet ef database update PreviousMigrationName --project Database
```

## 🔐 Security Features

- **ASP.NET Core Identity Integration**: Built-in user authentication and authorization
- **User-Owned Entities**: Entities implementing `IUserOwner` interface track ownership for multi-tenant operations
- **Nullable Reference Types**: Enabled for compile-time null safety

## 🏛️ Architecture Patterns

- **Fluent API Configuration**: Entity relationships defined in configuration classes
- **Design-Time Factory**: `AppDbContextFactory` enables migrations without runtime services
- **User Context Injection**: HttpContextAccessor integration for automatic user tracking on entities
- **Base Model Class**: Common properties defined in parent `Model` class

## 💡 Best Practices

1. **Always use migrations** for schema changes—never modify the database directly
2. **Test migrations** in development before applying to production
3. **Use descriptive migration names** that indicate what changes they introduce
4. **Keep models focused** on their domain responsibility
5. **Configure relationships** using Fluent API in configuration classes for clarity

## 📝 Usage Example

```csharp
// Example: Adding a new order through the context
using (var context = new AppDbContext(options, httpContextAccessor))
{
    var order = new Order
    {
        Total_Cost = 1500.00m,
        Order_Date = DateOnly.FromDateTime(DateTime.Now),
        Trader_Id = 1,
        UserId = "user-123"
```

The Database project is the foundation for:

- **Repository Layer**: Provides data access abstractions
- **Services Layer**: Implements business logic using repositories
- **API Layer**: Exposes REST endpoints to clients

---

# Repository Layer

A comprehensive data access abstraction layer implementing the **Repository Pattern** and **Unit of Work Pattern**, providing clean separation of concerns and testability for the Garment Factory Management System.

## 📋 Overview

The **Repository** project provides a robust abstraction over the Entity Framework Core DbContext, enabling:

- **Decoupled Data Access**: Isolates business logic from database implementation details
- **Reusable Patterns**: Generic repository for common CRUD operations
- **Unit of Work**: Coordinated transaction management across multiple repositories
- **Testability**: Easy to mock repositories for unit testing
- **Consistency**: Centralized query logic and data manipulation

## 🏗️ Project Structure

```
Repository/
├── Interfaces/
│   ├── IGenericRepository.cs        # Base interface for all repositories
│   ├── IUnitOfWork.cs               # Unit of work coordinator interface
│   ├── IAdvanceAndDeductionRepository.cs
│   ├── IAuthRepository.cs
│   ├── IExpenseRepository.cs
│   ├── IFabricRepository.cs
│   ├── IModelRepository.cs
│   ├── IOrderModelRepository.cs
│   ├── IOrderRepository.cs
│   ├── IPhoneRepository.cs
│   ├── IRevenueRepository.cs
│   ├── ITraderRepository.cs
│   └── IWorkerRepository.cs
├── Implementations/
│   ├── GenericRepository.cs         # Base implementation for all repositories
│   ├── UnitOfWork.cs                # Unit of work implementation
│   ├── AdvanceAndDeductionRepository.cs
│   ├── AuthRepository.cs
│   ├── ExpenseRepository.cs
│   ├── FabricRepository.cs
│   ├── ModelRepository.cs
│   ├── OrderModelRepository.cs
│   ├── OrderRepository.cs
│   ├── PhoneRepository.cs
│   ├── RevenueRepository.cs
│   ├── TraderRepository.cs
│   └── WorkerRepository.cs
└── Extensions/
    └── RepositoryServiceExtensions.cs   # Dependency injection setup
```

## 🔄 Design Patterns

### Generic Repository Pattern

The `IGenericRepository<T>` interface provides common CRUD operations:

```csharp
IGenericRepository<T> : IDisposable
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<bool> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
    Task<int> SaveChangesAsync();
}
```

**Benefits**:

- Eliminates repetitive boilerplate code
- Provides consistent CRUD interface across all entities
- Easy to extend for specific entity needs

### Unit of Work Pattern

The `IUnitOfWork` interface coordinates multiple repositories:

```csharp
IUnitOfWork
{
    IAdvanceAndDeductionRepository AdvanceAndDeductionRepository { get; }
    IExpenseRepository ExpenseRepository { get; }
    IFabricRepository FabricRepository { get; }
    IModelRepository ModelRepository { get; }
    IOrderRepository OrderRepository { get; }
    IOrderModelRepository OrderModelRepository { get; }
    IRevenueRepository RevenueRepository { get; }
    ITraderRepository TraderRepository { get; }
    IWorkerRepository WorkerRepository { get; }
    IPhoneRepository PhoneRepository { get; }

    Task<int> SaveChangesAsync();
    Task<bool> BeginTransactionAsync();
    Task<bool> CommitTransactionAsync();
    Task<bool> RollbackTransactionAsync();
}
```

**Benefits**:

- Single entry point for data access operations
- Atomic operations with transaction support
- Simplified dependency injection (one interface instead of many)
- Coordinated SaveChanges across multiple repositories

## 📊 Repository Types

| Repository                        | Purpose                             | Key Features                           |
| --------------------------------- | ----------------------------------- | -------------------------------------- |
| **GenericRepository**             | Base CRUD operations for any entity | Async/await, IQueryable support        |
| **AuthRepository**                | Authentication & user operations    | Login, registration, token management  |
| **OrderRepository**               | Order-specific queries & operations | Order filtering, status updates        |
| **WorkerRepository**              | Employee data access                | Worker searches, salary tracking       |
| **ExpenseRepository**             | Business expense operations         | Expense filtering, reporting           |
| **RevenueRepository**             | Revenue tracking & queries          | Revenue calculations, reporting        |
| **FabricRepository**              | Fabric inventory management         | Stock tracking, material queries       |
| **TraderRepository**              | Business partner operations         | Trader searches, contact management    |
| **AdvanceAndDeductionRepository** | Employee financial operations       | Advance tracking, deduction management |
| **ModelRepository**               | Garment model catalog access        | Model searches, design queries         |
| **OrderModelRepository**          | Order line item operations          | Order detail access                    |
| **PhoneRepository**               | Contact information access          | Phone lookups, updates                 |

## 🔧 Technology Stack

- **.NET Framework**: .NET 10.0
- **Pattern**: Repository & Unit of Work
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection
- **Async Support**: Full async/await implementation

## 🚀 Getting Started

### Dependency Injection Setup

The `RepositoryServiceExtensions` class provides DI configuration:

```csharp
// In your Startup.cs or Program.cs
services.AddRepositoryServices(configuration);
```

This extension method registers:

- `IUnitOfWork` → `UnitOfWork` (Scoped lifetime)
- All specialized repositories
- Generic repository factory

### Using Repositories in Services

```csharp
public class OrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Order> GetOrderAsync(int orderId)
    {
        return await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
    }

    public async Task<bool> CreateOrderAsync(Order order)
    {
        await _unitOfWork.OrderRepository.AddAsync(order);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }
}
```

### Transaction Management

```csharp
public async Task<bool> UpdateOrderWithItemsAsync(Order order, List<OrderModel> items)
{
    try
    {
        await _unitOfWork.BeginTransactionAsync();

        await _unitOfWork.OrderRepository.UpdateAsync(order);
        foreach (var item in items)
        {
            await _unitOfWork.OrderModelRepository.AddAsync(item);
        }

        await _unitOfWork.SaveChangesAsync();
        return await _unitOfWork.CommitTransactionAsync();
    }
    catch
    {
        await _unitOfWork.RollbackTransactionAsync();
        return false;
    }
}
```

## 💡 Best Practices

1. **Always use Unit of Work** for coordinated operations across multiple entities
2. **Implement specific repositories** only when you need custom query logic beyond GenericRepository
3. **Keep repositories focused** on data access concerns—business logic goes in services
4. **Use async/await** consistently throughout your repository layer
5. **Leverage transactions** for operations that must succeed or fail together
6. **Mock repositories** in unit tests using the interfaces

## 🔗 Integration

The Repository layer integrates with:

- **Database Layer**: Uses AppDbContext for DbSet access
- **Services Layer**: Consumed by business logic implementations
- **API Layer**: Services expose repository functionality via endpoints

## 📞 Support & Contribution

For issues, questions, or contributions related to the database schema and models, please refer to the main project documentation.

## 📄 License

Part of the Garment Factory Management System project.
