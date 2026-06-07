using HouseRentingSystem.Services.Data;
using HouseRentingSystem.Tests.Mocks;

using AutoMapper;
using HouseRentingSystem.Services.Data.Entities;

namespace HouseRentingSystem.Tests.UnitTests;

public abstract class UnitTestsBase
{
    protected HouseRentingDbContext data;
    protected IMapper mapper;

    [OneTimeSetUp]
    public void SetUpBase()
    {
        data = DatabaseMock.Instance;
        mapper = MapperMock.Instance;
        SeedDatabase();
    }
    public User Renter { get; private set; }
    public Agent Agent { get; private set; }
    public House RentedHouse { get; private set; }

    private void SeedDatabase()
    {
        Renter = new User
        {
            Id = "RenterUserId",
            Email = "rent@er.bg",
            FirstName = "Renter",
            LastName = "User"
        };
        data.Users.Add(Renter);

        Agent = new Agent
        {
            PhoneNumber = "+359111111111",
            User = new User
            {
                Id = "TestUserId",
                Email = "test@test.bg",
                FirstName = "Test",
                LastName = "Tester"
            }
        };
        data.Agents.Add(Agent);

        RentedHouse = new House
        {
            Title = "First Test House",
            Address = "Test, 201 Test",
            Description = "This is a test description. This is a test description. This is a test description.",
            ImageUrl = "https://www.bhg.com/thmb/0Fg0imFSA6HVZMS2DFWPvjbYDoQ=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/white-modern-house-curved-patio-archway-c0a4a3b3-aa51b24d14d0464ea15d36e05aa85ac9.jpg",
            Renter = Renter,
            Agent = Agent,
            Category = new Category { Name = "Cottage" }
        };
        data.Houses.Add(RentedHouse);

        House nonRentedHouse = new House
        {
            Title = "Second Test House",
            Address = "Test, 204 Test",
            Description = "This is another test description. This is another test description.",
            ImageUrl = "https://images.adsttc.com/media/images/629f/3517/c372/5201/650f/1c7f/large_jpg/hyde-park-house-robeson-architects_1.jpg?1654601149",
            Renter = Renter,
            Agent = Agent,
            Category = new Category { Name = "Single-Family" }
        };

        data.Houses.Add(nonRentedHouse);
        data.SaveChanges();

    }

    [OneTimeTearDown]
    public void TearDownBase()
        => data.Dispose();
}