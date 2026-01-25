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
using Travelo.Application.Services.Booking;
using Travelo.Application.Services.City;
using Travelo.Application.Services.FileService;
using Travelo.Application.Services.Flight;
using Travelo.Application.Services.Payment;
using Travelo.Application.Services.Ticket;
using Travelo.Application.UseCases.Auth;
using Travelo.Application.UseCases.Carts;
using Travelo.Application.UseCases.Hotels;
using Travelo.Application.UseCases.Menu;
using Travelo.Application.UseCases.Restaurant;
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
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegisterUseCase>();
builder.Services.AddScoped<IFileService, Travelo.Application.Services.FileService.FileService>();
builder.Services.AddScoped<IFileServices, Travelo.Application.Services.FileService.FileServices>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<ChangePasswordUseCase>();
builder.Services.AddScoped<AddAdminUseCase>();
builder.Services.AddScoped<AddRestaurantUseCase>();
builder.Services.AddScoped<AddHotelUseCase>();
builder.Services.AddScoped<ICartRepository, CartRepository>();

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", policy =>
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});
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


builder.Services.AddScoped<AddRestaurantUseCase>();
builder.Services.AddScoped<GetRestaurantUseCase>();
builder.Services.AddScoped<UpdateRestaurantUseCase>();
builder.Services.AddScoped<RemoveRestaurantUseCase>();
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


builder.Services.AddScoped<AddToCartUseCase>();
builder.Services.AddScoped<GetCartUseCase>();
builder.Services.AddScoped<RemoveCartItemUseCase>();
builder.Services.AddScoped<RemoveFromCartUseCase>();


var app = builder.Build();

// 1. DATA SEEDING SECTION
// Create a temporary scope to resolve services and seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        // Ensure this method is awaited properly
        await IdentitySeeder.SeedRoles(roleManager);

        // If you need to seed initial data into the context:
        // var context = services.GetRequiredService<ApplicationDbContext>();
    }
    catch (Exception ex)
    {
        // Log errors during seeding
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// 2. MIDDLEWARE PIPELINE SECTION
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseStaticFiles();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors("MyPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Start the application
await app.RunAsync();

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
app.UseCors("MyPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


