using static HouseRentingSystem.Services.Data.DataConstants.House;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HouseRentingSystem.Services.Data.Entities;

public class House
{
    public int Id { get; init; }

    [Required]
    [MaxLength(TitleMaxLength)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(AddressMaxLength)]
    public string Address { get; set; } = null!;

    [Required]
    [MaxLength(DescriptionMaxLength)]
    public string Description { get; set; } = null!;

    [Required]
    public string ImageUrl { get; set; } = null!;

    [Column(TypeName = "decimal(12,3)")]
    public decimal PricePerMonth { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; init; } = null!;

    public int AgentId { get; set; }

    public Agent Agent { get; init; } = null!;

    public string? RenterId { get; set; }
    
    public User Renter { get; init; } = null!;
}