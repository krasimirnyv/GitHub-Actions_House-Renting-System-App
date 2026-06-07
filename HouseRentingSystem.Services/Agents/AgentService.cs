using HouseRentingSystem.Services.Data;
using HouseRentingSystem.Services.Data.Entities;

namespace HouseRentingSystem.Services.Agents;

public class AgentService(HouseRentingDbContext data) : IAgentService
{
    //public bool UserHasRents(string userId)
    //  => this.data.Houses.Any(h => h.RenterId == userId);

    public bool AgentWithPhoneNumberExists(string phoneNumber)
        => data.Agents.Any(a => a.PhoneNumber == phoneNumber);
        
        
    public int GetAgentId(string userId)
        => data.Agents
            .FirstOrDefault(a => a.UserId == userId)!
            .Id;

    public bool ExistsById(string userId)
        => data.Agents.Any(a => a.UserId == userId);
    
    public void Create(string userId, string phoneNumber)
    {
        Agent agent = new Agent
        {
            UserId = userId,
            PhoneNumber = phoneNumber
        };

        data.Agents.Add(agent);
        data.SaveChanges();
    }
}