using Application.Common.Interfaces.EventInterfaces;
using Application.Common.Interfaces.RepositoryInterfaces;
using Application.DTOs;
using Application.FinancingLeads.Commands.SubmitFinancingLead;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Notifications;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);
var applicationAssembly = typeof(SubmitFinancingLeadRequest).Assembly;

// Add services to the container.
var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options
                    .UseSqlServer(ConnectionString)
                    );

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IFinancingLeadRepository, FinancingLeadRepository>();

builder.Services.AddHttpClient<INotification, FirebaseNotification>(client =>
{
    // Placeholder Base URL for Firebase Cloud Messaging (FCM) API
    client.BaseAddress = new Uri("https://fcm.googleapis.com/");
    // In a real app, the server key would be set here as a default Authorization header
});

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(applicationAssembly)
);

builder.Services.AddValidatorsFromAssembly(applicationAssembly);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClientAccess", policy =>
    {
        policy.AllowAnyHeader()
            .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE")
            .AllowAnyOrigin();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
