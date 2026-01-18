using Gascar.Data;
using Gascar.Models;
using Gascar.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddHostedService<MWbotService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
await DbSeeder.SeedAsync(app.Services);
app.Run();
