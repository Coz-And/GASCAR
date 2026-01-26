using Microsoft.AspNetCore.Mvc;
using GASCAR.API.Service;
using GASCAR.API.Models;
using GASCAR.API.Models.Auth;

namespace GASCAR.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var token = await _authService.Register(dto);
        if (token == null)
            return BadRequest("User already exists");

        return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _authService.Login(dto.Email, dto.Password);
        if (token == null)
            return Unauthorized("Invalid credentials");

        return Ok(new { token });
    }
}
