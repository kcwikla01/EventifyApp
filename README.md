Before run application we should install sqlExpress or change SqlConnectionString at appsettings.json file
Next we should at package managare console at Eventify.Database project write this commands
dotnet tool install --global dotnet-ef
dotnet ef database update --startup-project ../Eventify.WEB --project .
