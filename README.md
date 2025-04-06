Eventify is a simple and easy-to-use application that helps people manage events from start to finish. Our goal is to provide event organizers with a powerful tool to plan, organize, and run events smoothly. With features like event scheduling, attendee management, ticketing, and real-time updates, Eventify aims to make the event experience better for both organizers and attendees. 
We want to create a platform that works for all types of events, whether it’s a small meeting or a large conference, while keeping it simple and user-friendly. Our mission is to change the way events are managed, saving time and making events more enjoyable for everyone involved.

Before run application we should install sqlExpress or change SqlConnectionString at appsettings.json file <br>
Next we should at package managare console at Eventify.Database project write this commands: <br>
-dotnet tool install --global dotnet-ef <br>
-dotnet ef database update --startup-project ../Eventify.WEB --project .
