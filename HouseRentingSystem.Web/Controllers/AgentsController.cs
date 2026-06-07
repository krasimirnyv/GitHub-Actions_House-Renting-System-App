using HouseRentingSystem.Web.Infrastructure;
using HouseRentingSystem.Web.Models.Agents;
using HouseRentingSystem.Services.Agents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HouseRentingSystem.Services.Users;

namespace HouseRentingSystem.Web.Controllers;

public class AgentsController(
    IAgentService agents,
    IUserService users) : Controller
{
    [Authorize]
    public IActionResult Become()
    {
        if (agents.ExistsById(User.Id()))
        {
            return BadRequest();
        }
        return View();
    }

    [Authorize]
    [HttpPost]
    public IActionResult Become(BecomeAgentFormModel model)
    {
        string userId = User.Id();

        if (users.UserHasRents(userId))
        {
            ModelState.AddModelError("Error",
                "You should have no rents to become an agent!");
        }

        if (agents.ExistsById(userId))
        {
            return BadRequest();
        }

        if (agents.AgentWithPhoneNumberExists(model.PhoneNumber))
        {
            ModelState.AddModelError(nameof(model.PhoneNumber),
                "Phone number already exists. Enter another one.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        agents.Create(userId, model.PhoneNumber);

        TempData["message"] = "You have sussessfully become an agent";

        return RedirectToAction(nameof(HousesController.All), "Houses");
    }
}