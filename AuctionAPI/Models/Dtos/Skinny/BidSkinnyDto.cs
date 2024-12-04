namespace AuctionAPI.Models;
public class BidSkinnyDto
{
    public BidSkinnyDto(Bid bid)
    {
        Id = bid.Id;
        UserId = bid.UserId;
        UserName = bid.User.Name;
        BidDate = bid.BidDate;
        BidAmount = bid.BidAmount;
        AuctionId = bid.AuctionId;
        AuctionIsActive = bid.Auction.IsActive();
    }

    public BidSkinnyDto(){}

    public int Id { get; set; }

    public int UserId { get; init; } 
    public string UserName { get; init;} = null!;
    public int AuctionId { get; init; } 
    public DateTime BidDate { get; set; }
    public decimal BidAmount { get; set; }
    public bool AuctionIsActive {get; set;}
}