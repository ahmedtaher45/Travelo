using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;
using Travelo.Infrastracture.Repositories;

var builder = WebApplication.CreateBuilder(args);
//Database Connection
var connectionString = builder.Configuration.GetConnectionString("IdentityConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


// Add services to the container.
builder.Services.AddControllers();


builder.Services.AddOpenApi();

//Identity Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.User.RequireUniqueEmail=true;
    options.SignIn.RequireConfirmedEmail=true;

    // Password settings 
    options.Password.RequireDigit=true;
    options.Password.RequiredLength=6;
    options.Password.RequireNonAlphanumeric=false;
    options.Password.RequireUppercase=true;
    options.Password.RequireLowercase=true;

    // Lockout settings 
    options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts=5;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDataProtection();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(
    typeof(IGenericRepository<>),
    typeof(GenericRepository<>)
);
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
opt.TokenLifespan=TimeSpan.FromHours(2));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
