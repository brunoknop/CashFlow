using CashFlow.Api.Filters;
using CashFlow.Api.Middleware;
using CashFlow.Api.Token;
using CashFlow.Application;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure;
using CashFlow.Infrastructure.Extensions;
using CashFlow.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.EnableAnnotations();
    
    config.SwaggerDoc("v1", new OpenApiInfo
    { 
        Version = "v1",
        Title = "CashFlow.Api",
        Description = "Sistema de controle de gastos com relatórios",
        Contact = new OpenApiContact
        {
            Name = "Bruno Knop",
            Email = "brunog.knop@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/brunoknop/")
        }
    });
    
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = SecuritySchemeType.ApiKey
    });
    
    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddMvc(option => option.Filters.Add(typeof(ExceptionFilter)));
builder.Services.AddRouting(option => option.LowercaseUrls = true);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
builder.Services.AddHttpContextAccessor();

var signinKey = builder.Configuration.GetValue<string>("Settings:Jwt:SigninKey");
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = new TimeSpan(0),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signinKey!))
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if(builder.Configuration.IsTestEnvironment() is false)
    await MigrateDataBase();

app.Run();

async Task MigrateDataBase()
{
    await using var scope = app.Services.CreateAsyncScope();
    await DataBaseMigration.MigrateDataBase(scope.ServiceProvider);
}

namespace CashFlow.Api
{
    public partial class Program {}
}
