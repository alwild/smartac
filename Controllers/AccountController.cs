using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smartacfe.Models;
using smartacfe.Services;

public class AccountController : Controller
{
    private readonly UserService _userService;
 
    public AccountController(UserService userService)
    {
        _userService = userService;
    }
 
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
 
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel login)
    {
        if (ModelState.IsValid)
        {
            var user = await _userService.Authenticate(login.Email, login.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return View(login);
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, user.FirstName));
            identity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "API");
        }

        return View(login);
    }

    [HttpGet]
    public async Task<IActionResult> Register()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel register)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.CreateUser(register.Email, register.Password, register.FirstName,
                    register.LastName);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
                identity.AddClaim(new Claim(ClaimTypes.GivenName, user.FirstName));
                identity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "API");
            }
        }
        catch (Exception ex)
        {
            //TODO:  more target exceptions
            ModelState.AddModelError("", ex.Message);
        }
        return View(register);
    }
    
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
 
        return RedirectToAction(nameof(Login));
    }
 
    public IActionResult AccessDenied()
    {
        return View();
    }
}