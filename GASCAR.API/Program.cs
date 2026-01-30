using GASCAR.API.Data;
using GASCAR.API.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Server;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("gascar"));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MWBotService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddHttpClient<OpenChargeMapService>();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowBlazor", p =>
        p.WithOrigins("http://localhost:5213", "http://localhost:5174", "http://localhost:5184")
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var jwtKey = builder.Configuration["Jwt:Key"] ?? "SUPER_SECRET_KEY_123456";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "SmartParking";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "SmartParkingUsers";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

var app = builder.Build();

// Seed database with initial data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(db);
}

app.UseCors("AllowBlazor");
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run("http://localhost:5184");
