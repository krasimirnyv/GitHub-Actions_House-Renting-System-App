using AutoMapper;
using HouseRentingSystem.Services.Houses.Models;
using HouseRentingSystem.Web.Models.Houses;

namespace HouseRentingSystem.Web.Infrastructure;

public class ControllerMappingProfile
    : Profile
{
    public ControllerMappingProfile()
    {
        CreateMap<HouseDetailsServiceModel, HouseFormModel>();
        CreateMap<HouseDetailsServiceModel, HouseDetailsViewModel>();
    }
}