using HouseRentingSystem.Services.Data;
using HouseRentingSystem.Services.Data.Entities;
using HouseRentingSystem.Services.Houses.Models;
using HouseRentingSystem.Services.Agents.Models;
using HouseRentingSystem.Services.Users;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Services.Houses;

public class HouseService(
    HouseRentingDbContext data,
    IUserService users,
    IMapper mapper)
    : IHouseService
{
    public bool Exists(int id)
        => data.Houses.Any(h => h.Id == id);

    public HouseDetailsServiceModel HouseDetailsById(int id)
    {
        House? dbHouse = data
            .Houses
            .Include(h => h.Category)
            .Include(h => h.Agent.User)
            .FirstOrDefault(h => h.Id == id);

        HouseDetailsServiceModel? house = mapper.Map<HouseDetailsServiceModel>(dbHouse);

        AgentServiceModel? agent = mapper.Map<AgentServiceModel>(dbHouse?.Agent);
        if (dbHouse != null) agent.FullName = users.UserFullName(dbHouse.Agent.UserId);

        house.Agent = agent;

        return house;
    }


    public HouseQueryServiceModel All(
        string? category = null,
        string? searchTerm = null,
        HouseSorting sorting = HouseSorting.Newest,
        int currentPage = 1,
        int housesPerPage = 1)
    {
        IQueryable<House> housesQuery = data.Houses.AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
        {
            housesQuery = data.Houses
                .Where(h => h.Category.Name == category);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            housesQuery = housesQuery.Where(h =>
                h.Title.ToLower().Contains(searchTerm.ToLower()) ||
                h.Address.ToLower().Contains(searchTerm.ToLower()) ||
                h.Description.ToLower().Contains(searchTerm.ToLower()));
        }

        housesQuery = sorting switch
        {
            HouseSorting.Price => housesQuery
                .OrderBy(h => h.PricePerMonth),
            HouseSorting.NotRentedFirst => housesQuery
                .OrderBy(h => h.RenterId != null)
                .ThenByDescending(h => h.Id),
            _ => housesQuery.OrderByDescending(h => h.Id)
        };

        List<HouseServiceModel> houses = housesQuery
            .Skip((currentPage - 1) * housesPerPage)
            .Take(housesPerPage)
            .ProjectTo<HouseServiceModel>(mapper.ConfigurationProvider)
            .ToList();

        int totalHouses = housesQuery.Count();

        return new HouseQueryServiceModel
        {
            TotalHousesCount = totalHouses,
            Houses = houses
        };
    }

    public IEnumerable<string> AllCategoriesNames()
        => data
            .Categories
            .Select(c => c.Name)
            .Distinct()
            .ToList();

    public IEnumerable<HouseServiceModel> AllHousesByAgentId(int agentId)
    {
        List<HouseServiceModel> houses = data
            .Houses
            .Where(h => h.AgentId == agentId)
            .ProjectTo<HouseServiceModel>(mapper.ConfigurationProvider)
            .ToList();

        return houses;
    }

    public IEnumerable<HouseServiceModel> AllHousesByUserId(string userId)
    {
        List<HouseServiceModel> houses = data
            .Houses
            .Where(h => h.RenterId == userId)
            .ProjectTo<HouseServiceModel>(mapper.ConfigurationProvider)
            .ToList();

        return houses;
    }

    public IEnumerable<HouseCategoryServiceModel> AllCategories()
        => data
            .Categories
            .ProjectTo<HouseCategoryServiceModel>
                (mapper.ConfigurationProvider)
            .ToList();

    public bool CategoryExists(int categoryId)
        => data.Categories.Any(c => c.Id == categoryId);

    public int Create(string title, string address, string description,
        string imageUrl, decimal price, int categoryId, int agentId)
    {
        House house = new House
        {
            Title = title,
            Address = address,
            Description = description,
            ImageUrl = imageUrl,
            PricePerMonth = price,
            CategoryId = categoryId,
            AgentId = agentId
        };

        data.Houses.Add(house);
        data.SaveChanges();

        return house.Id;
    }

    public bool HasAgentWithId(int houseId, string currentUserId)
    {
        House? house = data.Houses.Find(houseId);
        Agent? agent = data.Agents.FirstOrDefault(a => house != null && a.Id == house.AgentId);

        if (agent == null)
        {
            return false;
        }

        if (agent.UserId != currentUserId)
        {
            return false;
        }

        return true;
    }

    public int GetHouseCategoryId(int houseId)
        => data.Houses.Find(houseId)!.CategoryId;

    public void Edit(int houseId, string title, string address, string description,
        string imageUrl, decimal price, int categoryId)
    {
        House? house = data.Houses.Find(houseId);

        if (house == null)
        {
            throw new InvalidOperationException("House not found");
        }

        house.Title = title;
        house.Address = address;
        house.Description = description;
        house.ImageUrl = imageUrl;
        house.PricePerMonth = price;
        house.CategoryId = categoryId;

        data.SaveChanges();
    }

    public void Delete(int houseId)
    {
        House? house = data.Houses.Find(houseId);

        if (house == null)
        {
            throw new InvalidOperationException("House not found");
        }

        data.Remove(house);
        data.SaveChanges();
    }

    public bool IsRented(int id)
        => data.Houses.Find(id)!.RenterId != null;

    public bool IsRentedByUserWithId(int houseId, string userId)
    {
        House? house = data.Houses.Find(houseId);

        if (house == null)
        {
            return false;
        }

        if (house.RenterId != userId)
        {
            return false;
        }

        return true;
    }

    public void Rent(int houseId, string userId)
    {
        House? house = data.Houses.Find(houseId);

        if (house == null)
        {
            throw new InvalidOperationException("House not found");
        }

        house.RenterId = userId;
        data.SaveChanges();
    }

    public void Leave(int houseId)
    {
        House? house = data.Houses.Find(houseId);

        house?.RenterId = null;
        data.SaveChanges();
    }

    public IEnumerable<HouseIndexServiceModel> LastThreeHouses()
        => data
            .Houses
            .OrderByDescending(c => c.Id)
            .ProjectTo<HouseIndexServiceModel>(mapper.ConfigurationProvider)
            .Take(3);
}