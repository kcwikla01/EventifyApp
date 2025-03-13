Before run application we should install sqlExpress or change SqlConnectionString at appsettings.json file <br>
Next we should at package managare console at Eventify.Database project write this commands: <br>
-dotnet tool install --global dotnet-ef <br>
-dotnet ef database update --startup-project ../Eventify.WEB --project .
