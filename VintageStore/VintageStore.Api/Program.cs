using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Text;
using VintageStore.Domain.Configuration;
using VintageStore.Persistence;
using VintageStore.Repositories.Implementations;
using VintageStore.Repositories.Interfaces;
using VintageStore.Services.Implementations;
using VintageStore.Services.Interfaces;
using VintageStore.Services;
using VintageStore.Repositories;
using VintageStore.Services.Profiles;
using VintageStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var logger = new LoggerConfiguration()
    .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("VintageStoreDb"),
        new MSSqlServerSinkOptions
        {
            AutoCreateSqlDatabase = true,
            AutoCreateSqlTable = true,
            TableName = "ApiLogs"
        }, restrictedToMinimumLevel: LogEventLevel.Warning)
    .CreateLogger();
builder.Logging.AddSerilog(logger);

if (builder.Environment.IsDevelopment())
{
    var loggerFile = new LoggerConfiguration()
        .WriteTo.Console(LogEventLevel.Information)
        .WriteTo.File("..\\log.txt",
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] - {Message}{NewLine}{Exception}",
            rollingInterval: RollingInterval.Day,
            restrictedToMinimumLevel: LogEventLevel.Warning)
    .CreateLogger();

    builder.Logging.AddSerilog(loggerFile);
}
builder.Services.Configure<AppConfig>(builder.Configuration);

var corsConfiguration = "VintageStoreAPI";

builder.Services.AddCors(setup =>
{
    setup.AddPolicy(corsConfiguration, policy =>
    {
        policy.AllowAnyOrigin(); // Que cualquiera pueda consumir el API

        //policy.WithOrigins(new[] { "https://localhost:5500" }); // Solo estos dominios pueden consumir el API
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();

    });
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddDbContext<VintageStoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("VintageStoreDb"));

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

builder.Services.AddIdentity<VintageStoreUserIdentity, IdentityRole>(policies =>
{
    policies.Password.RequireDigit = true;
    policies.Password.RequireLowercase = false;
    policies.Password.RequireUppercase = false;
    policies.Password.RequireNonAlphanumeric = false;
    policies.Password.RequiredLength = 6;

    policies.User.RequireUniqueEmail = true;

    // Politicas de bloqueo de cuentas
    policies.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    policies.Lockout.MaxFailedAccessAttempts = 5;
    policies.Lockout.AllowedForNewUsers = true;
}).AddEntityFrameworkStores<VintageStoreDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddRepositories()
    .AddServices()
    .AddUploader(builder.Configuration);

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<DataProfile>();
    config.AddProfile<ProductoProfile>();
    config.AddProfile<VentaProfile>();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Vintage Store API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "Autenticacion por JWT usando como ejemplo en el header: Authorization: Bearer [token]",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ??
                                     throw new InvalidOperationException("No se configuro el JWT"));

    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Emisor"],
        ValidAudience = builder.Configuration["Jwt:Audiencia"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
//builder.Services.AddTransient<IGenreRepository, GenreRepository>();
//builder.Services.AddTransient<IGenreService, GenreService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(corsConfiguration);

app.MapControllers();

app.MapReports();
app.MapHomeEndpoints();



using (var scope = app.Services.CreateScope())
{
   
    // Aqui vamos a ejecutar la creacion del usuario admin y los roles por default.
    await UserDataSeeder.Seed(scope.ServiceProvider);


}


app.Run();

