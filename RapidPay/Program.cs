using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using RapidPay.Application.Common;
using RapidPay.Application.Features.CardFeatures.Commands.CreateCard;
using RapidPay.Application.Features.UserFeatures.Commands.Login;
using RapidPay.Application.Mapper;
using RapidPay.Application.Repository;
using RapidPay.Application.Repository.Common;
using RapidPay.Application.Utils;
using RapidPay.Entities.Context;
using RapidPay.Filters;
using RapidPay.Persistance.Repository;
using RapidPay.Persistance.Repository.Common;
using RapidPay.Settings;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Logging.ClearProviders();
builder.Host.UseNLog(); // Loads NLog configuration from nlog.config by default


builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("localDb"));
});

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateCardCommandHandler).Assembly));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

// Register Repositories
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IAuthorizationLogRepository, AuthorizationLogRepository>();
builder.Services.AddScoped<IPaymentFeeRepository, PaymentFeeRepository>();
builder.Services.AddScoped<ICardChangeLogRepository, CardChangeLogRepository>();

builder.Services.AddSingleton<IUniversalFeesExchange, UniversalFeesExchange>();
builder.Services.AddHostedService(sp => (UniversalFeesExchange)sp.GetRequiredService<IUniversalFeesExchange>());


builder.Services.Configure<EncryptionSettings>(builder.Configuration.GetSection("EncryptionSettings"));
builder.Services.AddSingleton<IEncryptionService>(sp =>
{
    var encryptionSettings = sp.GetRequiredService<IOptions<EncryptionSettings>>().Value;
    return new EncryptionService(encryptionSettings.Key);
});

builder.Services.AddHttpClient("MyApi", client =>
{
    client.BaseAddress = new Uri("https://api.example.com");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
    client.DefaultRequestHeaders.Add("User-Agent", "MyApp/1.0");
});

// Register FluentValidation validators
builder.Services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();
builder.Services.AddFluentValidationAutoValidation();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add Authorization
builder.Services.AddAuthorization();

// Add Swagger
//this is just for Swagger, to allow you to insert a Bearer token for testing
builder.Services.AddSwaggerGen(x => {
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your JWT token."
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }

    });
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await using var scope = app.Services.CreateAsyncScope();
await using var db = scope.ServiceProvider.GetService<AppDbContext>();
await db.Database.MigrateAsync();

app.Run();
