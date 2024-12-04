using AuctionAPI.Models;

namespace AuktionMVC.Models;

public class AuctionManagementPageViewModel
{
    public AuctionManagementPageViewModel(UserFatDto user, List<AuctionFatDto> auctions, List<SaleFatDto> buys, List<SaleFatDto> sales)
    {
        PendingAuctions = auctions.Where(a => !a.HasStarted)
            .Select(a => AuctionSummaryViewModel.Create(a, user.Id))
            .ToList();
        ActiveAuctions = auctions.Where(a => a.IsActive)
            .Select(a => AuctionSummaryViewModel.Create(a, user.Id))
            .ToList();
        CompletedSales = sales
            .Select(s => SaleSummaryViewModel.Create(s, user.Id))
            .ToList();

        Stats = new StatsSummary
        {
            PendingCount = PendingAuctions.Count,
            ActiveCount = ActiveAuctions.Count,
            CompletedCount = CompletedSales.Count,
            TotalValue = sales.Sum(s => s.WinningBid.BidAmount)
        };
    }

    // Summary statistics
    public StatsSummary Stats { get; set; } = new();
    
    // Lists of auctions by status
    public List<AuctionSummaryViewModel> PendingAuctions { get; set; } = new();
    public List<AuctionSummaryViewModel> ActiveAuctions { get; set; } = new();
    public List<SaleSummaryViewModel> CompletedSales { get; set; } = new();
}

public class StatsSummary
{
    public int PendingCount { get; set; }
    public int ActiveCount { get; set; }
    public int CompletedCount { get; set; }
    public decimal TotalValue { get; set; }
}