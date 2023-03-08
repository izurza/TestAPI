using Microsoft.EntityFrameworkCore;
using System.Configuration;
using TestAPI;
using TestAPI.Funciones;
using TestAPI.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(EmailOptions.EmailConf));
builder.Services.AddSingleton<MailRender>();
builder.Services.AddSingleton<MailSender>();
builder.Services.AddDbContext<TodoContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DBLocal")));
builder.Services.AddAutoMapper(typeof(MapperProfile));
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
