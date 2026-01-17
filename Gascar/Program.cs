using Gascar.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.Run();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=gascar.db"));

builder.Services.AddSession();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
    });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();