using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicLib.API.Models.Database;

namespace MusicLib.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, DatabaseContext context)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public User Get()
    {
        var claims = HttpContext.User.Claims;
        

        return new User()
        {
            ID = Int32.Parse(claims.FirstOrDefault(c => c.Type == "UserId").Value),
            UserName = claims.FirstOrDefault(c => c.Type == "UserName").Value,
            DisplayName = claims.FirstOrDefault(c => c.Type == "DisplayName").Value,
            Email = claims.FirstOrDefault(c => c.Type == "Email").Value,
        };
    }
}