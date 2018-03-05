using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookstoreCore.Helpers;
using BookstoreCore.Models.AccountViewModels;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreCore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            LoginViewModel model = new LoginViewModel() { ReturnUrl = returnUrl };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(model.UserName);

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password,
                    false, false);

                if (result.Succeeded)
                {
                    string returnUrl = model.ReturnUrl;

                    if (returnUrl == null)
                        return RedirectToAction("Index", "Books");

                    return Redirect(returnUrl);
                }
                else
                {
                    return View();
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            RegistrationViewModel model = new RegistrationViewModel()
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = model.UserName
                };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    IdentityRole role = new IdentityRole(UserRoles.Client.ToString());
                    await _roleManager.CreateAsync(role);
                    await _userManager.AddToRoleAsync(user, UserRoles.Client.ToString());
                    await _signInManager.PasswordSignInAsync(user.UserName, model.Password, true, false);
                    if (model.ReturnUrl == null)
                    {
                        return RedirectToAction("Index", "Books");
                    }
                    return Redirect(model.ReturnUrl);
                }
            }
            return View();
        }

    }
}