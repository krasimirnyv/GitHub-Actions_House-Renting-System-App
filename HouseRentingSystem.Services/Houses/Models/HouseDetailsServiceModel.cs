using HouseRentingSystem.Services.Agents.Models;

namespace HouseRentingSystem.Services.Houses.Models;

public class HouseDetailsServiceModel : HouseServiceModel
{
    public string Description { get; init; } = null!;

    public string Category { get; init; } = null!;

    public AgentServiceModel Agent { get; set; } = null!;
}