using HouseRentingSystem.Web.Models.Houses;
using HouseRentingSystem.Web.Infrastructure;
using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Houses;
using HouseRentingSystem.Services.Houses.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

using static HouseRentingSystem.Web.Areas.Admin.AdminConstants;
using Microsoft.Extensions.Caching.Memory;

namespace HouseRentingSystem.Web.Controllers;

public class HousesController(
    IHouseService houses,
    IAgentService agents,
    IMapper mapper,
    IMemoryCache cache)
    : Controller
{
    [Authorize]
    public IActionResult Mine()
    {
        if (User.IsInRole(AdminRoleName))
        {
            return RedirectToAction(actionName: "Mine",
                controllerName: "Houses", new { area = "Admin" });
        }

        IEnumerable<HouseServiceModel>? myHouses = null;

        string userId = User.Id();

        if (agents.ExistsById(userId))
        {
            int currentAgentId = agents.GetAgentId(userId);

            myHouses = houses.AllHousesByAgentId(currentAgentId);
        }
        else
        {
            myHouses = houses.AllHousesByUserId(userId);
        }

        return View(myHouses);
    }

    public IActionResult All([FromQuery] AllHousesQueryModel query)
    {
        HouseQueryServiceModel queryResult = houses.All(
            query.Category,
            query.SearchTerm,
            query.Sorting,
            query.CurrentPage,
            AllHousesQueryModel.HousesPerPage);

        query.TotalHousesCount = queryResult.TotalHousesCount;
        query.Houses = queryResult.Houses;

        IEnumerable<string> houseCategories = houses.AllCategoriesNames();
        query.Categories = houseCategories;

        return View(query);
    }



    public IActionResult Details(int id, string information)
    {
        if (!houses.Exists(id))
        {
            return BadRequest();
        }

        HouseDetailsServiceModel houseModel = houses.HouseDetailsById(id);

        if (information != houseModel.GetInformation())
        {
            return BadRequest();
        }

        return View(houseModel);
    }

    [Authorize]
    public IActionResult Add()
    {
        if (!agents.ExistsById(User.Id()))
        {
            return RedirectToAction(nameof(AgentsController.Become), "Agents");
        }

        return View(new HouseFormModel
        {
            Categories = houses.AllCategories()
        });
    }

    [HttpPost]
    [Authorize]
    public IActionResult Add(HouseFormModel model)
    {
        if (!agents.ExistsById(User.Id()))
        {
            return RedirectToAction(nameof(AgentsController.Become), "Agents");
        }

        if (!houses.CategoryExists(model.CategoryId))
        {
            ModelState.AddModelError(nameof(model.CategoryId),
                "Category does not exist.");
        }

        if (!ModelState.IsValid)
        {
            model.Categories = houses.AllCategories();

            return View(model);
        }

        int agentId = agents.GetAgentId(User.Id());

        int newHouseId = houses.Create(model.Title, model.Address,
            model.Description, model.ImageUrl, model.PricePerMonth,
            model.CategoryId, agentId);

        TempData["message"] = "You have successfully added a house!";

        return RedirectToAction(nameof(Details),
            new { id = newHouseId, information = model.GetInformation() });
    }


    [Authorize]
    public IActionResult Edit(int id)
    {
        if (!houses.Exists(id))
        {
            return BadRequest();
        }

        if (!houses.HasAgentWithId(id, User.Id())
            && !User.IsAdmin())
        {
            return Unauthorized();
        }

        HouseDetailsServiceModel house = houses.HouseDetailsById(id);

        int houseCategoryId = houses.GetHouseCategoryId(house.Id);

        HouseFormModel? houseModel = mapper.Map<HouseFormModel>(house);
        houseModel.CategoryId = houseCategoryId;
        houseModel.Categories = houses.AllCategories();


        return View(houseModel);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Edit(int id, HouseFormModel model)
    {
        if (!houses.Exists(id))
        {
            return View();
        }

        if (!houses.HasAgentWithId(id, User.Id())
            && !User.IsAdmin())
        {
            return Unauthorized();
        }

        if (!houses.CategoryExists(model.CategoryId))
        {
            ModelState.AddModelError(nameof(model.CategoryId),
                "Category does not exist.");
        }

        if (!ModelState.IsValid)
        {
            model.Categories = houses.AllCategories();

            return View(model);
        }

        houses.Edit(id, model.Title, model.Address, model.Description,
            model.ImageUrl, model.PricePerMonth, model.CategoryId);


        TempData["message"] = "You have successfully edited a house!";

        return RedirectToAction(nameof(Details),
            new { id = id, information = model.GetInformation() });
    }

    [Authorize]
    public IActionResult Delete(int id)
    {
        if (!houses.Exists(id))
        {
            return BadRequest();
        }

        if (!houses.HasAgentWithId(id, User.Id())
            && !User.IsAdmin())
        {
            return Unauthorized();
        }

        HouseDetailsServiceModel house = houses.HouseDetailsById(id);

        HouseDetailsViewModel? model = mapper.Map<HouseDetailsViewModel>(house);

        return View(model);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Delete(HouseDetailsViewModel model)
    {
        if (!houses.Exists(model.Id))
        {
            return BadRequest();
        }

        if (!houses.HasAgentWithId(model.Id, User.Id())
            && !User.IsAdmin())
        {
            return Unauthorized();
        }

        houses.Delete(model.Id);

        TempData["message"] = "You have successfully deleted a house!";
            
            
        return RedirectToAction(nameof(All));
    }

    [HttpPost]
    [Authorize]
    public IActionResult Rent(int id)
    {
        if (!houses.Exists(id))
        {
            return BadRequest();
        }

        if (agents.ExistsById(User.Id())
            && !User.IsAdmin())
        {
            return Unauthorized();
        }

        if (houses.IsRented(id))
        {
            return BadRequest();
        }

        houses.Rent(id, User.Id());

        cache.Remove(RentsCacheKey);


        TempData["message"] = "You have successfully rented a house!";

        return RedirectToAction(nameof(Mine));
    }

    [HttpPost]
    [Authorize]
    public IActionResult Leave(int id)
    {
        if (!houses.Exists(id) ||
            !houses.IsRented(id))
        {
            return BadRequest();
        }

        if (!houses.IsRentedByUserWithId(id, User.Id()))
        {
            return Unauthorized();
        }

        houses.Leave(id);

        cache.Remove(RentsCacheKey);

        TempData["message"] = "You have successfully left a house!";

        return RedirectToAction(nameof(Mine));
    }
}