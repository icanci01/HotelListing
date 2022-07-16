# HotelListing

## SQL Server Migrations

**You can create your SQL Server migrations using PMC (Package Manager Console) or CLI**

_PMC (Package Manager Console)_

- `add-migration InitialCreate`
- `update-database -verbose`
- `remove-migration`
- `script-migration`

_CLI (Command Line Interface)_

- `dotnet ef migrations add InitialCreate`
- `dotnet ef database update`
- `dotnet ef migrations remove`
- `dotnet ef migrations script`
