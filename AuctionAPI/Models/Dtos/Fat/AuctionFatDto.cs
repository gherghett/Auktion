namespace AuctionAPI.Models;
public class AuctionFatDto
{
    public AuctionFatDto(Auction auction)
    {
        Id = auction.Id;
        Product = new ProductSkinnyDto(auction.Product);
        Period = auction.AuctionPeriod;
        Pricing = auction.AuctionPricing;
        Owner = new UserSkinnyDto(auction.Owner);
        Bids = auction.Bids.Select(b => new BidSkinnyDto(b)).ToList();
        Sale = auction.Sale is not null ? new SaleSkinnyDto(auction.Sale) : null;
        HasStarted = auction.HasStarted();
        HasEnded = auction.HasEnded();
        IsActive = auction.IsActive();
    }
    public AuctionFatDto(){}

    public int Id { get; set;}
    public ProductSkinnyDto Product { get; set; } = null!;
    public AuctionPeriod Period { get; set; } = null!;
    public AuctionPricing Pricing { get; set; } = null!;
    public UserSkinnyDto Owner { get; set; } = null!;
    public List<BidSkinnyDto> Bids { get; set; } = null!;
    //Set when product is part of a successfully finalized sale (sold)
    public bool HasStarted {get; set;}
    public bool HasEnded {get; set;}
    public bool IsActive {get; set;}

    public SaleSkinnyDto? Sale{ get; set; } = null;
}