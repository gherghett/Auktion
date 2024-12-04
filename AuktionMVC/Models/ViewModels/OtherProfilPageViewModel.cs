namespace AuktionMVC.Models;
class OtherProfilePageViewModel
{
    public UserSummary UserSummary {get; set;}  =null!;
    public UserStatistics Statistics { get; set; } = null!;
    public List<AuctionSummaryViewModel> ActiveAuctions { get; set; } = new();
    public List<SaleSummaryViewModel> CompletedSales { get; set; } = new();

    
}