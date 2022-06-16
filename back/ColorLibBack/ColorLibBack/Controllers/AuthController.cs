using ColorLibBack.DAL;
using ColorLibBack.Models;
using ColorLibBack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorLibBack.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<AppUser> _signInManager;
        private UserManager<AppUser> _userManager;
        private AppDbContext _context;
        public AuthController(SignInManager<AppUser> signinmanager, UserManager<AppUser> userManager,AppDbContext context)
        {
            _signInManager = signinmanager;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }


        
        [HttpPost][ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (loginVM == null) return RedirectToAction("Index","Home");
            if (!ModelState.IsValid) return View(loginVM);

            var result = await _signInManager.PasswordSignInAsync(loginVM.Login, loginVM.Password,loginVM.RememberMe,true) ;
            if(!result.Succeeded)
            {
                if(result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Locked");
                    return View(loginVM);
                }
                ModelState.AddModelError("", "Wrong UserName or Password");
                return View(loginVM);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);
            AppUser user = new AppUser()
            {
                UserName = registerVM.Login,
            };

            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(registerVM);
            }

            return RedirectToAction("Login", "Auth");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }
    }
}
