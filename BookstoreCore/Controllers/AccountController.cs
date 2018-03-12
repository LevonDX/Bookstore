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

            return View(new LoginRegisterViewModel()
            {
                LoginModel = model
            });
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(model.LoginModel.UserName);

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.LoginModel.Password,
                    false, false);

                if (result.Succeeded)
                {
                    string returnUrl = model.LoginModel.ReturnUrl;

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
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            LoginRegisterViewModel model = new LoginRegisterViewModel()
            {
                RegistrationModel = new RegistrationViewModel() { ReturnUrl = returnUrl }
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(LoginRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = model.RegistrationModel.UserName
                };
                IdentityResult result = await _userManager.CreateAsync(user, model.RegistrationModel.Password);
                if (result.Succeeded)
                {
                    IdentityRole role = new IdentityRole(UserRoles.Client.ToString());

                    await _roleManager.CreateAsync(role);
                    await _userManager.AddToRoleAsync(user, UserRoles.Client.ToString());
                    await _signInManager.PasswordSignInAsync(user.UserName,
                        model.RegistrationModel.Password, true, false);

                    if (model.RegistrationModel.ReturnUrl == null)
                    {
                        return RedirectToAction("Index", "Books");
                    }
                    return Redirect(model.RegistrationModel.ReturnUrl);
                }
            }
            return View();
        }

    }
}