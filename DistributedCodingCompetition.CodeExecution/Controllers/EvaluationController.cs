namespace DistributedCodingCompetition.CodeExecution.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class EvaluationController(ILogger<EvaluationController> logger) : ControllerBase
{

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
