using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace APIEndpoint.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private static readonly List<WeatherForecast> WeatherForecasts = new();

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    // GET: /weatherforecast
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return WeatherForecasts;
    }

    // POST: /weatherforecast
    [HttpPost]
    public ActionResult<WeatherForecast> Create(WeatherForecast forecast)
    {
        WeatherForecasts.Add(forecast);
        return CreatedAtAction(nameof(Get), forecast);
    }

    // PUT: /weatherforecast/{index}
    [HttpPut("{index}")]
    public ActionResult Update(int index, WeatherForecast forecast)
    {
        if (index < 0 || index >= WeatherForecasts.Count)
        {
            return NotFound();
        }

        WeatherForecasts[index] = forecast;
        return NoContent();
    }

    // DELETE: /weatherforecast/{index}
    [HttpDelete("{index}")]
    public ActionResult Delete(int index)
    {
        if (index < 0 || index >= WeatherForecasts.Count)
        {
            return NotFound();
        }

        WeatherForecasts.RemoveAt(index);
        return NoContent();
    }
}