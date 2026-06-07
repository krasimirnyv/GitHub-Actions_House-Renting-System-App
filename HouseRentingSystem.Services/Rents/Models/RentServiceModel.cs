namespace HouseRentingSystem.Services.Rents.Models;

public class RentServiceModel
{
    public string HouseTitle { get; init; } = null!;
    
    public string HouseImageURL { get; init; } = null!;
    
    public string AgentFullName { get; init; } = null!;
    
    public string AgentEmail { get; init; } = null!;
    
    public string RenterFullName { get; init; } = null!;
    
    public string RenterEmail { get; init; } = null!;
}