using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View();
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
    }
}