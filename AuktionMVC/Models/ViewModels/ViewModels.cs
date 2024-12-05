using System.ComponentModel.DataAnnotations;
using AuctionAPI.Models;
namespace AuktionMVC.Models;

public class StarRatingViewModel
{
    public int Rating { get; set; }
    public int Size { get; set; }
    public string? StarColor { get; set; } = "text-warning";
    public bool ShowNumber { get; set; } = true; // show the numeric rating

}

public class UserSummary
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;

    public DateTime MemberSince { get; set; }

    public int Rating;
    // public DateTime MemberSince { get; set; }
    // Other relevant user info
}

public class UserStatistics
{
    public int TotalPurchases { get; set; }
    public int TotalSales { get; set; }
    public int ActiveAuctions { get; set; }
    public decimal PositiveFeedbackPercentage { get; set; } = 98m; // Default placeholder value
}

public class AuctionCardView
{
    public bool Active;
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Url { get; set; } = null!;
    public decimal CurrentBid { get; set; }
    public TimeSpan TimeRemaining { get; set; }
}

// Basic auction information
public class AuctionSummaryViewModel
{
    public int Id { get; private set; }
    public bool UsersIsOwner { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string[] Tags { get; private set; }
    public decimal CurrentBid { get; private set; }
    public string ImageUrl { get; private set; }
    public bool IsHearted { get; private set; }
    public bool Started { get; private set; }
    public bool HasBids { get; private set; }
    public int BidCount { get; private set; }
    public decimal MinimumBidPrice { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public TimeSpan TimeRemaining => EndTime - DateTime.Now;
    public TimeSpan TimeUntilStart => StartTime - DateTime.Now;
    public decimal YourHighestBid {get; private set;}
    public int CurrentUser {get; set;}

    private AuctionSummaryViewModel(
        int id,
        bool usersIsOwner,
        string title,
        string description,
        string[] tags,
        string imageUrl,
        bool isHearted,
        bool started,
        DateTime startTime,
        DateTime endTime,
        bool hasBids,
        int bidCount,
        decimal currentBid,
        decimal minimumBidPrice,
        int currentUser = 0,
        decimal yourHighestBid = 0
        )
    {
        Id = id;
        UsersIsOwner = usersIsOwner;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Tags = tags;
        ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
        IsHearted = isHearted;
        Started = started;
        StartTime = startTime;
        EndTime = endTime;
        HasBids = hasBids;
        BidCount = bidCount;
        CurrentBid = currentBid;
        MinimumBidPrice = minimumBidPrice;
        YourHighestBid = yourHighestBid;
    }

    public static AuctionSummaryViewModel Create(AuctionFatDto auctionDto, int userId/*, bool hearted*/)
    {
        if (auctionDto == null) throw new ArgumentNullException(nameof(auctionDto));
        if (auctionDto.Product == null) throw new ArgumentNullException(nameof(auctionDto.Product));
        if (auctionDto.Period == null) throw new ArgumentNullException(nameof(auctionDto.Period));
        if (auctionDto.Pricing == null) throw new ArgumentNullException(nameof(auctionDto.Pricing));

        var bids = auctionDto.Bids ?? new List<BidSkinnyDto>();

        var latestBid = bids.OrderByDescending(b => b.BidDate).FirstOrDefault();

        var viewModel = new AuctionSummaryViewModel(
            id: auctionDto.Id,
            usersIsOwner: auctionDto.Owner.Id == userId,
            title: auctionDto.Product.Name,
            description: auctionDto.Product.Description,
            tags: [], //auctionDto.Product.Tags.ToArray(),
            imageUrl: auctionDto.Product.Images?.FirstOrDefault() ?? "/images/products/pants.jpg",
            isHearted: false, //hearted,
            started: auctionDto.HasStarted,
            startTime: auctionDto.Period.StartDate,
            endTime: auctionDto.Period.EndDate,
            hasBids: bids.Any(),
            bidCount: bids.Count(),
            currentBid: latestBid?.BidAmount ?? 0,
            minimumBidPrice: auctionDto.Pricing.MinimumBidPrice,
            currentUser: userId,
            yourHighestBid: bids.Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BidAmount)
                .Select(b => b.BidAmount)
                .FirstOrDefault()
        );

        return viewModel;
    }
}

// public class AuctionStatistics
// {
//     public int TotalActiveAuctions { get; set; }
//     public int TotalCompletedAuctions { get; set; }
//     public decimal TotalEarnings { get; set; }
//     // Other relevant statistics
// }