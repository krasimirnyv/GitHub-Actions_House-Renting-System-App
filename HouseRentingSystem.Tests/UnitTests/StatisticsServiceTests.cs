using HouseRentingSystem.Services.Statistics.Models;
using HouseRentingSystem.Services.Statistics;

namespace HouseRentingSystem.Tests.UnitTests;

[TestFixture]
public class StatisticsServiceTests : UnitTestsBase
{
    private IStatisticsService statisticsService;

    [OneTimeSetUp]
    public void SetUp()
        => statisticsService = new StatisticsService(data);

    [Test]
    public void Total_ShouldReturnCorrectCounts()
    {
        // Arrange

        // Act: invoke the service method
        StatisticsServiceModel result = statisticsService.Total();

        // Assert the returned result is not null
        Assert.IsNotNull(result);

        // Assert the returned houses' count is correct
        int housesCount = data.Houses.Count();
        Assert.AreEqual(housesCount, result.TotalHouses);

        // Assert the returned rents' count is correct
        int rentsCount = data.Houses.Where(h => h.RenterId != null).Count();
        Assert.AreEqual(rentsCount, result.TotalRents);
    }
}