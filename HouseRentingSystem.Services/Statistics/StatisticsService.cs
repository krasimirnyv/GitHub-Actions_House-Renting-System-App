using HouseRentingSystem.Services.Data;
using HouseRentingSystem.Services.Statistics.Models;

namespace HouseRentingSystem.Services.Statistics;

public class StatisticsService(HouseRentingDbContext data) : IStatisticsService
{
    public StatisticsServiceModel Total()
    {
        int totalHouses = data.Houses.Count();
        int totalRents = data.Houses.Count(h => h.RenterId != null);

        return new StatisticsServiceModel
        {
            TotalHouses = totalHouses,
            TotalRents = totalRents
        };
    }
}