using HouseRentingSystem.Services.Houses;
using Microsoft.AspNetCore.Mvc;
using HouseRentingSystem.Services.Houses.Models;
using static HouseRentingSystem.Web.Areas.Admin.AdminConstants;

namespace HouseRentingSystem.Web.Controllers;

public class HomeController(IHouseService? houses) : Controller
{
    public IActionResult Index()
    {
        if (User.IsInRole(AdminRoleName))
        {
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
            
        IEnumerable<HouseIndexServiceModel> houses1 = houses!.LastThreeHouses();
        return View(houses1);
    }

    public IActionResult Error(int statusCode)
    {
        if (statusCode == 400)
        {
            return View("Error400");
        }

        if (statusCode == 401)
        {
            return View("Error401");
        }

        return View();
    }
}