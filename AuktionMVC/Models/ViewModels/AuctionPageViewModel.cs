using System;
using System.Collections.Generic;
using AuctionAPI.Models;
namespace AuktionMVC.Models;

public class AuctionPageViewModel
{
    public AuctionSummaryViewModel Auction {get; set;} = null!;
    public SaleSummaryViewModel? SaleSummary {get; set;} = null;
    public List<BidViewModel> BidHistory { get; set; } = null!;
    public SellerViewModel Seller { get; set; } = null!;
    public int LeadingBuyerId {get; set;}  
    public int CurrentUserId {get; set;}


    public AuctionPageViewModel(AuctionFatDto auctionDto, int currentUserId)
    {
        var auction = AuctionSummaryViewModel.Create(auctionDto, currentUserId);
        Auction = auction;
        BidHistory = auctionDto.Bids
            .Select(b => new BidViewModel(b))
            .OrderByDescending(b => b.PlacedAt)
            .ToList();
        Seller = new SellerViewModel(auctionDto.Owner);
        LeadingBuyerId = BidHistory.Select(b => b.BidderId).FirstOrDefault();
        CurrentUserId = currentUserId;
    }
}
