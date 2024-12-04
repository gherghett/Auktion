using System.Security.Claims;
using AuctionAPI.Models;
using AuktionMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuktionMVC.Services;

public static class AuthService
{
    public static ClaimsIdentity CreateClaimsIdentity( UserFatDto user )
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var claimsIdentity = new ClaimsIdentity( claims, CookieAuthenticationDefaults.AuthenticationScheme );
        return claimsIdentity;
    }

    public static AuthenticationProperties CreateAuthenticationProperties( LoginModel loginModel )
    {
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = loginModel.RememberMe,
            //IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays( 7 )
        };
        return authProperties;
    }
}