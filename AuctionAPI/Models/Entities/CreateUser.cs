using System.ComponentModel.DataAnnotations;
// using auktionEFCore.Models;
namespace AuctionAPI.Models;

public class CreateUser
{
    [Required(ErrorMessage = "Användarnamn är obligatoriskt")]
    [Display(Name = "Användarnamn")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Email är obligatoriskt")]
    [EmailAddress(ErrorMessage = "Ogiltig email adress")]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Lösenord är obligatoriskt")]
    [MinLength(6, ErrorMessage = "Lösenordet måste vara minst 6 tecken")]
    [Display(Name = "Lösenord")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Address är obligatoriskt")]
    [Display(Name = "Address")]
    public string Address { get; set; } = null!;
}