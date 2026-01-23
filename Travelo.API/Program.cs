using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using Travelo.API.Middleware;
using Travelo.Application.Interfaces;
using Travelo.Application.Services.Auth;
using Travelo.Application.UseCases.Auth;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;
using Travelo.Infrastracture.Identity;
using Travelo.Infrastracture.Repositories;


var builder = WebApplication.CreateBuilder(args);
//Database Connection
var connectionString = builder.Configuration.GetConnectionString("IdentityConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

// Add services to the container.
builder.Services.AddControllers();


builder.Services.AddOpenApi();
builder.Services.AddScoped<Travelo.Application.Interfaces.IEmailSender, EmailSender>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegisterUseCase>();
builder.Services.AddScoped<ChangePasswordUseCase>();

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
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero 
    };
});
builder.Services.AddDataProtection();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(
    typeof(IGenericRepository<>),
    typeof(GenericRepository<>)
);

builder.Services.AddScoped<RegisterUseCase>();
builder.Services.AddScoped<Travelo.Application.UseCases.Hotels.GetFeaturedHotelsUseCase>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", policy =>
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
opt.TokenLifespan=TimeSpan.FromHours(2));

builder.Services.AddAuthentication(options => 
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}
    
)
.AddCookie(IdentityConstants.ApplicationScheme)
.AddCookie(IdentityConstants.ExternalScheme)
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Google:ClientID"];
    options.ClientSecret = builder.Configuration["Google:ClientSecret"];
    options.SaveTokens = true;
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.ClaimActions.MapJsonKey("picture", "picture");
});

builder.Services.AddScoped<IOAuthGoogleRepository, OAuthGoogleRepository>();
builder.Services.AddScoped<IJwtTokenRepository, JwtTokenRepository>();
builder.Services.AddScoped<GoogleLoginUseCase>();

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfigruration>();
builder.Services.AddSingleton(emailConfig);
//builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<ForgotPasswordUseCase>();
builder.Services.AddScoped<ResetPasswordUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors("MyPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentitySeeder.SeedRoles(roleManager);
}
app.Run();
