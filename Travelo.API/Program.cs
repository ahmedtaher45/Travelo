using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Stripe;
using System.Text;
using Travelo.API.Middleware;
using Travelo.Application.Interfaces;
using Travelo.Application.Services.Auth;
using Travelo.Application.Services.City;
using Travelo.Application.Services.FileService;
using Travelo.Application.Services.Flight;
using Travelo.Application.Services.Payment;
using Travelo.Application.UseCases.Auth;
using Travelo.Application.UseCases.Hotels;
using Travelo.Application.UseCases.Menu;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;
using Travelo.Infrastracture.Identity;
using Travelo.Infrastracture.Repositories;
using FileService = Travelo.Application.Services.FileService.FileService;


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
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegisterUseCase>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
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
    options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters=new TokenValidationParameters
    {
        ValidateIssuer=true,
        ValidateAudience=true,
        ValidateLifetime=true,
        ValidateIssuerSigningKey=true,
        ValidIssuer=jwtSettings["Issuer"],
        ValidAudience=jwtSettings["Audience"],
        IssuerSigningKey=new SymmetricSecurityKey(key),
        ClockSkew=TimeSpan.Zero
    };
});
builder.Services.AddDataProtection();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped(
    typeof(IGenericRepository<>),
    typeof(GenericRepository<>)
);

builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

builder.Services.AddScoped<RegisterUseCase>();
builder.Services.AddScoped<Travelo.Application.UseCases.Hotels.GetFeaturedHotelsUseCase>();
builder.Services.AddScoped<GetFeaturedHotelsUseCase>();
builder.Services.AddScoped<GetHotelByIdUseCase>();


builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
opt.TokenLifespan=TimeSpan.FromHours(2));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme=CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme=GoogleDefaults.AuthenticationScheme;
}

)
.AddCookie(IdentityConstants.ApplicationScheme)
.AddCookie(IdentityConstants.ExternalScheme)
.AddGoogle(options =>
{
    options.ClientId=builder.Configuration["Google:ClientID"];
    options.ClientSecret=builder.Configuration["Google:ClientSecret"];
    options.SaveTokens=true;
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.ClaimActions.MapJsonKey("picture", "picture");
});

builder.Services.AddScoped<IOAuthGoogleRepository, OAuthGoogleRepository>();
builder.Services.AddScoped<IJwtTokenRepository, JwtTokenRepository>();
builder.Services.AddScoped<GoogleLoginUseCase>();

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfigruration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentServices, PaymentServices>();
builder.Services.AddScoped<IRoomBookingRepository, RoomBookingRepository>();
builder.Services.AddScoped<ForgotPasswordUseCase>();
builder.Services.AddScoped<ResetPasswordUseCase>();
builder.Services.AddScoped<ConfirmEmailUseCase>();
builder.Services.AddScoped<ResendConfirmEmailUseCase>();
builder.Services.AddScoped<GetMenuUseCase>();
builder.Services.AddScoped<GetItemUseCase>();
builder.Services.AddScoped<AddCategoryUseCase>();
builder.Services.AddScoped<AddItemUseCase>();
builder.Services.AddScoped<DeleteItemUseCase>();
builder.Services.AddScoped<UpdateItemUseCase>();
builder.Services.AddScoped<UpdateCategoryUseCase>();
builder.Services.AddScoped<DeleteCategoryUseCase>();
// Configure Stripe settings
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
StripeConfiguration.ApiKey=builder.Configuration["Stripe:SecretKey"];
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // 2️⃣ Seed Hotel & Rooms using EF Core
    var context = services.GetRequiredService<ApplicationDbContext>();

    if (!context.Hotels.Any())
    {
        // Seed hotel
        var hotel = new Hotel
        {
            Name="Travelo Grand Hotel",
            ResponsibleName="Norma Khanafseh",
            Address="Main Street 45",
            Country="Palestine",
            CityId=1, // Make sure city with Id=1 exists
            Latitude=31.9522,
            Longitude=35.2332,
            PricePerNight=150,
            Rating=4.6,
            ReviewsCount=120,
            ImageUrl="https://picsum.photos/800/600",
            IsFeatured=true,
            Description="Luxury hotel located in the city center with modern rooms."
        };

        context.Hotels.Add(hotel);

        // Seed rooms
        var rooms = new List<Room>
        {
            new Room
            {
                HotelId = hotel.Id,
                Type = "Single Room",
                PricePerNight = 80,
                Capacity = 1,
                View = "City View",
                BedType = "Single Bed",
                Size = 20,
                ImageUrl = "https://picsum.photos/400/300",
                IsAvailable = true,
                Hotel = hotel
            },
            new Room
            {
                HotelId = hotel.Id,
                Type = "Double Room",
                PricePerNight = 120,
                Capacity = 2,
                View = "Sea View",
                BedType = "Queen Bed",
                Size = 30,
                ImageUrl = "https://picsum.photos/401/300",
                IsAvailable = true,
                Hotel = hotel
            },
            new Room
            {
                HotelId = hotel.Id,
                Type = "Family Suite",
                PricePerNight = 200,
                Capacity = 4,
                View = "Garden View",
                BedType = "King Bed",
                Size = 50,
                ImageUrl = "https://picsum.photos/402/300",
                IsAvailable = true,
                Hotel= hotel
            }
        };

        context.Rooms.AddRange(rooms);

        await context.SaveChangesAsync();
    }
}
// ============================================
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseStaticFiles();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentitySeeder.SeedRoles(roleManager);
}
app.Run();
