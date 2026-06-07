using HouseRentingSystem.Services.Users;
using HouseRentingSystem.Services.Users.Models;

namespace HouseRentingSystem.Tests.UnitTests;

[TestFixture]
public class UserServiceTests : UnitTestsBase
{
    private IUserService userService;

    [OneTimeSetUp]
    public void SetUp()
        => userService = new UserService(data, mapper);

    [Test]
    public void UserHasRents_ShouldReturnTrue_WithValidData()
    {
        // Arrange

        // Act: invoke the service method with valid renter id
        bool result = userService.UserHasRents(Renter.Id);

        // Assert the retunred result is true
        Assert.IsTrue(result);
    }

    [Test]
    public void UserFullName_ShouldReturnCorrectResult()
    {
        // Arrange

        // Act: invoke the service method with valid renter id
        string? result = userService.UserFullName(Renter.Id);

        // Assert the returned result is correct
        string renterFullName = Renter.FirstName + " " +
                                Renter.LastName;
        Assert.AreEqual(renterFullName, result);
    }

    [Test]
    public void All_ShouldReturnCorrectUsersAndAgents()
    {
        // Arrange

        // Act: invoke the service method
        IEnumerable<UserServiceModel> result = userService.All();

        // Assert the returned users' count is correct
        int usersCount = data.Users.Count();
        List<UserServiceModel> resultUsers = result.ToList();
        Assert.AreEqual(usersCount, resultUsers.Count());

        // Assert the returned agents' count is correct
        int agentsCount = data.Agents.Count();
        IEnumerable<UserServiceModel> resultAgents = resultUsers.Where(us => us.PhoneNumber != "");
        Assert.AreEqual(agentsCount, resultAgents.Count());

        // Assert a returned agent data is correct
        UserServiceModel? agentUser = resultAgents
            .FirstOrDefault(ag => ag.Email == Agent.User.Email);
        Assert.IsNotNull(agentUser);
        Assert.AreEqual(Agent.PhoneNumber, agentUser.PhoneNumber);
    }
}