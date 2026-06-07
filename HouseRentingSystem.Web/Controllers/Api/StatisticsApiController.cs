using HouseRentingSystem.Services.Statistics;
using HouseRentingSystem.Services.Statistics.Models;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Web.Controllers.Api;

[ApiController]
[Route("api/statistics")]
public class StatisticsApiController(IStatisticsService statistics) : ControllerBase
{
    [HttpGet]
    public StatisticsServiceModel GetStatistics()
        => statistics.Total();
}