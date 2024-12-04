using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
namespace AuctionAPI.Models;

public class User : Entity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int Rating { get; set; }
    public int Xp { get; set; }
    public DateTime CreationDate { get; set; }
    [JsonIgnore]
    public List<Auction> Auctions { get; set; } = [];
    [JsonIgnore]
    public List<Auction> Hearts {get; set;} = [];
    [JsonIgnore]
    public List<Product> Products { get; set; } = [];
    [JsonIgnore]
    public List<Bid> Bids { get; set; } = [];
    [JsonIgnore]
    public List<Sale> Sales { get; set; } =[];
    [JsonIgnore]
    public List<Sale> Buys { get; set; } = [];

    public User() {}

    public static Result<User> Create(CreateUser model)
    {
        // Validate the CreateUser model
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model);

        if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
        {
            // Combine all error messages into one exception
            List<string> errorMessages = validationResults
                .Where(v => !string.IsNullOrEmpty(v.ErrorMessage))
                .Select(v => v.ErrorMessage ?? "").ToList();
            return Result.Failure<User>(errorMessages);
        }

        // If validation passes, create the User object
        return new User
        {
            Name = model.Name,
            Email = model.Email,
            Password = model.Password,
            Address = model.Address,
            CreationDate = DateTime.Now,
            Rating = 0, // Default values for new users
            Xp = 0      // Default values for new users
        };
    }

    public override string ToString()
    {
        return $"{Id} {Name} {Email};";
    }
}
