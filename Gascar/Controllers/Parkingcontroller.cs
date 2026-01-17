using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class ParkingController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
