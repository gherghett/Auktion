using System.Security.Claims;

//Detta är helt onödiga men trevliga extentions för att få tillbaka 
// id o och mail från den nuvarande usern, 
// .FindFirstValue(ClaimTypes.NameIdentifier) är lite bökigt
namespace AuktionMVC.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId( this ClaimsPrincipal user )
    {
        string userIdAsString = user.FindFirstValue( ClaimTypes.NameIdentifier )
                                ?? throw new UnauthorizedAccessException( "User Id claim is missing" );

        if ( !int.TryParse( userIdAsString, out int userId ) )
            throw new FormatException( "User Id claim is not in the correct format" );

        return userId;
    }


    public static string? GetEmail( this ClaimsPrincipal user )
        => user.FindFirstValue( ClaimTypes.Email );
}