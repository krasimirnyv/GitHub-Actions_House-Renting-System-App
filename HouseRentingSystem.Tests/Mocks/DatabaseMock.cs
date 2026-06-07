using HouseRentingSystem.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Tests.Mocks;

public static class DatabaseMock
{
    public static HouseRentingDbContext Instance
    {
        get
        {
            DbContextOptions<HouseRentingDbContext> dbContextOptions = new DbContextOptionsBuilder<HouseRentingDbContext>()
                .UseInMemoryDatabase("HouseRentingInMemoryDb"
                                     + DateTime.Now.Ticks)
                .Options;

            return new HouseRentingDbContext(dbContextOptions, false);
        }
    }
}