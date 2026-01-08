using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddOpenApi();

//Identity Configuration
builder.Services.AddDbContext<UserIdentityDbContex>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
