using AuctionAPI.Models;

public class UserSkinnyDto
{

    public UserSkinnyDto(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        Address = user.Address;
        CreationDate = user.CreationDate;
    }
    public UserSkinnyDto(){}


    public int Id { get; set;}
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int Rating { get; set; }
    public int Xp { get; set; }
    public DateTime CreationDate { get; set; }
}
