using HouseRentingSystem.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Tests.IntegrationsTests;

public class HomeControllerTests
{
    private HomeController homeController;

    [OneTimeSetUp]
    public void SetUp()
        => homeController = new HomeController(null);

    [OneTimeTearDown]
    public void TearDown()
    => homeController.Dispose();
    
    [Test]
    public void Error_ShouldReturnCorrectView()
    {
        // Arrange: assign a valid status code to a variable
        int statusCode = 500;

        // Act: invoke the controller method with valid data
        IActionResult result = homeController.Error(statusCode);

        // Assert the returned result is not null
        Assert.IsNotNull(result);

        // Assert the returned result is a view
        ViewResult? viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);
    }
}