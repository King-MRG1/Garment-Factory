# DatabaseCreation

A robust Entity Framework Core-based database layer for the Garment Factory Management System, providing comprehensive data models, migrations, and database context configuration for enterprise-level garment production and inventory management.

## 📋 Overview

The **DatabaseCreation** project establishes the foundational data access layer for the Garment Factory application. It leverages Entity Framework Core with SQL Server to manage all database operations, including:

- **Entity Models**: Comprehensive domain models for garment production, inventory, employees, and financial operations
- **Database Context**: ASP.NET Core Identity integration with custom DbContext implementation
- **Migrations**: Version-controlled database schema evolution with support for multi-user environments
- **Configuration**: Fluent API configurations for complex entity relationships and constraints

## 🏗️ Project Structure

```
DatabaseCreation/
├── Data/
│   ├── AppDbContext.cs              # Main database context with Identity integration
│   ├── AppDbContextFactory.cs       # Design-time context factory for migrations
│   └── Configurations/              # Entity configuration classes
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
   dotnet ef database update --project DatabaseCreation
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
| **Phone**               | Contact phone numbers for users and traders                |

## 🔄 Migrations

The project uses Entity Framework Core migrations for version-controlled schema management. Current migrations include:

- `InitialCreate`: Initial database schema setup
- `addInventoryToToken`: Added inventory tracking features
- `addDateFabric`: Added date tracking to fabric entities
- And subsequent refinements

### Creating New Migrations

```bash
# Generate a new migration
dotnet ef migrations add MigrationName --project DatabaseCreation

# Update database with new migration
dotnet ef database update --project DatabaseCreation

# Revert to previous migration (if needed)
dotnet ef database update PreviousMigrationName --project DatabaseCreation
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

The DatabaseCreation project is the foundation for:

- **Repository Layer**: Provides data access abstractions
- **Services Layer**: Implements business logic using repositories
- **API Layer**: Exposes REST endpoints to clients

## 📞 Support & Contribution

For issues, questions, or contributions related to the database schema and models, please refer to the main project documentation.

## 📄 License

Part of the Garment Factory Management System project.
