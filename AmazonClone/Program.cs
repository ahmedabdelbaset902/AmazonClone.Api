using AmazonClone.API.Extensions;
using AmazonClone.API.Middlewares;
using AmazonClone.Extensions;
using Stripe;



var builder = WebApplication.CreateBuilder(args);

// قراءة إعدادات JWT و Stripe
var jwtKey = builder.Configuration["Jwt:Key"];
var stripeSecret = Environment.GetEnvironmentVariable("STRIPE_SECRET");
StripeConfiguration.ApiKey = stripeSecret;


// Controllers
builder.Services.AddControllers();


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Custom Extensions
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAutoMapperConfig();
builder.Services.AddUnitOfWork();
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerDocumentation();

//  Serilog Extension
builder.Host.AddSerilogLogging();


var app = builder.Build();

// Exception Middleware 
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
