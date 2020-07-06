# Setup

#### Secrets
The application `ExporterWeb/` uses secrets in `secrets.json` which is located in
```
# Linux / macOS
~/.microsoft/usersecrets/7f33acfa-d777-41be-857e-6f6d4a582394/secrets.json

# Windows
%APPDATA%\Microsoft\UserSecrets\7f33acfa-d777-41be-857e-6f6d4a582394\secrets.json
```
https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=linux

However, you don't have to touch the file. You can add secrets via the `dotnet` CI tool.

For example, in `ExporterWeb/`, run
```bash
dotnet user-secrets set "ConnectionStrings:ExportersDbConnection" "Server=(localdb)\\MSSQLLocalDB;Database=MyExporterDatabase;Trusted_Connection=True;MultipleActiveResultSets=true"

dotnet user-secrets set "db:administrators:admin@example.com" "1234qwE!"
dotnet user-secrets set "db:managers:manager1@example.com" "1234qwE!"
dotnet user-secrets set "db:managers:manager2@example.com" "1234qwE!"
```
This command will add the connection string to your local database `MyExporterDatabase` and properties to create 1 administrator and 2 managers. You can create as many administrators/managers as you want adding them in this way.

# Run

When you just cloned the repository and added all the secrets, you should run the `ExporterWeb/` in the RELEASE mode once. In that mode, **the migrations** and **your data in `"db:..."`** will apply.

Then you can switch back to the DEBUG mode and run the application as usual.

#### Result
After you have built and run, go to `https://localhost:PORT/_test` to see the result.

`PORT` may be found in `ExporterWeb/Properties/launchSettings.json`, in the `sslPort` key
