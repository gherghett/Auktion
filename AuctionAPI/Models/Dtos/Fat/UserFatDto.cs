namespace AuctionAPI.Models;

public class UserFatDto
{
    public UserFatDto(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        Address = user.Address;
        CreationDate = user.CreationDate;
        Auctions = user.Auctions.Select(a => new AuctionSkinnyDto(a)).ToList();
        Hearts = user.Hearts.Select(a => new AuctionSkinnyDto(a)).ToList();
        Products = user.Products.Select(p => new ProductSkinnyDto(p)).ToList();
        Bids = user.Bids.Select(b => new BidSkinnyDto(b)).ToList();
        Sales = user.Sales.Select(s => new SaleSkinnyDto(s)).ToList();
        Buys = user.Buys.Select(b => new SaleSkinnyDto(b)).ToList();
    }
    public UserFatDto(){}

    public int Id { get; set;}
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int Rating { get; set; }
    public int Xp { get; set; }
    public DateTime CreationDate { get; set; }
    public List<AuctionSkinnyDto> Auctions { get; set; } = [];
    public List<AuctionSkinnyDto> Hearts {get; set;} = [];
    public List<ProductSkinnyDto> Products { get; set; } = [];
    public List<BidSkinnyDto> Bids { get; set; } = [];
    public List<SaleSkinnyDto> Sales { get; set; } =[];
    public List<SaleSkinnyDto> Buys { get; set; } = [];
}
