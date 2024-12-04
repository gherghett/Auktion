using System.Security.Claims;
using AuctionAPI.Models;
using AuktionMVC.Models;
using AuctionAPI.Util;
using AuktionMVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//enables [Authorize] attribute


namespace AuktionMVC.Controllers;

public class AuthController : Controller
{
    // private readonly IAuctionRepository _repo; // Your user service that connects to the database
    private readonly UserService _userService;

    public AuthController(
        UserService userService
    )
    {
        _userService = userService;
    }

    public IActionResult Login() => View();


    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        Result<UserFatDto> result = await _userService.ValidateUser(model);

        if (!result.IsSuccess)
            return RedirectToAction("Login");

        UserFatDto user = result.Value!;

        ClaimsIdentity claimsIdentity = AuthService.CreateClaimsIdentity(user);
        AuthenticationProperties authProperties = AuthService.CreateAuthenticationProperties(model);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return RedirectToAction("TestAuth", "Auth");
    }

    // [HttpPost]
    // public async Task<IActionResult> CreateAccount(CreateUserModel model)
    // {
    //     //TODO Validate model
    //     var result = _userService.CreateUser(model);
    //     if (!result.IsSuccess)
    //         return RedirectToAction("NewUser", "Auth");

    //     //TODO feedback to user that useraccount did get created
    //     return RedirectToAction("Login", "Auth");
    // }

    [HttpGet]
    public IActionResult Test() => Ok("Test works!");

    [HttpGet]
    public IActionResult NewUser() => View();


    [HttpGet]
    [Authorize] // This endpoint requires authentication
    public IActionResult TestAuth()
    {
        return Ok(new
        {
            message = "You are authenticated!",
            username = User.Identity!.Name,
            claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }

    [HttpGet]
    [Authorize] // This endpoint requires authentication
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
        // return Ok( new { message = "Logged out successfully" } );
    }
}