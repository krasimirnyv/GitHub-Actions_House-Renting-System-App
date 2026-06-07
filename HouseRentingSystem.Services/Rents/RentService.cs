using AutoMapper;
using AutoMapper.QueryableExtensions;
using HouseRentingSystem.Services.Data;
using HouseRentingSystem.Services.Rents.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Services.Rents;

public class RentService(
    HouseRentingDbContext data,
    IMapper mapper) : IRentService
{
    public IEnumerable<RentServiceModel> All()
    {
        return data
            .Houses
            .Include(h => h.Agent.User)
            .Include(h => h.Renter)
            .Where(h => h.RenterId != null)
            .ProjectTo<RentServiceModel>(mapper.ConfigurationProvider)
            .ToList();
    }
}