Migrations

- Migrations are done using the PTANonCrown.Data project; this is where the models are defined. 
- EF Core can't rely on MAUI Program.cs for dependency injection, or FileSystem.AppDataDirectory.
- We use: 
`public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>`
- It builds a DB Context

How to Make Schema Changes: 
- Update the files in `PTANonCrown/PTANonCrown.Data/Models`
- Open CMD prompt in root `PTANonCrown`
- Delete the folder in AppData folder:
	- e.g., `C:\Users\PETER21\AppData\Local\Packages\com.companyname.PTANonCrown_9tvtrkeae8602\LocalState\app.db`
- Run `dotnet ef migrations add MyMigration --project PTANonCrown.Data`
- Run `dotnet ef database update --project PTANonCrown.Data`

Errors:
- Unable to create a 'DbContext' of type 'AppDbContext'... Unable to resolve service for type 'DbContextOptions'