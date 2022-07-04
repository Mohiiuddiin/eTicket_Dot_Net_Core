﻿using eTicket.Data;
using eTicket.Data.ViewModels;
using eTicket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTicket.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager; 
        private readonly SignInManager<ApplicationUser> _signInManager; 
        private readonly AppDbContext _context; 
        public AccountController(UserManager<ApplicationUser> userManager,
               SignInManager<ApplicationUser> signInManager,
               AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult Login() => View(new LoginVM());
        [HttpPost]
        public async Task<ActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View();

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user!=null)
            {
                var isMatchPass = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (isMatchPass)
                {
                    var res = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (res.Succeeded)
                    {
                        return RedirectToAction("Index","Movies");
                    }
                }
                else
                {
                    TempData["Error"] = "wrong credentials, try again!";
                    return View(loginVM);
                }
            }

            TempData["Error"] = "wrong credentials, try again!";
            return View(loginVM);
        }

        public IActionResult SignUp() => View(new SignUpVM());
    }
}
