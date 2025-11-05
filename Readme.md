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


How to Complile to EXE

In the command line, run: 
- `dotnet publish PTANonCrown.csproj -c Release -f:net8.0-windows10.0.19041.0 --self-contained true -p:PublishSingleFile=true -o ./publish`
- 

Errors:
- Unable to create a 'DbContext' of type 'AppDbContext'... Unable to resolve service for type 'DbContextOptions'
- When doing `dotnet ef database update`:
		- Message: `Applying migration '20250618123936_EastingNorthing'. The migration operation 'PRAGMA foreign_keys = 0;' from migration 'EastingNorthing' cannot be executed in a transaction. If the app is terminated or an unrecoverable error occurs while this operation is being executed then the migration will be left in a partially applied state and would need to be reverted manually before it can be applied again. Create a separate migration that contains just this operation. Done.`
		- Solution: Delete the database in `C:\Users\<User>\AppData\Local\Packages\<AppFolder>\LocalState` and try again.



Updating Lookups
- RefreshLookupsAsync is done in App.xaml.cs