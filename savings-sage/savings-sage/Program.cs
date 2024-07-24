using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using savings_sage.Context;
using savings_sage.Model;
using savings_sage.Service.Authentication;
using savings_sage.Service.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});

// Configure JSON options
builder.Services.AddMvc()
    .AddJsonOptions(opts =>
    {
        var enumConverter = new JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
        opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });


var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["ValidIssuer"],
            ValidAudience = jwtSettings["ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:IssuerSigningKey"])
            )
        };
        options.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["access_token"];
                return Task.CompletedTask;
            } 
        };
    });

builder.Services
    .AddIdentityCore<User>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddRoles<IdentityRole>() //Enable Identity roles 
    .AddEntityFrameworkStores<UsersContext>();

var rolesSection = builder.Configuration.GetSection("Roles");
var adminRole = rolesSection["Admin"];
var userRole = rolesSection["User"];

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequiredAdminRole", policy => policy.RequireRole(adminRole))
    .AddPolicy("RequiredUserRole", policy => policy.RequireRole(userRole))
    .AddPolicy("RequiredUserOrAdminRole", policy => policy.RequireRole(userRole, adminRole));

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();

// Add DbContext and repositories
var connectionString = builder.Configuration.GetConnectionString("Default");
//builder.Services.AddDbContext<SavingsSageContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<UsersContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddLogging();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<AuthenticationSeeder>();



// Access the configuration directly
var frontendUrl = builder.Configuration.GetValue<string>("frontend_url");

//todo cookies-t befejezni
// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(innerBuilder =>
    {
        innerBuilder.WithOrigins(frontendUrl).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});

var app = builder.Build();

using var
    scope = app.Services
        .CreateScope(); 
var authenticationSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();
authenticationSeeder.AddRoles();
authenticationSeeder.AddAdmin();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();