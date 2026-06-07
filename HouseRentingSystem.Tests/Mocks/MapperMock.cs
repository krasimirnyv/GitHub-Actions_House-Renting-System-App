using AutoMapper;
using HouseRentingSystem.Services.Infrastructure;
using Microsoft.Extensions.Logging;

namespace HouseRentingSystem.Tests.Mocks;

public static class MapperMock
{
    private static ILoggerFactory loggerFactory = new LoggerFactory();

    public static IMapper Instance
    {
        get
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<ServiceMappingProfile>();
            }, loggerFactory);

            return new Mapper(mapperConfiguration);
        }
    }
}