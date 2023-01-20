using CrudOperationApi.Models;
using CrudOperationApi.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BrandContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BrandCS")));

builder.Services.AddScoped<IBrand, BrandRepository>();

var _logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext()
   // WriteTo.File("D:\\Projects\\DotNetCore\\WebApi\\ApiLog-.log", rollingInterval: RollingInterval.Day).
    .CreateLogger();
builder.Logging.AddSerilog(_logger);

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseAuthorization();

app.MapControllers();

app.Run();

