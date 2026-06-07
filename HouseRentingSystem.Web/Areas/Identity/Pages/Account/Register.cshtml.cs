using System.ComponentModel.DataAnnotations;
using HouseRentingSystem.Services.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

using HouseRentingSystem.Web.Areas.Admin;
using static HouseRentingSystem.Services.Data.DataConstants.User;
using Microsoft.Extensions.Caching.Memory;

namespace HouseRentingSystem.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterModel(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IMemoryCache cache)
    : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = null!;

    public string? ReturnUrl { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [Display(Name = "First Name")]
        [StringLength(UserFirstNameMaxLength, 
            MinimumLength = UserFirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(UserLastNameMaxLength,
            MinimumLength = UserLastNameMinLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(100, 
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", 
            MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", 
            ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }

    public void OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            User user = new User
            {
                UserName = Input.Email,
                Email = Input.Email,
                FirstName = Input.FirstName,
                LastName = Input.LastName
            };

            IdentityResult result = await userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                // Clear the users cache for the "All Users" page
                cache.Remove(AdminConstants.UsersCacheKey);
                return LocalRedirect("~/Login");
            }
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return Page();
    }
}