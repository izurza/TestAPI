using Microsoft.EntityFrameworkCore;
using System.Configuration;
using TestAPI.Models;


var builder = WebApplication.CreateBuilder(args);
var dbConfig = builder.Configuration.GetSection("Database").Get<DatabaseSettings>();
string _connectionString = dbConfig.ConnectionString;
string _connectionString = builder.Configuration.U;
Console.WriteLine("Hola");
// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDbContext<TodoContext>(opt => opt.UseSqlServer("Data Source=DESKTOP-5D225VS;Initial Catalog=Test;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False" ?? throw new Exception("Missing Connection String")));
builder.Services.AddDbContext<TodoContext>(opt => opt.UseSqlServer(_connectionString ?? throw new Exception("Missing Connection String")));
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
