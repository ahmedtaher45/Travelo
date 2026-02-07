using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Stripe;
using System.Security.Claims;
using System.Text;
using Travelo.API.Middleware;
using Travelo.Application.Interfaces;
using Travelo.Application.Services.Auth;
using Travelo.Application.Services.Booking;
using Travelo.Application.Services.City;
using Travelo.Application.Services.FileService;
using Travelo.Application.Services.Flight;
using Travelo.Application.Services.Payment;
using Travelo.Application.Services.Room;
using Travelo.Application.Services.Ticket;
using Travelo.Application.Services.Wishlist;
using Travelo.Application.Services.WishlistItem;
using Travelo.Application.UseCases.Auth;
using Travelo.Application.UseCases.Carts;
using Travelo.Application.UseCases.Hotels;
using Travelo.Application.UseCases.Menu;
using Travelo.Application.UseCases.Restaurant;
using Travelo.Application.UseCases.Review;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;
using Travelo.Infrastracture.Repositories;
using Travelo.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

//Database Connection
var connectionString = builder.Configuration.GetConnectionString("IdentityConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDataProtection();

// Infrastructure & Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IRoomBookingRepository, RoomBookingRepository>();
builder.Services.AddScoped<IOAuthGoogleRepository, OAuthGoogleRepository>();
builder.Services.AddScoped<IJwtTokenRepository, JwtTokenRepository>();

// Services
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IFileService, Travelo.Application.Services.FileService.FileService>();
builder.Services.AddScoped<IFileServices, Travelo.Application.Services.FileService.FileServices>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IPaymentServices, PaymentServices>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IWishlistItemService, WishlistItemService>();
builder.Services.AddScoped<IGeneralBookingRepository, GeneralBookingRepository>();
builder.Services.AddScoped<IRoomService, RoomService>();
// Use Cases
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegisterUseCase>();
builder.Services.AddScoped<ChangePasswordUseCase>();
builder.Services.AddScoped<AddAdminUseCase>();
builder.Services.AddScoped<ForgotPasswordUseCase>();
builder.Services.AddScoped<ResetPasswordUseCase>();
builder.Services.AddScoped<ConfirmEmailUseCase>();
builder.Services.AddScoped<ResendConfirmEmailUseCase>();
builder.Services.AddScoped<GoogleLoginUseCase>();
builder.Services.AddScoped<GetUserProfileUseCase>();
builder.Services.AddScoped<UpdateUserProfileUseCase>();
builder.Services.AddScoped<SearchAvailableRoomsUseCase>();
builder.Services.AddScoped<GetAllHotelsUseCase>();


// Restaurant & Menu Use Cases
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

// Hotel Use Cases
builder.Services.AddScoped<AddHotelUseCase>();
builder.Services.AddScoped<GetFeaturedHotelsUseCase>();
builder.Services.AddScoped<GetHotelByIdUseCase>();
builder.Services.AddScoped<GetHotelRoomsUseCase>();
builder.Services.AddScoped<GetThingsToDoUseCase>();
builder.Services.AddScoped<GetSimilarHotelsUseCase>();
builder.Services.AddScoped<GetHotelReviewsUseCase>();


// Cart Use Cases
builder.Services.AddScoped<AddToCartUseCase>();
builder.Services.AddScoped<GetCartUseCase>();
builder.Services.AddScoped<RemoveCartItemUseCase>();
builder.Services.AddScoped<RemoveFromCartUseCase>();

//review Use Cases
builder.Services.AddScoped<AddReviewUseCase>();
builder.Services.AddScoped<GetAirlineReviewsUseCase>();
builder.Services.AddScoped<GetHotelReviewsUseCase>();
builder.Services.AddScoped<GetFlightReviewsUseCase>();
builder.Services.AddScoped<GetReviewByIdUseCase>();
builder.Services.AddScoped<GetUserReviewsUseCase>();
builder.Services.AddScoped<RemoveReviewUseCase>();
builder.Services.AddScoped<UpdateReviewUseCase>();



// Configuration & External
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfigruration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
StripeConfiguration.ApiKey=builder.Configuration["Stripe:SecretKey"];

// Identity Configuration
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.User.RequireUniqueEmail=true;
    options.SignIn.RequireConfirmedEmail=true;
    options.Password.RequireDigit=true;
    options.Password.RequiredLength=6;
    options.Password.RequireNonAlphanumeric=false;
    options.Password.RequireUppercase=true;
    options.Password.RequireLowercase=true;
    options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts=5;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
    opt.TokenLifespan=TimeSpan.FromHours(2));

// Authentication
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
        ClockSkew=TimeSpan.Zero,
        RoleClaimType=ClaimTypes.Role
    };
})
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


builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", policy =>
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
StripeConfiguration.ApiKey=builder.Configuration["Stripe:SecretKey"];
var app = builder.Build();
// 2. MIDDLEWARE PIPELINE
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    await SeedData.SeedAsync(context);
}

//if (app.Environment.IsDevelopment())
//{
app.MapOpenApi();
app.MapScalarApiReference();
//}

app.UseStaticFiles();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors("MyPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();