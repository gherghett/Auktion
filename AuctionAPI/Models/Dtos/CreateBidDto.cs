using AuktionMVC.Models;

namespace AuctionAPI.Models;
public class CreateBidDto
{
    public int UserId { get; set; }
    public int AuctionId { get; set; }
    public decimal BidAmount { get; set; }
}
