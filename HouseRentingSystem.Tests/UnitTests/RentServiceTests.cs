using HouseRentingSystem.Services.Data.Entities;
using HouseRentingSystem.Services.Rents;
using HouseRentingSystem.Services.Rents.Models;

namespace HouseRentingSystem.Tests.UnitTests;

[TestFixture]
public class RentServiceTests : UnitTestsBase
{
    private IRentService rentService;

    [OneTimeSetUp]
    public void SetUp()
        => rentService = new RentService(data, mapper);

    [Test]
    public void All_ShouldReturnCorrectData()
    {
        // Arrange

        // Act: invoke the service method
        IEnumerable<RentServiceModel> result = rentService.All();

        // Assert the result is not null
        Assert.IsNotNull(result);

        // Assert the returned rents' count is correct
        IQueryable<House> rentedHousesInDb = data.Houses
            .Where(h => h.RenterId != null);
        Assert.AreEqual(rentedHousesInDb.Count(), result.ToList().Count());

        // Assert a returned rent's data is correct
        RentServiceModel? resultHouse = result.ToList()
            .Find(h => h.HouseTitle == RentedHouse.Title);
        Assert.IsNotNull(resultHouse);
        Assert.AreEqual(Renter.Email, resultHouse.RenterEmail);
        Assert.AreEqual(Renter.FirstName + " " + Renter.LastName,
            resultHouse.RenterFullName);
        Assert.AreEqual(Agent.User.Email, resultHouse.AgentEmail);
        Assert.AreEqual(Agent.User.FirstName + " " + Agent.User.LastName,
            resultHouse.AgentFullName);
    }
}