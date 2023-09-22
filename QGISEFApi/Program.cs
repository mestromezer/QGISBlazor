using QGISEFApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QGISEFApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<QGISEFApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QGISEFApiContext") 
    ?? throw new InvalidOperationException("Connection string 'QGISEFApiContext' not found."), x => x.UseNetTopologySuite()));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
