using HouseRentingSystem.Services.Rents;
using HouseRentingSystem.Services.Rents.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using static HouseRentingSystem.Web.Areas.Admin.AdminConstants;

namespace HouseRentingSystem.Web.Areas.Admin.Controllers;

public class RentsController(
    IRentService rents,
    IMemoryCache cache) : AdminController
{
    [Route("Rents/All")]
    public IActionResult All()
    {
        IEnumerable<RentServiceModel>? rents1 = cache
            .Get<IEnumerable<RentServiceModel>>(RentsCacheKey);

        if (rents1 == null)
        {
            rents1 = rents.All();

            MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            cache.Set(RentsCacheKey, rents1, cacheOptions);
        }

        return View(rents1);
    }
}