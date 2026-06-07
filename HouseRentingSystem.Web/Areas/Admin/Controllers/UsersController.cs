using HouseRentingSystem.Services.Users.Models;
using HouseRentingSystem.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using static HouseRentingSystem.Web.Areas.Admin.AdminConstants;
namespace HouseRentingSystem.Web.Areas.Admin.Controllers;

public class UsersController(
    IUserService users,
    IMemoryCache cache) : AdminController
{
    [Route("Users/All")]
    public IActionResult All()
    {
        IEnumerable<UserServiceModel>? users1 = cache
            .Get<IEnumerable<UserServiceModel>>(UsersCacheKey);

        if (users1 == null)
        {
            users1 = users.All();

            MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            cache.Set(UsersCacheKey, users1, cacheOptions);
        }

        return View(users1);
    }
}