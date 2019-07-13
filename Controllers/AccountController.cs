using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _userService.Authenticate(username, password);
        if (user == null)
        {
            ModelState.AddModelError("", "User not found");
            return View();
        }
 
        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
        identity.AddClaim(new Claim(ClaimTypes.GivenName, user.FirstName));
        identity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));
 
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
 
        return RedirectToAction("Index","API");
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