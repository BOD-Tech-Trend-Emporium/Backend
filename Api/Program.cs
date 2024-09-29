using System.Text;
using System.Text.Json.Serialization;
using Api.src.Auth.application.validations;
using Api.src.Auth.domain.repository;
using Api.src.Auth.infraestructure;
using Api.src.Cart.domain.repository;
using Api.src.Cart.infraestructure.api;
using Api.src.CartToProduct.domain.repository;
using Api.src.CartToProduct.infraestructure.api;
using Api.src.Category.domain.repository;
using Api.src.Category.infraestructure.api;
using Api.src.Common.middleware;
using Api.src.Favorite.domain.repository;
using Api.src.Favorite.infraestructure.api;
using Api.src.Product.domain.repository;
using Api.src.Product.infraestructure.api;
using backend.Data;
using backend.src.User.application.service;
using backend.src.User.domain.repository;
using backend.src.User.infraestructure.api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
        Description = "Standard AUthorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:TokenKey").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddDbContext<ApplicationDBContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("server"));
});

builder.Services.AddScoped<UserRepository, UserService>();
builder.Services.AddScoped<AuthRepository, AuthService>();
builder.Services.AddScoped<CategoryRepository, CategoryService>();
builder.Services.AddScoped<ProductRepository, ProductService>();
builder.Services.AddScoped<CartRepository, CartService>();
builder.Services.AddScoped<CartToProductRepository, CartToProductService>();
builder.Services.AddScoped<FavoriteRepository, FavoriteService>();

var app = builder.Build();

//Middleware for exceptions
app.UseMiddleware<ErrorhandlingMiddleware>();

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
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();