using System.ComponentModel.DataAnnotations;

using static HouseRentingSystem.Services.Data.DataConstants.Agent;

namespace HouseRentingSystem.Services.Data.Entities;

public class Agent
{
    public int Id { get; init; }

    [Required]
    [MaxLength(PhoneNumberMaxLength)]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    public string UserId { get; set; } = null!;

    public User User { get; init; } = null!;
}