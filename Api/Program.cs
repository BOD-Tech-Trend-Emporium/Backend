using Api.src.Product.domain.repository;
using Api.src.Product.infraestructure.api;
using backend.Data;
using backend.src.User.application.service;
using backend.src.User.domain.repository;
using backend.src.User.infraestructure.api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDBContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("server"));
});

builder.Services.AddScoped<UserRepository, UserService>();
builder.Services.AddScoped<ProductRepository, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsEnvironment("Local"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
    Console.WriteLine("Local bro");
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();