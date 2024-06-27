using System.Text.Json.Serialization;
using savings_sage.Context;
using savings_sage.Service.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddMvc()
    .AddJsonOptions(opts =>
    {
        var enumConverter = new JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
        opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddDbContext<SavingsSageContext>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddLogging();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();
//
// builder.Services.AddCors(options =>
// {
//     var frontendURL = configuration.GetValue<string>("frontend_url");
//     
//     options.AddDefaultPolicy(builder =>
//     {
//         builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader();
//     });
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();
app.Run();