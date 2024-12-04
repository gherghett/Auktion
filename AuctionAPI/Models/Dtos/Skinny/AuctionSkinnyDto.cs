namespace AuctionAPI.Models;
public class AuctionSkinnyDto
{
    public AuctionSkinnyDto(Auction auction)
    {
        Id = auction.Id;
        ProductId = auction.ProductId;
        AuctionPeriod = auction.AuctionPeriod;
        AuctionPricing = auction.AuctionPricing;
        OwnerId = auction.OwnerId;
        SaleId = auction.SaleId;
        HasStarted = auction.HasStarted();
        HasEnded = auction.HasEnded();
        IsActive = auction.IsActive();
    }
    public AuctionSkinnyDto() {}


    public int Id { get; set;}
    public int? ProductId { get; set; } = null!;
    public AuctionPeriod AuctionPeriod { get; set; } = null!;
    public AuctionPricing AuctionPricing { get; set; } = null!;
    public int? OwnerId { get; set; } = null!;
    //public List<BidSkinnyDto> Bids { get; set; } = null!;
    //Set when product is part of a successfully finalized sale (sold)
    public int? SaleId{ get; set; } = null;
    public bool HasStarted {get; set;}
    public bool HasEnded {get; set;}
    public bool IsActive {get; set;}
}