using AuctionAPI.Models;

namespace AuktionMVC.Models;

public class MinaSidorViewModel
{
    public UserSummary User { get; set; } = null!;
    public int ActiveAuctions { get; set; }
    public int ActiveBids { get; set; }
    public int BuysCount { get; set; }
    public int SalesCount { get; set; }

    public static MinaSidorViewModel Create(UserFatDto user)
    {
        return new MinaSidorViewModel{
            ActiveAuctions = user.Auctions.Where(a => a.IsActive).Count(),
            BuysCount = user.Buys.Count(),
            SalesCount = user.Sales.Count(),
            ActiveBids = user.Bids.Where(b => b.AuctionIsActive).Count()
        };
    }

    // public List<AuctionSummary> Auctions { get; set; }
    // public AuctionStatistics Statistics { get; set; }

    // // Optional: Pagination info if needed
    // public int CurrentPage { get; set; }
    // public int TotalPages { get; set; }

}