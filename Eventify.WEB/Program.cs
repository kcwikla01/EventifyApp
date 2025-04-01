using Autofac;
using Autofac.Extensions.DependencyInjection;
using Eventify.Database.DbContext;
using Eventify.WEB.Autofac;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using AutoMapper;
using Eventify.Automapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS configuration to allow requests from the frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder =>
    {
        builder.WithOrigins("http://localhost:5173")  // Frontend URL (React app)
               .AllowAnyMethod()                    // Allow all HTTP methods (GET, POST, PUT, etc.)
               .AllowAnyHeader();                   // Allow any headers (Content-Type, Authorization, etc.)
    });
});

// Add AutoMapper configuration
builder.Services.AddAutoMapper(typeof(MapperProfile));

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext to interact with the database
builder.Services.AddDbContext<EventifyDbContext>();

// Configure Autofac dependency injection
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new AutofacModule());
});

var app = builder.Build();

// Apply database migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EventifyDbContext>();
    dbContext.Database.Migrate();  // Ensure database is up-to-date
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS with the "AllowLocalhost" policy
app.UseCors("AllowLocalhost");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();  // Map controller routes

app.Run();
