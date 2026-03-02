# Migrations

- Migrations are done using the PTANonCrown.Data project; this is where the models are defined. 
- EF Core can't rely on MAUI Program.cs for dependency injection, or FileSystem.AppDataDirectory.
- We use: 
`public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>`
- It builds a DB Context

## How to Make Schema Changes: 
- Update the files in `PTANonCrown/PTANonCrown.Data/Models`
- Open CMD prompt in root `PTANonCrown`
- Delete the folder in AppData folder and the `app.db` file:
	- e.g., `C:\Users\PETER21\AppData\Local\Packages\com.companyname.PTANonCrown_9tvtrkeae8602`
	- e.g., `C:\Users\PETER21\AppData\Local\app.db`
- Run `dotnet ef migrations add MyMigration --project PTANonCrown.Data`
- Run `dotnet ef database update --project PTANonCrown.Data`


## How to Complile to EXE

In the command line, run: 
- `dotnet publish PTANonCrown.csproj -c Release -f:net8.0-windows10.0.19041.0 --self-contained true -p:PublishSingleFile=true -o ./publish`
- 

### Errors:
- Unable to create a 'DbContext' of type 'AppDbContext'... Unable to resolve service for type 'DbContextOptions'
- When doing `dotnet ef database update`:
		- Message: `Applying migration '20250618123936_EastingNorthing'. The migration operation 'PRAGMA foreign_keys = 0;' from migration 'EastingNorthing' cannot be executed in a transaction. If the app is terminated or an unrecoverable error occurs while this operation is being executed then the migration will be left in a partially applied state and would need to be reverted manually before it can be applied again. Create a separate migration that contains just this operation. Done.`
		- Solution: Delete the database in `C:\Users\<User>\AppData\Local\Packages\<AppFolder>\LocalState` and try again.


### Database Contexts:
- Database connections were causing a lot of headaches, because I was trying to switch database connections midway, when the user opens/saves/creates new file. 
- The issue stemps that when you e.g. save a temp file to a new location, it creates a new DB connection, but then all the Entity Framework tracking gets messed up. 
- Solution is to maintain a single connection to a working file in the LocalCache folder. When you save, it saves to that location, and then simply copies that 
file to the save location. It is never actually creating a database connection with the save location, even if it looks 
to the user like you are saving directly there.
- Need to maintain the context alive during the editing sessions, so no "using var context"; instead, "var context". And then the GetContext() method returns the existing context.

### Updating Lookups
- Lookup files are stored in `PTANonCrown.Data/Resources`
- RefreshLookupsAsync is done in App.xaml.cs



# Publishing 

## Setup.EXE / .MSI
Pre-requisite: Download and install `wix314.exe` from https://github.com/wixtoolset/wix3/releases.

Need to: 
1. Build the project: Right click on the `PTANonCrown` and click `Build`
2. Build the `.MSI` file: Right click on `PTANonCrownWix` and click `Build` 
	- This bundles the build files in a single file
	- `WIX` is used; this does two things: creates `Files.wsx` which lists all files that need to be included in the `.MSI`
3. Create the `setup` file: Right click on the `PTANonCrownBootstrapper` and click `Build`

The setup file acts as a wrapper


## .MSIX

2026-02-19:
Too many issues using this format. We went through the process of getting a certificate from `SSL.COM`, and finally got the bulid to `.MSIX` working. 
However, main issue is that many 

### SSL Security Certificate Steps

1. Go to https://secure.ssl.com/team/a48-1kkalsv/signed_certificates
2. Click the order number (“co-...” hyperlink)
3. Follow the steps here: https://www.ssl.com/guide/clickonce-esigner-cloud-key-adapter-integration/
4. Enter 4 Digit Pin (2026):  This gives you the “Secret Code” 
5. May need to sign up for the OTP; customer support will send you a QR code, which you scan with your phone, and then in your Authenticator app (e.g., on your work phone), it gives e.g. a 6-digit code that you type into the website when prompted When you follow the steps in “clickonce-esigner-cloud-key-adapter-integration/”, it adds the certificates to the certmgr.msc. 
6. Make sure you build the project (Release, Any CPU)
7. When you publish, it just takes these built files and packages them up in MSIX etc. 
8. Right click on CSPROJ
9. Click Publish 
10. Select “Sideloading” 
11. Select Signing Method: 
- Yes, Select A Certificate
- Select From Store